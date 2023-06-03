using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public static Director Instance { get; private set; }

    public enum GameState { MainMenu, Tutorial, Pregame, InProgress, Endgame}
    public GameState gameState;

    public int playerLives;

    int tutorialPhase;

    Spawner spawner;
    GameFlow gameFlow;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        gameFlow = FindObjectOfType<GameFlow>();
        
        gameState = GameState.MainMenu; //Start GameState on MainMenu state
    }

    // Update is called once per frame
    void Update()
    {
        MenuNavigation();
    }

    void MenuNavigation()
    {
        if (gameState == GameState.MainMenu) //If the game is in the main menu
        {
            if (PlayerInput.Instance.buttonInput == PlayerInput.ButtonInput.SingleClick) //If the player single clicks
            {
                StartCoroutine(StartGame()); //Start the game
                PlayerInput.Instance.buttonInput = PlayerInput.ButtonInput.NoInput; //Quickly set to No Input
            }
            else if (PlayerInput.Instance.buttonInput == PlayerInput.ButtonInput.DoubleClick) //Otherwise, if the player double clicks
            {
                gameState = GameState.Tutorial; //Switch to Tutorial state
                PlayerInput.Instance.buttonInput = PlayerInput.ButtonInput.NoInput; //Quickly set to No Input
            }
        }
        else if (gameState == GameState.Tutorial) //Otherwise, if the game is in the tutorial
        {
            //Bring up tutorial parent

            if (PlayerInput.Instance.buttonInput == PlayerInput.ButtonInput.SingleClick)
            {
                /* 
                if ((tutorialPhase += 1) >= TutorialArray.Length - 1)
                {
                    TutorialArray[tutorialPhase].SetActive(false); //Turn off final tutorial phase
                    
                    tutorialPhase = 0; //Reset tutorial phase (in case player needs to read it again)
                    
                    TutorialArray[tutorialPhase].SetActive(true); //Turn back on first tutorial phase (to prepare for tutorial reread)
                    
                    //Turn off tutorial parent
                    
                    gameState = GameState.MainMenu; //Switch to MainMenu state
                }
                else
                {
                    TutorialArray[tutorialPhase].SetActive(false); //Turn off current tutorial phase
                    
                    tutorialPhase++; //Move to next phase
                    
                    TutorialArray[tutorialPhase].SetActive(true); //Turn on new tutorial phase
                }
                 
                 */
            }
        }
        else if (gameState == GameState.Endgame) //Otherwise, if the game is in the end game menu
        {
            //Bring up end game menu
        }
    }

    public IEnumerator StartGame() //For when the game starts from main menu
    {
        gameState = GameState.Pregame; //Switch to Pregame state

        yield return new WaitForSeconds(3f); //Delay for starting animations

        gameState = GameState.InProgress; //After intro, switch to InProgress state

        //Start necessary coroutines
        StartCoroutine(gameFlow.SearchingForElements());
        StartCoroutine(spawner.SpawnMonsters());
    }

    public void PlayerTookDamage() //When the player takes damage
    {
        playerLives--; //Remove 1 life

        if (playerLives <= 0) //If the player has lost all their lives
        {
            gameState = GameState.Endgame; //Switch to Endgame state
        }
        else //Otherwise
        {
            spawner.KillMonster(true); //Tell Spawner to kill all monsters
        }
    }
}
