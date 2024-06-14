using Microsoft.AspNetCore.Identity;

namespace CosmosDbManagment.Models;

public class TeamMember : IdentityUser<string>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}
