using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFire : MonoBehaviour
{
    private EnemyHealth eh;
    private Animator anim;
    private bool onFire;
    private PlayerHealth ph;
    public int damage;
    private DamageType dt = DamageType.fire;
    private ParticleSystem ps;
    public float duration;




    void Start()
    {
        eh = GetComponentInParent<EnemyHealth>();
        ps = GetComponentInChildren<ParticleSystem>();
        anim = GetComponent<Animator>();
        anim.SetBool("OnFire", true);
        StartCoroutine(FireDuration());
    }

    //Update fire to yes while alive
    void Update()
    {
        if(eh.currentHealth > 0)
        {
            if(onFire == false)
            {
                onFire = true;
                InvokeRepeating("DealDamage", 0.5f, 1f);
                anim.SetBool("OnFire", true);
            }
        }
        if (eh.currentHealth <= 0 || eh.onFire == false)
        {
            if (onFire == true)
            {
                
                CancelInvoke();
                StartCoroutine(EndFire());
            }
        }
    }

    private IEnumerator FireDuration()
    {
        var elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(EndFire());
    }
    private IEnumerator EndFire()
    {
        eh.onFire = false;
        var emission = ps.emission;
        emission.enabled = false;
        anim.SetBool("OnFire", false);
        onFire = false;
        while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("EndFire")) //wait for first frame of anim before starting to check if the anim is over.
        {
            yield return null;
        }
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        // check  if anim is still playing
        Destroy(gameObject);
    }

    void DealDamage()
    {
        eh.TakeDamage(damage, DamageType.fire, false, gameObject.transform.position);
    }

}
