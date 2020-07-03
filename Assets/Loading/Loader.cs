using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{
    private class LoadingMonoBehaviour : MonoBehaviour { } //dummy class to allow us to run the Ienumerator below (Not possible insise static classes)
    public enum Scene
    {
        PreAlphaTestScene,
        Loading,
        MainMenu,
    }

    private static Action OnLoaderCallback;
    private static AsyncOperation loadingAsyncOperation;

    public static void Load(Scene scene)
    {
        //Set the loader callback to load the target scene
        OnLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object"); //create a dummy object to hold the mono behaviour created above.
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };
        //load the loading scene
        SceneManager.LoadScene(Scene.Loading.ToString());
    }


    private static IEnumerator LoadSceneAsync(Scene scene) //The loading bar is run in the coroutine
    {
        yield return null;
        loadingAsyncOperation =  SceneManager.LoadSceneAsync(scene.ToString());

        while (!loadingAsyncOperation.isDone) //each update that the game is still loading...
        {
            yield return null;
        }
    }

    public static float GetLoadingProgress()
    {
        if(loadingAsyncOperation != null)
        {
            return loadingAsyncOperation.progress;
        }
        else
        {
            return 1f;
        }
    }


    public static void LoaderCallback()
    {
        //Triggered after the first update with lets the screen refresh
        //Execute the loader callback action which will load the target scene
        if(OnLoaderCallback != null)
        {
            OnLoaderCallback();
            OnLoaderCallback = null;
        }
    }


}
