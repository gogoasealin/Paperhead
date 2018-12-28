using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformReSize : MonoBehaviour {

//    35 .... 0.1
//    2.485 ... x   =) x = 0.1 * 2.485 / 35 

//    35 ... 0.1
//    0.9 ... 

    public GameObject platforma;
    public GameObject Player;
    private PlayerController playerController;

    private void Start()
    {
        playerController = Player.GetComponent<PlayerController>();
    }


	void Update () {
        if (!playerController.pause)
        {
            if (platforma.transform.localScale.x > 0.4)
            {
                Vector3 scale = platforma.transform.localScale;
                scale.x -= 0.0015f;
                platforma.transform.localScale = scale;
            }
            else if (platforma.transform.localScale.x <= 0.4)
            {
                Destroy(GetComponent<PlatformReSize>());
            }
        }
    }
    
	

}
