using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunRuneController : MonoBehaviour
{
    private ParticleSystem ps;
    private EnemyHealth eh;
    public float duration;
    public float timeout;
    private Animator anim;
    private Stunnable stunnable;
    public GameObject stunBreak;
    private EnemyAI2 ai;
    public Animator vulnTriAnim;


    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        anim = GetComponent<Animator>();
        eh = GetComponentInParent<EnemyHealth>();
        stunnable = GetComponentInParent<Stunnable>();
        ai = GetComponentInParent<EnemyAI2>();
        
        StartCoroutine(StunRune());
    }


    public IEnumerator StunRune()
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle")) //wait for first frame of anim before starting to check if the anim is over.
        {
            yield return null;
        }
        var emission = ps.emission; //creates a dusk cloud while charging
        emission.enabled = true;
        yield return new WaitForSeconds(duration - 0.2f);
        anim.SetTrigger("Time");
        while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Time")) //wait for first frame of anim before starting to check if the anim is over.
        {
            yield return null;
        }

        yield return new WaitForSeconds(timeout - 0.2f);
        StartCoroutine(EndStun());
    }
    public void CallEndStun()
    {
        StartCoroutine(EndStun());
    }
    private IEnumerator EndStun()
    {
        Debug.Log("End StunCalled");
        vulnTriAnim.SetTrigger("End");
        anim.SetTrigger("End");
        var em = ps.emission;
        em.enabled = false;
        eh.stunStr = false;
        eh.stunIce = false;
        eh.stunFire = false;
        eh.stunLigh = false;
        stunnable.isStunned = false;
        ai.stunned = false;
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

    public void StunBreak()
    {
        Instantiate(stunBreak, gameObject.transform.position + new Vector3(0f, 0.4f, 0f), Quaternion.identity, gameObject.transform);
    }


}
