using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameFlow : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    int buttonPress;
    float buttonHold;
    TMP_Text pressTypeIndicator;
    Image pressIndicator;

    // Start is called before the first frame update
    void Start()
    {
        pressTypeIndicator = GameObject.Find("Press Input Type Text").GetComponent<TMP_Text>();
        pressIndicator = GameObject.Find("Press Indicator").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ButtonInput();
    }

    void ButtonInput()
    {
        if (Input.GetKey(KeyCode.Space)) //True for as long as Space is held
        {
            buttonHold += Time.deltaTime; //Increase buttonHold float w/ time
        }

        if (Input.GetKeyDown(KeyCode.Space)) //True for the first frame Space is pressed
        {
            buttonPress++; //Raise buttonPress by one
            pressIndicator.color = Color.green;
        }

        if (Input.GetKeyUp(KeyCode.Space)) //True for the first frame Space is released
        {
            StartCoroutine(ButtonReset()); //Start ButtonTapReset coroutine

            pressIndicator.color = Color.white;
        }
    }

    IEnumerator ButtonReset()
    {
        yield return new WaitForSeconds(0.25f); //Wait the amount of seconds before continuing

        ButtonFunction(buttonHold, buttonPress);

        buttonPress = 0;
        buttonHold = 0;
    }

    void ButtonFunction(float buttonHeld, int buttonTapped)
    {
        if (buttonHeld < 0.45f) //If the button was held for less than the amount of time, then it was tapped
        {
            if (buttonTapped == 1) //If the button was tapped once, it's a single click; twice, double click; etc.
            {
                pressTypeIndicator.text = "Single Click";
            }
            else if (buttonTapped == 2)
            {
                pressTypeIndicator.text = "Double Click";
            }
            else if (buttonTapped == 3)
            {
                pressTypeIndicator.text = "Triple Click";
            }
        }
        else
        {
            pressTypeIndicator.text = "Held for " + buttonHeld.ToString();
        }
    }
    */
}
