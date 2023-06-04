using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    TMP_Text discoveringText;
    TMP_Text leftHandText;
    TMP_Text rightHandText;
    TMP_Text livesText;
    TMP_Text timeSurvivedText;
    
    // Start is called before the first frame update
    void Start()
    {
        discoveringText = GameObject.Find("Discovering Text").GetComponent<TMP_Text>();
        leftHandText = GameObject.Find("Left Hand Text").GetComponent<TMP_Text>();
        rightHandText = GameObject.Find("Right Hand Text").GetComponent<TMP_Text>();
        livesText = GameObject.Find("Lives Text").GetComponent<TMP_Text>();
        timeSurvivedText = GameObject.Find("Time Survived").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Director.Instance.gameState == Director.GameState.InProgress)
        {
            UpdateTexts();
        }
    }

    void UpdateTexts()
    {
        livesText.text = "Lives: " + Director.Instance.playerLives.ToString();
        timeSurvivedText.text = "Survived for: " + Director.Instance.timeSurvived;

        if (FindFirstObjectByType<GameFlow>().elementDiscovered == "")
            discoveringText.text = "";
        else if (FindFirstObjectByType<GameFlow>().elementDiscovered == "Searching...")
            discoveringText.text = FindFirstObjectByType<GameFlow>().elementDiscovered;
        else if (FindFirstObjectByType<GameFlow>().elementDiscovered != "Searching...") //If the GameFlow isn't searching for an element
            discoveringText.text = "Found: " + FindFirstObjectByType<GameFlow>().elementDiscovered + "\n*Click Once*";

        if (FindFirstObjectByType<GameFlow>().lefthand != "") //If the left hand isn't empty
            leftHandText.text = FindFirstObjectByType<GameFlow>().lefthand + "\n*Double-Click*";
        else
            leftHandText.text = FindFirstObjectByType<GameFlow>().lefthand;

        if (FindFirstObjectByType<GameFlow>().righthand != "") //If the right hand isn't empty
            rightHandText.text = FindFirstObjectByType<GameFlow>().righthand + "\n*Triple-Click*";
        else
            rightHandText.text = FindFirstObjectByType<GameFlow>().righthand;
    }
}
