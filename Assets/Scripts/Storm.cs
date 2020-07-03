using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    private PlayerCombat pc;
    private PlayerStats stats;
    private PlayerMovement pm;
    private Rigidbody2D rb;
    private Animator anim;
    private Animator stormAnim;
    public GameObject stormPrefab;
    public Sprite icon;

    public int staminaCost;
    [HideInInspector] public int damage = 0;
    public int baseDamage;
    [HideInInspector] public float speed;
    public float cooldown;
    public DamageType damageType;


    public bool rotateProjectile = false;


    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponentInParent<PlayerCombat>();
        pm = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();
        stats = GetComponentInParent<PlayerStats>();
        anim = GetComponentInParent<Animator>();
        stormAnim = stormPrefab.GetComponent<Animator>();
    }

    public void UseAbility()
    {
        var tempdamage = baseDamage / 100.0f * stats.fireDamage;
        damage = Mathf.RoundToInt(tempdamage);
        StartCoroutine(AbilityStorm());
        pc.UpdateCooldowns(cooldown);
    }
    public void EndAbility()
    {

    }

    public IEnumerator AbilityStorm()
    {
        if (staminaCost < pc.currentStamina)
        {
            pc.usingAbility = true;
            pc.UseStamina(staminaCost);
            pm.currentState = PlayerState.attack;
            rb.velocity = Vector2.zero;

            //play attack animation
            anim.SetTrigger("Evoke");
            while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Evoke")) //wait for first frame of anim before starting to check if the anim is over.
            {
                yield return null;
            }
            StartCoroutine(InstantiateStorm());
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // check  if anim is still playing
            {
                //The animation will call the function below to instantiate the prefab
                yield return null;
            }
            pm.currentState = PlayerState.active;
            pc.usingAbility = false;

        }
        yield return null;


    }

    private IEnumerator InstantiateStorm(){
        yield return new WaitForSeconds(0.1f);
        var direction = new Vector3(pm.lastHoriz, pm.lastVert, 0).normalized;
        direction *= 3;
        Instantiate(stormPrefab, gameObject.transform.position + direction, Quaternion.identity);
        yield return null;
    }

}
