using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbiterBuffController : MonoBehaviour
{
    public List<GameObject> targets; //list of target points, this orbiter will always have one picked and will move towards it.
    public GameObject target;
    private GameObject player;
    private PlayerBuffsController pbc;
    private PlayerStats stats;
    private Animator anim;
    private ParticleSystem ps;
    
    private float speed = 2000;
    private Rigidbody2D rb;
    public DamageType dt;
    private float buffAmount = 20;
    private float buffDuration = 10;
    private float elapsedTime = 0;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pbc = player.GetComponentInChildren<PlayerBuffsController>();
        stats = player.GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        ps = GetComponentInChildren<ParticleSystem>();

        rb = GetComponent<Rigidbody2D>();
        for(int i = 0; i< 4; i++)
        {
        targets.Add(pbc.transform.GetChild(i).gameObject);
        }

        switch (dt)
        {
            case DamageType.fire:
                stats.fireDamage += buffAmount;
                break;
            case DamageType.ice:
                stats.iceDamage += buffAmount;
                break;
            case DamageType.lightning:
                stats.lightningDamage += buffAmount;
                break;
            case DamageType.strength:
                stats.strength += buffAmount;
                break;
        }
        StartCoroutine(Timer());
        FindTarget();
    }
    private IEnumerator Timer()
    {
        while (elapsedTime < buffDuration - 5f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        anim.SetTrigger("Trigger");
        yield return new WaitForSeconds(5);
        CallDestroy();

    }

    void Update()
    {
        var randomizer = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        Vector2 direction = ((Vector2)target.transform.position - rb.position).normalized + randomizer;
        float distance = ((Vector2)target.transform.position - rb.position).magnitude;
        rb.AddForce(direction * speed * distance * Time.deltaTime);
    }

    public void FindTarget()
    {
        if (target == null)
        {

            for (int i = 0; i < 4; i++)
            {
                if (targets[i].GetComponent<OrbitPoint>().full == false)
                {
                    target = targets[i];
                    target.GetComponent<OrbitPoint>().full = true;
                    break;
                }
            }
        }
    }
    

    public void CallDestroy()
    {
        switch (dt)
        {
            case DamageType.fire:
                stats.fireDamage -= buffAmount;
                break;
            case DamageType.ice:
                stats.iceDamage -= buffAmount;
                break;
            case DamageType.lightning:
                stats.lightningDamage -= buffAmount;
                break;
            case DamageType.strength:
                stats.strength -= buffAmount;
                break;
        }
        StartCoroutine(TimedDestroy());
    }
    private IEnumerator TimedDestroy()
    {
        var emission = ps.emission;
        emission.enabled = false;
        anim.SetTrigger("Trigger");
        pbc.orbitBuffs.RemoveAt(0);
        target.GetComponent<OrbitPoint>().full = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        yield return null;

    }


}
