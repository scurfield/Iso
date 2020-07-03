using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVendor : MonoBehaviour
{
    public GameHandler gh;


public void SpecialEvent()
    {

        gh.SendMessage("OpenAPMenu");

    }
}
