using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    public ParticleSystem ps;
    public ParticleSystem fakePs;
    private int damage;
    public Lightning lightningAbility;
    public Animator lightAnim;
    private PlayerCombat pc;
    private GameObject player;
    private PlayerStats playerStats;
    private bool criticalHit;
    public DamageType damageType;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        ps = gameObject.GetComponent<ParticleSystem>();
        pc = player.GetComponent<PlayerCombat>();
        lightningAbility = player.GetComponentInChildren<Lightning>();
        damage = lightningAbility.damage;
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Enemy")
        {
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
            enemy.TakeDamage(damage, damageType, criticalHit, enemy.transform.position + new Vector3(0f, 0.4f, 0f));

        }
    }

    //public void EndLightning()
    //{
    //    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    //}

    private void Update()
    {
        if (lightningAbility.castingLightning)
        {
            var emission = ps.emission;
            var fakeEmission = fakePs.emission;
            emission.enabled = true;
            fakeEmission.enabled = true;
            gameObject.transform.position = new Vector3(pc.attackPoint.position.x, pc.attackPoint.position.y +0.4f, pc.attackPoint.position.z);
            Vector3 vectorToTarget = pc.attackPoint.transform.position - player.transform.position;
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * vectorToTarget;
            Quaternion rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
            gameObject.transform.rotation = rotation;
            return;
        }

        else
        {
            lightAnim.SetTrigger("EndLightning");
            var emission = ps.emission;
            var fakeEmission = fakePs.emission;
            emission.enabled = false;
            fakeEmission.enabled = false;
        }

    }

    public void DestroyLightning()
    {
        //Debug.Log("Destroy called");
        Destroy(gameObject);
    }

}
