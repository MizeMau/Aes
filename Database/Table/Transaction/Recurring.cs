using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aes.Database.Table.Transaction
{
    public class Recurring
    {
        [Table("Recurring")]
        public class Model
        {
            [Key]
            public long TransactionRecurringID { get; set; }
            public DateOnly StartDate { get; set; }
            public FrequencyType FrequencyType { get; set; }
            public int FrequencyInterval { get; set; }
            public long TransactionTransactionID { get; set; }
        }
        public enum FrequencyType
        {
            Daily = 0,
            Weekly = 1,
            Monthly = 2,
            Yearly = 3,
        }

        public class Service : Table.Service<Model>
        {

        }
    }
}
