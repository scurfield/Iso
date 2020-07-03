using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class OrbiterBuffPickup : MonoBehaviour
{
    private GameObject player;
    private Animator anim;
    public bool playerFound = false;
    //public int healthAmount;
    //private PlayerHealth ph;
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

    //buffs variables

    public DamageType dt;
    private GameObject buffPrefab; //The orbiting buff prefab that will be instantiated
    private PlayerBuffsController pbc;
    private bool addingBuff = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        orbtransform = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        pbc = player.GetComponentInChildren<PlayerBuffsController>();
        calculatedSpeed = speed * 100f;
        cc = GetComponentInChildren<CircleCollider2D>();
        StartCoroutine("SpawnDelay");
        //rb.velocity = transform.up * speed;
        var buffs = GameObject.FindGameObjectWithTag("Load").GetComponent<LoadedPrefabs>().orbiterBuffs;
        switch (dt) 
        {
            case DamageType.fire:
                buffPrefab = buffs[0];
                break;
            case DamageType.ice:
                buffPrefab = buffs[1];
                break;
            case DamageType.lightning:
                buffPrefab = buffs[2];
                break;
            case DamageType.strength:
                buffPrefab = buffs[3];
                break;
        }
        

    }


    public void FixedUpdate()
    {
        if (playerFound)
        {
            cc.isTrigger = true;
            direction = (player.transform.position - orbtransform.position).normalized;
            force = direction * speed;
            rb.AddForce(force * 15);
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
        if (active)
        {
            if (other.tag == "ItemPickup")
            {


                player = GameObject.FindGameObjectWithTag("Player");
                playerFound = true;
            }
            if (other.tag == "Player")
            {
                //ph = player.GetComponent<PlayerHealth>();
                absorb = true;
                StartCoroutine("Absorb");
                //after absorbing anim done, will Add Buff to list and rearange all buffs
                
            }

        }
    }

    private void AddBuff()
    {
        addingBuff = true;
        Debug.Log("Addbuff called");
        var newBuff = Instantiate(buffPrefab, pbc.gameObject.transform.position + new Vector3(0.1f,0.1f, 0f), Quaternion.identity, pbc.gameObject.transform);
        if (pbc.orbitBuffs.Count == 4)
        {
            var oldbuff = pbc.orbitBuffs[0];
            oldbuff.SendMessage("CallDestroy");
        }
        pbc.orbitBuffs.Add(newBuff);
        //foreach (var buff in pbc.orbitBuffs)
        //{
        //    buff.SendMessage("UpdateTarget");
        //}

    }


    IEnumerator SpawnDelay() //this creates a satisfying delay between when the object is instantiated to when it can be picked up by the player
    {
        //direction = (orbtransform.position - player.transform.position).normalized;
        //force = direction * speed;

        while (spawntimer < timeTakes)
        {
            //rb.AddForce(force);
            if (spawntimer < timeTakes / 2)
            {
                //cc.isTrigger = false;
            }
            else
            {
                //cc.isTrigger = true;
            }
            active = false;
            spawntimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (spawntimer >= timeTakes)
        {

            active = true;
            yield return null;
        }
    }




    IEnumerator Absorb()
    {
        anim.SetTrigger("Absorb");
        while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Absorb")) //wait for first frame of anim before starting to check if the anim is over.
        {
            yield return null;
        }
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        // check  if anim is still playing
        if (!addingBuff)
        {

        AddBuff();
        }
        Destroy(gameObject);



    }
}
