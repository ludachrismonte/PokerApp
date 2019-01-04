using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayoutManager : MonoBehaviour {

    private BettingManager betting_manager;
    private StackManager stack_manager;

    public List<PokerPlayer> players;

    // Use this for initialization
    void Start () {
        betting_manager = GetComponent<BettingManager>();
        stack_manager = GetComponent<StackManager>();
    }

    public void InitializePlayerList(int num_registered)
    {
        Debug.Log("Betting Manager is initializing player list to size " + num_registered);
        for (int i = 0; i < num_registered; i++)
        {
            players.Add(GameObject.Find("Player" + i).GetComponent<PokerPlayer>());
        }
    }

    public void RunPayout() {
        List<PokerPlayer> winners = new List<PokerPlayer>();
        Hand BestHand = new Hand(HandValue.None);
        for (int i = 0; i < players.Count; i++) {
            if (players[i].HasCalled()) {
                int compare = players[i].GetHand().IsBetterThan(BestHand);
                if (compare == 1)
                {
                    winners.Clear();
                    winners.Add(players[i]);
                    BestHand = players[i].GetHand();
                }
                else if (compare == 0)
                {
                    winners.Add(players[i]);
                }
            }
        }
        int prize_per_player = betting_manager.GetPot() / winners.Count;
        for (int w = 0; w < winners.Count; w++) {
            Debug.Log("Player " + winners[w].GetID().ToString() + " won " + prize_per_player.ToString());
            stack_manager.ModStack(winners[w].GetID(), prize_per_player);
        }
    }
}
