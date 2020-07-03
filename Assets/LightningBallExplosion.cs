using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBallExplosion : MonoBehaviour
{

    private int damage;
    private bool criticalHit;
    private GameObject player;
    private PlayerStats stats;
    private DamageType dt;
    private int baseDamage;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
        dt = DamageType.lightning;
        baseDamage = player.GetComponentInChildren<BallLightning>().baseDamage;
    }

    public void OnTriggerEnter2D(Collider2D other) //Deal damage when the ball itself hits an enemy
    {
        if (other.tag == "Enemy")
        {
            var ai = other.GetComponent<EnemyAI2>();
            //damage
            float critChance = Random.Range(0f, 1f);
            Debug.Log(critChance);
            if (critChance <= stats.criticalChance)
            {
                criticalHit = true;
            }
            else
            {
                criticalHit = false;
            }
            Debug.Log(baseDamage);
            var damageCalc = (baseDamage * 1.5f * stats.lightningDamage) / 100f;
            damage = Mathf.RoundToInt(damageCalc);
            Debug.Log("Sending " + damage);
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            enemy.TakeDamage(damage, dt, criticalHit, gameObject.transform.position);
            //knockback
            ai.Knockback(gameObject.transform.position);
        }
    }


    public void CallDestroy()
    {
        Destroy(gameObject);

    }


    
    //private LightningballController controller;
    //private int damage;
    //public GameObject hitSparks;
    //public GameObject finalExplosion;

    //void Start()
    //{
    //    controller = GetComponentInParent<LightningballController>();
    //}
    //public void OnTriggerEnter2D(Collider2D other) //Deal damage when the ball itself hits an enemy
    //{
    //    if (other.tag == "Enemy")
    //    {
    //        var ai = other.GetComponent<EnemyAI2>();
    //        if (ai.currentState == EnemyState.dead)
    //        {
    //            return;
    //        }
    //        //damage
    //        float critChance = Random.Range(0f, 1f);
    //        Debug.Log(critChance);
    //        if (critChance <= controller.stats.criticalChance)
    //        {
    //            controller.criticalHit = true;
    //        }
    //        else
    //        {
    //            controller.criticalHit = false;
    //        }
    //        Debug.Log(controller.baseDamage);
    //        var damageCalc = (controller.baseDamage * controller.stats.lightningDamage) / 100f;
    //        damage = Mathf.RoundToInt(damageCalc);
    //        Debug.Log("Sending " + damage);
    //        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
    //        enemy.TakeDamage(damage, controller.damageType, controller.criticalHit, gameObject.transform.position);
    //        if (enemy.currentHealth <= 0)
    //        {
    //            controller.finder.enemies.Remove(other.gameObject);
    //        }
    //        //sparks
    //        Instantiate(hitSparks, other.transform.position, Quaternion.identity);
    //        //hitcount
    //        if (controller.hitCount >= controller.maxHits)
    //        {
    //            Debug.Log("Max hits, exploding!");
    //            Instantiate(finalExplosion, other.transform.position, Quaternion.identity);
    //            controller.CallDestroy();
    //        }



    //    }

    //}
}
