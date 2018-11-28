using System.Data.Entity;

namespace ImportCSV
{
	public class StorageContext : DbContext
	{
		public DbSet<SalesLog> Storages { get; set; }

		public StorageContext(string connectionString)
		{
			Database.Connection.ConnectionString = connectionString;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// other code 
			Database.SetInitializer<StorageContext>(null);
			// more code
		}
	}
}
