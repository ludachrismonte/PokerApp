using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableCardsManager : MonoBehaviour {

    public static TableCardsManager TCManager;

    public Card[] TableCards;
    public Image[] TableCardSprites;

    private void Awake()
    {
        if (TableCardsManager.TCManager == null)
        {
            TableCardsManager.TCManager = this;
        }
        else
        {
            if (TableCardsManager.TCManager != this)
            {
                Destroy(this.gameObject);
            }
        }
        TableCards = new Card[5];
    }

    public void SetTableCard(Card c, int which) {
        TableCards[which] = c;
        UpdateTableCardsUI();
    }

    public void ResetTableCards() {
        for (int i = 0; i < 5; i++) {
            TableCards[i] = Cards.cards.GetBlankCard();
        }
        UpdateTableCardsUI();
    }

    public void UpdateTableCardsUI()
    {
        for (int i = 0; i < 5; i++)
        {
            if (TableCards[i].String() == "NoneNone")
            {
                TableCardSprites[i].enabled = false;
            }
            else
            {
                TableCardSprites[i].enabled = true;
                TableCardSprites[i].sprite = SpriteManager.sprite_manager.GetSpriteByName(TableCards[i].String());
            }
        }
    }
}
