using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classify
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassifierCollection coll = new ClassifierCollection();
            using (var conn = new SqlConnection("Server=localhost;Database=Classify;Trusted_Connection=True;"))
            {
                conn.Open();
                ClassifierCollection.Load(coll, conn);
            }
            foreach (var c in coll["Bordereau"]["IndigoRisk"]["UnderwritingYear"])
            {
                Console.WriteLine(c.GetRelatedByRelationshipCode("InceptionDate").First());
                Console.WriteLine(c.GetRelatedByRelationshipCode("ExpiryDate").First());
            }
            Console.ReadLine();
        }
    }
}
