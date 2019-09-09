using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace Classify
{
    public class DAL
    {
        private readonly string connectionString;

        public Dictionary<string, ClassifierType> typeCache = new Dictionary<string, ClassifierType>();
        public Dictionary<string, Classifier> classifierCache = new Dictionary<string, Classifier>();
        public Dictionary<string, ClassifierRelationship> relationshipCache = new Dictionary<string, ClassifierRelationship>();

        public DAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private SqlConnection GetSqlConnection()
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }

        public ClassifierType GetClassifierType(string classifierTypeCode)
        {
            if (!typeCache.Keys.Contains(classifierTypeCode))
            {
                using (var connection = GetSqlConnection())
                {
                    using (var command = new SqlCommand("dbo.GetClassifierType", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ClassifierTypeCode", classifierTypeCode);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                typeCache[classifierTypeCode] = new ClassifierType(
                                    code: reader.GetString(0),
                                    name: reader.GetString(1),
                                    description: reader.GetString(2)
                                    );
                            }
                        }
                    }
                }
            }
            return typeCache[classifierTypeCode];
        }

        public IEnumerable<Classifier> GetClassifiers(string classifierTypeCode)
        {
            using (var connection = GetSqlConnection())
            {
                using (var command = new SqlCommand("GetClassifiers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClassifierTypeCode", classifierTypeCode);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var c = new Classifier(
                                    dal: this,
                                    classifierTypeCode: classifierTypeCode,
                                    code: reader.GetString(1),
                                    name: reader.GetString(2),
                                    description: reader.GetString(3)
                                    );
                                classifierCache[c.FullPath] = c;
                                yield return c;
                            }
                        }
                    }
                }
            }
        }





        public IEnumerable<ClassifierType> GetClassifierTypes()
        {
            using (var connection = GetSqlConnection())
            {
                using (var command = new SqlCommand("dbo.GetClassifierTypes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                yield return new ClassifierType(
                                    code: reader.GetString(0),
                                    name: reader.GetString(1),
                                    description: reader.GetString(2)
                                    );
                            }
                        }
                    }
                }
            }
        }


        public void SaveClassifierType(string connectionString, ClassifierType classifierType)
        {
            using (var connection = GetSqlConnection(connectionString))
            {
                using (var command = new SqlCommand("dbo.SaveClassifierType;", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TypeCode", classifierType.Code);
                    command.Parameters.AddWithValue("@Name", classifierType.Name);
                    command.Parameters.AddWithValue("@Description", classifierType.Description);
                    command.ExecuteNonQuery();
                }
            }
        }



        public IEnumerable<ClassifierRelationship> GetClassifierRelationships(string classifierTypeCode, string code)
        {
            using (var connection = GetSqlConnection(connectionString))
            {
                using (var command = new SqlCommand("dbo.GetClassifierRelationships", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                yield return new ClassifierRelationship(
                                    classifierTypeCode: reader.GetString(0),
                                    classifierCode: reader.GetString(1),
                                    relationshipTypeCode: reader.GetString(2),
                                    relatedClassifierTypeCode: reader.GetString(3),
                                    relatedClassifierCode: reader.GetString(4),
                                    description: reader.GetString(5),
                                    weight: reader.GetFloat(6)
                                    );
                            }
                        }
                    }
                }
            }
        }
    }
}

    */