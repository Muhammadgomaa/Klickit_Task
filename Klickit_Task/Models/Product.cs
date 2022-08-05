namespace Klickit_Task.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Invoices = new HashSet<Invoice>();
        }

        [Key]
        public long Prod_ID { get; set; }

        [Required(ErrorMessage = "Product Name is Required")]
        [StringLength(250, ErrorMessage = "Please Enter Valid Product Name", MinimumLength = 2)]
        [Remote("CheckProduct", "Admin", ErrorMessage = "This Product is Already Exist", AdditionalFields = "Prod_ID")]
        public string Prod_Name { get; set; }

        [Required(ErrorMessage = "Product Price is Required")]
        [Range(1,200000, ErrorMessage = "Please Enter Valid Product Price")]
        public double Prod_Price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

    }
}
