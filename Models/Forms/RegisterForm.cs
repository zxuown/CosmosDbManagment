using System.ComponentModel.DataAnnotations;

namespace CosmosDbManagment.Models.Forms;

public class RegisterForm
{
    [Required]
    public string Username { get; set; }

	[Required]
	public string Surname { get; set; }

	[Required]
    [EmailAddress]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }    
}
