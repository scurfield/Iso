using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : MonoBehaviour
{
    private PlayerCombat pc;
    private PlayerMovement pm;
    public float cooldown;
    public int baseDamage;
    [HideInInspector] public int damage;
    private Animator anim;
    public int staminaCost;

    public DamageType damageType;
    private PlayerStats ps;
    private bool criticalHit;

    public float abilityTime;

    public GameObject effectsGroup;
    public ParticleSystem partic;
    private CapsuleCollider2D hitBox;

    void Start()
    {
        pc = GetComponentInParent<PlayerCombat>();
        pm = GetComponentInParent<PlayerMovement>();
        anim = GetComponentInParent<Animator>();
        ps = GetComponentInParent<PlayerStats>();
        //effectsGroup.SetActive(false);
        partic = GetComponentInChildren<ParticleSystem>();
        hitBox = GetComponent<CapsuleCollider2D>();
        hitBox.enabled = false;
    }

    public void UseAbility()
    {
        StartCoroutine(WhirlwindAttack());
        pc.UpdateCooldowns(cooldown);
    }
    public void EndAbility()
    {

    }

    IEnumerator WhirlwindAttack()
    {
        if (staminaCost < pc.currentStamina)
        {
            pc.usingAbility = true;
            pc.UseStamina(staminaCost);
            pm.currentState = PlayerState.active;
            anim.SetBool("Whirlwind", true);
            while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Whirlwind")) //wait for first frame of anim before starting to check if the anim is over.
            {
                yield return null;
            }
            //effectsGroup.SetActive(true); //hit box enabled
            hitBox.enabled = true;
            var emission = partic.emission; //creates a dusk cloud while charging
            emission.enabled = true;
            yield return new WaitForSeconds(abilityTime); // check  if anim is still playing
            {

                yield return null;
            }
            var em = partic.emission;
            em.enabled = false;
            anim.SetBool("Whirlwind", false);
            pm.currentState = PlayerState.active;
            pc.usingAbility = false;
            //effectsGroup.SetActive(false);
            hitBox.enabled = false;

        }
        yield return null;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy") //do not explode against the player please
        {
            float critChance = Random.Range(0f, 1f);
            Debug.Log(critChance);
            if (critChance <= ps.criticalChance)
            {
                criticalHit = true;
            }
            else
            {
                criticalHit = false;
            }
            Debug.Log("Calc = " + baseDamage + " / 100 * " + ps.strength);
            var calculatedDamage = baseDamage / 100f* ps.strength;
            Debug.Log(calculatedDamage);
            damage = Mathf.RoundToInt(calculatedDamage);
            Debug.Log("Sending " + damage);
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            enemy.TakeDamage(damage, damageType, criticalHit, enemy.transform.position + new Vector3(0f, 0.4f, 0f));


        }

    }
}
