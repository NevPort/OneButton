using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    public static Director Instance { get; private set; }

    public enum GameState { MainMenu, Tutorial, Pregame, InProgress, Endgame}
    public GameState gameState;

    public GameObject[] TutorialArray;

    public AudioClip[] MusicClipsArray;

    public AudioClip[] SfxClipsArray;

    public AudioSource MusicSource;
    public AudioSource SfxSource;

    public int playerLives;

    int tutorialPhase;

    public string timeSurvived;
    float secondsTimer;
    int minutesTimer;

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

        //MusicSource.clip = MusicClipsArray[0]; //Replace the clip in the Music Audio Source
        //MusicSource.Play();
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

        if (gameState == GameState.InProgress) //If the game is in progress, start timer
        {
            secondsTimer += Time.deltaTime;

            if (secondsTimer >= 60)
            {
                minutesTimer++;
                secondsTimer = 0;
            }

            if (secondsTimer < 10)
                timeSurvived = minutesTimer + ":0" + (int)secondsTimer;
            else
                timeSurvived = minutesTimer + ":" + (int)secondsTimer;
        }
    }

    public void SoundEffect(int audioClipIndex)
    {

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
            TutorialArray[0].gameObject.transform.parent.gameObject.SetActive(true); //Turn on the Tutorial parent gameobject

            if (PlayerInput.Instance.buttonInput == PlayerInput.ButtonInput.SingleClick)
            {
                if ((tutorialPhase += 1) >= TutorialArray.Length - 1) //If there's no more of the tutorial to continue with
                {
                    TutorialArray[tutorialPhase].SetActive(false); //Turn off final tutorial phase
                    
                    tutorialPhase = 0; //Reset tutorial phase (in case player needs to read it again)
                    
                    TutorialArray[tutorialPhase].SetActive(true); //Turn on first tutorial phase again

                    TutorialArray[0].gameObject.transform.parent.gameObject.SetActive(false); //Turn off the Tutorial parent gameobject

                    gameState = GameState.MainMenu; //Switch to MainMenu state
                }
                else
                {
                    TutorialArray[tutorialPhase].SetActive(false); //Turn off current tutorial phase
                    
                    tutorialPhase++; //Move to next phase
                    
                    TutorialArray[tutorialPhase].SetActive(true); //Turn on new tutorial phase
                }
            }
        }
        else if (gameState == GameState.Endgame) //Otherwise, if the game is in the end game menu
        {
            //Take out in-game HUD

            //Bring up end game menu

            if (PlayerInput.Instance.buttonInput == PlayerInput.ButtonInput.SingleClick)
            {
                //Take out end game menu

                //Replenish lives and timer
                playerLives = 3;
                secondsTimer = 0f;
                minutesTimer = 0;
                
                StartCoroutine(StartGame()); //Start game once again
            }
            else if (PlayerInput.Instance.buttonInput == PlayerInput.ButtonInput.DoubleClick)
            {
                SceneManager.LoadScene("MainScene", LoadSceneMode.Single); //Reload game (since restarting to main menu)
            }
        }
    }

    public IEnumerator StartGame() //For when the game starts from main menu
    {
        gameState = GameState.Pregame; //Switch to Pregame state

        yield return new WaitForSeconds(3f); //Delay for starting animations

        gameState = GameState.InProgress; //After intro, switch to InProgress state

        //MusicSource.clip = MusicClipsArray[1]; //Replace the clip in the Music Audio Source
        //MusicSource.Play(); //Play Battle music

        //Bring up in-game HUD

        //Start necessary methods and coroutines
        gameFlow.GameHasStarted();
        StartCoroutine(spawner.Progression());
        StartCoroutine(spawner.SpawnMonsters());
    }

    public void PlayerTookDamage() //When the player takes damage
    {
        playerLives--; //Remove 1 life

        if (playerLives <= 0) //If the player has lost all their lives
        {
            gameState = GameState.Endgame; //Switch to Endgame state
            MusicSource.Stop(); //Stop playing Battle music
        }
        else //Otherwise
        {
            spawner.KillMonster(true); //Tell Spawner to kill all monsters
        }
    }
}
