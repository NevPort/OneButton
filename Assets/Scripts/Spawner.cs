using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //[0] Water [1] Fire [2] Earth [3] Wind
    public GameObject[] monsterPrefabs = new GameObject[4]; //An array containing the prefabs of each type of monster elemental
    public List<GameObject> monstersToSpawn = new List<GameObject>(); //An array containing monsters to spawn (to limit overflow of singular monster type)
    
    public List<GameObject> monstersInGame = new List<GameObject>(); //List keeps track of monsters that are currently alive

    float progressionRate;
    float spawnRate;
    float monsterSpeed;

    GameObject monsterTempRemoved;
    string pastMonsterType;
    int spawnStreak;
    
    // Start is called before the first frame update
    void Start()
    {
        monstersToSpawn.AddRange(monsterPrefabs);
    }

    // Update is called once per frame
    void Update()
    {
        if (Director.Instance.gameState == Director.GameState.Pregame)
        {
            //Starting values
            progressionRate = 16f; //How often should a progressional change occur (in seconds)
            spawnRate = 5.2f; //How often should a new monster spawn (in seconds)
            monsterSpeed = 1.25f; //What should the new monster's speed be

            pastMonsterType = "";
            spawnStreak = 1;
        }
    }

    public IEnumerator Progression()
    {
        yield return new WaitForSeconds(progressionRate);

        spawnRate -= 0.2f;
        monsterSpeed -= 0.05f;
        Debug.Log("Spawn Rate: " + spawnRate + " | Monster Speed: " + monsterSpeed);

        if (spawnRate <= 2.2 || monsterSpeed <= 0.5) //If spawn rate or monster speed get too low
        {
            //Stop looping progression (should happen in 4 minutes)
        }
        else
        {
            StartCoroutine(Progression());
        }
    }

    void PreventSpawnSpam(GameObject monster, int listPos) //Used to prevent spawner from spawning the same type of monster a lot
    {
        string newMonsterType = monster.GetComponent<MonsterBehavior>().elementType.ToString();

        if (newMonsterType == pastMonsterType) //If the new monster is the same as the past one
        {
            spawnStreak++; //Increase spawnStreak

            if (spawnStreak >= 2) //If spawnStreak reaches 3 or more (meaning the spawner spawned the same monster 3 times)
            {
                monsterTempRemoved = monstersToSpawn[listPos]; //Save the monster that will be temporarily removed
                monstersToSpawn.RemoveAt(listPos); //Remove the monster from the list
                Debug.Log("Temporarily removed monster to spawn");
            }
        }
        else if (newMonsterType != pastMonsterType) //Otherwise, if the new monster is different from the past one
        {
            spawnStreak = 1; //Reset spawnStreak
            pastMonsterType = newMonsterType;

            if (monstersToSpawn.Count < 4) //If the monstersToSpawn count is less than 4 (meaning a monster was temporarily removed)
            {
                monstersToSpawn.Add(monsterTempRemoved); //Put the removed monster back into the list
                Debug.Log("Placed monster back to spawn");
            }
        }
    }

    public IEnumerator SpawnMonsters()
    {
        int r = Random.Range(0, monstersToSpawn.Count); //Choose a random monster
        
        GameObject monsterClone = Instantiate(monstersToSpawn[r], this.gameObject.transform.position, monstersToSpawn[r].transform.rotation); //Instantiate a clone of the monster where this spawner is
        monstersInGame.Add(monsterClone); //Add the monster to the monstersInGame list
        monsterClone.GetComponent<MonsterBehavior>().speed = monsterSpeed; //Give the monster the speed needed

        PreventSpawnSpam(monsterClone, r);
        
        yield return new WaitForSeconds(spawnRate); //Wait for as long as the spawnRate value is
        
        if (Director.Instance.gameState == Director.GameState.InProgress) //If the game is currently in progress, loop this coroutine
        {
            StartCoroutine(SpawnMonsters());
        }
        else if (Director.Instance.gameState == Director.GameState.Endgame) //Otherwise, if the game has ended, kill all monsters and stop loop
        {
            KillMonster(true);
            StopAllCoroutines();
        }
    }

    public void KillMonster(bool allMonsters) //Function takes care of killing monsters, if false then only front monster, if true then all monsters
    {
        if (allMonsters)
        {
            for (int x = 0; x < monstersInGame.Count; x++)
            {
                monstersInGame[x].GetComponent<ParticleSystem>().Play();
                Destroy(monstersInGame[x], 0.4f);

                if (x >= monstersInGame.Count - 1)
                {
                    monstersInGame.Clear();
                }
            }
        }
        else
        {
            monstersInGame[0].GetComponent<ParticleSystem>().Play();
            Destroy(monstersInGame[0], 0.4f);
            monstersInGame.RemoveAt(0);
        }
    }
}
