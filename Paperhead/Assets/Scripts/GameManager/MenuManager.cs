using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{


    public void LoadMainLevel()
    {
        SceneManager.LoadScene("Main");
    }

    //public void LoadOption()
    //{
    //    SceneManager.LoadScene("Settings");
    //}
}
