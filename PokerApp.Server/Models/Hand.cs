namespace PokerApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Hand
{
    [Key]
    public int HandID { get; set; }

    public int GamePlayerID { get; set; }

    [StringLength(5)]
    public string Card1 { get; set; }

    [StringLength(5)]
    public string Card2 { get; set; }

    [ForeignKey("GamePlayerID")]
    public GamePlayer GamePlayer { get; set; }
}

