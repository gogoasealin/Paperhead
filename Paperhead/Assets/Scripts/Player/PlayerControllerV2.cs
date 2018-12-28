using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerControllerV2 : MonoBehaviour {

    public float speed;
    private Rigidbody2D rb2d;
    private GameController gameControllerScript;
    public GameObject gameController;
    public Joystick moveJoystick;
    public bool faceRight;
    public bool pause;
    public bool resume;
    public bool dash;
    public int dashSpeed;
    public float timerDash;
    private int timerPause;
    private Vector2 dir;
    private Quaternion movingLeft = Quaternion.Euler(0, 180, 0);
    private Quaternion movingRight = Quaternion.Euler(0, 0, 0);
    private bool playerOnJoystickPosition;
    public GameObject PauseWindow;
    public BoxCollider2D boxCollider2d;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        gameControllerScript = gameController.GetComponent<GameController>();
        faceRight = false;
        pause = false;
        resume = false;
        dash = false;
        timerDash = 0;
        dashSpeed = 1;
        dir = Vector2.zero;
    }


    void Update()
    {


        if (moveJoystick.InputDirection != Vector3.zero)
        {
            if (dir.x < 0)
            {
                gameObject.transform.rotation = movingLeft;
            }
            else
            {
                gameObject.transform.rotation = movingRight;
            }

            dir.x = moveJoystick.InputDirection.x * dashSpeed;
            dir.y = moveJoystick.InputDirection.z * dashSpeed;
        }
        else dir = Vector2.zero;

        if (CrossPlatformInputManager.GetButtonDown("Horizontal"))
        {
            dash = true;
        }
        //else if (Input.GetKeyDown(KeyCode.E))
        //{
        //    dash = true;
        //}


        if (dash)
        {
            timerDash += Time.deltaTime;
            dashSpeed = 5;
            if (timerDash > 0.1f)
            {
                dashSpeed = 1;
                dash = false;
                timerDash = 0;
            }
        }

        CheckPlayerPosition();
        JoystickInvisible();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameControllerScript.SwitchPause();
        }
        if (pause)
        {
            gameControllerScript.Pause();
        }
        else if (!pause)
        {
            if (resume)
            {
                gameControllerScript.ResumeButton();
            }
        }

    }

    private void FixedUpdate()
    {
        rb2d.velocity = dir * speed;
    }

    private void CheckPlayerPosition()
    {
        Vector3 tmpPos = Camera.main.WorldToScreenPoint(transform.position);
        if (tmpPos.x < (Screen.width/3) && tmpPos.y < (Screen.height / 3))
        {
            playerOnJoystickPosition = true;
        }else playerOnJoystickPosition = false;
        //Debug.Log(tmpPos.x + " " + (Screen.width / 3) + " " + tmpPos.y + " " + (Screen.height / 3));
    }

    private void JoystickInvisible()
    {
        if (playerOnJoystickPosition)
        {
            Color tempColor = new Color(1f, 1f, 1f, 0.1765f);
            moveJoystick.bgImg.color = tempColor;
            moveJoystick.joystickImg.color = tempColor;
        }
        else
        {
            moveJoystick.bgImg.color = new Color(255, 255, 255, 255);
            moveJoystick.joystickImg.color = new Color(255, 255, 255, 255);
        }
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        gameControllerScript.GameOver();       
    }

}
