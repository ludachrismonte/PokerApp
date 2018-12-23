using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardSuit {
    None = 0,
    Spades = 1,
    Clubs = 2,
    Diamonds = 3,
    Hearts = 4
};

public enum CardValue {
    None = 0,
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13,
};

public struct Card
{
    public CardSuit suit;
    public CardValue value;

    public Card(CardSuit s, CardValue v)
    {
        this.suit = s;
        this.value = v;
    }

    public string String() {
        return this.value.ToString() + this.suit.ToString();
    }
}

/*
        "KS", "QS", "JS", "TS", "9S", "8S", "7S", "6S", "5S", "4S", "3S", "2S",
        "AC", "KC", "QC", "JC", "TC", "9C", "8C", "7C", "6C", "5C", "4C", "3C", "2C",
        "AD", "KD", "QD", "JD", "TD", "9D", "8D", "7D", "6D", "5D", "4D", "3D", "2D",
        "AH", "KH", "QH", "JH", "TH", "9H", "8H", "7H", "6H", "5H", "4H", "3H", "2H"
        */
public class Cards : MonoBehaviour {

    private List<Card> Deck = new List<Card> {
        new Card(CardSuit.Spades, CardValue.Ace),
        new Card(CardSuit.Spades, CardValue.King),
        new Card(CardSuit.Spades, CardValue.Queen),
        new Card(CardSuit.Spades, CardValue.Jack),
        new Card(CardSuit.Spades, CardValue.Ten),
        new Card(CardSuit.Spades, CardValue.Nine),
        new Card(CardSuit.Spades, CardValue.Eight),
        new Card(CardSuit.Spades, CardValue.Seven),
        new Card(CardSuit.Spades, CardValue.Six),
        new Card(CardSuit.Spades, CardValue.Five),
        new Card(CardSuit.Spades, CardValue.Four),
        new Card(CardSuit.Spades, CardValue.Three),
        new Card(CardSuit.Spades, CardValue.Two)
    };

    private int deal_card_id;

    // Use this for initialization
    void Start () {
        deal_card_id = 0;

    }

    public Card GetTopCard() {
        deal_card_id++;
        return Deck[deal_card_id - 1];
    }

    public Card GetBlankCard()
    {
        return new Card(CardSuit.None, CardValue.None);
    }

    public void Shuffle() {

    }

    public void Reset() {
        deal_card_id = 0;
    }
}
