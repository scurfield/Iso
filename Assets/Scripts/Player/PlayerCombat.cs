using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// Should probably be called "PlayerAbilityInputs"
/// </summary>

public class PlayerCombat : MonoBehaviour
{
    //Core
    CircleCollider2D intBox; //intbox circleCollider
    Rigidbody2D rb;
    Animator anim;
    PlayerMovement pm;
    public bool usingAbility = false;
    [HideInInspector] public bool canRotateWhileAttacking = false;
    public int currentDamage;
    private GameHandler gh;


    //Stamina system
    public int startingStamina = 100;
    public int staminaRegenRate;
    public int currentStamina;
    public int previousStamina;
    public int staminaCost;
    public Slider staminaSlider;
    private bool stamRegenerating = false;
    private bool staminaSliderWait;
    public Slider staminaSliderAnim;

    //Health
    PlayerHealth ph;
    public bool rolling = false; //immune while rolling, health wont change

    //Melee Range
    CircleCollider2D hb; //player HitBox
    public GameObject playerIntBox;
    public Transform attackPoint;
    public float attackRange = 0.25f;
    public LayerMask enemyLayers;
    public DamageType damageType;
    [HideInInspector] public bool criticalHit;


    //Abilities
    public List<GameObject> activeAbilities;
    private GameObject abilityOne;
    private string abOneName;
    [HideInInspector] public Component abOneScript;
    private GameObject abilityTwo;
    private string abTwoName;
    [HideInInspector] public Component abTwoScript;
    private GameObject abilityThree;
    private string abThreeName;
    [HideInInspector] public Component abThreeScript;

    private AbilitiesUI abUIPanel;
    public UiAbilitySlots abUI;
    private int currentAbility;
    public float abOneCooldown;
    public float abTwoCooldown;
    public float abThreeCooldown;


    //projectiles
    public GameObject projectile;
    public float projectileSpeed;
    public Vector2 projectileDirection;
    public bool rotateProjectile;
    public bool holdCast = false;
    private float chestheight;
    public bool castFromChest;

    //Ability Icons
    public Image AbilitySlotOne;
    public Image AbilitySlotTwo;
    public Image AbilitySlotThree;
    public Image MaskInteract;
    public Image abMaskOne;
    public Image abMaskTwo;
    public Image abMaskThree;

    //Interact
    private List<GameObject> withinReach;
    public DialogManager dm;
    public GameObject currentInteraction;
    public bool talking = false;

    //Input System

    public InputAction interactAction;
    public InputAction leftAction;
    public InputAction topAction;
    public InputAction rightAction;
    public InputAction abilityPanelAction;
    public InputAction escapeAction;

    public bool abOneHold = false;
    public bool abTwoHold = false;
    public bool abThreeHold = false;

    //void Awake()
    //{
    //    interactAction.performed += InteractInput;
    //    leftAction.performed += LeftInput;
    //    topAction.performed += TopInput;
    //    rightAction.performed += RightInput;
    //    abilityPanelAction.performed += AbilityPanelInput;
    //}

    //void OnEnable()
    //{
    //    interactAction.Enable();
    //    leftAction.Enable();
    //    topAction.Enable();
    //    rightAction.Enable();
    //    abilityPanelAction.Enable();
    //}

    //void OnDisable()
    //{
    //    interactAction.performed -= InteractInput;
    //    interactAction.Disable();
    //    leftAction.performed -= LeftInput;
    //    leftAction.Disable();
    //    topAction.performed -= TopInput;
    //    topAction.Disable();
    //    rightAction.performed -= RightInput;
    //    rightAction.Disable();
    //    abilityPanelAction.performed -= AbilityPanelInput;
    //    abilityPanelAction.Disable();
    //}

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = startingStamina;
        anim = GetComponent<Animator>();
        intBox = playerIntBox.GetComponent<CircleCollider2D>();
        hb = GetComponent<CircleCollider2D>();
        ph = GetComponent<PlayerHealth>();
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        withinReach = GetComponentInChildren<IntBoxController>().withinReach;
        gh = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();
        abUIPanel = GameObject.FindGameObjectWithTag("GameController").GetComponent<AbilitiesUI>();

    }



    public void InteractInput(InputAction.CallbackContext context)
    {
        if (!ph.isDead && !pm.knockback)
        {

            if (context.performed)//This if(context.performed) seems to be required, or else the input will call started, performed, and canceled.
            {                     //This could also be useful for charging up attacks by checking both performed and canceled.
                if (gh.inEscMenu)
                {
                    //used to make selection in menu
                    
                    return;
                }
                else
                if (gh.abilityEditing)
                {
                    return;
                }
                MaskInteract.color = new Color(1f, 1f, 1f, 0.4f); //to show the button was pressed
                if (!usingAbility || !gh.abilityEditing)
                {
                    //Interact ability goes here
                    if (talking)
                    {
                        //go to next line of dialog
                        dm.DisplayNextSentence();
                    }
                    if (!talking && withinReach.Count != 0)
                    {
                        currentInteraction = withinReach[0];
                        if (currentInteraction.tag == "NPC")
                        {
                            talking = true;
                            currentInteraction.SendMessage("TriggerDialog");
                            currentInteraction.SendMessage("NPCTalk");

                        }
                        if (currentInteraction.tag == "Openable")
                        {
                            currentInteraction.SendMessage("OpenObject");
                        }
                    }
                }

                if (context.canceled)
                {
                    MaskInteract.color = new Color(1f, 1f, 1f, 0f);
                }
            }

        }
    }
    public void LeftInput(InputAction.CallbackContext context)
    {

        if (!ph.isDead && !talking && !pm.knockback)
        {
            if (context.performed)//This if(context.performed) seems to be required, or else the input will call started, performed, and canceled.
            {                 //This could also be useful for charging up attacks by checking both performed and canceled.
                abOneHold = true;
                if (!usingAbility && !gh.abilityEditing)
                {

                    //play an animation, like the button getting pressed
                    if (abMaskOne.enabled == false)
                    {
                        currentAbility = 1;
                        abilityOne = activeAbilities[0];
                        abOneName = abilityOne.name;
                        abOneScript = abilityOne.GetComponent(abOneName);
                        abOneScript.SendMessage("UseAbility", currentAbility);
                        return;
                    }
                }
                if(gh.abilityEditing){
                  activeAbilities[0] = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.GetComponent<AbilitySlot>().ability;//this find the current selected ability in the UI. Assign this to the correct active abilities slot
                  //lastly Update

                  abUIPanel.UpdateUiAbilities();
                  return;
                }
            }
            if (context.canceled && !gh.abilityEditing)
            {
                abOneHold = false;
                abOneScript.SendMessage("EndAbility");
            }
        }
    }
    public void TopInput(InputAction.CallbackContext context)
    {
        if (!ph.isDead && !talking && !pm.knockback)
        {
            if (context.performed)//This if(context.performed) seems to be required, or else the input will call started, performed, and canceled.
            {            //This could also be useful for charging up attacks by checking both performed and canceled.
                abTwoHold = true;
                if (!usingAbility && !gh.abilityEditing)
                {

                    //play an animation, like the button getting pressed
                    if (abMaskTwo.enabled == false)
                    {
                        currentAbility = 2;
                        abilityTwo = activeAbilities[1];
                        abTwoName = abilityTwo.name;
                        abTwoScript = abilityTwo.GetComponent(abTwoName);
                        abTwoScript.SendMessage("UseAbility", currentAbility);
                    }
                }
                if(gh.abilityEditing){
                  activeAbilities[1] = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.GetComponent<AbilitySlot>().ability;//this find the current selected ability in the UI. Assign this to the correct active abilities slot
                  //lastly Update

                  abUIPanel.UpdateUiAbilities();
                  return;
                }
            }
            if (context.canceled)
            {
                abTwoHold = false;
                abTwoScript.SendMessage("EndAbility");
            }
        }
    }
    public void RightInput(InputAction.CallbackContext context)
    {
        if (!ph.isDead && !talking && !pm.knockback)
        {
            if (context.performed)//This if(context.performed) seems to be required, or else the input will call started, performed, and canceled.
            {                  //This could also be useful for charging up attacks by checking both performed and canceled.
                abThreeHold = true;
                if (!usingAbility && !gh.abilityEditing)
                {

                    //play an animation, like the button getting pressed
                    if (abMaskThree.enabled == false)
                    {
                        currentAbility = 3;
                        abilityThree = activeAbilities[2];
                        abThreeName = abilityThree.name;
                        abThreeScript = abilityThree.GetComponent(abThreeName);
                        abThreeScript.SendMessage("UseAbility", currentAbility);
                    }
                }
                if(gh.abilityEditing){
                  activeAbilities[2] = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.GetComponent<AbilitySlot>().ability;//this find the current selected ability in the UI. Assign this to the correct active abilities slot
                  //lastly Update

                  abUIPanel.UpdateUiAbilities();
                  return;
                }
            }
            if (context.canceled)
            {
                abThreeHold = false;
                abThreeScript.SendMessage("EndAbility");
            }
        }
    }
    public void AbilityPanelInput(InputAction.CallbackContext context)
    {
        if (!ph.isDead && !talking)
        {
            if (context.performed)//This if(context.performed) seems to be required, or else the input will call started, performed, and canceled.
            {                    //This could also be useful for charging up attacks by checking both performed and canceled.
                if (!usingAbility && !gh.purchasing)
                {
                    gh.OpenAbilityPanel();
                }
                if (gh.purchasing)
                {
                    gh.OpenAPMenu();
                }
            }
            if (context.canceled)
            {

            }
        }
    }

    public void EscapeMenuInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Esc Pressed");
            gh.OpenEscMenu();
        }
        if (context.canceled)
        {

        }
        
    }


    // Update is called once per frame
    void Update()
    {
        if (!ph.isDead)
        {
            //if (staminaSlider.value > currentStamina)
            //{
            //    staminaSlider.value = currentStamina;
            //}

            if (currentStamina == startingStamina)
            {
                stamRegenerating = false;
            }


            if (!usingAbility || !gh.abilityEditing)
            {
                //passive stamina regeneration
                StartCoroutine("StaminaRegen");

            }


            }

        }

    
    public void UseStamina(int amount)
    {
        previousStamina = currentStamina;
        currentStamina -= amount;
        // Set the health bar's value to the current health.
        staminaSlider.value = currentStamina;
        StartCoroutine("StaminaSliderAnimation");
    }

    public void StaminaHeal(int amount)
    {   
        previousStamina = currentStamina;
        currentStamina += amount;
        if (currentStamina >= startingStamina)
        {
            currentStamina = startingStamina;
        }
        StartCoroutine("StaminaSliderAnimation");
    }

    IEnumerator StaminaSliderAnimation()
    {
        //if (!staminaSliderWait)
        //{
        //    staminaSliderWait = true;
        //    yield return new WaitForSeconds(0.5f);
        //}
        while (previousStamina < currentStamina)
        {
            previousStamina += 10;
            staminaSlider.value = previousStamina;
            staminaSliderAnim.value = previousStamina;
            yield return new WaitForSeconds(0.01f);
        }
        while (previousStamina > currentStamina)
        {
            previousStamina -= 5;
            staminaSlider.value = currentStamina;
            staminaSliderAnim.value = previousStamina;
            yield return new WaitForSeconds(0.01f);
        }

    }
    IEnumerator StaminaRegen()
    {
        if(currentStamina < startingStamina && !usingAbility && !stamRegenerating)
        {
            stamRegenerating = true;
            currentStamina += 10;
            //staminaSlider.value = currentStamina;
            StartCoroutine("StaminaSliderAnimation");
            yield return new WaitForSeconds(0.5f);
            stamRegenerating = false;
        }
        yield return null;
    }


    public void UpdateCooldowns(float cooldown)
    {
        if(currentAbility == 1)
        {
            abOneCooldown = cooldown;
            abUI.StartCooldown(1);
        }
        if (currentAbility == 2)
        {
            abTwoCooldown = cooldown;
            abUI.StartCooldown(2);
        }
        if (currentAbility == 3)
        {
            abThreeCooldown = cooldown;
            abUI.StartCooldown(3);
        }
    }

    public IEnumerator StopTalking()//Needed to reset the talking bool to stop the text box from looping
    {
        yield return new WaitForEndOfFrame();
        talking = false;
    }


    //Animation Functions
    //Below are the trigger effects called by certain frames in each player animation

    void DamageEnemiesMelee()
    {
        //All melee range abilities should be called to this function
        //They will only hit within the interact box


        //detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(currentDamage, damageType, criticalHit, playerIntBox.transform.position + new Vector3(0f, 0.4f, 0f));
            //Debug.Log("We hit " + enemy.name);
        }
    }

    void InstantiateProjectile()
    {
        //All spells that generate a prefab that will be shot call here
        //The Spell Ability prefab will have to assign which prefab is to be shot by changing variable in this part of the script.
        if (!holdCast)
        {

            //rotation and instatiation
            Vector3 vectorToTarget = gameObject.transform.position - attackPoint.position;
            if(pm.lastVert < 0 || !castFromChest)
            {
                chestheight = 0f;
            }
            else
            {
                chestheight = 0.4f;
            }
            if (rotateProjectile == true)
            {
                Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 180) * vectorToTarget;
                Instantiate(projectile, new Vector3(attackPoint.position.x, attackPoint.position.y + chestheight, attackPoint.position.z), Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget)); // .y + 0.5f to be at chest height
            }
            else
            {
                Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 180) * vectorToTarget;
                Instantiate(projectile, new Vector3(attackPoint.position.x, attackPoint.position.y + chestheight, attackPoint.position.z), Quaternion.identity); // .y + 0.5f to be at chest height
            }

            projectileDirection = (Vector2)(attackPoint.position - gameObject.transform.position).normalized;
            Debug.Log("Sending " + projectileDirection);

        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
