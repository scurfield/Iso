using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class IceCrystalController : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite;
    private CircleCollider2D cc; //During Grow Animation, will be a trigger and will hurt anyone it hits. Then Becomes Solid to act as wall
    public GameObject subCrystal; // SubCrystals are spawned as child Objects of each other 

     public IceCrystalController OGController; //the Original crystal will track all of the stats
     public GameObject finalCrystal; //Once the final crystal Melts, only then will we destroy the whole chain by destroying the OG Crystal
    public int maxCrystals;
    public int currentCrystals;
    private Vector3 direction;
    private float spacing = 0.25f;
    public float duration;
    public float timeBetween;
    private int baseDamage;
    private PlayerStats stats;
    public DamageType damageType;
    private GameObject player;
    private ParticleSystem ps;

    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        cc = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
        baseDamage = player.GetComponentInChildren<IceBarrier>().baseDamage;
        direction = ((Vector2)gameObject.transform.position - (Vector2)player.transform.position).normalized;
        OGController = transform.root.gameObject.GetComponent<IceCrystalController>();
        ps = GetComponent<ParticleSystem>();
        if (OGController.currentCrystals == OGController.maxCrystals)
        {
            OGController.finalCrystal = gameObject;
        }
        var xFlip = Random.Range(0f, 1f);
        if (xFlip < 0.5f)
        {
            sprite.flipX = true;
        }
        else { sprite.flipX = false; }
        var chooseAnim = Random.Range(0f, 1f);
        if (chooseAnim < 0.33f)
        {
            anim.SetBool("AnimOne", true);
        }
        else if (chooseAnim < 0.66f)
        {
            anim.SetBool("AnimTwo", true);
        }
        else
        {
            anim.SetBool("AnimThree", true);
        }
        StartCoroutine(CrystalTimeline());
    }

    private IEnumerator CrystalTimeline()
    {
        yield return new WaitForSeconds(timeBetween);
        //Next Crystal
        if (OGController.currentCrystals < OGController.maxCrystals)
        {
            SpawnNextCrystal();
        }
        yield return new WaitForSeconds(0.2f);
        SolidifyCollider();
        yield return new WaitForSeconds(duration);
        anim.SetTrigger("Melt");
        var emission = ps.emission;
        emission.enabled = false;
        if (OGController.finalCrystal != null)
        {
            StartCoroutine(EndChain());
        }
    }
    private IEnumerator EndChain()
    {
        var finalAnim = OGController.finalCrystal.GetComponent<Animator>();
        while (!finalAnim.GetCurrentAnimatorStateInfo(0).IsTag("Melt")) //wait for first frame of attack anim before starting
        {
            yield return null;
        }
        yield return new WaitForSeconds(finalAnim.GetCurrentAnimatorStateInfo(0).length - 0.5f ); //wait while current anim is still playing
        cc.isTrigger = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(OGController.gameObject);
    }
    public void SpawnNextCrystal()
    {
            OGController.currentCrystals += 1;
            Instantiate(subCrystal, gameObject.transform.position + OGController.direction * spacing, Quaternion.identity, gameObject.transform);
        
    }

    public void SolidifyCollider()
    {
        //called by animator
        cc.isTrigger = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle" || other.tag == "NPC")
        {

            OGController.currentCrystals = maxCrystals;
            OGController.finalCrystal = gameObject;
            StartCoroutine(EndChain());
            return; //end the chain upon impact with an obstacle
        }

        if(cc.isTrigger == true && other.tag == "Enemy")
        {
            float critChance = Random.Range(0f, 1f);
            Debug.Log(critChance);
            bool criticalHit;
            if (critChance <= stats.criticalChance)
            {
                criticalHit = true;
            }
            else
            {
                criticalHit = false;
            }
            var calculatedDamage = baseDamage / 100f * stats.iceDamage;
            Debug.Log(calculatedDamage);
            int damage = player.GetComponentInChildren<IceBarrier>().baseDamage;
            damage = Mathf.RoundToInt(calculatedDamage);
            Debug.Log(gameObject.name + " sending " + damage + " damage.");
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            enemy.TakeDamage(damage, damageType, criticalHit, enemy.transform.position + new Vector3(0f, 0.4f, 0f));
            EnemyAI2 ai = other.GetComponent<EnemyAI2>();
            ai.Knockback((Vector2)gameObject.transform.position);
        }
    }



}
