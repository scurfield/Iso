using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPurchaseSlot : MonoBehaviour
{
    public Image icon;
    public GameObject ability;
    private Button button;
    public bool learned;
    private GameObject player;
    public GameObject ringGlow;

    void Start()
    {
        button = gameObject.GetComponentInChildren<Button>();
        icon.sprite = ability.GetComponent<Image>().sprite;
        if (learned)
        {
            ringGlow.SetActive(true);
        }
        else
        {
            ringGlow.SetActive(false);
        }

    }

    public void LearnThisAbility()
    {
        learned = true;
        ringGlow.SetActive(true);
    }

}
