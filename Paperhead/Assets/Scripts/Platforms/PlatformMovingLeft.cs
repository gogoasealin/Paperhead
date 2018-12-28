using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovingLeft : MonoBehaviour {

    public float speed;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        rb2d.AddForce(transform.right * speed);
    }
}
