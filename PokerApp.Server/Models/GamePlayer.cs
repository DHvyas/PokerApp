namespace PokerApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class GamePlayer
{
    [Key]
    public int GamePlayerID { get; set; }

    public int GameID { get; set; }

    public int UserID { get; set; }

    public bool IsActive { get; set; } = true;

    public decimal InitialChips { get; set; }

    [ForeignKey("GameID")]
    public required Game Game { get; set; }

    [ForeignKey("UserID")]
    public required User User { get; set; }
}

