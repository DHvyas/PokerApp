namespace PokerApp.Server.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Round
{
    [Key]
    public int RoundID { get; set; }

    public int GameID { get; set; }

    public int RoundNumber { get; set; }

    [StringLength(20)]
    public string RoundName { get; set; } // 'pre-flop', 'flop', 'turn', 'river'

    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    public DateTime? EndTime { get; set; }

    [ForeignKey("GameID")]
    public Game Game { get; set; }
}

