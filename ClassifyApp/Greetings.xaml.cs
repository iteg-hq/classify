using Classify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace ClassifierApp
{
    public class Test
    {
        public static string foo = "hej";
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DAL CAL;
        public MainWindow()
        {
            /*
            var coll = new ServiceCollection();
            coll.AddTransient<ICollection<ClassifierType>, ObservableCollection<ClassifierType>>();
            coll.AddTransient<ICollection<Classifier>, ObservableCollection<Classifier>>();
            coll.AddTransient<ClassifierCollection, ClassifierCollection>();
            var prov = coll.BuildServiceProvider();
            */
            string connectionString = "Data Source=localhost;Integrated Security=True;Database=Classifier";

            CAL = new DAL(connectionString);

            InitializeComponent();
            //this.DataContext = ClassifierCollection;
            //lbClassifierTypes.ItemsSource = CAL.ClassifierTypes;
        }

        private void LbClassifierTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ClassifierType type = (ClassifierType)lbClassifierTypes.SelectedItem;
            //if (type == null) return;
            //lbClassifiers.ItemsSource = type.GetMembers();
        }

        private void LbClassifiers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //IClassifier classifier = (IClassifier)lbClassifiers.SelectedItem;
            //if (classifier == null) return;
            //lbRelationships.ItemsSource = classifier.Relationships;
        }
    }
}
