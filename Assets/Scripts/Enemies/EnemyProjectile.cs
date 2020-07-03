using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Rigidbody2D rb;
    private Animator anim;

    EnemyAI2 ai;
    [HideInInspector] public Vector2 direction;
    public int damage;
    public float force;
    public float startspeed;


    public bool explode;
    bool startExploding = false;
    bool exploding = false;

    public DamageType dt;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        direction = (GameObject.FindGameObjectWithTag("Player").transform.position - gameObject.transform.position).normalized;
        rb.velocity = direction * startspeed;
    }

    private void FixedUpdate()
    {
        if (!exploding)
        {
            rb.AddForce(direction * force);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Obstacle")
        {

            exploding = true;
            StartCoroutine("Explode");

            if (other.tag == "Player")
            {
                PlayerHealth player = other.GetComponent<PlayerHealth>();
                player.TakeDamage(damage, dt);
            }

        }
    }

    IEnumerator Explode()
    {
        if (explode)
        {
            startExploding = true;
            rb.velocity = Vector2.zero;

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

        }
        Destroy(gameObject);
    }
}
