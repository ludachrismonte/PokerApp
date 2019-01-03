using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HandValue
{
    None = -1,
    HighCard = 0,
    OnePair = 1,
    TwoPairs = 2,
    ThreeOfAKind = 3,
    Straight = 4,
    Flush = 5,
    FullHouse = 6,
    FourOfAKind = 7,
    StraightFlush = 8,
};

public struct Hand
{
    public HandValue type;
    public Card[] top_five;

    public Hand(HandValue t)
    {
        this.type = t;
        this.top_five = null;
    }

    public Hand(HandValue t, Card[] five)
    {
        this.type = t;
        this.top_five = five;
    }

    public string String() {
        string s = this.type.ToString() + "\n";
        for (int i = 0; i < this.top_five.Length; i++)
        {
            s += this.top_five[i].String() + "\n";
        }
        return s;
    }
}

public class HandDeterminer : MonoBehaviour {

    public Hand myHand;

    public int[] suit_count;
    public int[] value_count;
    public Card[] top_five;

    public Card[] test_hand;

    public Text HandText;
   
    // Use this for initialization
    void Start () {
        suit_count = new int[4];
        value_count = new int[13];
        top_five = new Card[5];
        HandText.text = "";

        //test_hand = new Card[7];
        //FillTestHand();
        //Debug.Log(Determine(test_hand).String());
    }

    public void Clear()
    {
        HandText.text = "";
    }

    private void FillTestHand()
    {
        test_hand[0] = new Card(CardSuit.Clubs, CardValue.Four);
        test_hand[1] = new Card(CardSuit.Clubs, CardValue.Five);
        test_hand[2] = new Card(CardSuit.Clubs, CardValue.Three);
        test_hand[3] = new Card(CardSuit.Clubs, CardValue.Ace);
        test_hand[4] = new Card(CardSuit.Clubs, CardValue.Two);
        test_hand[5] = new Card(CardSuit.Clubs, CardValue.Nine);
        test_hand[6] = new Card(CardSuit.Clubs, CardValue.Jack);
    }

    private void FillCounts(Card[] cards) {
        for (int i = 0; i < cards.Length; i++) {
            if (cards[i].suit != CardSuit.None && cards[i].value != CardValue.None) {
                suit_count[(int)cards[i].suit]++;
                value_count[(int)cards[i].value]++;
            }
        }
    }

    private void ResetCounts()
    {
        for (int i = 0; i < suit_count.Length; i++)
        {
            suit_count[i] = 0;
        }
        for (int i = 0; i < value_count.Length; i++)
        {
            value_count[i] = 0;
        }
    }

    private void PrintCards(Card[] cards) {
        string s = "";
        for (int i = 0; i < cards.Length; i++) {
            s += cards[i].String() + "\n";
        }
        Debug.Log(s);
    }

    private Card[] Sort(Card[] cards) {
        int key;
        for (int i = 1; i < cards.Length; i++) {
            key = i;
            while (key > 0 && cards[key].isGreaterThan(cards[key - 1])) {
                Card temp = cards[key];
                cards[key] = cards[key - 1];
                cards[key - 1] = temp;
                key--;
            }
        }
        return cards;
    }

    private CardValue GetHighestGroupValue(int size) {
        for (int i = value_count.Length - 1; i >= 0; i--)
        {
            if (value_count[i] == size)
            {
                return (CardValue)i;
            }
        }
        return CardValue.None;
    }

    private bool TestForStraightFlush(Card[] cards)
    {
        CardSuit hot_suit = CardSuit.None;
        for (int i = 0; i < suit_count.Length; i++)
        {
            if (suit_count[i] >= 5)
            {
                hot_suit = (CardSuit)i;
            }
        }
        if (hot_suit == CardSuit.None)
        {
            return false;
        }
        List<Card> hot_cards = new List<Card>();
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].suit == hot_suit)
            {
                hot_cards.Add(cards[i]);
            }
        }
        int[] temp_value_count = new int[13];
        for (int i = 0; i < hot_cards.Count; i++)
        {
            if (hot_cards[i].value != CardValue.None)
            {
                temp_value_count[(int)hot_cards[i].value]++;
            }
        }
        int this_sequnce = 0;
        CardValue temp_top = CardValue.None;
        CardValue top_value = CardValue.None;
        for (int i = temp_value_count.Length - 1; i >= 0; i--)
        {
            if (temp_value_count[i] > 0)
            {
                if (this_sequnce == 0)
                {
                    temp_top = (CardValue)i;
                }
                this_sequnce++;
                if (this_sequnce == 5)
                {
                    top_value = temp_top;
                }
            }
            else this_sequnce = 0;
        }
        //Check Low Ace
        bool get_low_ace = false;
        if (temp_value_count[12] > 0)
        {
            this_sequnce++;
            if (this_sequnce == 5)
            {
                get_low_ace = true;
                top_value = temp_top;
            }
        }
        if (top_value == CardValue.None)
        {
            return false;
        }
        int fill_index = 0;
        for (int i = 0; i < hot_cards.Count && ((fill_index < 5 && !get_low_ace) || (fill_index < 4 && get_low_ace)); i++)
        {
            if (hot_cards[i].value == top_value)
            {
                top_five[fill_index] = hot_cards[i];
                fill_index++;
            }
            else if (fill_index > 0)
            {
                top_five[fill_index] = hot_cards[i];
                fill_index++;
            }
            if (get_low_ace && hot_cards[i].value == CardValue.Ace)
            {
                top_five[4] = hot_cards[i];
            }
        }
        myHand = new Hand(HandValue.StraightFlush, top_five);
        return true;
    }

    private bool TestForFourKind(Card[] cards)
    {
        CardValue quads_value = GetHighestGroupValue(4);
        if (quads_value == CardValue.None)
        {
            return false;
        }
        int fill_index = 0;
        int kickers = 0;
        for (int i = 0; i < cards.Length && fill_index < 5; i++)
        {
            if (cards[i].value == quads_value)
            {
                top_five[fill_index] = cards[i];
                fill_index++;
            }
            else if (kickers < 1) {
                top_five[4] = cards[i];
                kickers++;
            }
        }
        myHand = new Hand(HandValue.FourOfAKind, top_five);
        return true;
    }

    private bool TestForFullHouse(Card[] cards)
    {
        CardValue trips_value = CardValue.None;
        CardValue pair_value = CardValue.None;
        int num_trips = 0;
        for (int i = 0; i < value_count.Length; i++)
        {
            if (value_count[i] == 3)
            {
                num_trips++;
                if (num_trips == 2) {
                    pair_value = trips_value;
                }
                trips_value = (CardValue)i;
            }
            if (value_count[i] == 2)
            {
                pair_value = (CardValue)i;
            }
        }
        if (trips_value == CardValue.None || pair_value == CardValue.None)
        {
            return false;
        }
        int trip_index = 0;
        int pair_index = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].value == trips_value && trip_index < 3)
            {
                top_five[trip_index] = cards[i];
                trip_index++;
            }
            if (cards[i].value == pair_value && pair_index < 2)
            {
                top_five[pair_index + 3] = cards[i];
                pair_index++;
            }
        }
        myHand = new Hand(HandValue.FullHouse, top_five);
        return true;
    }

    private bool TestForFlush(Card[] cards)
    {
        CardSuit hot_suit = CardSuit.None;
        for (int i = 0; i < suit_count.Length; i++)
        {
            if (suit_count[i] >= 5) {
                hot_suit = (CardSuit)i;
            }
        }
        if (hot_suit == CardSuit.None) {
            return false;
        }
        int fill_index = 0;
        for (int i = 0; i < cards.Length && fill_index < 5; i++) {
            if (cards[i].suit == hot_suit) {
                top_five[fill_index] = cards[i];
                fill_index++;
            }
        }
        myHand = new Hand(HandValue.Flush, top_five);
        return true;
    }

    private bool TestForStraight(Card[] cards)
    {
        int this_sequnce = 0;
        CardValue temp_top = CardValue.None;
        CardValue top_value = CardValue.None;
        for (int i = value_count.Length - 1; i >= 0; i--)
        {
            if (value_count[i] > 0)
            {
                if (this_sequnce == 0) {
                    temp_top = (CardValue)i;
                }
                this_sequnce++;
                if (this_sequnce == 5)
                {
                    top_value = temp_top;
                }
            }
            else this_sequnce = 0;
        }
        //Check Low Ace
        bool get_low_ace = false;
        if (value_count[12] > 0)
        {
            this_sequnce++;
            if (this_sequnce == 5)
            {
                get_low_ace = true;
                top_value = temp_top;
            }
        }
        if (top_value == CardValue.None) {
            return false;
        }
        int fill_index = 0;
        bool got_top = false;
        for (int i = 0; i < cards.Length && ((fill_index < 5 && !get_low_ace) || (fill_index < 4 && get_low_ace)); i++)
        {
            if (cards[i].value == top_value)
            {
                top_five[fill_index] = cards[i];
                fill_index++;
                got_top = true;
            }
            else if (got_top && cards[i].value != cards[i-1].value) {
                top_five[fill_index] = cards[i];
                fill_index++;
            }
            if (get_low_ace && cards[i].value == CardValue.Ace) {
                top_five[4] = cards[i];
            }
        }
        myHand = new Hand(HandValue.Straight, top_five);
        return true;
    }

    private bool TestForThreeKind(Card[] cards)
    {
        CardValue trips_value = GetHighestGroupValue(3);
        if (trips_value == CardValue.None)
        {
            return false;
        }
        int trip_index = 0;
        int kicker_index = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].value == trips_value && trip_index < 3)
            {
                top_five[trip_index] = cards[i];
                trip_index++;
            }
            if (cards[i].value != trips_value && kicker_index < 2)
            {
                top_five[kicker_index + 3] = cards[i];
                kicker_index++;
            }
        }
        myHand = new Hand(HandValue.ThreeOfAKind, top_five);
        return true;
    }

    private bool TestForTwoPair(Card[] cards)
    {
        CardValue high_pair_value = CardValue.None;
        CardValue low_pair_value = CardValue.None;
        bool got_high = false;
        bool got_low = false;
        for (int i = value_count.Length - 1; i >= 0; i--)
        {
            if (value_count[i] == 2)
            {
                if (!got_high)
                {
                    got_high = true;
                    high_pair_value = (CardValue)i;
                }
                else if (!got_low)
                {
                    got_low = true;
                    low_pair_value = (CardValue)i;
                }
            }
        }
        if (!got_high || !got_low)
        {
            return false;
        }
        int top_index = 0;
        int bottom_index = 0;
        int kicker = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].value == high_pair_value)
            {
                top_five[top_index] = cards[i];
                top_index++;
            }
            else if (cards[i].value == low_pair_value)
            {
                top_five[bottom_index + 2] = cards[i];
                bottom_index++;
            }
            else if (kicker < 1)
            {
                top_five[4] = cards[i];
                kicker++;
            }
        }
        myHand = new Hand(HandValue.TwoPairs, top_five);
        return true;
    }

    private bool TestForOnePair(Card[] cards)
    {
        CardValue pair_value = CardValue.None;
        for (int i = value_count.Length - 1; i >= 0; i--)
        {
            if (value_count[i] == 2)
            {
                pair_value = (CardValue)i;
            }
        }
        if (pair_value == CardValue.None)
        {
            return false;
        }
        int fill_index = 0;
        int kickers = 0;
        for (int i = 0; i < cards.Length && fill_index < 5; i++)
        {
            if (cards[i].value == pair_value)
            {
                top_five[fill_index] = cards[i];
                fill_index++;
            }
            else if (kickers < 3)
            {
                top_five[kickers + 2] = cards[i];
                kickers++;
            }
        }
        myHand = new Hand(HandValue.OnePair, top_five);
        return true;
    }

    private bool HighCard(Card[] cards)
    {
        for (int i = 0; i < 5; i++)
        {
            top_five[i] = cards[i];
        }
        myHand = new Hand(HandValue.HighCard, top_five);
        return true;
    }

    public Hand Determine(Card[] cards) {
        Sort(cards);
        //PrintCards(cards);
        ResetCounts();
        FillCounts(cards);
        if (TestForStraightFlush(cards)) { }
        else if (TestForFourKind(cards)) { }
        else if (TestForFullHouse(cards)) { }
        else if (TestForFlush(cards)) { }
        else if (TestForStraight(cards)) { }
        else if (TestForThreeKind(cards)) { }
        else if (TestForTwoPair(cards)) { }
        else if (TestForOnePair(cards)) { }
        else if (HighCard(cards)) { }
        HandText.text = myHand.type.ToString();
        return myHand;
    }
}
