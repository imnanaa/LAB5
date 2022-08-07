using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
namespace LAB5
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>

	public class Record
	{
		public int ID = 0;
		public string Coef;
		public string Name;

		public void loadFromTable(DataRowView selected)
		{
			if (selected != null)
			{
				object[] items = selected.Row.ItemArray;

				ID = Convert.ToInt32(items[0]);
				Name = Convert.ToString(items[1]);
				Coef = Convert.ToString(items[2]);
			}
			else
			{
				ID = 0;
				Name = "";
				Coef = "";
			}
		}
	}
	public partial class MainWindow : Window
	{
		string connectionString = null;
		SqlConnection connection = null;
		SqlCommand command;
		SqlDataAdapter adapter;

		Record record = new Record();

		public MainWindow()
		{
			InitializeComponent();
			connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			//MessageBox.Show(connectionString);
			try
			{
				showData();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void GetAndDhowData(string SQLQuery, DataGrid dataGrid)
		{
			connection = new SqlConnection(connectionString);
			connection.Open();
			command = new SqlCommand(SQLQuery, connection);
			adapter = new SqlDataAdapter(command);
			DataTable Table = new DataTable();
			adapter.Fill(Table);
			dataGrid.ItemsSource = Table.DefaultView;
			connection.Close();
		}

		private void ExecuteQuery(string SQLQuery)
		{
			connection = new SqlConnection(connectionString);
			connection.Open();
			command = new SqlCommand(SQLQuery, connection);
			command.ExecuteNonQuery();
			connection.Close();
		}

		private void INSERT_Click(object sender, RoutedEventArgs e)
		{
			string Name = this.Name.Text.Trim();
			string Coef = this.Coef.Text.Trim();
			string sqlQ = $@"INSERT INTO City
	(
		Name,
		Coef
	)
	Values
	(
		'{Name}',
		'{Coef}'
	);
";
			try
			{
				ExecuteQuery(sqlQ);
				showData();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		
		
		private void showData()
		{
			string sqlSelect = "SELECT * FROM City;";
			GetAndDhowData(sqlSelect, City);
		}

		private void CityDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				DataGrid grid = (DataGrid)sender;
				DataRowView selected = (DataRowView)grid.SelectedItem;
				record.loadFromTable(selected);
				loadFromRecord();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				record = new Record();
			}
		}

		private void loadFromRecord()
		{
			if (record.ID != 0)
			{
				Name.Text = record.Name.ToString();
				Coef.Text = record.Coef.ToString();

			}
			else
			{
				Name.Text = "";
				Coef.Text = "";

			}
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			if (record.ID == 0)
			{
				MessageBox.Show("Жоден рядок не вибраний");
				return;
			}

			string Name = this.Name.Text.Trim();
			string Coef = this.Coef.Text.Trim();
			string sqlQ = $@"
		UPDATE
			City
		SET
			Name = '{Name}',
			Coef = '{Coef}'
		WHERE  ID = '{record.ID}';";
			try
			{
				ExecuteQuery(sqlQ);
				showData();
				record = new Record();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

        private void DELETE_Click_1(object sender, RoutedEventArgs e)
        {
			if (record.ID == 0)
			{
				MessageBox.Show("Жоден рядок не вибраний");
				return;
			}
			string sqlQ = $"DELETE City WHERE  ID = '{record.ID}';";
			try
			{
				ExecuteQuery(sqlQ);
				showData();
				record = new Record();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
    }


}