using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs = new GameObject[4]; //An array containing the prefabs of each type of monster elemental
    
    public List<GameObject> monstersInGame = new List<GameObject>(); //List keeps track of monsters that are currently alive

    public float spawnRate;
    public float monsterSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnMonsters()
    {
        int r = Random.Range(0, monsterPrefabs.Length); //Choose a random monster from the monsterPrefabs array
        
        GameObject monsterClone = Instantiate(monsterPrefabs[r], this.gameObject.transform.position, Quaternion.identity); //Instantiate a clone of the monster where this spawner is
        monstersInGame.Add(monsterClone); //Add the monster to the monstersInGame list
        monsterClone.GetComponent<MonsterBehavior>().speed = monsterSpeed; //Give the monster the speed needed
        
        yield return new WaitForSeconds(spawnRate); //Wait for as long as the spawnRate value is
        
        if (Director.Instance.gameState == Director.GameState.InProgress) //If the game is currently in progress, loop this coroutine
        {
            StartCoroutine(SpawnMonsters());
        }
    }

    public void KillMonster(bool allMonsters) //Function takes care of killing monsters, if false then only front monster, if true then all monsters
    {
        if (allMonsters)
        {
            for (int x = 0; x < monstersInGame.Count; x++)
            {
                Destroy(monstersInGame[x]);

                if (x >= monstersInGame.Count - 1)
                {
                    monstersInGame.Clear();
                }
            }
        }
        else
        {
            Destroy(monstersInGame[0]);
            monstersInGame.RemoveAt(0);
        }
    }
}
