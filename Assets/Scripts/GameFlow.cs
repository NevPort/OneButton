using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameFlow : MonoBehaviour
{
    PlayerInput playerInput;
    
    string[] Elements = new string[] { "Water", "Fire", "Earth", "Wind"}; //Water (douses) > Fire (burns) > Earth (blocks) > Wind (blows)
    public string elementDiscovered;
    int cycledElement;
    
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
        
    }

    IEnumerator SearchingForElements()
    {
        elementDiscovered = "Searching...";
        
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

        yield return new WaitForSeconds(1f); //Discovery Phase (for 1 second)

        StartCoroutine(SearchingForElements()); //Loop this coroutine
    }
}
