using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classify
{
    public class ClassifierCollection
    {
        public ICollection<ClassifierType> ClassifierTypes = new List<ClassifierType>();

        public ClassifierType this[string code]
        {
            get
            {
                return ClassifierTypes.First(t => t.Code == code);
            }
        }

        public ClassifierCollection()
        {
        }

        public static ClassifierCollection Load(ClassifierCollection collection, SqlConnection connection)
        {
            using (var command = new SqlCommand("SELECT TypeCode, [Description] FROM dbo.ClassifierType;", connection))
            {
                command.CommandType = CommandType.Text;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            collection.ClassifierTypes.Add(new ClassifierType(reader.GetString(0), reader.GetString(1)));
                        }
                    }
                }
            }
            using (var command = new SqlCommand("SELECT ClassifierTypeCode, ClassifierCode, ClassifierDescription FROM dbo.Classifier ORDER BY rv", connection))
            {
                command.CommandType = CommandType.Text;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var classifierType = collection.ClassifierTypes.First(t => t.Code == reader.GetString(0));
                            classifierType.AddMember(reader.GetString(1), reader.GetString(2));
                        }
                    }
                }
            }

            using (var command = new SqlCommand("SELECT ClassifierTypeCode, ClassifierCode, RelationshipTypeCode, RelatedClassifierTypeCode, RelatedClassifierCode, [Description], [Weight] FROM dbo.ClassifierRelationship ORDER BY rv", connection))
            {
                command.CommandType = CommandType.Text;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var classifierType = collection.ClassifierTypes.First(t => t.Code == reader.GetString(0));
                            var classifier = classifierType.Members.First(c => c.Code == reader.GetString(1));
                            string relationshipTypeCode = reader.GetString(2);
                            string description = reader.GetString(5);
                            double weight = reader.GetDouble(6);
                            var relatedClassifierType = collection.ClassifierTypes.First(t => t.Code == reader.GetString(3));
                            var relatedClassifier = relatedClassifierType.Members.First(c => c.Code == reader.GetString(4));
                            var rln = new ClassifierRelationship(classifier, relationshipTypeCode, relatedClassifier) { Weight = weight };
                            classifier.Relationships.Add(rln);
                        }
                    }
                }
            }


            return collection;
        }
    }
}
