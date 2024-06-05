namespace PokerApp.Server.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CommunityCards
{
    [Key]
    public int CommunityCardsID { get; set; }

    public int GameID { get; set; }

    [StringLength(5)]
    public string Card1 { get; set; }

    [StringLength(5)]
    public string Card2 { get; set; }

    [StringLength(5)]
    public string Card3 { get; set; }

    [StringLength(5)]
    public string Card4 { get; set; }

    [StringLength(5)]
    public string Card5 { get; set; }

    [ForeignKey("GameID")]
    public Game Game { get; set; }
}

