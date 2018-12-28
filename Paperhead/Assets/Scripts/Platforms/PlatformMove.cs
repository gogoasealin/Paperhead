using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour {

    public GameObject platform;

    public float moveSpeed;

    public Transform currentPoint;
    public Transform[] points;
    public bool goToCenter;
    public int pointSelection;


	void Start () {
        currentPoint = points[pointSelection];
        pointSelection = Random.Range(0, points.Length);
    }
	

	void Update () {
        if (!goToCenter)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);

            if (platform.transform.position == currentPoint.position)
            {
                pointSelection = Random.Range(0, points.Length);
            }
            currentPoint = points[pointSelection];
        }
        else if (goToCenter)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, points[2].position, Time.deltaTime * moveSpeed);
            if(platform.transform.position == points[2].position)
            {
                goToCenter = false;
                GetComponent<PlatformMove>().enabled = false;
            }
        }
	}

}
