
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class FireWallController : MonoBehaviour
{
    private Animator anim;

    private CircleCollider2D cc; //During Grow Animation, will be a trigger and will hurt anyone it hits. Then Becomes Solid to act as wall
    public GameObject SubFire; // SubCrystals are spawned as child Objects of each other 

    public FireWallController OGController; //the Original crystal will track all of the stats
    public GameObject finalFire; //Once the final crystal Melts, only then will we destroy the whole chain by destroying the OG Crystal
    public int maxFires;
    public int currentFires;
    private Vector3 direction;
    private float spacing = 0.25f;
    public float duration;
    public float timeBetween;
    private int baseDamage;
    private PlayerStats stats;
    public DamageType damageType;
    private GameObject player;
    private ParticleSystem ps;

    private List<GameObject> inFire = new List<GameObject>();

    void Start()
    {
        anim = GetComponent<Animator>();

        cc = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
        baseDamage = player.GetComponentInChildren<FireWall>().baseDamage;
        direction = ((Vector2)gameObject.transform.position - (Vector2)player.transform.position).normalized;
        OGController = transform.root.gameObject.GetComponent<FireWallController>();
        ps = GetComponent<ParticleSystem>();
        if (OGController.currentFires == OGController.maxFires)
        {
            OGController.finalFire = gameObject;
        }
        var xFlip = Random.Range(0f, 1f);
        if (xFlip < 0.5f)
        {
            gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0F);
        }
        else { gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0F); }
        var varSpeed = Random.Range(0.5f, 1f);
        anim.speed = varSpeed;
        
        StartCoroutine(FireWallTimeline());
        InvokeRepeating("StandingInFire", 0f, 0.5f);
    }

    private IEnumerator FireWallTimeline()
    {
        yield return new WaitForSeconds(timeBetween);
        //Next Crystal
        if (OGController.currentFires < OGController.maxFires)
        {
            SpawnNextFire();
        }
        yield return new WaitForSeconds(duration);
        anim.SetTrigger("Fade");
        var emission = ps.emission;
        emission.enabled = false;
        if (OGController.finalFire == gameObject)
        {
            StartCoroutine(EndChain());
        }

    }

    private IEnumerator EndChain()
    {
        var finalAnim = OGController.finalFire.GetComponent<Animator>();
        while (!finalAnim.GetCurrentAnimatorStateInfo(0).IsTag("Fade")) //wait for first frame of attack anim before starting
        {
            yield return null;
        }
        CancelInvoke();
        yield return new WaitForSeconds(finalAnim.GetCurrentAnimatorStateInfo(0).length + 2);

        Destroy(OGController.gameObject);
    }

    public void SpawnNextFire()
    {
        OGController.currentFires += 1;
        var randomVector = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
        Instantiate(SubFire, gameObject.transform.position + (OGController.direction + randomVector) * spacing, Quaternion.identity, gameObject.transform);

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle")
        {

            OGController.currentFires = maxFires;
            OGController.finalFire = gameObject;
            StartCoroutine(EndChain());
            return; //end the chain upon impact with an obstacle
        }

        if (other.tag == "Enemy")
        {
            GameObject otherGO = other.gameObject;
            inFire.Add(otherGO);
            DealDamage(other.gameObject);
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            GameObject otherGO = other.gameObject;
            inFire.Remove(otherGO);
        }
    }

    private void StandingInFire()
    {
        foreach (GameObject target in inFire)
        {
            DealDamage(target);
        }
    }

    private void DealDamage(GameObject enemy)
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
        var calculatedDamage = baseDamage / 100f * stats.fireDamage;
        Debug.Log(calculatedDamage);
        int damage = player.GetComponentInChildren<IceBarrier>().baseDamage;
        damage = Mathf.RoundToInt(calculatedDamage);
        Debug.Log(gameObject.name + " sending " + damage + " damage.");
        EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
        eh.TakeDamage(damage, damageType, criticalHit, enemy.transform.position + new Vector3(0f,0.4f,0f));
    }
}
