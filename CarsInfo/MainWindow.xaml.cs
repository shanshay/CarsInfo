using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace CarsInfo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlDataAdapter dataAdapter;
        DataTable carsTable;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection("server=(localdb)\\MSSQLLocalDB; Trusted_Connection=Yes;DataBase=CarsInfo;");
                connection.Open();
                string query = "SELECT ModelName AS 'Модель', Mark AS 'Марка', Year AS 'Год выпуска' FROM Cars";
                SqlCommand command = new SqlCommand(query, connection);
                carsTable = new DataTable();
                dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(carsTable);
                carsList.ItemsSource = carsTable.AsDataView();
                carsList.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }            
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SqlConnection connection = null;
            DataTable factoryTable;
            textNoFact.Visibility = Visibility.Collapsed;
            factoryList.Visibility = Visibility.Collapsed;
            if (carsList.SelectedItem != null)
            {
                DataRowView row = carsList.SelectedItem as DataRowView;
                string model = row.Row.ItemArray[0].ToString();
                try
                {
                    connection = new SqlConnection("server=(localdb)\\MSSQLLocalDB; Trusted_Connection=Yes;DataBase=CarsInfo;");
                    connection.Open();
                    string query = "SELECT Factories.FactoryName AS 'Завод', Cars.ModelName AS 'Модель', Factories.Country AS 'Страна-производитель' FROM Factories INNER JOIN Cars ON Factories.Model_ID = Cars.Model_ID WHERE Cars.ModelName=N'" + model + "'" ; //WHERE Cars.Model='" + model + "'"
                    //WHERE tRoutesPuncts.ID_Route=" + id
                    SqlCommand command = new SqlCommand(query, connection);
                    factoryTable = new DataTable();
                    dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(factoryTable);                    
                    if (factoryTable.Rows.Count == 0)
                    {
                        textNoFact.Text = "Нигде не производится";
                        textNoFact.Visibility = Visibility.Visible;
                    }

                    else
                    {
                        factoryList.ItemsSource = factoryTable.AsDataView();
                        factoryList.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (connection != null)
                        connection.Close();
                }
            }
        }
    }
}
