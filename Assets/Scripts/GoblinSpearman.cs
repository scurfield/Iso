using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSpearman : MonoBehaviour
{
    public GameObject spearPrefab;
    public Vector2 projectileDirection;
    private Animator anim;
    private EnemyAI2 ai;
    private bool hop = false;
    public int stabDamage;
    private bool randomThrowing;
    private bool throwing = false;
    public DamageType dt;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ai = GetComponent<EnemyAI2>();
    }

    public void Attack()
    {
        if (ai.range.playerInRange == true)
        {
            ai.fleeWhenTooClose = false;
            StartCoroutine("Stab");

        }
        else
        {
            ai.TooClose();
            if (!ai.tooClose)
            {
                if (!throwing)
                {

                StartCoroutine("Throw");
                }

            }
            else
            {
                ai.fleeWhenTooClose = true;
                ai.currentState = EnemyState.flee;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        //hop Anim
        if(ai.idleWait && !hop)
        {
            StartCoroutine("Hop");
        }
        if(hop && !ai.idleWait)
        {
            StopCoroutine("Hop");
        }

        //Aggro
        if(ai.currentState == EnemyState.aggro)
        {
            anim.SetBool("Aggro", true);
            if (randomThrowing == false)
            {
                StartCoroutine("RandomThrow");
            }
        }
        else if(ai.currentState == EnemyState.idle || ai.currentState == EnemyState.flee || ai.currentState == EnemyState.retreating)
        {
            anim.SetBool("Aggro", false);
        }
    }

    private IEnumerator RandomThrow()
    {
        randomThrowing = true;
        yield return new WaitForSeconds(Random.Range(1f, 4f));
        if(ai.currentState == EnemyState.aggro)
        {
                StartCoroutine("Throw");
        }
        randomThrowing = false;
    }
    public IEnumerator Hop()
    {

        hop = true;
        yield return new WaitForSeconds(Random.Range(1f, 4f));
        ai.rb.velocity = Vector2.zero;
        ai.anim.SetTrigger("Hop");
        while (!ai.anim.GetCurrentAnimatorStateInfo(0).IsTag("Hop")) //wait for first frame of attack anim before starting
        {
            yield return null;
        }
        yield return new WaitForSeconds(ai.anim.GetCurrentAnimatorStateInfo(0).length); //wait while current anim is still playing
        {
            yield return null;
        }
        hop = false;
        yield break;
    }
    public IEnumerator Stab()
    {
        if (!ai.attacking)
        {
            ai.anim.speed = ai.startAnimSpeed;
            ai.attacking = true;
            ai.rb.velocity = Vector2.zero;
            ai.anim.SetTrigger("Stab");
            while (!ai.anim.GetCurrentAnimatorStateInfo(0).IsTag("Stab")) //wait for first frame of attack anim before starting
            {
                yield return null;
            }
            yield return new WaitForSeconds(ai.anim.GetCurrentAnimatorStateInfo(0).length); //wait while current anim is still playing
            {
                
                yield return new WaitForSeconds(0.5f);//to give space between attacks
            }
            ai.attacking = false;
        }
        ai.CheckEnemyState();
        yield return null;
    }
    public void StabDamage()
    {
        if (ai.enemyIntBox.playerInRange)
        {
            //deal damage
            if (ai.playerHealth.currentHealth > 0)
            {
                ai.playerHealth.TakeDamage(stabDamage, dt);
            }
        }
        else
        {
            return;
        }
    }

    public IEnumerator Throw()
    {
        if (!ai.attacking)
        {

            ai.anim.speed = ai.startAnimSpeed;
            ai.attacking = true;
            ai.rb.velocity = Vector2.zero;     //Stop moving to make the animation look better
            ai.anim.SetTrigger("Throw");
            while (!ai.anim.GetCurrentAnimatorStateInfo(0).IsTag("Throw")) //wait for first frame of attack anim before starting
            {

                yield return null;
            }
            yield return new WaitForSeconds(ai.anim.GetCurrentAnimatorStateInfo(0).length); //wait while current anim is still playing
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);//to give space between attacks
            ai.attacking = false;
        }
        ai.CheckEnemyState();
        throwing = false;
        yield return null;
    }


    public void ThrowSpear()
    {
        if (!throwing)
        {
            ai.CalledFacingUpdate();
            throwing = true;
            projectileDirection = (ai.player.transform.position - gameObject.transform.position).normalized;
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * projectileDirection;
            Instantiate(spearPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.3f, gameObject.transform.position.z), Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget)); // .y + 0.nf to be at chest height
        }

    }
}
