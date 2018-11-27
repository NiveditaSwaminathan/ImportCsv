using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImportCSV
{
	[Table("theBigStorage")]
	public class SalesLog
	{
		[Key]
		public int RecordId { get; set; }

		[StringLength(3, MinimumLength = 3)]
		public string ShopCode { get; set; }

		public int ItemId { get; set; }

		public string ItemName { get; set; }

		public decimal PricePerItem { get; set; }

		public int CountOfItems { get; set; }

		public decimal TotalPrice { get; set; }

		public DateTime TransactionDate { get; set; }
	}
}
