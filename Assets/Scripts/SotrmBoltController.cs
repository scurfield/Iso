using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SotrmBoltController : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D cc;
    private PlayerStats playerStats;
    private bool criticalHit;
    private int damage;
    private DamageType damageType;
    public Storm stormAbility;

    void Start()
    {
        stormAbility = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Storm>();
        playerStats = stormAbility.GetComponentInParent<PlayerStats>();
        damageType = stormAbility.damageType;
        damage = stormAbility.baseDamage;
        anim = GetComponent<Animator>();
        cc = GetComponent<CircleCollider2D>();
        StartCoroutine(BoltPrefab());
    }
    public IEnumerator BoltPrefab()
    {

        while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Bolt")) //wait for first frame of anim before starting to check if the anim is over.
        {
            
            yield return null;
        }

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // check  if anim is still playing
        {
            
            //The animation will call the function below to instantiate the prefab
            yield return null;
        }
        
        Destroy(gameObject);
        yield return null;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
            {
                float critChance = Random.Range(0f, 1f);
                Debug.Log(critChance);
                if (critChance <= playerStats.criticalChance)
                {
                    criticalHit = true;
                }
                else
                {
                    criticalHit = false;
                }
                
                Debug.Log("Sending " + damage);
                EnemyHealth enemy = other.GetComponent<EnemyHealth>();
                enemy.TakeDamage(damage, damageType, criticalHit, enemy.transform.position + new Vector3(0f, 0.4f, 0f));
    }


}}


