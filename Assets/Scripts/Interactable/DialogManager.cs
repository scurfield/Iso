using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private Queue<string> sentences;
    private GameObject player;
    private PlayerMovement pm;
    private PlayerCombat pc;
    public Animator anim;

    public Text nameText;
    public Text dialogText;
    public bool textCrawl = false;
    public char[] displayingSentence;
    public int sentenceLength;
    public string currentSentence;
    public Text continueEndText;
    public GameObject continueEnd;
    public string currentContEndText;
    private bool farewellText;
    private GameObject currentObject;
    private bool endEvent;

    void Start()
    {
        sentences = new Queue<string>();
        player = GameObject.FindGameObjectWithTag("Player");
        pm = player.GetComponent<PlayerMovement>();
        pc = player.GetComponent<PlayerCombat>();
    }

    public void StartDialog(Dialog dialog, GameObject obj, bool hasEvent)
    {
        pm.currentState = PlayerState.interact;
        farewellText = dialog.farewellText;
        currentObject = obj;
        endEvent = hasEvent;

        continueEnd.SetActive(false);
        anim.SetBool("TextOpen", true);
        nameText.text = dialog.name;
        sentences.Clear();

        foreach(string sentence in dialog.sentences)
        {
           
            sentences.Enqueue(sentence);

        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if (textCrawl)
        {
            StopAllCoroutines();//to interrupt current anim if the next line is called
            dialogText.text = currentSentence;
            textCrawl = false;
            ContinueEnd();
            return;
        }
        if (!textCrawl)
        {
            if(sentences.Count == 0)
            {
                EndDialog();
                return;
            }
            continueEnd.SetActive(false);
            string sentence = sentences.Dequeue();
            StartCoroutine(TypeSentence(sentence));
        }
    }

    IEnumerator TypeSentence(string displayingSentence)
    {
        currentSentence = displayingSentence;
        dialogText.text = "";
        textCrawl = true;
        foreach(char letter in displayingSentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.03f);
            if(dialogText.text.ToCharArray().Length == displayingSentence.ToCharArray().Length)
            {
                textCrawl = false;
                ContinueEnd();
            }
        }


    }

    private void ContinueEnd()
    {
        currentContEndText = "Continue";
        if (sentences.Count == 0 && !endEvent)
        {
            if (farewellText)
            {
                currentContEndText = "Farewell";
            }
            else
            {
                currentContEndText = "Leave";
            }
        }
        continueEndText.text = currentContEndText;
        continueEnd.SetActive(true);
    }



    void EndDialog()
    {
        anim.SetBool("TextOpen", false);
        if (endEvent)
        {
            currentObject.SendMessage("SpecialEvent");//can be used to open a shop panel, initiate a cutscene, (execute anything) after finishing talking to this NPC.
        }
        pm.currentState = PlayerState.active;
        //nameText.text = null;
        //dialogText.text = null;
        pc.currentInteraction.SendMessage("EndDialog"); //ends dialog state in NpcController
        pc.StartCoroutine("StopTalking");
    }

}
