using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour {

    public float speed;
    public Transform objectPosition;


    void Update()
    {
        float step = speed * Time.deltaTime;     
        Vector2 position = objectPosition.position;
        position.x -= 0.5f;
        objectPosition.transform.position = Vector2.MoveTowards(objectPosition.transform.position, position, step);
    }
}