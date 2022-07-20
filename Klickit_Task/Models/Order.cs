namespace Klickit_Task.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [Key]
        public long Order_ID { get; set; }

        public long User_ID { get; set; }

        public long Prod_ID { get; set; }

        [Required]
        [StringLength(250)]
        public string Date { get; set; }

        [Required]
        [StringLength(250)]
        public string Time { get; set; }

        [Required]
        [StringLength(250)]
        public string Status { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
