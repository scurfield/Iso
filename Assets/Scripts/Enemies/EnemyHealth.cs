using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;            // The amount of health the enemy starts the game with.
    public int currentHealth;
    private int previousHealth;
    float prevH;                                // The current health the enemy has.
    float currH;
    public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
    public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.
    //public AudioClip deathClip;                 // The sound to play when the enemy dies.

    EnemyAI2 ai;
    Animator anim;                              // Reference to the animator.
    //AudioSource enemyAudio;                     // Reference to the audio source.
    ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
    LootDrops loot;
    public CircleCollider2D circleCollider;            // Reference to the capsule collider.
    public GameObject healthbarValue;
    public GameObject healthbar;
    public bool isDead;                                // Whether the enemy is dead.
    bool isSinking;                             // Whether the enemy has started sinking through the floor.
    private Resurrectable rez;
    private int totalDamage;

    public DamageType weakToDamageType;
    public DamageType immuneToDamageType;

    public bool isStunnable;
    private Stunnable stunnable;
    public bool stunStr = false;
    public bool stunIce = false;
    public bool stunFire = false;
    public bool stunLigh = false;

    private int stunBreakMultiplier = 1;
    private bool healthbarWaiting = false;

    private SpriteRenderer spriteRend;
    public Material defaultMaterial;
    public Material whiteMaterial;

    public GameObject critPrefab;

    //on fire
    public bool onFire;
    private GameObject firePrefab;
    private GameObject currentOnFire;
    public float fireDuration;
    //frozen
    public bool frozen;
    private GameObject frozenPrefab;
    private GameObject currentFrozen;



    void Start()
    {
        // Setting up the references.
        ai = GetComponent<EnemyAI2>();
        anim = GetComponent<Animator>();
        //enemyAudio = GetComponent<AudioSource>();
        //hitParticles = GetComponentInChildren<ParticleSystem>();
        circleCollider = GetComponent<CircleCollider2D>();
        loot = GetComponent<LootDrops>();
        // Setting the current health when the enemy first spawns.
        currentHealth = startingHealth;
        previousHealth = startingHealth;
        healthbar.SetActive(false);
        rez = GetComponent<Resurrectable>();
        spriteRend = GetComponent<SpriteRenderer>();

        if (isStunnable)
        {
            stunnable = GetComponent<Stunnable>();
        }
        var prefabs = GameObject.FindGameObjectWithTag("Load").GetComponent<LoadedPrefabs>().prefabs;
        foreach (var prefab1 in prefabs)
        {
            if (prefab1.name == "CritAnim")
            {
                critPrefab = prefab1;
                break;
            }
        }
        CheckOnFire();
        CheckFrozen();
    }

    void Update()
    {
        //Things that cause these effects should call these status Methods(), this should be removed from Update()
        CheckOnFire();
        CheckFrozen();
    }

    private void CheckOnFire()
    {
        if (onFire && currentOnFire == null)
        {
            //load the fire prefab
            if (firePrefab == null)
            {
                var prefabs = GameObject.FindGameObjectWithTag("Load").GetComponent<LoadedPrefabs>().prefabs;
                foreach (var prefab1 in prefabs)
                {
                    if (prefab1.name == "OnFire")
                    {
                        firePrefab = prefab1;
                        break;
                    }
                }

            }
            //instantiate th fire prefab, then the fire prefab will handle everything else.
            currentOnFire = Instantiate(firePrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            currentOnFire.GetComponent<OnFire>().duration = fireDuration;
            //currently, if something is on fire, it stays on fire forever while alive, even if resurrected.
            return;
        }
    }

    private void CheckFrozen()
    {
        if (frozen && currentFrozen == null)
        {
            //load the fire prefab
            if (frozenPrefab == null)
            {
                var prefabs = GameObject.FindGameObjectWithTag("Load").GetComponent<LoadedPrefabs>().prefabs;
                foreach (var prefab1 in prefabs)
                {
                    if (prefab1.name == "Frozen")
                    {
                        frozenPrefab = prefab1;
                        break;
                    }
                }

            }
            //instantiate th fire prefab, then the fire prefab will handle everything else.
            currentFrozen = Instantiate(frozenPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            //currently, if something is on fire, it stays on fire forever while alive, even if resurrected.
            return;
        }
    }

    public void EnemyHeal(int amount)
    {
        previousHealth = currentHealth;
        currentHealth += amount;
        currH = (float)currentHealth / (float)startingHealth;
        prevH = (float)previousHealth / (float)startingHealth;
        healthbarValue.transform.localScale = new Vector3(currH, 1f, 1f);
        previousHealth = currentHealth;
    }

    public void TakeDamage(int amount, DamageType damageType, bool criticalHit, Vector3 pos)
    {
        if(damageType == DamageType.fire && frozen)
        {
            frozen = false;
        }
        if (damageType == DamageType.ice && onFire)
        {
            onFire = false;
        }
        // If the enemy is dead or immune...
        if (isDead || damageType == immuneToDamageType)
            // ... no need to take damage so exit the function.
            return;
        if (ai.currentState != EnemyState.aggro) //aggro the enemy if it waas previously not aggro'd
        {
            ai.currentState = EnemyState.aggro;
        }
        if (isStunnable)
        {
            stunnable.CallStun(damageType);
        }
        // Reduce the current health by the amount of damage sustained.
        previousHealth = currentHealth;
        if (weakToDamageType == damageType)
        {
            Debug.Log("Weak to type " + damageType + ", its super effective!");
            totalDamage = amount * 2;
            //Player SuperEffective Animation
            StartCoroutine(SuperEffectiveAnimation());
        }
        else
        {
            totalDamage = amount;
            Debug.Log("Base Damage = " + amount);
            //Play standard hit Animation
            StartCoroutine(HitAnimation());

        }
        if (criticalHit == true)
        {
            Debug.Log("Critical Hit!");
            totalDamage *= 2;
            //play Critical Hit animation
            StartCoroutine(CritAnimation(pos));
        }
        CheckStunType(damageType);

        currentHealth -= totalDamage;

        Debug.Log(gameObject.name + " takes " + totalDamage + " damage");
        currH = (float)currentHealth / (float)startingHealth;
        prevH = (float)previousHealth / (float)startingHealth;
        previousHealth = currentHealth;

        StartCoroutine("HealthSliderAnimation");


        // Set the position of the particle system to where the hit was sustained.
        //hitParticles.transform.position = hitPoint;

        // And play the particles.
        //hitParticles.Play();

        // If the current health is less than or equal to zero...
        if (currentHealth <= 0)
        {
            // ... the enemy is dead.
            Death();
            loot.DropLoot(); // true, loot is from an enemy
        }
    }

    private void CheckStunType(DamageType type) //Each Stun type gets broken by a differnt damage type
    {
        if (stunStr)
        {
            if (type == DamageType.ice)
            {
                StunSuccess();
                stunnable.strCount = 0;
            }
        }
        else if (stunLigh)
        {
            if (type == DamageType.strength)
            {
                StunSuccess();
                stunnable.lighCount = 0;
            }

        }
        else if (stunIce)
        {
            if (type == DamageType.fire)
            {
                StunSuccess();
                stunnable.iceCount = 0;
            }

        }
        else if (stunFire)
        {
            if (type == DamageType.lightning)
            {
                StunSuccess();
                stunnable.fireCount = 0;
            }

        }
        else
        {
            return;
        }

    }
    private void StunSuccess()
    {
        totalDamage *= 2;
        StunEnd();
        stunnable.currentStun.SendMessage("StunBreak");
        Debug.Log("StunBreak Hit!");
    }

    private void StunEnd()
    {
        stunStr = false;
        stunIce = false;
        stunFire = false;
        stunLigh = false;
        stunnable.isStunned = false;
        if (stunnable.currentStun != null)
        {
            stunnable.currentStun.SendMessage("CallEndStun");
        }
    }
    IEnumerator HealthSliderAnimation()
    {
        if (healthbarValue.transform.localScale.x > 0)
        {
            StartCoroutine("HealthSliderTimeout");
            while (prevH > currH)
            {
                if (healthbarValue.transform.localScale.x > 0)
                {
                    prevH -= 0.01f;
                    healthbarValue.transform.localScale = new Vector3(prevH, 1f, 1f);
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    healthbar.SetActive(false);
                    yield return null;
                }

            }

        }
    }

    IEnumerator HealthSliderTimeout()
    {
        if (!healthbarWaiting)
        {
            healthbar.SetActive(true);
            healthbarWaiting = true;
            yield return new WaitForSeconds(5f);
            healthbar.SetActive(false);
            healthbarWaiting = false;
            yield return null;


        }
    }

    void Death()
    {
        // The enemy is dead.
        isDead = true;
        ai.currentState = EnemyState.dead;
        // Turn the collider into a trigger so other wan move through
        circleCollider.isTrigger = true;
        StunEnd(); // to end all stun anims
        if (rez != null)
        {
            Debug.Log("resurrectable");
            rez.deathCount += 1;
            if (rez.deathCount >= rez.maxDeaths)
            {
                Debug.Log("Final Death");
                rez.SendMessage("FinalDeath");
            }
            else
            {
                Debug.Log("die");
                anim.SetTrigger("Dead");
                if (rez.autoRez)
                {

                    rez.SendMessage("AutoRez");
                }

            }
        }
        else
        {

            // Tell the animator that the enemy is dead.
            anim.SetTrigger("Dead");
        }


        // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
        //enemyAudio.clip = deathClip;
        //enemyAudio.Play();
    }

    private IEnumerator HitAnimation() //These two next coroutines replace the material of the sprite renderer to make the image all white. Color is adjusted for effect.
    {
        spriteRend.material = whiteMaterial;
        spriteRend.color = new Color(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.02f);
        spriteRend.material = defaultMaterial;
        spriteRend.color = new Color(1f, 1f, 1f);
        yield return new WaitForSeconds(0.02f);
        spriteRend.material = whiteMaterial;
        spriteRend.color = new Color(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.02f);
        spriteRend.material = defaultMaterial;
        spriteRend.color = new Color(1f, 1f, 1f);

    }

    private IEnumerator SuperEffectiveAnimation()
    {
        spriteRend.material = whiteMaterial;
        spriteRend.color = new Color(0.8f, 0.8f, 0.8f);
        yield return new WaitForSeconds(0.05f);
        spriteRend.material = defaultMaterial;
        spriteRend.color = new Color(1f, 1f, 1f);
        yield return new WaitForSeconds(0.05f);
        spriteRend.material = whiteMaterial;
        spriteRend.color = new Color(0.8f, 0.8f, 0.8f);
        yield return new WaitForSeconds(0.05f);
        spriteRend.material = defaultMaterial;
        spriteRend.color = new Color(1f, 1f, 1f);

    }
    private IEnumerator CritAnimation(Vector3 pos)
    {
        //Here, we should have a particle effect to compliment either the basic or the SE damage flicker anims
        var tempCrit = Instantiate(critPrefab, pos, Quaternion.identity, transform.parent);
        var tempanim = tempCrit.GetComponent<Animator>();
        while (!tempanim.GetCurrentAnimatorStateInfo(0).IsTag("Crit")) //wait for first frame of attack anim before starting
        {
            yield return null;
        }
        yield return new WaitForSeconds(tempanim.GetCurrentAnimatorStateInfo(0).length); //wait while current anim is still playing
        Destroy(tempCrit);
        yield return null;
    }


}