using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovingLeftInstantiate : MonoBehaviour {

    public bool stop;
    public float timer;
    public GameObject platformInstatiateMovingLeft;
    public GameObject platforms;
    public Transform[] points;
    private int position;
    private int last;

    void Start () {
        stop = false;
        timer = 1;
        position = 0;
        last = 0;
    }
	
	
	void Update () {
        timer += Time.deltaTime;
        if(timer > 2.8)
        {
            position = Random.Range(1, 24);
            position %= 3;
            if (position == 0 && position != last)
            {
                last = position;
                GameObject platforms3;
                Vector2 spawnPosition = new Vector2(25f, -5f);
                platforms3 = Instantiate(platformInstatiateMovingLeft,spawnPosition, Quaternion.identity);
                platforms3.transform.parent = platforms.transform;
                timer = 0;
            }
            else if (position == 1 && position != last)
            {
                last = position;
                GameObject platforms3;
                Vector2 spawnPosition = new Vector2(25f, -1.5f);
                platforms3 = Instantiate(platformInstatiateMovingLeft, spawnPosition, Quaternion.identity);
                platforms3.transform.parent = platforms.transform;
                timer = 0;
            }
            else if( position == 2 && position != last)
            {
                last = position;
                GameObject platforms3;
                Vector2 spawnPosition = new Vector2(25f, 4f);
                platforms3 = Instantiate(platformInstatiateMovingLeft, spawnPosition, Quaternion.identity);
                platforms3.transform.parent = platforms.transform;
                timer = 0;
            }

        }
        if (stop)
        {
            Destroy(GetComponent<PlatformMovingLeftInstantiate>());
        }
	}
}
