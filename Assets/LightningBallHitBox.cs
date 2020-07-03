using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class LightningBallHitBox : MonoBehaviour
{
    private LightningballController controller;
    private int damage;
    public GameObject hitSparks;
    public GameObject finalExplosion;

    void Start()
    {
        controller = GetComponentInParent<LightningballController>();
    }
    public void OnTriggerEnter2D(Collider2D other) //Deal damage when the ball itself hits an enemy
    {
        if (other.tag == "Enemy")
        {
            var ai = other.GetComponent<EnemyAI2>();
            if (ai.currentState == EnemyState.dead)
            {
                return;
            }
            //damage
            float critChance = Random.Range(0f, 1f);
            Debug.Log(critChance);
            if (critChance <= controller.stats.criticalChance)
            {
                controller.criticalHit = true;
            }
            else
            {
                controller.criticalHit = false;
            }
            Debug.Log(controller.baseDamage);
            var damageCalc = (controller.baseDamage * controller.stats.lightningDamage) / 100f;
            damage = Mathf.RoundToInt(damageCalc);
            Debug.Log("Sending " + damage);
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            enemy.TakeDamage(damage, controller.damageType, controller.criticalHit, gameObject.transform.position);
            if(enemy.currentHealth <= 0)
            {
                controller.finder.enemies.Remove(other.gameObject);
            }
            //sparks
            Instantiate(hitSparks, other.transform.position, Quaternion.identity);
            //hitcount
            if (controller.hitCount >= controller.maxHits)
            {
                Debug.Log("Max hits, exploding!");
                Instantiate(finalExplosion, other.transform.position, Quaternion.identity);
                Destroy(controller.gameObject);
            }



        }

    }
}
