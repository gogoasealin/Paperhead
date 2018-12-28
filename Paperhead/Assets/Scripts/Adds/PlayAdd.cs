using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;



public class PlayAdd : MonoBehaviour
{

#if UNITY_ANDROID
    private string gameId = "1618531";
#elif UNITY_IOS
    private string gameId = "1618532";
#endif



    private GameObject gameManager;
    private GameManager gameManagerScript;
    private GameObject gameController;
    private GameController gameControllerScript;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameControllerScript = gameController.GetComponent<GameController>();
    }

    public void InitializeAdd()
    {
        Advertisement.Initialize(gameId);
    }

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("video", new ShowOptions() { resultCallback = HandleAdResult });
        }else
        {
            gameManagerScript.Save();
            gameControllerScript.GameOverWindow.SetActive(true);
        }
    }

    private void HandleAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                gameManagerScript.Save();
                gameControllerScript.GameOverWindow.SetActive(true);
                break;
            case ShowResult.Skipped:
                gameManagerScript.Save();
                gameControllerScript.GameOverWindow.SetActive(true);
                break;
            case ShowResult.Failed:
                gameManagerScript.Save();
                gameControllerScript.GameOverWindow.SetActive(true);
                break;
        }
    }
}
