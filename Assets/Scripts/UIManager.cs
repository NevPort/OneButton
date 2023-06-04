using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Sprite[] elementalSymbols = new Sprite[4];
    
    TMP_Text discoveringText;
    TMP_Text leftHandText;
    TMP_Text rightHandText;
    TMP_Text livesText;
    TMP_Text timeSurvivedText;

    Image discoveredImage;
    Image leftHandImage;
    Image rightHandImage;

    GameFlow gameFlow;
    
    // Start is called before the first frame update
    void Start()
    {
        gameFlow = FindFirstObjectByType<GameFlow>();

        discoveringText = GameObject.Find("Discovering Text").GetComponent<TMP_Text>();
        leftHandText = GameObject.Find("Left Hand Text").GetComponent<TMP_Text>();
        rightHandText = GameObject.Find("Right Hand Text").GetComponent<TMP_Text>();
        livesText = GameObject.Find("Lives Text").GetComponent<TMP_Text>();
        timeSurvivedText = GameObject.Find("Time Survived").GetComponent<TMP_Text>();

        discoveredImage = GameObject.Find("Discovered Image").GetComponent<Image>();
        leftHandImage = GameObject.Find("Left Hand").GetComponent<Image>();
        rightHandImage = GameObject.Find("Right Hand").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Director.Instance.gameState == Director.GameState.InProgress)
        {
            UpdateTexts();
            UpdateImages();
        }
    }

    void UpdateTexts()
    {
        livesText.text = "Lives: " + Director.Instance.playerLives.ToString();
        timeSurvivedText.text = "Survived for: " + Director.Instance.timeSurvived;

        if (gameFlow.elementDiscovered == "")
            discoveringText.text = "";
        else if (gameFlow.elementDiscovered == "Searching...")
            discoveringText.text = gameFlow.elementDiscovered;
        else if (gameFlow.elementDiscovered != "Searching...") //If the GameFlow isn't searching for an element
            discoveringText.text = "Found: " + gameFlow.elementDiscovered + "\n*Click Once*";

        if (gameFlow.lefthand != "") //If the left hand isn't empty
            leftHandText.text = gameFlow.lefthand + "\n*Double-Click*";
        else
            leftHandText.text = gameFlow.lefthand;

        if (gameFlow.righthand != "") //If the right hand isn't empty
            rightHandText.text = gameFlow.righthand + "\n*Triple-Click*";
        else
            rightHandText.text = gameFlow.righthand;
    }

    void UpdateImages()
    {
        //Element Discovered Image
        if (gameFlow.elementDiscovered == "Water")
        {
            discoveredImage.color = new Color(255, 255, 255, 255);
            discoveredImage.sprite = elementalSymbols[0];
        }
        else if (gameFlow.elementDiscovered == "Fire")
        {
            discoveredImage.color = new Color(255, 255, 255, 255);
            discoveredImage.sprite = elementalSymbols[1];
        }
        else if (gameFlow.elementDiscovered == "Earth")
        {
            discoveredImage.color = new Color(255, 255, 255, 255);
            discoveredImage.sprite = elementalSymbols[2];
        }
        else if (gameFlow.elementDiscovered == "Wind")
        {
            discoveredImage.color = new Color(255, 255, 255, 255);
            discoveredImage.sprite = elementalSymbols[3];
        }
        else
        {
            discoveredImage.color = new Color(0, 0, 0, 0);
            discoveredImage.sprite = null;
        }

        //Left Hand Image
        if (gameFlow.lefthand == "Water")
        {
            leftHandImage.color = new Color(255, 255, 255, 255);
            leftHandImage.sprite = elementalSymbols[0];
        }
        else if (gameFlow.lefthand == "Fire")
        {
            leftHandImage.color = new Color(255, 255, 255, 255);
            leftHandImage.sprite = elementalSymbols[1];
        }
        else if (gameFlow.lefthand == "Earth")
        {
            leftHandImage.color = new Color(255, 255, 255, 255);
            leftHandImage.sprite = elementalSymbols[2];
        }
        else if (gameFlow.lefthand == "Wind")
        {
            leftHandImage.color = new Color(255, 255, 255, 255);
            leftHandImage.sprite = elementalSymbols[3];
        }
        else
        {
            leftHandImage.color = new Color(255, 255, 255, 120);
            leftHandImage.sprite = null;
        }

        //Right Hand Image
        if (gameFlow.righthand == "Water")
        {
            rightHandImage.color = new Color(255, 255, 255, 255);
            rightHandImage.sprite = elementalSymbols[0];
        }
        else if (gameFlow.righthand == "Fire")
        {
            rightHandImage.color = new Color(255, 255, 255, 255);
            rightHandImage.sprite = elementalSymbols[1];
        }
        else if (gameFlow.righthand == "Earth")
        {
            rightHandImage.color = new Color(255, 255, 255, 255);
            rightHandImage.sprite = elementalSymbols[2];
        }
        else if (gameFlow.righthand == "Wind")
        {
            rightHandImage.color = new Color(255, 255, 255, 255);
            rightHandImage.sprite = elementalSymbols[3];
        }
        else
        {
            rightHandImage.color = new Color(255, 255, 255, 120);
            rightHandImage.sprite = null;
        }
    }
}
