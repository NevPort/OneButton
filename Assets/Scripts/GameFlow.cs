using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    PlayerInput playerInput;
    
    string[] Elements = new string[] { "Water", "Fire", "Earth", "Wind"}; //Water (douses) > Fire (burns) > Earth (blocks) > Wind (blows)
    public string elementDiscovered;
    int cycledElement;

    bool discoveredAnElement;

    bool stopSearch;

    public string hand;
    
    // Start is called before the first frame update
    void Start()
    {
        playerInput = FindFirstObjectByType<PlayerInput>(); //Reference to PlayerInput script

        cycledElement = 3; //cycledElement value starts at 3 so that it can cycle back to the first value in the Elements array at the beginning of the game

        StartCoroutine(SearchingForElements());
    }

    // Update is called once per frame
    void Update()
    {
        CollectElement();
        UseElement();
    }

    void DetectChangedInput()
    {
        if (playerInput.buttonInput != PlayerInput.ButtonInput.NoInput) //If the buttonInput is something other than No Input, then an input has been detected
        {
            
        }
    }

    IEnumerator SearchingForElements() //Step 1 of game flow
    {
        elementDiscovered = "Searching...";
        discoveredAnElement = false;
        
        yield return new WaitForSeconds(1f); //Searching Phase (for 1 second)

        if ((cycledElement + 1) >= Elements.Length) //If the cycledElement value + 1 would go over the length of the Elements array
        {
            cycledElement = 0; //Cycle it back to 0
        }
        else if ((cycledElement + 1) < Elements.Length) //Otherwise, if the cycledElement value + 1 would be less than the length
        {
            cycledElement++; //Add 1 to the cycledElement value
        }

        elementDiscovered = Elements[cycledElement];
        Debug.Log(elementDiscovered);

        discoveredAnElement = true;

        yield return new WaitForSeconds(1f); //Discovery Phase (for 1 second)

        if (stopSearch) //If CollectElement() sets stopSearch to true
        {
            stopSearch = false; //Don't loop and reset stopSearch
        }
        else //Otherwise, if stopSearch is false
        {
            StartCoroutine(SearchingForElements()); //Loop this coroutine
        }
    }

    void CollectElement() //Step 2 of game flow
    {
        if (discoveredAnElement) //If the player discovered an element during the Searching phase
        {
            if (playerInput.buttonInput == PlayerInput.ButtonInput.SingleClick) //If the player clicked once
            {
                hand = elementDiscovered;

                stopSearch = true; //This bool prevents the SearchingForElements coroutine from looping (since it wouldn't stop on StopCoroutine())
                discoveredAnElement = false;
                elementDiscovered = "";
            }
        }
    }

    void UseElement() //Step 3 of game flow
    {
        if (hand != "") //If the player's hand isn't empty
        {
            if (playerInput.buttonInput == PlayerInput.ButtonInput.DoubleClick) //If the player clicks twice
            {
                hand = ""; //Consume the item

                StartCoroutine(SearchingForElements());
            }
        }
    }
}
