using System.ComponentModel.DataAnnotations;

namespace Frontend.Models;

public class UserLogin
{
	[Required(ErrorMessage = "Campo Obbligatorio")]
	public required string? UsernameEmail { get; set; }

	[Required(ErrorMessage = "Campo Obbligatorio")]
	public required string? Password { get; set; }
}