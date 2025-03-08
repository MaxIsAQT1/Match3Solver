public class Card(SuitType suit, RankType rank)
{
    public SuitType Suit { get; set; } = suit;
    public RankType Rank { get; set; } = rank;

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
    public string ToShortString()
    {
        string suitSymbol = Suit switch
        {
            SuitType.Hearts => "♥",
            SuitType.Diamonds => "♦",
            SuitType.Clubs => "♣",
            SuitType.Spades => "♠",
            _ => throw new ArgumentOutOfRangeException()
        };

        return $"{Rank} of {suitSymbol}";
    }
    public string ToImageLocation()
    {
        int rank = (int)Rank;
        if (rank < 9)
        {
            return $"Resources/{rank + 2}_of_{Suit.ToString().ToLower()}.png";
        }
        else
        {
            return $"Resources/{Rank.ToString().ToLower()}_of_{Suit.ToString().ToLower()}.png";
        }
    }
}

public enum SuitType
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}

public enum RankType
{
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}
