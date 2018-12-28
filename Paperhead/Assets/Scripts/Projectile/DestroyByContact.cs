using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject gameController;
    private GameController gameControllerScript;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameControllerScript = gameController.GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.SetActive(false);
            gameControllerScript.GameOver();
        }
    }
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.tag == "BackGround")
    //    {
    //        gameController.UpdateScore(1);
    //        Destroy(this.gameObject);
    //    }

    //}
}
