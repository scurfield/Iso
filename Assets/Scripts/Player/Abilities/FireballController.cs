using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    private PlayerCombat pc;
    public CircleCollider2D cc;
    private Animator anim;
    private float speed;
    public bool exploding = false;
    public GameObject target;
    public bool targetFound;
    private bool startExploding = false;
    //public int baseDamage;
    //[HideInInspector] public int damage;
    private PlayerStats stats;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
        pc = player.GetComponent<PlayerCombat>();
        anim = GetComponent<Animator>();
        direction = pc.projectileDirection;
        speed = pc.projectileSpeed;
        rb.velocity = (direction * speed);

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!exploding)
        {
            if (!targetFound)
            {
                rb.AddForce(direction * speed * 20);
            }
            else if (targetFound)
            {
                //rotate the "front" of the fireball to point at the target.
                Vector3 vectorToTarget =  gameObject.transform.position - target.transform.position;
                Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 180) * vectorToTarget;
                gameObject.transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
                //apply force in that direciton
                //damage = Mathf.RoundToInt(baseDamage / 100 * stats.fireDamage);
                direction = (Vector2)(target.transform.position - gameObject.transform.position).normalized;
                rb.AddForce(direction * speed * 20);
            }
        }
        
    }



    IEnumerator Explode()
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
            Destroy(gameObject);
    }

}
