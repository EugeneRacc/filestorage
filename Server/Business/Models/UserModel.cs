using System.Numerics;
using Data.Entities;

namespace Business.Models;

public class UserModel
{
    //TODO add business logic to UserModel
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public BigInteger UsedDiskSpace { get; set; }
    public ICollection<int> FilesIds { get; set; }
}