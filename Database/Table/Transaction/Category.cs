using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aes.Database.Table.Transaction
{
    public class Category
    {
        [Table("Category")]
        public class Model
        {
            [Key]
            public long TransactionCategoryID { get; set; }
            public string Name { get; set; }
            public decimal PlannedAmount { get; set; }
            public Type Type { get; set; }
            public string Color { get; set; }
        }
        public enum Type
        {
            Expense = 0,
            Income = 1,
        }

        public class Service : Table.Service<Model>
        {

        }
    }
}
