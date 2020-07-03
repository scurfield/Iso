using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIntBox : MonoBehaviour
{
    //Enemy Int Box is the collider range that a monster will automatically attack the player
    //This is different from the Player Int Box which is really only used to interact with NPCs, open doors, read signs, etc.



    public Vector2 offset;
    private Vector2 boxOffset;
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

    // Update is called once per frame
    public void UpdateOffset()
    {
        if (ai.lastHoriz != 0 && ai.lastVert != 0)
        {

            offset.x = (ai.lastHoriz) / 2.825f;
            offset.y = (ai.lastVert) / 2.825f;
            cc.offset = offset;
        }
        else
        {
            offset.x = (ai.lastHoriz) / 2;
            offset.y = (ai.lastVert) / 2;
            cc.offset = offset;
        }


        

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(ai.currentState != EnemyState.dead)
        {
            if (other.gameObject.tag == "Player")
            {
                if (!ph.isDead)
                {
                    playerInRange = true;
                    ai.currentState = EnemyState.attack;
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
