using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameHandler : MonoBehaviour
{

    public CameraFollow cameraFollow;
    public Transform playerTransform;
    private Animator abilityPanelAnim;
    private Animator playerAnim;
    private PlayerMovement pm;
    public Button firstAbilityselected;
    public Button firstEscMenuSelected;
    public Button firstPurchaseSelected;
    public Button previousSelection;
    public GameObject EscapeMenu;
    public GameObject AbilityPurchaseMenu;
    public GameObject AbilityPanel;
    private AbilityPurchaseManager aPM;
    private EventSystem es;
    
    public bool purchasing = false;
    public bool abilityEditing = false;
    public bool inEscMenu = false;

    // Start is called before the first frame update
    private void Start()
    {
        EscapeMenu.SetActive(false);
        AbilityPurchaseMenu.SetActive(false);
        es = gameObject.GetComponent<EventSystem>();

        aPM = AbilityPurchaseMenu.GetComponent<AbilityPurchaseManager>();
        abilityPanelAnim = gameObject.transform.Find("AbilitySwapPanel").gameObject.GetComponent<Animator>();
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        cameraFollow.Setup(() => playerTransform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenAbilityPanel()
    {
        if (!inEscMenu)
        {
            firstAbilityselected.Select();
            //animate to open panel
            abilityPanelAnim.SetTrigger("PanelOpen");
            if (!abilityEditing)
            {
                //AbilityPanel.SetActive(true);
                playerAnim.SetBool("Moving", false);
                abilityEditing = true;
                return;
            }
            if (abilityEditing)
            {
                //AbilityPanel.SetActive(false);
                abilityEditing = false;
                return;
            }

        }


    }

    public void OpenEscMenu()
    {
        
        if (!inEscMenu)
        {
            Debug.Log("Check1");
            //firstEscMenuSelected.Select();
            playerAnim.SetBool("Moving", false);
            inEscMenu = true;
            EscapeMenu.SetActive(true);
            StartCoroutine(SelectOnOpen(firstEscMenuSelected));
            return;
        }
        if (inEscMenu)
        {
            if (abilityEditing && !purchasing)
            {
                StartCoroutine(SelectOnOpen(firstAbilityselected));
            }
            if (purchasing)
            {
                StartCoroutine(SelectOnOpen(firstPurchaseSelected));
            }
            Debug.Log("Check2");
            EscapeMenu.SetActive(false);
            inEscMenu = false;
            return;
        }
    }

    public void OpenAPMenu()
    {
        playerAnim.SetBool("Moving", false);
        if (!abilityEditing && !purchasing)
        {
            playerAnim.SetBool("Moving", false);
            abilityEditing = true;
            purchasing = true;
            //firstPurchaseSelected.Select();
            AbilityPurchaseMenu.SetActive(true);
            aPM.SendMessage("OnMenuOpen");
            return;
        }
        if (abilityEditing || purchasing) 
        {
            AbilityPurchaseMenu.SetActive(false) ;
            abilityEditing = false;
            purchasing = false;
            return;
        }
    }

    private IEnumerator SelectOnOpen(Button button)
    {
        yield return null;
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(button.gameObject);
    }


}
