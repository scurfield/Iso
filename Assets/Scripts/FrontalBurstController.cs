using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontalBurstController : MonoBehaviour
{
    private BoxCollider2D bc;
    public int damage;
    private GameObject parent;
    private EnemyAI2 ai;

    public DamageType dt;

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        ai = GetComponentInParent<EnemyAI2>();
        parent = ai.gameObject;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            player.TakeDamage(damage, dt);
            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            pm.Knockback(parent.transform.position);
        }

    }
    



    public void EndAnim()
    {
        Destroy(gameObject);
    }


}
