using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionManager : MonoBehaviour {

    public void LoadMainLevel()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoForward()
    {
        ///
    }
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
