namespace Klickit_Task.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Invoices = new HashSet<Invoice>();
        }


        [Key]
        public long Order_ID { get; set; }

        public long User_ID { get; set; }

        [Required]
        [StringLength(250)]
        public string Date { get; set; }

        [Required]
        [StringLength(250)]
        public string Time { get; set; }

        [Required]
        [StringLength(250)]
        public string Status { get; set; }

        public double Total { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        public virtual User User { get; set; }
    }
}
