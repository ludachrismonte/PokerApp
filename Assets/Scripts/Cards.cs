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
        new Card(CardSuit.Spades, CardValue.Two),
        new Card(CardSuit.Diamonds, CardValue.Ace),
        new Card(CardSuit.Diamonds, CardValue.King),
        new Card(CardSuit.Diamonds, CardValue.Queen),
        new Card(CardSuit.Diamonds, CardValue.Jack),
        new Card(CardSuit.Diamonds, CardValue.Ten),
        new Card(CardSuit.Diamonds, CardValue.Nine),
        new Card(CardSuit.Diamonds, CardValue.Eight),
        new Card(CardSuit.Diamonds, CardValue.Seven),
        new Card(CardSuit.Diamonds, CardValue.Six),
        new Card(CardSuit.Diamonds, CardValue.Five),
        new Card(CardSuit.Diamonds, CardValue.Four),
        new Card(CardSuit.Diamonds, CardValue.Three),
        new Card(CardSuit.Diamonds, CardValue.Two),
        new Card(CardSuit.Hearts, CardValue.Ace),
        new Card(CardSuit.Hearts, CardValue.King),
        new Card(CardSuit.Hearts, CardValue.Queen),
        new Card(CardSuit.Hearts, CardValue.Jack),
        new Card(CardSuit.Hearts, CardValue.Ten),
        new Card(CardSuit.Hearts, CardValue.Nine),
        new Card(CardSuit.Hearts, CardValue.Eight),
        new Card(CardSuit.Hearts, CardValue.Seven),
        new Card(CardSuit.Hearts, CardValue.Six),
        new Card(CardSuit.Hearts, CardValue.Five),
        new Card(CardSuit.Hearts, CardValue.Four),
        new Card(CardSuit.Hearts, CardValue.Three),
        new Card(CardSuit.Hearts, CardValue.Two),
        new Card(CardSuit.Clubs, CardValue.Ace),
        new Card(CardSuit.Clubs, CardValue.King),
        new Card(CardSuit.Clubs, CardValue.Queen),
        new Card(CardSuit.Clubs, CardValue.Jack),
        new Card(CardSuit.Clubs, CardValue.Ten),
        new Card(CardSuit.Clubs, CardValue.Nine),
        new Card(CardSuit.Clubs, CardValue.Eight),
        new Card(CardSuit.Clubs, CardValue.Seven),
        new Card(CardSuit.Clubs, CardValue.Six),
        new Card(CardSuit.Clubs, CardValue.Five),
        new Card(CardSuit.Clubs, CardValue.Four),
        new Card(CardSuit.Clubs, CardValue.Three),
        new Card(CardSuit.Clubs, CardValue.Two),
    };

    private int deal_card_id;

    // Use this for initialization
    void Start () {
        deal_card_id = 0;

    }

    public Card GetTopCard() {
        deal_card_id++;
        Debug.Log("Top Card = " + Deck[deal_card_id - 1].String());
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
