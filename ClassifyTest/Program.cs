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
        static void Main()
        {
            DAL dal = new DAL("Server=localhost;Database=Classify;Trusted_Connection=True;");
            dal.Reset();
            ClassifierType t = dal.SaveClassifierType("country", "Country", "Lande" );
            var dk = t.AddMember("DK", "Denmark", "Danmark");
            t.AddMember("DE", "Germany", "Deutschland");
            t.AddMember("SE", "Sweden", "Sverige")
                .AddRelated("Neighbor", dk);



            /*
            dal.GetContext(t).AddMember("cl", "Cl", "desc"); // Classifier
            dal["test"]["cl"]["rel"];

            t.AddMember("c", "C", "Desc");
            Console.WriteLine(t.Description)
            */
        }
    }
}
