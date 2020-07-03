using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballHitBox : MonoBehaviour
{
    public CircleCollider2D cc;
    private bool hit = false;
    private FireballController fbc;
    private int damage;
    private Fireball fireball;
    private GameObject player;
    private PlayerStats playerStats;
    private bool criticalHit;
    public DamageType damageType;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        fbc = GetComponentInParent<FireballController>();
        fireball = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Fireball>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" || other.tag == "Obstacle") //do not explode against the player please
        {
            fbc.exploding = true;
            fbc.StartCoroutine("Explode");
            if (other.tag == "Enemy")
            {
                float critChance = Random.Range(0f, 1f);
                Debug.Log(critChance);
                if (critChance <= playerStats.criticalChance)
                {
                    criticalHit = true;
                }
                else
                {
                    criticalHit = false;
                }
                damage = fireball.damage;
                Debug.Log("Sending " + damage);
                EnemyHealth enemy = other.GetComponent<EnemyHealth>();
                enemy.TakeDamage(damage, damageType, criticalHit, gameObject.transform.position);
            }

        }
    }
}
