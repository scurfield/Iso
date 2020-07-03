using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMainMenu : MonoBehaviour
{


    public void LoadMenu()
    {
        Debug.Log("Click Main Menu BUtton");
        Loader.Load(Loader.Scene.MainMenu);
    }

}
