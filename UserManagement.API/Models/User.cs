using System.ComponentModel.DataAnnotations;

namespace UserManagement.API.Models;

public class User
{
    public int Id { get; set; }

    [Required, StringLength(80, MinimumLength = 2)]
    public string Name { get; set; } = "";

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = "";

    [Required, StringLength(60, MinimumLength = 2)]
    public string Department { get; set; } = "";
}

public class UserInput
{
    [Required, StringLength(80, MinimumLength = 2)]
    public string Name { get; set; } = "";

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = "";

    [Required, StringLength(60, MinimumLength = 2)]
    public string Department { get; set; } = "";
}
