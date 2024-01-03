using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeCrud.Models
{
    [Table("Department", Schema = "dbo")]
    public class Department
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Department ID")]
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Department Name is required.")]
        [Column(TypeName = "varchar(150)")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        [StringLength(5, ErrorMessage = "Department Abbreviation cannot exceed 5 characters.")]
        [Column(TypeName = "varchar(5)")]
        [Display(Name = "Department Abbreviation")]
        public string DepartmentAbbr { get; set; }

    }
}
