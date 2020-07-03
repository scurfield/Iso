using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrectable : MonoBehaviour
{
    private EnemyAI2 ai;
    private EnemyHealth eh;
    public bool autoRez;
    public float autoRezTime;
    private bool resurrecting = false;
    public bool finalDeath = false;
    public int deathCount = 0;
    public int maxDeaths;

    CircleCollider2D circleCollider;
    void Start()
    {
        ai = GetComponent<EnemyAI2>();
        eh = GetComponent<EnemyHealth>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    public IEnumerator Resurrect()
    {
        Debug.Log("resurrect");
        if (ai.currentState == EnemyState.dead && !resurrecting && !finalDeath)
        {
            resurrecting = true;
            //health
            var halfHealth = eh.startingHealth / 2;
            eh.EnemyHeal(halfHealth);
            eh.circleCollider.isTrigger = false;
            eh.isDead = false;
            //play animation
            ai.currentState = EnemyState.special;
            ai.anim.speed = ai.startAnimSpeed;
            circleCollider.isTrigger = false;
            ai.anim.SetBool("Moving", false);
            ai.anim.SetTrigger("Resurrect");
            while (!ai.anim.GetCurrentAnimatorStateInfo(0).IsTag("Resurrect")) //wait for first frame of attack anim before starting
            {
                yield return null;
            }
            yield return new WaitForSeconds(ai.anim.GetCurrentAnimatorStateInfo(0).length); //wait while current anim is still playing
            {
                yield return null;
            }
            resurrecting = false;
            ai.currentState = EnemyState.idle;
            yield return null;
        }
    }

    public IEnumerator AutoRez()
    {
        Debug.Log("autorez start: " + autoRezTime);
        yield return new WaitForSeconds(autoRezTime);
            deathCount -= 1; //for infinite deaths by natural respawn
            StartCoroutine(Resurrect());
            yield return null;
    }

    public void FinalDeath()
    {
        finalDeath = true;
        ai.anim.SetTrigger("FinalDeath");
    }
}
