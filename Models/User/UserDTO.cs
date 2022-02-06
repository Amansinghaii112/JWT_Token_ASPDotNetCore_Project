
#region References.

using System.ComponentModel.DataAnnotations;

#endregion References.

namespace JWT_Token_Project
{
    public class UserDTO
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
