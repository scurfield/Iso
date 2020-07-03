using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    //most of this script will get rewritten. This one is to measure whether the player is too close, then the enemy will run away. THis entire script might get taken out if all of it can be done through the AI script.




    //Enemy Int Box is the collider range that a monster will automatically attack the player
    //This is different from the Player Int Box which is really only used to interact with NPCs, open doors, read signs, etc.




    public EnemyAI2 ai;
    private CircleCollider2D cc;
    public bool playerInRange = false;
    GameObject player;
    PlayerHealth ph;





    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponentInParent<EnemyAI2>();
        cc = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        ph = player.GetComponent<PlayerHealth>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (ai.currentState != EnemyState.dead)
        {
            if (other.gameObject.tag == "Player")
            {
                if (!ph.isDead)
                {
                    playerInRange = true;
                    //if (!ai.tooClose)
                    //{
                    //    ai.currentState = EnemyState.attack;

                    //}
                }


            }

        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

}
