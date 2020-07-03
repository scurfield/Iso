using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnight : MonoBehaviour
{
    private EnemyAI2 ai;
    public int swordDamage;
    public int attackNumber = 0;
    public GameObject swordBurstPrefab;
    private Vector3 projectileDirection;
    private bool burstattacking = false;
    private bool endBurst = false;
    private float yAmount;
    private Animator anim;

    public DamageType dt;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<EnemyAI2>();
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (attackNumber < 3)
        {

            StartCoroutine(SwordAttack());
        }
        else
        {
            StartCoroutine(ChargedSwordAttack());
        }
    }

    public void SwordAttackDamage() //called by animator on a specific frame
    {
        if (!burstattacking)
        {

            if (ai.enemyIntBox.playerInRange)
            {
                //deal damage
                if (ai.playerHealth.currentHealth > 0)
                {
                    ai.playerHealth.TakeDamage(swordDamage, dt);
                }
            }

        }
        else
        {
            FrontBurstPrefab();//damage from this attack is controlled by the prefab.
        }
    }

    public IEnumerator SwordAttack()
    {

        if (!ai.attacking)
        {
           
            attackNumber += 1;
            anim.speed = ai.startAnimSpeed * 2;
            ai.attacking = true;
            ai.rb.velocity = Vector2.zero;     //Stop moving to make the animation look better
            anim.SetBool("Attacking", true);
            while (!ai.anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) //wait for first frame of attack anim before starting
            {
                yield return null;
            }
            yield return new WaitForSeconds(ai.anim.GetCurrentAnimatorStateInfo(0).length); //wait while current anim is still playing
            {
                yield return null;
            }
            anim.SetBool("Attacking", false);
            anim.speed = ai.startAnimSpeed;
            ai.attacking = false;
        }
        ai.CheckEnemyState();

        yield return null;
    }

    public IEnumerator ChargedSwordAttack()
    {

        if (!ai.attacking)
        {
            attackNumber = 0;
            burstattacking = true;
            anim.speed = ai.startAnimSpeed;
            ai.attacking = true;
            ai.rb.velocity = Vector2.zero;     //Stop moving to make the animation look better
            anim.SetBool("Attacking", true);
            while (!ai.anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) //wait for first frame of attack anim before starting
            {
                yield return null;
            }
            ai.anim.speed = 0;
            yield return new WaitForSeconds(1);
            ai.anim.speed = 1;
        }


        yield return null;
    }

    public IEnumerator EndChargedSword()
    {
        if (!burstattacking)
        {
            StopCoroutine(EndChargedSword());
        }
        else if (!endBurst)
        {
            endBurst = true;
            ai.anim.speed = 0;
            yield return new WaitForSeconds(0.5f);
            anim.speed = 1;
            anim.SetBool("Attacking", false);
            anim.SetTrigger("Point");
            ai.currentState = EnemyState.special;
            while (!ai.anim.GetCurrentAnimatorStateInfo(0).IsTag("Point")) //wait for first frame of attack anim before starting
            {
                yield return null;
            }
            yield return new WaitForSeconds(ai.anim.GetCurrentAnimatorStateInfo(0).length); //wait while current anim is still playing
            {
                yield return null;
            }
            ai.attacking = false;
            burstattacking = false;
            ai.CheckEnemyState();
            endBurst = false;
            yield return null;
        }
    }

    public void FrontBurstPrefab()
    {
        projectileDirection = (new Vector3(ai.enemyIntBox.transform.position.x + ai.enemyIntBox.offset.x,
                                            ai.enemyIntBox.transform.position.y + ai.enemyIntBox.offset.y,
                                            ai.enemyIntBox.transform.position.z)
                                            - gameObject.transform.position).normalized;
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * projectileDirection;
        if (ai.enemyIntBox.offset.y > 0)
        {
            yAmount = ai.enemyIntBox.offset.y + 0.5f;
        }
        else
        {
            yAmount = ai.enemyIntBox.offset.y;

        }
        var FrontBurstInstance = Instantiate(swordBurstPrefab, new Vector3(
                                            ai.enemyIntBox.transform.position.x + (ai.enemyIntBox.offset.x * 2f),
                                            ai.enemyIntBox.transform.position.y + (yAmount),
                                            ai.enemyIntBox.transform.position.z),
                                            Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget));
        FrontBurstInstance.transform.parent = gameObject.transform;
    }

}
