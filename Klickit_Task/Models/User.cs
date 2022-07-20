namespace Klickit_Task.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        public long User_ID { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression("^[A-Za-z0-9._%+-]*@[A-Za-z0-9.-]*\\.[A-Za-z0-9-]{2,}$",ErrorMessage = "Enter Valid Email")]
        [StringLength(250)]
        [Remote("CheckUser", "Home", ErrorMessage = "This Email is Already Exist", AdditionalFields = "User_ID")]
        public string User_Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [StringLength(250, ErrorMessage = "The Password Must be 6 Digits at Least", MinimumLength = 6)]
        public string User_Password { get; set; }

        [Required]
        [StringLength(250)]
        public string User_Role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
