using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayoutManager : MonoBehaviour {

    public void RunPayout() {
        List<PokerPlayer> winners = new List<PokerPlayer>();
        Hand BestHand = new Hand(HandValue.None);
        for (int i = 0; i < GameManager.game_manager.GetNumRegistered(); i++) {
            if (GameManager.game_manager.AllPlayers[i].HasCalled()) {
                int compare = GameManager.game_manager.AllPlayers[i].GetHand().IsBetterThan(BestHand);
                if (compare == 1)
                {
                    winners.Clear();
                    winners.Add(GameManager.game_manager.AllPlayers[i]);
                    BestHand = GameManager.game_manager.AllPlayers[i].GetHand();
                }
                else if (compare == 0)
                {
                    winners.Add(GameManager.game_manager.AllPlayers[i]);
                }
            }
        }
        int prize_per_player = PotManager.pot_manager.GetPot() / winners.Count;
        for (int w = 0; w < winners.Count; w++) {
            Debug.Log("Player " + winners[w].GetID().ToString() + " won " + prize_per_player.ToString());
            GameManager.game_manager.AllPlayers[winners[w].GetID()].ModStack(prize_per_player);
        }
    }
}
