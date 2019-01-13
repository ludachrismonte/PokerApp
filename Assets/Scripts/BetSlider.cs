using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetSlider : MonoBehaviour {

    public PokerPlayer player;
    private Slider slider;

    private Text BetText;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Use this for initialization
    void Start () {
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

    public void SetValueTo(int value) {
        if (value < slider.maxValue) {
            slider.value = value;
        }
        else slider.value = slider.maxValue;
    }
}
