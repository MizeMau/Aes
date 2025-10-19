using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aes.Database.Table.Transaction
{
    public class Transaction
    {
        [Table("Transaction")]
        public class Model
        {
            [Key]
            public long TransactionTransactionID { get; set; }
            public decimal Amount { get; set; }
            public string Note { get; set; }
            public DateOnly Date { get; set; }
            public long TransactionCategoryID { get; set; }
        }

        public class Service : Table.Service<Model>
        {
            public List<Model> GetAllByMonth(DateOnly? date = null)
            {
                date ??= DateOnly.FromDateTime(DateTime.Now);

                return GetQuery()
                       .Where(w => w.Date.Year == date.Value.Year)
                       .Where(w => w.Date.Month == date.Value.Month)
                       .ToList();
            }
        }
    }
}
