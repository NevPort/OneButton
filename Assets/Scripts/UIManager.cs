using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    TMP_Text discoveringText;
    TMP_Text leftHandText;
    
    // Start is called before the first frame update
    void Start()
    {
        discoveringText = GameObject.Find("Discovering Text").GetComponent<TMP_Text>();
        leftHandText = GameObject.Find("Left Hand Text").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTexts();
    }

    void UpdateTexts()
    {
        discoveringText.text = FindFirstObjectByType<GameFlow>().elementDiscovered;
        leftHandText.text = FindFirstObjectByType<GameFlow>().hand;
    }
}