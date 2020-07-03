using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private PlayerCombat pc;
    private AbilitiesUI abilitiesUI;
    private UiAbilitySlots abilitySlotsUI;
    public List<GameObject> learnedAbilities;
    

    // Start is called before the first frame update
    void Start()
    {
        UpdateAbilities();
    }

    public void UpdateAbilities()
    {
        //this process is called whenever any ability management changes
        //When a new ability is learned 
        //When the player moves around abilities

        //Update the gameplay buttons last
        //abilitySlotsUI.UpdateSlots();
    }
}
