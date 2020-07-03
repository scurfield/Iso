using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostballHitBox : MonoBehaviour
{
    public CircleCollider2D cc;
    private bool hit = false;
    private FrostballController fbc;
    private int damage;
    FrostOrbit frostball;
    private GameObject player;
    private PlayerStats ps;
    private bool criticalHit;
    public DamageType damageType;

    void Start()
    {
        fbc = GetComponentInParent<FrostballController>();
        player = GameObject.FindGameObjectWithTag("Player");
        frostball = player.GetComponentInChildren<FrostOrbit>();
        ps = player.GetComponent<PlayerStats>();
        damage = frostball.damage;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" || other.tag == "Obstacle") //do not explode against the player please
        {
            fbc.exploding = true;
            fbc.StartCoroutine("Explode");
            frostball.orbCount -= 1;
            if (other.tag == "Enemy")
            {
                float critChance = Random.Range(0f, 1f);
                if (critChance <= ps.criticalChance)
                {
                    criticalHit = true;
                }
                else
                {
                    criticalHit = false;
                }
                EnemyHealth enemy = other.GetComponent<EnemyHealth>();
                enemy.TakeDamage(damage, damageType, criticalHit, gameObject.transform.position);
            }

        }
    }
}
