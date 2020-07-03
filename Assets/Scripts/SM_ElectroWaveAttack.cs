using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.Experimental.Rendering.Universal;

public class SM_ElectroWaveAttack : MonoBehaviour
{
    private CapsuleCollider2D cc;
    private Animator anim;
    public int damage;
    private PlayerMovement pm;
    private PlayerHealth ph;
    private GameObject player;
    public DamageType dt;



    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CapsuleCollider2D>();
        cc.enabled = false;
        anim = GetComponent<Animator>();
        anim.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
        pm = player.GetComponent<PlayerMovement>();
        ph = player.GetComponent<PlayerHealth>();

    }

    public void Shockwave()
    {
        
        anim.enabled = true;
        StartCoroutine(ShockwaveCoroutine());
    }

    IEnumerator ShockwaveCoroutine()
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Shockwave")) //wait for first frame of attack anim before starting
        {
            yield return null;
        }
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); //wait while current anim is still playing
        {
            anim.enabled = false;
            yield return null;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            
            ph.TakeDamage(damage, dt);
            
            pm.Knockback(gameObject.transform.position);
        }
    }
}
