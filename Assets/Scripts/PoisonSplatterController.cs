using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class PoisonSplatterController : MonoBehaviour
{
    public List<Sprite> splatterSprites;
    private Sprite chosenSplatter;
    public float lifeTime;
    private Light2D _light;
    private Animator anim;
    private CircleCollider2D cc;
    public int damage;
    private bool steppedOn = false;
    private PlayerHealth ph;
    public DamageType dt;




    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        _light = GetComponent<Light2D>();
        var randInt = Random.Range(0, 5);
        chosenSplatter = splatterSprites[randInt];
        anim.SetInteger("SplatterInt", randInt);
        //_light.lightCookieSprite = chosenSplatter;
        var randFacing = Random.Range(0f, 1f);
        if(randFacing >= 0.5)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        StartCoroutine(SplatterAnim());
    }

    // Update is called once per frame
    private IEnumerator SplatterAnim()
    {
        while(lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            yield return null;
        }
        anim.SetTrigger("FadeOut");
        yield return null;

    }

    public void CallDestroy()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
            if (other.tag == "Player")
            {
            ph = other.GetComponent<PlayerHealth>();
            InvokeRepeating("DamageOverTime", 0, 1);
            }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("StopAllCoroutines");
            CancelInvoke("DamageOverTime");
        }
    }

    public void DamageOverTime()
    {
        ph.TakeDamage(damage, dt);
    }

}