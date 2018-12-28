using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameManager : MonoBehaviour {

    public static GameManager gameManager;
    public GameObject gameController;
    public GameController gameControllerScript;
    public GameObject gameControllerInMenu;

    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    

    public void Save()
    {
      
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerinfo.dat");

        PlayerInfo data = new PlayerInfo();

        data.numberOfAllGames = gameControllerScript.numberOfAllGames;
        if (data.highScore < gameControllerScript.highScore)
        {
            data.highScore = gameControllerScript.highScore;
        }


        bf.Serialize(file, data);
        file.Close();


    }


    public void Load()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameControllerScript = gameController.GetComponent<GameController>();
        if (File.Exists(Application.persistentDataPath + "/playerinfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);
            PlayerInfo data = (PlayerInfo)bf.Deserialize(file);
            file.Close();
            gameControllerScript.numberOfAllGames = data.numberOfAllGames;
            gameControllerScript.highScore = data.highScore;

        }
    }
}

[System.Serializable]
class PlayerInfo
{
    public int highScore;
    public int numberOfAllGames;
}



