using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public enum ButtonInput { NoInput, Hold, SingleClick, DoubleClick}
    public ButtonInput buttonInput;
    
    int buttonPress;
    public float buttonHold;

    // Start is called before the first frame update
    void Start()
    {
        buttonInput = ButtonInput.NoInput;
    }

    // Update is called once per frame
    void Update()
    {
        ReceiveButtonInput();
    }

    void ReceiveButtonInput()
    {
        if (Input.GetKey(KeyCode.Space)) //True for as long as Space is held
        {
            buttonHold += Time.deltaTime; //Increase buttonHold float w/ time
        }

        if (Input.GetKeyDown(KeyCode.Space)) //True for the first frame Space is pressed
        {
            buttonPress++; //Raise buttonPress by one
        }

        if (Input.GetKeyUp(KeyCode.Space)) //True for the first frame Space is released
        {
            StartCoroutine(ButtonReset()); //Start ButtonTapReset coroutine
        }
    }

    IEnumerator ButtonReset()
    {
        yield return new WaitForSeconds(0.2f); //Wait the amount of seconds before continuing
        
        StartCoroutine(ButtonFunction(buttonHold, buttonPress));

        buttonPress = 0;
        buttonHold = 0;
    }

    IEnumerator ButtonFunction(float buttonHeld, int buttonTapped)
    {
        if (buttonHeld < 0.3f) //If the button was held for less than the amount of time, then it was tapped
        {
            if (buttonTapped == 1) //If the button was tapped once, it's a single click
            {
                buttonInput = ButtonInput.SingleClick;
            }
            else if (buttonTapped >= 2) //If the button was tapped twice, it's a double click
            {
                buttonInput = ButtonInput.DoubleClick;
            }
        }
        else //Otherwise, the button was held
        {
            buttonInput = ButtonInput.Hold;
        }

        yield return new WaitForSeconds(0.1f); //After a very short delay, reset buttonInput to have no input

        buttonInput = ButtonInput.NoInput;
    }
}
