using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiAbilitySlots : MonoBehaviour
{
    //Ability Icons
    public Image AbilitySlotOne;
    public Image AbilitySlotTwo;
    public Image AbilitySlotThree;

    private GameObject abOne;
    private GameObject abTwo;
    private GameObject abThree;

    public Image abMaskOne;
    public Image abMaskTwo;
    public Image abMaskThree;

    public float abOneTimer;
    public float abTwoTimer;
    public float abThreeTimer;

    public float abOneNextReadyTime;
    public float abTwoNextReadyTime;
    public float abThreeNextReadyTime;


    public GameObject player;
    PlayerCombat pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = player.GetComponent<PlayerCombat>();
        abOneReady();
        abTwoReady();
        abThreeReady();
        UpdateSlots();
    }

    // Update is called once per frame

    public void Update()
    {
        //ability One
        bool abOneCdComplete = (Time.time > abOneNextReadyTime);
        if (abOneCdComplete)
        {
            abOneReady();
        }
        else
        {
            abOneCooldown();
        }
        //ability Two
        bool abTwoCdComplete = (Time.time > abTwoNextReadyTime);
        if (abTwoCdComplete)
        {
            abTwoReady();
        }
        else
        {
            abTwoCooldown();
        }
        //ability Three
        bool abThreeCdComplete = (Time.time > abThreeNextReadyTime);
        if (abThreeCdComplete)
        {
            abThreeReady();
        }
        else
        {
            abThreeCooldown();
        }
    }

    private void abOneReady()
    {
        abMaskOne.enabled = false; //PlayerCombat will need to check if this is false before attacking.
    }
    private void abTwoReady()
    {
        abMaskTwo.enabled = false; //PlayerCombat will need to check if this is false before attacking.
    }
    private void abThreeReady()
    {
        abMaskThree.enabled = false; //PlayerCombat will need to check if this is false before attacking.
    }

    private void abOneCooldown()
    {
        abOneTimer -= Time.deltaTime;
        float roundedCd = Mathf.Round(abOneTimer);
        abMaskOne.fillAmount = (abOneTimer / pc.abOneCooldown);
    }
    private void abTwoCooldown()
    {
        abTwoTimer -= Time.deltaTime;
        float roundedCd = Mathf.Round(abTwoTimer);
        abMaskTwo.fillAmount = (abTwoTimer / pc.abTwoCooldown);
    }
    private void abThreeCooldown()
    {
        abThreeTimer -= Time.deltaTime;
        float roundedCd = Mathf.Round(abThreeTimer);
        abMaskThree.fillAmount = (abThreeTimer / pc.abThreeCooldown);
    }

    public void StartCooldown(int abilityNumber)
    {
        if(abilityNumber == 1)
        {
            abOneTimer = pc.abOneCooldown;
            abMaskOne.enabled = true;
            abOneNextReadyTime = pc.abOneCooldown + Time.time;
        }
        if (abilityNumber == 2)
        {
            abTwoTimer = pc.abTwoCooldown;
            abMaskTwo.enabled = true;
            abTwoNextReadyTime = pc.abTwoCooldown + Time.time;
        }
        if (abilityNumber == 3)
        {
            abThreeTimer = pc.abThreeCooldown;
            abMaskThree.enabled = true;
            abThreeNextReadyTime = pc.abThreeCooldown + Time.time;
        }
    }


    public void UpdateSlots() //updates the gameplay buttons to the top three abilities on the Ability Prefabs list
    {
        pc = player.GetComponent<PlayerCombat>();
        //Ability One
        if (pc.activeAbilities[0] != null)
        {
            abOne = pc.activeAbilities[0];
            AbilitySlotOne.sprite = abOne.GetComponent<Image>().sprite;
            AbilitySlotOne.color = new Color(1f, 1f, 1f, 1f);
            StartCooldown(1);//when ability swapped out, must wait for cooldown.
        }
        else
        {
            AbilitySlotOne.color = new Color(1f, 1f, 1f, 0f);
        }
        //Ability Two
        if (pc.activeAbilities[1] != null)
        {
            abTwo = pc.activeAbilities[1];
            AbilitySlotTwo.sprite = abTwo.GetComponent<Image>().sprite;
            AbilitySlotTwo.color = new Color(1f, 1f, 1f, 1f);
            StartCooldown(2);//when ability swapped out, must wait for cooldown.
        }
        else
        {
            AbilitySlotTwo.color = new Color(1f, 1f, 1f, 0f);
        }
        //Ability Three
        if (pc.activeAbilities[2] != null)
        {
            abThree = pc.activeAbilities[2];
            AbilitySlotThree.sprite = abThree.GetComponent<Image>().sprite;
            AbilitySlotThree.color = new Color(1f, 1f, 1f, 1f);
            StartCooldown(3);//when ability swapped out, must wait for cooldown.
        }
        else
        {
            AbilitySlotThree.color = new Color(1f, 1f, 1f, 0f);
        }
    }
}
