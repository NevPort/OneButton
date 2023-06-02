using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public static Director Instance { get; private set; }

    public enum GameState { InProgress}
    public GameState gameState;

    Spawner spawner;

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
        
        gameState = GameState.InProgress;

        StartCoroutine(spawner.SpawnMonsters());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
