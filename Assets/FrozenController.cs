using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenController : MonoBehaviour
{
    private float duration = 5f;
    private Animator anim;
    private SpriteRenderer sprite;
    private ParticleSystem ps;
    private EnemyHealth eh;
    private GameObject enemy;
    private Rigidbody2D enemyRb;
    private SpriteRenderer enemySprite;
    private Animator enemyAnim;
    private float previousSpeed;
    private bool melting = false;
    private Color previousColor;
    

    // Start is called before the first frame update
    void Start()
    {
        //enemy
        eh = GetComponentInParent<EnemyHealth>();
        enemy = eh.gameObject;
        enemyRb = enemy.GetComponent<Rigidbody2D>();
        enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
        enemySprite = enemy.GetComponent<SpriteRenderer>();
        previousColor = enemySprite.color;
        enemySprite.color = new Color(0.25f, 0.75f, 1f, 1f);
        enemyAnim = enemy.GetComponent<Animator>();
        enemyAnim.enabled = false;

        //this object
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();

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
    void Update()
    {

        if (!eh.frozen && !melting)
        {
            StartCoroutine(Melt());
        }
    }

    private IEnumerator CrystalTimeline()
    {
        yield return new WaitForSeconds(duration);
        StartCoroutine(Melt());
    }

    public IEnumerator Melt()
    {
        enemyAnim.enabled = true;
        eh.frozen = false;
        enemyRb.constraints = RigidbodyConstraints2D.None;
        enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Debug.Log("Melt Called");
        melting = true;
        enemySprite.color = previousColor;
        anim.SetTrigger("Melt");
        var emission = ps.emission;
        emission.enabled = false;
        while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Melt")) //wait for first frame of attack anim before starting
        {
            yield return null;
        }
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);

    }
}
