using System.Collections;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    string[] Elements = new string[] { "Water", "Fire", "Earth", "Wind" }; //Water (douses) > Fire (burns) > Earth (blocks) > Wind (blows)
    public string elementDiscovered;
    int cycledElement;
    float cycleRate;

    bool discoveredAnElement;

    bool stopSearch;

    public string lefthand;
    public string righthand;
    bool rightHandIsUsed;

    Animator ProtagAnimator;

    // Start is called before the first frame update
    void Start()
    {
        ProtagAnimator = GameObject.Find("Protag_Anim").GetComponent<Animator>();

        cycleRate = 1.1f; 
    }

    public void GameHasStarted()
    {
        stopSearch = false;
        lefthand = "";
        righthand = "";
        cycledElement = 3; //cycledElement value starts at 3 so that it can cycle back to the first value in the Elements array at the beginning of the game

        StartCoroutine(SearchingForElements());
    }

    // Update is called once per frame
    void Update()
    {
        if (Director.Instance.gameState == Director.GameState.InProgress)
        {
            CollectElement();
            UseElement();
        }
        else if (Director.Instance.gameState == Director.GameState.Endgame)
        {
            stopSearch = true;
            elementDiscovered = "";
            discoveredAnElement = false;
            lefthand = "";
            righthand = "";
            StopAllCoroutines();
        }
    }

    public IEnumerator SearchingForElements() //Step 1 of game flow
    {
        ProtagAnimator.SetBool("Searching", true); //Activates the Searching animation

        Director.Instance.SoundEffect(1, 0.5f, false); //Rummaging sfx
        
        elementDiscovered = "Searching...";
        discoveredAnElement = false;

        yield return new WaitForSeconds(cycleRate); //Searching Phase

        if ((cycledElement + 1) >= Elements.Length) //If the cycledElement value + 1 would go over the length of the Elements array
        {
            cycledElement = 0; //Cycle it back to 0
        }
        else if ((cycledElement + 1) < Elements.Length) //Otherwise, if the cycledElement value + 1 would be less than the length
        {
            cycledElement++; //Add 1 to the cycledElement value
        }

        elementDiscovered = Elements[cycledElement];

        discoveredAnElement = true;

        int s = Random.Range(2, 4 + 1); //Choose a random Discover sfx
        Director.Instance.SoundEffect(s, 0.5f, false); //Rummaging sfx

        yield return new WaitForSeconds(cycleRate); //Discovery Phase

        elementDiscovered = "";
        discoveredAnElement = false;

        yield return new WaitWhile(() => stopSearch); //Coroutine pauses until stopSearch is no longer true

        StartCoroutine(SearchingForElements()); //Loop this coroutine
    }

    void CollectElement() //Step 2 of game flow
    {
        if (discoveredAnElement) //If the player discovered an element during the Searching phase
        {
            if (PlayerInput.Instance.buttonInput == PlayerInput.ButtonInput.SingleClick) //If the player clicked once
            {
                Director.Instance.SoundEffect(0, 1f, false); //Button Click sfx
                
                if (lefthand == "") //If the left hand is empty
                {
                    lefthand = elementDiscovered; //Element goes on left hand

                    PlayerInput.Instance.buttonInput = PlayerInput.ButtonInput.NoInput; //Instantly set to NoInput for quick feedback
                }
                else if (lefthand != "") //If the left hand is full
                {
                    righthand = lefthand; //Old element moves from left to right hand
                    lefthand = elementDiscovered; //New element goes on left hand

                    PlayerInput.Instance.buttonInput = PlayerInput.ButtonInput.NoInput; //Instantly set to NoInput for quick feedback
                }
            }
        }

        if (lefthand != "" && righthand != "") //If both hands are full
        {
            ProtagAnimator.SetBool("Searching", false); //Goes back to Idle animation

            //Stop search
            stopSearch = true;
            discoveredAnElement = false;
            elementDiscovered = "";
        }
        else
        {
            stopSearch = false;
        }

    }

    void UseElement() //Step 3 of game flow
    {
        if (lefthand != "") //If the player's left hand is full
        {
            if (PlayerInput.Instance.buttonInput == PlayerInput.ButtonInput.DoubleClick) //If the player clicks twice
            {
                ProtagAnimator.SetTrigger("Attack"); //Activate the Attack animation
                
                rightHandIsUsed = false; //The left hand is being used
                CheckElementMatching();

                if (CheckElementMatching()) //If the element used is good for killing the monster, kill them
                {
                    FindObjectOfType<Spawner>().KillMonster(false);
                }

                lefthand = ""; //Consume the item

                PlayerInput.Instance.buttonInput = PlayerInput.ButtonInput.NoInput;
            }
        }
        if (righthand != "") //If the player's right hand is full
        {
            if (PlayerInput.Instance.buttonInput == PlayerInput.ButtonInput.TripleClick) //If the player clicks trice
            {
                ProtagAnimator.SetTrigger("Attack"); //Activate the Attack animation

                rightHandIsUsed = true; //The right hand is being used
                CheckElementMatching();

                if (CheckElementMatching()) //If the element used is good for killing the monster, kill them
                {
                    FindObjectOfType<Spawner>().KillMonster(false);
                }

                righthand = ""; //Consume the item

                PlayerInput.Instance.buttonInput = PlayerInput.ButtonInput.NoInput;
            }
        }
    }

    bool CheckElementMatching()
    {
        MonsterBehavior firstMonster = FindObjectOfType<Spawner>().monstersInGame[0].GetComponent<MonsterBehavior>(); //Get reference to the code of the first monster

        //If the element the player has is able to overcome the element of the monster, then it's able to kill it. Otherwise, it can't
        if (rightHandIsUsed == false)
        {
            if (lefthand == "Water" && firstMonster.elementType.ToString() == "Fire")
            {
                Director.Instance.SoundEffect(6, 0.5f, true); //Water Attack sfx
                return true;
            }
            else if (lefthand == "Fire" && firstMonster.elementType.ToString() == "Earth")
            {
                Director.Instance.SoundEffect(7, 0.5f, true); //Fire Attack sfx
                return true;
            }
            else if (lefthand == "Earth" && firstMonster.elementType.ToString() == "Wind")
            {
                Director.Instance.SoundEffect(8, 0.5f, true); //Earth Attack sfx
                return true;
            }
            else if (lefthand == "Wind" && firstMonster.elementType.ToString() == "Water")
            {
                Director.Instance.SoundEffect(9, 0.5f, true); //Wind Attack sfx
                return true;
            }
            else
            {
                Director.Instance.SoundEffect(5, 0.5f, true); //Failed Attack sfx
                return false;
            }
        }
        else
        {
            if (righthand == "Water" && firstMonster.elementType.ToString() == "Fire")
            {
                Director.Instance.SoundEffect(6, 0.5f, true); //Water Attack sfx
                return true;
            }
            else if (righthand == "Fire" && firstMonster.elementType.ToString() == "Earth")
            {
                Director.Instance.SoundEffect(7, 0.5f, true); //Fire Attack sfx
                return true;
            }
            else if (righthand == "Earth" && firstMonster.elementType.ToString() == "Wind")
            {
                Director.Instance.SoundEffect(8, 0.5f, true); //Earth Attack sfx
                return true;
            }
            else if (righthand == "Wind" && firstMonster.elementType.ToString() == "Water")
            {
                Director.Instance.SoundEffect(9, 0.5f, true); //Wind Attack sfx
                return true;
            }
            else
            {
                Director.Instance.SoundEffect(5, 0.5f, true); //Failed Attack sfx
                return true;
            }
        }
    }
}
