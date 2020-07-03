using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityPurchaseManager : MonoBehaviour
{
    public GameObject confirmationWindow;
    public Button firstConfirmationButton;
    private GameObject ability;
    private GameHandler gh;
    private EventSystem es;
    public Image abilityIcon;
    private AbilityPurchaseSlot currentPurchaseSlot;

    private AbilitiesUI abUI;
    private AbilityManager am;

    private PlayerStats ps;

    public TextMeshProUGUI orbCountText;


    void Start()
    {
        abUI = GetComponentInParent<AbilitiesUI>();
        am = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityManager>();
        gh = GetComponentInParent<GameHandler>();
        es = GetComponentInParent<EventSystem>();
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public IEnumerator OnMenuOpen()
    {
        yield return null;
        orbCountText.text = "Orbs of Power: " + ps.powerOrbs;
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(gh.firstPurchaseSelected.gameObject);
        
    }
    
    public void CallPurchase()
    {
        currentPurchaseSlot = es.currentSelectedGameObject.GetComponentInParent<AbilityPurchaseSlot>();
        if (!currentPurchaseSlot.learned && ps.powerOrbs >=1)
        {
            ability = currentPurchaseSlot.ability;
            abilityIcon.sprite = ability.GetComponent<Image>().sprite;
            StartCoroutine(PurchaseThisAbility());
        }
        if(!currentPurchaseSlot.learned && ps.powerOrbs <= 0)
        {
            //not enough to buy
        }
    }
    public IEnumerator PurchaseThisAbility()
    {
        Debug.Log("Purchase This Ability.");
        confirmationWindow.SetActive(true);
        yield return null;
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(firstConfirmationButton.gameObject);
    }

    public void ConfirmSelection()
    {
        ps.powerOrbs -= 1;
        orbCountText.text = "Orbs of Power: " + ps.powerOrbs;
        confirmationWindow.SetActive(false);
        Debug.Log("You Bought " + ability);
        //Adding the ability to your learned abilities list
        am.learnedAbilities.Add(ability.gameObject);
        currentPurchaseSlot.SendMessage("LearnThisAbility");
        //once done adding, refresh the selection menu
        abUI.UpdateUiAbilities();
        StartCoroutine(OnMenuOpen());//closes the window
    }

    public void ExitConfirmation()
    {
        confirmationWindow.SetActive(false);
        Debug.Log("Cancel PUrchase");
        StartCoroutine(OnMenuOpen());

    }

}
