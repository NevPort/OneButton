using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    public static Director Instance { get; private set; }

    public enum GameState { MainMenu, Tutorial, Pregame, InProgress, Endgame }
    public GameState gameState;

    public GameObject[] IntroArray;

    public AudioClip[] MusicClipsArray;

    public AudioClip[] SfxClipsArray;

    public AudioSource MusicSource;
    public AudioSource SfxSource;

    public int playerLives;

    public string timeSurvived;
    float secondsTimer;
    int minutesTimer;

    GameObject MainMenu;
    GameObject HUD;
    GameObject GameOver;

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

        MusicSource.clip = MusicClipsArray[0]; //Replace the clip in the Music Audio Source
        MusicSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        gameFlow = FindObjectOfType<GameFlow>();
        MainMenu = GameObject.Find("Main Menu");
        HUD = GameObject.Find("In-Game HUD");
        GameOver = GameObject.Find("Game Over");

        MainMenu.SetActive(true);
        HUD.SetActive(false);
        GameOver.SetActive(false);

        IntroArray[0].SetActive(true);
        IntroArray[1].SetActive(false);
        IntroArray[2].SetActive(false);

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

    public void SoundEffect(int audioClipIndex, float volume, bool oneShot)
    {
        if (oneShot == false)
        {
            SfxSource.clip = SfxClipsArray[audioClipIndex];
            SfxSource.volume = volume;
            SfxSource.Play();
        }
        else
            SfxSource.PlayOneShot(SfxClipsArray[audioClipIndex], volume);
    }

    void MenuNavigation()
    {
        if (gameState == GameState.MainMenu) //If the game is in the main menu
        {
            MainMenu.SetActive(true); //Bring up Main Menu

            if (PlayerInput.Instance.buttonInput == PlayerInput.ButtonInput.SingleClick) //If the player single clicks
            {
                StartCoroutine(StartGame()); //Start the game

                PlayerInput.Instance.buttonInput = PlayerInput.ButtonInput.NoInput; //Quickly set to No Input
            }
        }
        else if (gameState == GameState.Endgame) //Otherwise, if the game is in the end game menu
        {
            GameOver.SetActive(true);
            HUD.SetActive(false);

            if (PlayerInput.Instance.buttonInput == PlayerInput.ButtonInput.SingleClick)
            {
                SoundEffect(0, 1, false);
                SoundEffect(0, 1, false);

                SceneManager.LoadScene("MainScene", LoadSceneMode.Single); //Reload game (since restarting to main menu)
            }
        }
    }

    public IEnumerator StartGame() //For when the game starts from main menu
    {
        gameState = GameState.Pregame; //Switch to Pregame state

        MusicSource.Stop(); //Stop music
        SoundEffect(11, 0.5f, false); //Explosion sfx

        IntroArray[0].SetActive(false);
        IntroArray[1].SetActive(true);
        IntroArray[2].SetActive(true);

        yield return new WaitForSeconds(6f); //Delay for starting animations

        gameState = GameState.InProgress; //After intro, switch to InProgress state

        MusicSource.clip = MusicClipsArray[1]; //Replace the clip in the Music Audio Source
        MusicSource.Play(); //Play Battle music

        MainMenu.SetActive(false);
        HUD.SetActive(true);

        //Start necessary methods and coroutines
        gameFlow.GameHasStarted();
        StartCoroutine(spawner.Progression());
        StartCoroutine(spawner.SpawnMonsters());
    }

    public void PlayerTookDamage() //When the player takes damage
    {
        playerLives--; //Remove 1 life
        SoundEffect(10, 1f, true); //Taking Damage sfx

        if (playerLives <= 0) //If the player has lost all their lives
        {
            gameState = GameState.Endgame; //Switch to Endgame state
            MusicSource.Stop(); //Stop playing Battle music
            SoundEffect(11, 0.5f, false); //Explosion sfx
        }
        else //Otherwise
        {
            spawner.KillMonster(true); //Tell Spawner to kill all monsters
        }
    }
}
