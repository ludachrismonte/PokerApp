using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardSuit {
    None = -1,
    Spades = 0,
    Clubs = 1,
    Diamonds = 2,
    Hearts = 3
};

public enum CardValue {
    None = -1,
    Two = 0,
    Three = 1,
    Four = 2,
    Five = 3,
    Six = 4,
    Seven = 5,
    Eight = 6,
    Nine = 7,
    Ten = 8,
    Jack = 9,
    Queen = 10,
    King = 11,
    Ace = 12,
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

    public bool isGreaterThan(Card other)
    {
        return this.value > other.value;
    }

    public string String() {
        return this.value.ToString() + this.suit.ToString();
    }
}

public class Cards : MonoBehaviour {

    private PhotonView PV;

    public static Cards cards;

    private List<Card> Deck;

    private int deal_card_id;

    private void Awake()
    {
        if (Cards.cards == null)
        {
            Cards.cards = this;
        }
        else
        {
            if (Cards.cards != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Use this for initialization
    void Start() {
        PV = GetComponent<PhotonView>();
        deal_card_id = 0;
        Reset();
    }

    public Card GetTopCard() {
        deal_card_id++;
        //Debug.Log("Top Card = " + Deck[deal_card_id - 1].String());
        return Deck[deal_card_id - 1];
    }

    public Card GetBlankCard()
    {
        return new Card(CardSuit.None, CardValue.None);
    }

    public void Shuffle() {
        if (!PhotonNetwork.IsMasterClient) {
            return;
        }
        Debug.Log("Shuffling");
        List<int> straight_order = new List<int>() {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
            21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
            41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51
        };
        int[] new_order = new int[52];

        int max = straight_order.Count;
        int i = 0;
        while (max > 0)
        {
            int offset = UnityEngine.Random.Range(0, max);
            new_order[i] = straight_order[offset];
            straight_order.RemoveAt(offset);
            max -= 1;
            i++;
        }
        PV.RPC("RPC_Shuffle", RpcTarget.All, new_order);
    }

    [PunRPC]
    public void RPC_Shuffle(int[] new_order){
        Reset();
        List<Card> tmp = new List<Card>();
        for (int i = 0; i < new_order.Length; i++) {
            tmp.Add(Deck[new_order[i]]);
        }
        Deck = tmp;
    }

    public void Reset() {
        deal_card_id = 0;
        if (Deck != null) {
            Deck.Clear();
        }
        Deck = new List<Card> {
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
    }
}
