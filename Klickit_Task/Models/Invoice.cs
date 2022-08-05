using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Klickit_Task.Models
{
    public class Invoice
    {
        [Key]
        public long Invo_ID { get; set; }

        public long Order_ID { get; set; }

        public long Prod_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Quantity { get; set; }

        public double Price { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}