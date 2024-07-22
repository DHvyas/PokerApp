namespace PokerApp.Server.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Game
{
    [Key]
    public int GameID { get; set; }

    [StringLength(100)]
    public required string GameName { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [StringLength(20)]
    public required string Status { get; set; }

    public decimal PotAmount { get; set; }
    [ForeignKey("CurrentTurnUserID")]
    public int? CurrentTurnUserID { get; set; }
    public int WinnerUserID { get; set; }
}

