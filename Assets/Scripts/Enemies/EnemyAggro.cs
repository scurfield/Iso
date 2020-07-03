using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    public EnemyAI2 ai;
    private CircleCollider2D cc;
    private List<Collider2D> multiPull;
    public LayerMask enemyLayers;
    public float multiPullRange;
    public Transform center;


    void Start()
    {
        ai = GetComponentInParent<EnemyAI2>();
        cc = GetComponent<CircleCollider2D>();

    }

    public void Aggro()//called in void below to multipull enemies;
    {
        ai.currentState = EnemyState.aggro;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (ai.currentState == EnemyState.dead)
        {
            return;
        }
            else if (other.gameObject.tag == "Player")
            {
                ai.following = true;
                ai.currentState = EnemyState.aggro;
                ai.UpdatePath();

                multiPull = new List<Collider2D>(Physics2D.OverlapCircleAll(center.position, multiPullRange, enemyLayers)); //Check which enemies are within his swarm range
                foreach (Collider2D enemy in multiPull)
                {
                SendMessage("Aggro");
                
                }

        }


    }
    private void OnDrawGizmosSelected()             //Draw the anit-swarm range for calculations
    {
        if (center.position == null)
            return;
        Gizmos.DrawWireSphere(center.position, multiPullRange);
    }
}
