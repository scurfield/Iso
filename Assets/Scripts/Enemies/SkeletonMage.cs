using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMage : MonoBehaviour
{
    private EnemyAI2 ai;
    private SM_ElectroWaveAttack sw;
    //Projectile
    public GameObject FrostballProjectile;
    public Vector2 projectileDirection;
    private Animator anim;
    private ResurrectAbility rezAb;

    // Start is called before the first frame update
    void Awake()
    {
        
        ai = GetComponent<EnemyAI2>();
        sw = GetComponentInChildren<SM_ElectroWaveAttack>();
        anim = GetComponent<Animator>();
        rezAb = GetComponent<ResurrectAbility>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CallShockwave()
    {
        if(rezAb.usingRezAbility == true)
        {
            rezAb.ShootRezOrbs();
        }
        else
        sw.Shockwave();
    }

    public void Attack()
    {
        StartCoroutine(CastSpellAnimation());
    }

    public IEnumerator CastSpellAnimation()
    {

        if (!ai.attacking)
        {
            ai.anim.speed = ai.startAnimSpeed;
            ai.attacking = true;
            ai.rb.velocity = Vector2.zero;     //Stop moving to make the animation look better
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
            yield return new WaitForSeconds(0.5f);//to give space between attacks
            ai.attacking = false;
        }
        if (ai.currentState != EnemyState.dead)
        {
            if (ai.range.playerInRange == true)
            {
                ai.TooClose();
                if (!ai.tooClose)
                {
                    ai.currentState = EnemyState.attack;
                }
                else
                {
                    StartShockwave();

                    yield break;
                }
            }
            else
                ai.currentState = EnemyState.idle;
        }
        yield return null;
    }

    private void StartShockwave()
    {
        ai.attacking = true;
        anim.SetTrigger("Taunt");
    }

    public void EndShockwave()
    {
        ai.attacking = false;
        ai.currentState = EnemyState.flee;
        ai.UpdatePath();
    }


    public void CastProjectile()
    {
        projectileDirection = (ai.player.transform.position - gameObject.transform.position).normalized;
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * projectileDirection;
        Instantiate(FrostballProjectile, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.4f, gameObject.transform.position.z), Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget)); // .y + 0.5f to be at chest height
        //FrostballProjectile.SendMessage("UpdateDirection", projectileDirection);
    }



}
