using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadSceneAsync("PlayingScene");
    }
    public void RunAway()
    {
        Application.Quit();
    }
}
