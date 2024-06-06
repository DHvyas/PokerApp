namespace PokerApp.Server.Models;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser
{
    [Key]
    public int UserID { get; set; }

    [Required]
    [StringLength(50)]
    public required string Username { get; set; }

    [Required]
    [StringLength(100)]
    public required string Email { get; set; }

    [Required]
    public required string PasswordHash { get; set; }

    public string? ProfilePicture { get; set; }

    public decimal TotalWinnings { get; set; }

    public int GamesPlayed { get; set; }
}
