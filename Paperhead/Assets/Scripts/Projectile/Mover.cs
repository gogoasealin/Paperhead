using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{

    public float speed;
    public float rotatingSpeed = 200f;
    private Rigidbody2D rb2d;
    public GameObject target;
    public bool rotateToPlayer = false;
    public float timer = 0;

    void Start()
    {

        rb2d = GetComponent<Rigidbody2D>();  
        target = GameObject.FindGameObjectWithTag("Player");
        rb2d.AddForce(transform.right * speed);
    }

    private void Update()
    {

        if (rotateToPlayer)
        {
            if (target != null)
            {
                Vector2 point2Target = (Vector2)transform.position - (Vector2)target.transform.position;
                point2Target.Normalize();
                float value = Vector3.Cross(point2Target, transform.right).z;
                if (value > 0)
                {
                    rb2d.angularVelocity = rotatingSpeed;

                }
                else if (value < 0)
                {
                    rb2d.angularVelocity = -rotatingSpeed;
                }
            }

            
            rb2d.velocity = transform.right * 10f;

            timer += Time.deltaTime;
            if (timer > 2f)
            {
                rotateToPlayer = false;
                rb2d.angularVelocity = 0f;
                Destroy(GetComponent<Mover>());
            }
        }
    }

}


