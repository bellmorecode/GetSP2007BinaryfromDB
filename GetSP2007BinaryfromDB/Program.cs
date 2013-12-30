using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace GetSP2007BinaryfromDB
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePath = @"c:\temp\";
            Console.Write("Enter ObjectID to Extract: ");
            var idString = Console.ReadLine();

            Console.WriteLine("ID: {0}", idString);

            var cs = ConfigurationManager.ConnectionStrings["ConfigDB"].ConnectionString;

            using (var cn = new SqlConnection(cs))
            {
                cn.Open();

                string object_name = string.Empty;

                var getNameSql = @"SELECT NAME FROM OBJECTS WHERE ID = '" + idString + "'";
                var getDataSql = @"SELECT FILEIMAGE FROM BINARIES WHERE ObjectID = '" + idString + "'";

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = getNameSql;
                    object_name = (string)cmd.ExecuteScalar();
                }

                Console.WriteLine("Name: {0}", object_name);

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = getDataSql;
                    byte[] rawdata = (byte[])cmd.ExecuteScalar();

                    var path = Path.Combine(basePath, object_name);
                    File.WriteAllBytes(path, rawdata);
                }

                

                cn.Close();
            }

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
