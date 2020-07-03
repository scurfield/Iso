using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLeaveAggro : MonoBehaviour
{
    public EnemyAI2 ai;
    private CircleCollider2D cc;

    void Start()
    {
        ai = GetComponentInParent<EnemyAI2>();
        cc = GetComponent<CircleCollider2D>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (ai.currentState != EnemyState.dead)
        {
                if (other.gameObject.tag == "Player")
            {
                ai.following = false;
                ai.currentState = EnemyState.retreating;
                ai.UpdatePath();

            }

        }
    }
}