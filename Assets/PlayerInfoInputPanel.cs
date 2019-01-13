using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoInputPanel : MonoBehaviour {

    public InputField NameField;
    public InputField StackField;
    public GameObject SubmitButton;

    public PokerPlayer MyPlayer;

    public void OnSubmit() {
        string name = NameField.text;
        string stack_string = StackField.text;
        int stack_int;
        bool isNumeric = int.TryParse(stack_string, out stack_int);
        if (name == "" || !isNumeric) {
            return;
        }
        else MyPlayer.SitDown(name, stack_int);
        gameObject.SetActive(false);
    }
}
