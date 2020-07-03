using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private PlayerCombat pc;
    private PlayerStats stats;
    private PlayerMovement pm;
    private Rigidbody2D rb;
    private Animator anim;
    private Animator fbAnim;
    public GameObject fireballPrefab;
    public Sprite icon;

    public int staminaCost;
    [HideInInspector] public int damage = 0;
    public int baseDamage;
    public float speed;
    public float cooldown;
    private float fireRate = 0.1f;
    

    public bool rotateProjectile = true;


    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponentInParent<PlayerCombat>();
        pm = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();
        stats = GetComponentInParent<PlayerStats>();
        anim = GetComponentInParent<Animator>();
        fbAnim = fireballPrefab.GetComponent<Animator>();
    }

    public void UseAbility()
    {
        var tempdamage = baseDamage / 100.0f * stats.fireDamage;
        Debug.Log(tempdamage + " temp damage");
        damage = Mathf.RoundToInt(tempdamage);
        Debug.Log(baseDamage + "Base Damage");
        Debug.Log(stats.fireDamage + "Fire damage stat");
        Debug.Log("Fireball.damage " + damage);
        StartCoroutine(AbilityFireball());
        pc.UpdateCooldowns(cooldown);
    }
    public void EndAbility()
    {

    }

    public IEnumerator AbilityFireball()
    {
        if (staminaCost < pc.currentStamina)
        {
            pc.usingAbility = true;
            pc.UseStamina(staminaCost);
            pm.currentState = PlayerState.attack;
            rb.velocity = Vector2.zero;
            pc.projectile = fireballPrefab;
            pc.projectileSpeed = speed;
            pc.rotateProjectile = rotateProjectile;
            pc.castFromChest = true;

            //play attack animation
            anim.SetTrigger("Cast");
            while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Cast")) //wait for first frame of anim before starting to check if the anim is over.
            {
                yield return null;
            }
            yield return new WaitForSeconds(fireRate);//this is the timing for the fastest you can mash attack    
            pm.currentState = PlayerState.active;
            pc.usingAbility = false;
            yield return new WaitForSeconds(0.1f); // check  if anim is still playing
            anim.SetTrigger("SkipAnim");
   

        }
        yield return null;


    }

}
