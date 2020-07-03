using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSpearmanAggro : MonoBehaviour
{
    private CircleCollider2D cc;
    public EnemyAI2 ai;
    private GoblinSpearman gs;


    void Start()
    {
        cc = GetComponent<CircleCollider2D>();
        ai = GetComponentInParent<EnemyAI2>();
        gs = GetComponentInParent<GoblinSpearman>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (ai.currentState == EnemyState.dead)
        {
            return;
        }
        else if (other.gameObject.tag == "Player")
        {
            ai.currentState = EnemyState.attack;
            gs.SendMessage("Throw");

        }


    }


}
