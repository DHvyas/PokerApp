namespace PokerApp.Server.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Bet
{
    [Key]
    public int BetID { get; set; }

    public int RoundID { get; set; }

    public int GamePlayerID { get; set; }

    public decimal BetAmount { get; set; }

    public DateTime BetTime { get; set; } = DateTime.UtcNow;

    [ForeignKey("RoundID")]
    public Round Round { get; set; }

    [ForeignKey("GamePlayerID")]
    public GamePlayer GamePlayer { get; set; }
}

