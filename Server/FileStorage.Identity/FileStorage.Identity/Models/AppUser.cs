using Microsoft.AspNetCore.Identity;

namespace FileStorage.Identity.Models;

public class AppUser : IdentityUser
{
    public string Name { get; set; }
    public string LastName { get; set; }
}