using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    public GameObject ability;
    public Image icon;
    public Image frame;
    private bool isEnabled = false;
    public Button button;


    private void Awake()
    {
        button = gameObject.GetComponentInChildren<Button>();
        button.interactable = false;
    }

    public void AddAbility(GameObject newAbility)
    {
        ability = newAbility;
        icon.sprite = ability.GetComponent<Image>().sprite;
        button.interactable = true ;
        button.enabled = true;
        icon.enabled = true;
        frame.enabled = true;
        isEnabled = true;

    }

    public void ClearAbility()
    {
        ability = null;
        icon.sprite = null;
        icon.enabled = false;
        button.enabled = false;
        frame.enabled = false;
    }

    private void Update()
    {
        if (isEnabled)
        {
            button.interactable = true;
        }
    }


}
