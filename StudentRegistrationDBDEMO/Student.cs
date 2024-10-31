using System.ComponentModel.DataAnnotations;

namespace StudentRegistrationDBDEMO
{
    internal class Student
    {
        [Key]
        public int StudentId { get; private set; }

        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "First name cannot contain spaces")]
        public string? StudentFirstName { get; set; }

        public string? StudentLastName { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? StudentCity { get; set; }
    }
}
