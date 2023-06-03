using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehavior : MonoBehaviour
{
    public enum ElementType { Water, Fire, Earth, Wind}
    public ElementType elementType; //Set when creating the prefab
    
    public float speed; //Set by the Spawner

    Rigidbody rb;
    Transform tf;

    Vector3 target;
    
    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform.position;
        rb = this.gameObject.GetComponent<Rigidbody>();
        tf = this.gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Director.Instance.gameState == Director.GameState.InProgress) //If the game is currently in progress, this monster will move towards the player, otherwise, it stands still (likely for endgame)
        {
            tf.position = Vector3.MoveTowards(tf.position, target, speed * Time.deltaTime);
        }
        else
        {
            tf.position = tf.position;
        }
    }

    private void OnCollisionEnter(Collision collision) //If the monster touches the player, tell the Director
    {
        if (collision.gameObject.tag == "Player")
        {
            Director.Instance.PlayerTookDamage();
        }
    }
}
