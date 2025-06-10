using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public Department Department { get; set; }

        [Required(ErrorMessage = "Hire Date is required")]
        [Display(Name = "Hire Date")]
        [CustomValidation(typeof(HireDateValidator), nameof(HireDateValidator.ValidateHireDate))]
        public DateTime? HireDate { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Column(TypeName = "decimal(18, 2)")] 
        [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be a positive value.")]
        public decimal Salary { get; set; }
    }
}
