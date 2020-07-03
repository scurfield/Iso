using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOrb : MonoBehaviour
{
    private GameObject player;
    private Animator anim;
    private bool playerFound = false;
    private PlayerStats ps;
    private Transform orbtransform;
    private Rigidbody2D rb;
    private CircleCollider2D cc;
    private Vector2 direction;
    private Vector2 force;
    public float speed;
    private float calculatedSpeed;
    private float shrinkFactor = 1.2f;
    private bool absorb = false;
    private bool active = false;
    private float spawntimer = 0f;

    private float timeTakes = 1f;






    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        orbtransform = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        calculatedSpeed = speed * 100f;
        cc = GetComponentInChildren<CircleCollider2D>();
        //StartCoroutine("SpawnDelay");
        //rb.velocity = transform.up * speed;

    }


    public void FixedUpdate()
    {
        if (playerFound)
        {
            cc.isTrigger = true;
            direction = (player.transform.position - orbtransform.position).normalized;
            force = direction * speed;
            rb.AddForce(force * 5);
            if (cc.IsTouchingLayers(LayerMask.GetMask("Player")))
            {
                StartCoroutine(Absorb());
            }
        }
        else
        {
            if (cc.IsTouchingLayers(LayerMask.GetMask("Item")))
            {
                StartCoroutine(Absorb());
            }
        }
        if (active && !playerFound)
        {
            playerFound = cc.IsTouchingLayers(LayerMask.GetMask("Pickup"));
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ItemPickup")
        {


            player = GameObject.FindGameObjectWithTag("Player");
            playerFound = true;
        }
        if (other.tag == "Player")
        {
            ps = player.GetComponent<PlayerStats>();
            gameObject.transform.parent = player.gameObject.transform;
            StartCoroutine("Absorb");
            player.GetComponentInChildren<AuraVFX>().SendMessage("HealAura");
        }

    }


    IEnumerator Absorb()
    {
        if (!absorb)
        {
            absorb = true;
            anim.SetTrigger("Absorb");
            while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Absorb")) //wait for first frame of anim before starting to check if the anim is over.
            {
                yield return null;
            }

            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // check  if anim is still playing
            {

                yield return null;
            }
            ps.powerOrbs += 1;
            Destroy(gameObject);
        }
    }

    public void CallRuneCircle()
    {
        player.GetComponentInChildren<AuraVFX>().SendMessage("RuneCircle");
    }

}
