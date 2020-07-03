using System.Collections;
using UnityEngine;

public class Minotaur : MonoBehaviour
{
    private EnemyAI2 ai;
    public int axeDamage;
    public int chargeDamage;
    private float distanceToPlayer;
    private Vector2 directionToPlayer;
    public CircleCollider2D aggroCC;
    public bool charging = false;
    private ParticleSystem ps;
    private PlayerMovement pm;
    private GameObject player;
    private PlayerHealth ph;
    private Rigidbody2D rb;

    public DamageType dt;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<EnemyAI2>(); 
        player = GameObject.FindGameObjectWithTag("Player");
        ph = player.GetComponent<PlayerHealth>();
        ps = GetComponentInChildren<ParticleSystem>();
        pm = player.GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.z != 0) //keep this object on z = 0;
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        if(ai.currentState == EnemyState.special)
        {
            return;
        }
        //chargeing attack
        //if aggro, measure distance betwee player and minotaur, if within specific range, continue
        if (ai.currentState == EnemyState.aggro && !charging)
        {
            distanceToPlayer = Vector3.Distance(gameObject.transform.position, ai.player.transform.position);
            if(distanceToPlayer >2 && distanceToPlayer < aggroCC.radius)
            {
                StartCoroutine(StartCharge());
            }
        }
        if (charging)
        {
            distanceToPlayer = Vector3.Distance(gameObject.transform.position, ai.player.transform.position);
            if (distanceToPlayer <= 0.3f)
            {
                rb.velocity = Vector2.zero;
                ai.anim.SetBool("Charge", false);
                ai.anim.speed = 1;
                charging = false;
                ai.attacking = false;
                if (ai.playerHealth.currentHealth > 0)
                {
                   
                    ph.TakeDamage(chargeDamage, dt);
                    pm.Knockback(gameObject.transform.position);
                    var em = ps.emission;
                    em.enabled = false;
                }
                StopAllCoroutines();
                StartCoroutine(Axe());
            }
        }
    }

    IEnumerator StartCharge()
    {
        if (!charging)
        {

            ai.currentState = EnemyState.special;
            Debug.Log("start charge");
            charging = true;
            ai.attacking = true;
            rb.velocity = Vector2.zero;
            ai.anim.SetBool("Moving", false);
            ai.anim.SetBool("Charge", true);
            while (!ai.anim.GetCurrentAnimatorStateInfo(0).IsTag("Charge")) //wait for first frame of attack anim before starting
            {
                yield return null;
            }
        }

    }
    public void ChargeMove()
    {
        charging = false;
        StartCoroutine(ChargeMoveCoroutine());
    }

    public IEnumerator ChargeMoveCoroutine()
    {
        //charge until it hits something
        if (!charging)
        {
            charging = true;
            ai.anim.speed = 0;
            directionToPlayer = (ai.player.transform.position - gameObject.transform.position).normalized;
            float normalizedTime = 0;
            while (normalizedTime <= 0.25f)
            {
                var emission = ps.emission; //creates a dusk cloud while charging
                emission.enabled = true;
                rb.AddForce(directionToPlayer * 10000);
                normalizedTime += Time.deltaTime;
                yield return null;
            }
            var em = ps.emission;
            em.enabled = false;
            ai.anim.speed = 1;
            charging = false;
            ai.attacking = false;
            yield return null;
        }
    }

     public void Attack()
    {
        if (charging)
        {
            return;
        }
        else
        StartCoroutine(Axe());
    }

    public void AxeDamage() //called by animator on a specific frame
    {
        if (ai.enemyIntBox.playerInRange)
        {
            //deal damage
            if (ai.playerHealth.currentHealth > 0)
            {
                ph.TakeDamage(axeDamage, dt);
                pm.Knockback(gameObject.transform.position);
            }
        }
        else
        {
            return;
        }
    }

    public IEnumerator Axe()
    {
        if (!ai.attacking)
        {
            ai.anim.speed = ai.startAnimSpeed;
            ai.attacking = true;
            rb.velocity = Vector2.zero;
            ai.anim.SetBool("Axe", true);
            while (!ai.anim.GetCurrentAnimatorStateInfo(0).IsTag("Axe")) //wait for first frame of attack anim before starting
            {
                yield return null;
            }
            directionToPlayer = (ai.player.transform.position - gameObject.transform.position).normalized;
            rb.AddForce(directionToPlayer * 20000);
            yield return new WaitForSeconds(ai.anim.GetCurrentAnimatorStateInfo(0).length); //wait while current anim is still playing
            {
                yield return null;
            }
            ai.anim.SetBool("Axe", false);
            ai.attacking = false;
        }
        ai.CheckEnemyState();
        yield return null;
    }

    

}


