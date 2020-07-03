using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Dialog dialog;
    public bool triggerEvent;

    public void TriggerDialog()
    {
        FindObjectOfType<DialogManager>().StartDialog(dialog, gameObject, triggerEvent);
    }
}
