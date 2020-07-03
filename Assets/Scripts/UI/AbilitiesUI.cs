using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesUI : MonoBehaviour
{
    public Sprite selectLeft;
    public Sprite selectTop;
    public Sprite selectRight;

    private Image slotSelectionLeft;
    private Image slotSelectionTop;
    private Image slotSelectionRight;

    private int currentSlot;
    private int currentActiveAbility;

    public List<GameObject> learnedAbilities;
    public List<GameObject> activeAbilities;

    private GameObject player;
    private AbilityManager am;

    public Transform abilitiesParent;
    AbilitySlot[] slots;

    public GameObject currentSelectionSlot;
    public UiAbilitySlots abUI;

    private bool abilityFound = false;




    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        am = player.GetComponent<AbilityManager>();

        slots = abilitiesParent.GetComponentsInChildren<AbilitySlot>();


        activeAbilities = player.GetComponent<PlayerCombat>().activeAbilities;



        UpdateUiAbilities();
    }

    public void UpdateUiAbilities()
    {

        activeAbilities = player.GetComponent<PlayerCombat>().activeAbilities;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < am.learnedAbilities.Count)
            {
                slots[i].AddAbility(am.learnedAbilities[i]);
                currentSlot = i;
                abilityFound = false;
                CompareAbilitySlots(am.learnedAbilities[i]);
            }
            else
            {
                slots[i].ClearAbility();
            }
        }
        abUI.UpdateSlots();
    }

    private void CompareAbilitySlots(GameObject learnedAbility)
    {

        currentSelectionSlot = slots[currentSlot].gameObject.transform.Find("SlotButton/SlotSelectionIcon").gameObject;
        currentSelectionSlot.GetComponent<Image>().enabled = false;
        for (int i = 0; i < activeAbilities.Count; i++)
        {
            if (abilityFound == false)
            {
                if (activeAbilities[i].name == learnedAbility.name)
                {
                    abilityFound = true;
                    currentSelectionSlot.GetComponent<Image>().enabled = true;

                    if (i == 0)
                    {

                        currentSelectionSlot.GetComponent<Image>().sprite = selectLeft;
                    }
                    if (i == 1)
                    {
                        currentSelectionSlot.GetComponent<Image>().sprite = selectTop;
                    }
                    if (i == 2)
                    {
                        currentSelectionSlot.GetComponent<Image>().sprite = selectRight;
                    }

                }
            }
        }


    }

    public void SelectedAbilityButton()
    {

    }

}
