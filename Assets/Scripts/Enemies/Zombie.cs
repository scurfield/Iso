using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private EnemyAI2 ai;
    public int attackGrabDmg;
    private DamageType dt;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<EnemyAI2>();
        dt = ai.dt;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        StartCoroutine(AttackZombieGrab());
    }

    public void AttackGrabDamage() //called by animator on a specific frame
    {
        //Debug.Log("AttackGrabDamage called");
        //Debug.Break();
        if (ai.enemyIntBox.playerInRange)
        {
            //deal damage
            if (ai.playerHealth.currentHealth > 0)
            {
                ai.playerHealth.TakeDamage(attackGrabDmg, dt);
            }
        }
        else
        {
            return;
        }
    }

    public IEnumerator AttackZombieGrab()
    {

        if (!ai.attacking)
        {
            ai.anim.speed = ai.startAnimSpeed;
            ai.attacking = true;
            ai.rb.velocity = Vector2.zero;     //Stop moving to make the animation look better
            //Debug.Log("Moving Set false at AttackZombieGRab");
            //Debug.Break();
            ai.anim.SetBool("Moving", false);
            ai.anim.SetBool("Attacking", true);
            while (!ai.anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) //wait for first frame of attack anim before starting
            {
                yield return null;
            }
            yield return new WaitForSeconds(ai.anim.GetCurrentAnimatorStateInfo(0).length); //wait while current anim is still playing
            {
                yield return null;
            }
            ai.anim.SetBool("Attacking", false);
            ai.attacking = false;
        }
        if (ai.currentState != EnemyState.dead)
        {
            if (ai.enemyIntBox.playerInRange == true)
            {
                ai.currentState = EnemyState.attack;
            }
            else
            if (ai.following)
            {
                ai.currentState = EnemyState.aggro;
            }
            else
            {
                ai.currentState = EnemyState.idle;
            }
        }
        yield return null;
    }




}


