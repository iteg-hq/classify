using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classify
{
    public class DAL
    {
        private readonly string connectionString;

        public DAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        ///  Get zero or one classifier type by Code
        /// </summary>
        /// <param name="classifierTypeCode">The type code of the classifier to return</param>
        /// <param name="result">The classifier type the the code `classifierTypeCode`</param>
        /// <returns></returns>
        public bool TryGetClassifierType(string classifierTypeCode, out ClassifierType result)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.GetClassifierType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClassifierTypeCode", classifierTypeCode);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            result = new ClassifierType(this, reader.GetString(0))
                            {
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                UpdatedBy = reader.GetString(3),
                                UpdatedOn = reader.GetDateTime(4)
                            };
                            return true;
                        }
                        else
                        {
                            result = null;
                            return false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get a classifier type with the code `ClassifierTypeCode`. If the type does not exist, an error is thrown.
        /// </summary>
        /// <param name="classifierTypeCode">The code of the classifier type to return.</param>
        /// <returns></returns>
        public ClassifierType GetClassifierType(string classifierTypeCode)
        {
            if (TryGetClassifierType(classifierTypeCode, out ClassifierType type))
            {
                return type;
            }
            throw new KeyNotFoundException();
        }

        public ClassifierType this[string typeCode] => GetClassifierType(typeCode);

        public IEnumerable<ClassifierType> GetClassifierTypes()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.GetClassifierTypes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                yield return new ClassifierType(this, reader.GetString(0))
                                {
                                    Name = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    UpdatedBy = reader.GetString(3),
                                    UpdatedOn = reader.GetDateTime(4)
                                };
                            }
                        }
                    }
                }
            }
        }

        public bool DeleteClassifierType(string code)
        {
            //if (!HasClassifierType(code)) return false;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.DeleteClassifierType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClassifierTypeCode", code);
                    command.ExecuteNonQuery();
                }
                return true;
            }
        }

        public ClassifierType SaveClassifierType(IClassifierType classifierType)
            => SaveClassifierType(classifierType.Code, classifierType.Name, classifierType.Description);

        public ClassifierType SaveClassifierType(string code, string name, string description)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.SaveClassifierType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Code", code);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Description", description);
                    command.ExecuteNonQuery();
                }

            }
            return GetClassifierType(code);
        }

        // Classifiers
        public IEnumerable<Classifier> GetClassifiers(string typeCode)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.GetClassifiers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClassifierTypeCode", typeCode);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                yield return new Classifier(this, reader.GetString(0), reader.GetString(1))
                                {
                                    Name = reader.GetString(2),
                                    Description = reader.GetString(3),
                                    UpdatedBy = reader.GetString(4),
                                    UpdatedOn = reader.GetDateTime(5)
                                };
                            }
                        }
                    }
                }
            }
        }

        public bool TryGetClassifier(string classifierTypeCode, string classifierCode, out Classifier result)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.GetClassifier", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClassifierTypeCode", classifierTypeCode);
                    command.Parameters.AddWithValue("@ClassifierCode", classifierCode);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            result = new Classifier(this, reader.GetString(0), reader.GetString(1))
                            {
                                Name = reader.GetString(2),
                                Description = reader.GetString(3),
                                UpdatedBy = reader.GetString(4),
                                UpdatedOn = reader.GetDateTime(5)
                            };
                            return true;
                        }
                        else
                        {
                            result = null;
                            return false;
                        }
                    }
                }
            }
        }



        public Classifier GetClassifier(string typeCode, string code)
            => GetClassifiers(typeCode).Single(c => c.Code.ToLower() == code.ToLower());

        public Classifier SaveClassifier(IClassifier classifier)
        {
            return SaveClassifier(classifier.TypeCode, classifier.Code, classifier.Name, classifier.Description);
        }

        public Classifier SaveClassifier(string typeCode, string code, string name, string description)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.SaveClassifier", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TypeCode", typeCode);
                    command.Parameters.AddWithValue("@Code", code);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Description", description);
                    command.ExecuteNonQuery();
                }
            }
            return GetClassifier(typeCode, code);
        }

        public bool TryDeleteClassifier(string classifierTypeCode, string classifierCode)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.DeleteClassifier", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClassifierTypeCode", classifierTypeCode);
                    command.Parameters.AddWithValue("@ClassifierCode", classifierCode);
                    command.ExecuteNonQuery();
                }
            }
            return true; // TODO: return real value
        }
        //public IEnumerable<ClassifierRelationship> GetRelatedClassifiers(IClassifier classifier)
        //    => GetClassifierRelationships(classifier.TypeCode, classifier.Code);

        public IEnumerable<ClassifierRelationship> GetClassifierRelationships(string classifierTypeCode, string classifierCode)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.GetClassifierRelationships", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClassifierTypeCode", classifierTypeCode);
                    command.Parameters.AddWithValue("@ClassifierCode", classifierCode);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                yield return new ClassifierRelationship(
                                    dal: this,
                                    classifierTypeCode: reader.GetString(0),
                                    classifierCode: reader.GetString(1),
                                    relationshipTypeCode: reader.GetString(2),
                                    relatedClassifierTypeCode: reader.GetString(3),
                                    relatedClassifierCode: reader.GetString(4)
                                    )
                                {
                                    Description = reader.GetString(5),
                                    Weight = reader.GetDouble(6),
                                    IsInbound = reader.GetInt32(7) == 1,
                                    UpdatedBy = reader.GetString(8),
                                    UpdatedOn = reader.GetDateTime(9)
                                };
                            }
                        }
                    }
                }
            }
        }
        public void SaveClassifierRelationship(string typeCode, string code, string relationshipCode,
            string relatedTypeCode, string relatedCode,
            string description, double weight)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.SaveClassifierRelationship", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClassifierTypeCode", typeCode);
                    command.Parameters.AddWithValue("@ClassifierCode", code);
                    command.Parameters.AddWithValue("@ClassifierRelationshipTypeCode", relationshipCode);
                    command.Parameters.AddWithValue("@RelatedClassifierTypeCode", relatedTypeCode);
                    command.Parameters.AddWithValue("@RelatedClassifierCode", relatedCode);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@Weight", weight);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteClassifierRelationship(
            string typeCode,
            string code,
            string relationshipCode,
            string relatedTypeCode,
            string relatedCode)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.DeleteClassifierRelationship", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ClassifierTypeCode", typeCode);
                    command.Parameters.AddWithValue("@ClassifierCode", code);
                    command.Parameters.AddWithValue("@ClassifierRelationshipTypeCode", relationshipCode);
                    command.Parameters.AddWithValue("@RelatedClassifierTypeCode", relatedTypeCode);
                    command.Parameters.AddWithValue("@RelatedClassifierCode", relatedCode);
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<string> GetClassifierRelationshipTypes()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("dbo.GetClassifierRelationshipTypes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                yield return reader.GetString(0);
                            }
                        }
                    }
                }
            }
        }

        public void Reset()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE internal.Classifier WHERE ClassifierTypeCodeID > 0;DELETE internal.ClassifierRelationship;", connection))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}