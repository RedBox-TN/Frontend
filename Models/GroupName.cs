using System.ComponentModel.DataAnnotations;

namespace Frontend.Models;

public class GroupName
{
	[Required(ErrorMessage = "Campo obbligatorio")]
	public string? groupName { get; set; }
}