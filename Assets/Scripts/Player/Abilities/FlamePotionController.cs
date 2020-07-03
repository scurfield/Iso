using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePotionController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    private PlayerCombat pc;
    private Animator anim;
    private float speed;
    private bool startExploding = false;
    private CircleCollider2D cc;
    public float knockback;
    private int damage;
    private GameObject player;
    private PlayerStats playerStats;
    private bool criticalHit;
    public DamageType damageType;
    public float onFireDuration;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        pc = player.GetComponent<PlayerCombat>();
        anim = GetComponentInChildren<Animator>();
        direction = pc.projectileDirection;
        speed = pc.projectileSpeed;
        rb.velocity = (direction * speed);
        cc = GetComponentInChildren<CircleCollider2D>();
        cc.enabled = false;
        damage = player.GetComponentInChildren<FlamePotion>().damage;

        StartCoroutine("ThrowProjectile");


    }

    // Update is called once per frame
    IEnumerator ThrowProjectile()
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Throw")) //wait for first frame of anim before starting to check if the anim is over.
        {
            rb.AddForce(direction * speed);
            yield return null;
        }


        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // check  if anim is still playing
        {
            StartCoroutine("Explode");
            yield return null;
        }
    }


    IEnumerator Explode()
    {

        rb.velocity = Vector2.zero;
        cc.enabled = true;
        //expode and damage
        anim.SetTrigger("Contact");
        while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Explode")) //wait for first frame of anim before starting to check if the anim is over.
        {
            yield return null;
        }

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // check  if anim is still playing
        {

            yield return null;
        }
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            //Vector3 enemyLocation = other.transform.position;
            //Vector3 knockbackDirection = gameObject.transform.position - enemyLocation;
            //other.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockback);
            other.GetComponent<EnemyAI2>().Knockback(gameObject.transform.position);
            float critChance = Random.Range(0f, 1f);
            if (critChance <= playerStats.criticalChance)
            {
                criticalHit = true;
            }
            else
            {
                criticalHit = false;
            }
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            enemy.fireDuration = onFireDuration;
            enemy.onFire = true;
            
            enemy.TakeDamage(damage, damageType, criticalHit, enemy.transform.position + new Vector3(0f, 0.4f, 0f));
        }

    }
}




