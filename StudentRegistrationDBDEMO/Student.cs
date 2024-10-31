using System.ComponentModel.DataAnnotations;

namespace StudentRegistrationDBDEMO
{
    internal class Student
    {
        [Key]
        public int StudentId { get; private set; }

        /// <summary>
        /// The student's first name
        /// </summary>
        /// <remarks>
        /// Property cannot be null or empty, nor contain any whitespace characters
        /// </remarks>
        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "First name cannot contain spaces")]
        public string? StudentFirstName { get; set; }

        /// <summary>
        /// The student's last name
        /// </summary>
        public string? StudentLastName { get; set; }

        /// <summary>
        /// The student's city of residence
        /// </summary>
        [Required(ErrorMessage = "City is required.")]
        public string? StudentCity { get; set; }
    }
}
