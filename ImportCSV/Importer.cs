using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ImportCSV
{
	/// <summary>
	/// Importer class reads the log file and inserts rows into the table
	/// </summary>
	public class Importer
	{
		private readonly string _connectionString;
		private readonly string _filePath;
		private readonly SalesLogRepository _repository;
		private DataTable _salesTable;
		private string errorLog;

		public Importer(string filePath, string datasource, string database)
		{
			_connectionString = SetConnectionString(datasource, database);
			_repository = new SalesLogRepository(_connectionString);
			_filePath = filePath;
			_salesTable = CreateSalesTable();
		}

		public void Import()
		{
			var counter = 0;
			try
			{
				using (StreamReader sr = new StreamReader(_filePath))
				{
					while (!sr.EndOfStream)
					{

						string line = sr.ReadLine();
						if (string.IsNullOrEmpty(line))
						{
							continue;
						}

						string[] fields = line.Split(',');

						if (fields[0] == "AAA")
						{
							_salesTable.Rows.Add(fields[0]
										, Convert.ToInt32(fields[1])
										, fields[2]
										, Convert.ToDecimal(fields[4])
										, Convert.ToInt32(fields[5])
										, Convert.ToDecimal(fields[7])
										, Convert.ToDateTime(fields[6]));
						}
						else if (fields[0] == "BBB")
						{
							_salesTable.Rows.Add(fields[0]
										, Convert.ToInt32(fields[4])
										, fields[2]
										, Convert.ToDecimal(fields[5])
										, Convert.ToInt32(fields[6])
										, Convert.ToDecimal(fields[7])
										, Convert.ToDateTime(fields[1] + " " + fields[3]));
						}
						counter++;

						try
						{
							if (counter >= 5000000)
							{
								ProcessData(_salesTable);
								counter = 0;
								_salesTable.Clear();
							}
						}
						catch (SqlException ex)
						{
							// handle the exception
							errorLog = ex.Message;
							throw;
						}
					}
				}
			}
			catch (Exception ex)
			{
				// code to log exception
				errorLog = ex.Message;
				throw;
			}
		}

		private DataTable CreateSalesTable()
		{
			var salesTable = new DataTable();
			salesTable.Columns.Add("ShopCode");
			salesTable.Columns.Add("ItemId");
			salesTable.Columns.Add("ItemName");
			salesTable.Columns.Add("PricePerItem");
			salesTable.Columns.Add("CountOfItems");
			salesTable.Columns.Add("TotalPrice");
			salesTable.Columns.Add("TransactionDate");
			return salesTable;
		}

		private void ProcessData(DataTable salesTable)
		{
			_repository.InsertIntoMembers(salesTable);
		}

		private string SetConnectionString(string datasource, string database)
		{
			var sqlConn = new SqlConnectionStringBuilder()
			{
				DataSource = datasource,
				InitialCatalog = database,
				IntegratedSecurity = true
			};

			return sqlConn.ConnectionString;
		}
	}
}

