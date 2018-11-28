using System;
using System.Data;
using System.Data.SqlClient;

namespace ImportCSV
{
	public class SalesLogRepository
	{
		private readonly string _connectionString;

		private string errorLog;
		/// <summary>
		/// Gets the connection string and creates the storage context
		/// </summary>
		/// <param name="connectionString"></param>
		public SalesLogRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		/// <summary>
		/// Insert multiple records into storage table
		/// </summary>
		/// <param name="storages"></param>		
		public void InsertIntoMembers(DataTable salesTable)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				SqlTransaction transaction = null;
				connection.Open();
				try
				{
					transaction = connection.BeginTransaction();
					using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
					{
						sqlBulkCopy.BulkCopyTimeout = 60;
						sqlBulkCopy.DestinationTableName = "theBigStorage";
						sqlBulkCopy.ColumnMappings.Add("ShopCode", "ShopCode");
						sqlBulkCopy.ColumnMappings.Add("ItemId", "ItemId");
						sqlBulkCopy.ColumnMappings.Add("ItemName", "ItemName");
						sqlBulkCopy.ColumnMappings.Add("PricePerItem", "PricePerItem");
						sqlBulkCopy.ColumnMappings.Add("CountOfItems", "CountOfItems");
						sqlBulkCopy.ColumnMappings.Add("TotalPrice", "TotalPrice");
						sqlBulkCopy.ColumnMappings.Add("TransactionDate", "TransactionDate");

						sqlBulkCopy.WriteToServer(salesTable);
					}
					transaction.Commit();
				}
				catch (Exception ex)
				{
					errorLog = ex.Message;
					transaction.Rollback();
					throw;
				}
			}
		}
	}
}
