using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInCircle : MonoBehaviour {

    private float RotateSpeed = 3.5f;
    private float Radius = 5f;

    private Vector2 _centre;
    private float _angle;

    public bool onPlayer = false;

    private void Start()
    {
        _centre = transform.position;
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            _angle += RotateSpeed * Time.deltaTime;

            Vector2 offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
            transform.position = (_centre + offset) ;
            if (onPlayer)
            {
                Radius += 0.03f;
            }
        }
    }
}
