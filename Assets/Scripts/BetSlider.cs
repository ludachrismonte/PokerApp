using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetSlider : MonoBehaviour {

    private Player player;
    private Slider slider;

    private Text BetText;

    private int MaxValue;

	// Use this for initialization
	void Start () {
        player = transform.parent.transform.parent.
            transform.parent.GetComponent<Player>();
        slider = GetComponent<Slider>();
        BetText = transform.Find("BetAmountText").GetComponent<Text>();
        slider.maxValue = player.GetStack();
        OnValueChange();
    }
	
	// Update is called once per frame
	void Update () {
        slider.maxValue = player.GetStack();
        BetText.text = slider.value.ToString() + " / " + slider.maxValue.ToString();
    }

    public void OnValueChange() {
        player.SetMyBet((int) slider.value);
    }
}
