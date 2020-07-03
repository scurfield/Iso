using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectAbility : MonoBehaviour
{

    private Transform rezCenter;
    public float resurrectionRange;
    public LayerMask enemyLayers;
    public List<Collider2D> inRezRange;
    public GameObject groundLightVFX;
    private EnemyAI2 ai;
    public bool usingRezAbility;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<EnemyAI2>();
        InvokeRepeating("CheckResurrection", 0f, 20f);
    }

    // Update is called once per frame
    private void CheckResurrection()
    {
        if(ai.currentState == EnemyState.dead)
        {
            return;
        }
        Debug.Log("Checking Rez");
        rezCenter = gameObject.transform;
        inRezRange = new List<Collider2D>(Physics2D.OverlapCircleAll(rezCenter.position, resurrectionRange, enemyLayers)); //Check which enemies are within his swarm range
        foreach (Collider2D enemy in inRezRange)
        {
            var ai = enemy.GetComponent<EnemyAI2>();
            if (ai.currentState == EnemyState.dead)
            {
                var rezScript = enemy.GetComponent<Resurrectable>();
                if (rezScript != null)
                {
                    if(rezScript.finalDeath == false)
                    {
                        StartCoroutine(RezAbility());
                        return;
                    }
                }

            }
        }
    }

    private IEnumerator RezAbility()
    {
        usingRezAbility = true;
        //Play rez animation
        Instantiate(groundLightVFX, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
        ai.attacking = true;
        ai.anim.speed = ai.startAnimSpeed / 2;
        ai.rb.velocity = Vector2.zero;     //Stop moving to make the animation look better
        ai.anim.SetTrigger("Taunt");
        while (!ai.anim.GetCurrentAnimatorStateInfo(0).IsTag("Taunt")) //wait for first frame of attack anim before starting
        {
            yield return null;
        }
        yield return new WaitForSeconds(ai.anim.GetCurrentAnimatorStateInfo(0).length); //wait while current anim is still playing
        usingRezAbility = false;
        ai.attacking = false;
        ai.anim.speed = ai.startAnimSpeed;
    }

    public void ShootRezOrbs()
    {
        Debug.Log("start actual rez");
        foreach (Collider2D enemy in inRezRange)
        {
            Debug.Log("Check 1");
            var ai = enemy.GetComponent<EnemyAI2>();
            if (ai.currentState == EnemyState.dead)
            {
                Debug.Log("Check 2");
                var rezScript = enemy.GetComponent<Resurrectable>();
                if (rezScript != null)
                {
                    Debug.Log("Check 3");
                    if (rezScript.finalDeath == false)
                    {
                        Debug.Log(rezScript);
                        rezScript.SendMessage("Resurrect");
                    }
                }

            }
        }
    }

    private void OnDrawGizmosSelected()             //Draw the anit-swarm range for calculations
    {
        if (rezCenter == null)
            return;
        Gizmos.DrawWireSphere(rezCenter.position, resurrectionRange);
    }

}
