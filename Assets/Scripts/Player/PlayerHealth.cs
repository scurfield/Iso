using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public int previousHealth;
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Slider healthSliderAnim;
    bool healthSliderWait = false;
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public Image deathMessage;
    CircleCollider2D circleCollider;
    //public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float fadeSpeed = 0.1f;
    bool fadeOut = true;// The speed the damageImage will fade at.
    public Color alphaColour = new Vector4(1f, 1f, 1f, 0f);    // The colour the damageImage is set to, to flash.
    private PlayerStats stats;

    Animator anim;                                              // Reference to the Animator component.
    //AudioSource playerAudio;                                    // Reference to the AudioSource component.
    PlayerMovement playerMovement;                              // Reference to the player's movement.
    PlayerCombat pc;
    //PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
    public bool isDead;                                                // Whether the player is dead.
    bool damaged;

    private float totalDamage;

    // True when the player gets damaged.


    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        //playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        pc = GetComponent<PlayerCombat>();
        //playerShooting = GetComponentInChildren<PlayerShooting>();
        circleCollider = GetComponent<CircleCollider2D>();
        // Set the initial health of the player.
        currentHealth = startingHealth;
        previousHealth = startingHealth;
        stats = GetComponent<PlayerStats>();

    }

    void Update()
    {
        //damage image fadeout

        if(damageImage.color.a >= 0.01f)
            {
            damageImage.color -= new Color(0f, 0f, 0f, fadeSpeed*Time.deltaTime);
            
            }
        if (previousHealth == currentHealth)
        {
            healthSliderWait = false;
        }




    }
    IEnumerator HealthSliderAnimation()
    {
        if (!healthSliderWait)
        {
            healthSliderWait = true;
            yield return new WaitForSeconds(0.5f);
        }
        while (previousHealth > currentHealth)
        {
            previousHealth -= 1;
            healthSliderAnim.value = previousHealth;
            yield return new WaitForSeconds(0.01f);
        }
        while (previousHealth < currentHealth)
        {
            previousHealth += 1;
            healthSlider.value = previousHealth;
            healthSliderAnim.value = previousHealth;
            yield return new WaitForSeconds(0.01f);
        }
    }


    void DamageImageFade()
    {

        damageImage.color = new Vector4(1f, 1f, 1f, 0.5f);
        

    }


    public void TakeDamage(int amount, DamageType damageType)
    {
        if (!pc.rolling)
        {
            if(!playerMovement)

            // Set the damaged flag so the screen will flash.
            damaged = true;
            DamageImageFade();

            // Reduce the current health by the damage amount.
            
            //armour Save
            totalDamage = amount * 100.0f / stats.armour;
            int blocked = Mathf.RoundToInt((1.0f * amount) - totalDamage);
            if (blocked >= 1)
            {
                Debug.Log("blocked " + blocked + " damage");
            }
            //damage type saves
            if (damageType == DamageType.fire)
            {
                totalDamage *= 100.0f / stats.fireResist;
                int difference = Mathf.RoundToInt((1.0f * amount) - totalDamage);
                if(difference >= 1)
                {
                    //Debug.Log("Resisted " + difference + " fire damage");
                }
            }
            if (damageType == DamageType.ice)
            {
                totalDamage *= 100.0f / stats.iceResist;
                int difference = Mathf.RoundToInt((1.0f * amount) - totalDamage);
                if (difference >= 1)
                {
                    //Debug.Log("Resisted " + difference + " ice damage");
                }
            }
            if (damageType == DamageType.lightning)
            {
                totalDamage *= 100.0f / stats.lightningResist;
                int difference = Mathf.RoundToInt((1.0f * amount) - totalDamage);
                if (difference >= 1)
                {
                    //Debug.Log("Resisted " + difference + " lightning damage");
                }
            }
            //final damage
            previousHealth = currentHealth;
            currentHealth -= Mathf.RoundToInt(totalDamage);
            //Debug.Log("You take " + Mathf.RoundToInt(totalDamage) + " damage.");

            // Set the health bar's value to the current health.
            healthSlider.value = currentHealth;
            StartCoroutine("HealthSliderAnimation");

        }


        // Play the hurt sound effect.
        //playerAudio.Play();

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            StartCoroutine("Death");
        }
    }

    public void Heal(int amount)
    {
        previousHealth = currentHealth;
        currentHealth += amount;
        if(currentHealth >= startingHealth)
        {
            currentHealth = startingHealth;
        }
        StartCoroutine("HealthSliderAnimation");
    }



    IEnumerator Death()
    {
        if (!isDead)
        {
            anim.speed = 1;
            EndHoldAbilities();
            anim.SetBool("Dead", true);
            isDead = true;
        }
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // check  if anim is still playing
        {
            circleCollider.isTrigger = true;
            yield return new WaitForSeconds(1);
            deathMessage.color = new Vector4(1f, 1f, 1f, 1f);
            yield return null;

        }
    }

    private void EndHoldAbilities()
    {
        if (pc.abOneHold == true)
        {
            pc.abOneHold = false;
            pc.abOneScript.SendMessage("EndAbility");
        }
        if (pc.abTwoHold == true)
        {
            pc.abTwoHold = false;
            pc.abTwoScript.SendMessage("EndAbility");
        }
        if (pc.abThreeHold == true)
        {
            pc.abThreeHold = false;
            pc.abThreeScript.SendMessage("EndAbility");
        }

    }

}
