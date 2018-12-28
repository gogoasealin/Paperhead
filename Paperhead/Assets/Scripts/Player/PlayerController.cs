using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float dirX;
    public float dirY;
    public float downSpeed;
    public float jumpForce;
    private Rigidbody2D rb2d;
    private bool canJump;
    private GameController gameControllerScript;
    public GameObject gameController;
    public Joystick moveJoystick;
    public int jumpNumber;
    public bool jumpDestroy;
    public bool faceRight;
    public bool pause;
    public bool resume;
    public bool dash;
    public int dashSpeed;
    public float timerDash;
    private float timergoDown;
    public bool destroy;
    public PlayerControllerV2 playerControllerV2;
    public bool goDown;
    private Quaternion movingLeft = Quaternion.Euler(0, 180, 0);
    private Quaternion movingRight = Quaternion.Euler(0, 0, 0);
    //public bool goUp;
    //private float timergoUp;
    private bool playerOnJoystickPosition;

    void Awake () {
        rb2d = GetComponent<Rigidbody2D>();
        gameControllerScript = gameController.GetComponent<GameController>();
        playerControllerV2 = GetComponent<PlayerControllerV2>();
        playerControllerV2.enabled = false;
        destroy = false;
        jumpNumber = 0;
        jumpDestroy = false;
        faceRight = false;
        pause = false;
        resume = false;
        dash = false;
        timerDash = 0;
        dashSpeed = 1;
        timergoDown = 0;
        goDown = false;
        //timergoUp = 0;
        //goUp = false;
    }
	
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.S))
        {
            Down();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoJump();
        }
        if (!jumpDestroy)
        {        
            dirX = CrossPlatformInputManager.GetAxis("Horizontal");
            if (moveJoystick.InputDirection != Vector3.zero)
            {
                dirX = moveJoystick.InputDirection.x * dashSpeed;
                if(dirX < 0)
                {
                    gameObject.transform.rotation = movingLeft;
                }else
                {
                    gameObject.transform.rotation = movingRight;
                }
            }
            else
            {
                dirX = 0;
            }
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                DoJump();
            }
            if (CrossPlatformInputManager.GetButtonDown("Down"))
            {
                Down();
            }
        }
        else if (jumpDestroy)
        {
            dirX = CrossPlatformInputManager.GetAxis("Horizontal");
            dirY = CrossPlatformInputManager.GetAxis("Vertical");
            if (moveJoystick.InputDirection != Vector3.zero)
            {
                dirX = moveJoystick.InputDirection.x * dashSpeed;
                if (dirX < 0)
                {
                    gameObject.transform.rotation = movingLeft;
                }
                else
                {
                    gameObject.transform.rotation = movingRight;
                }
                if (moveJoystick.InputDirection.x == 0)
                {
                    gameObject.transform.rotation = movingRight;
                    dirX = 0;
                }
            }
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
            if (destroy)
            {
                playerControllerV2.enabled = true;
                rb2d.gravityScale = 0;
                Destroy(GetComponent<PlayerController>());
            }
        }
        if (goDown)
        {
            timergoDown += Time.deltaTime;
            if (timergoDown > 0.05f)
            {
                goDown = false;
                timergoDown = 0;
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

    public void FixedUpdate()
    {
        rb2d.velocity = new Vector2(dirX * speed, rb2d.velocity.y);
    }

    public void DoJump()
    {
        if (canJump)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            //rb2d.gravityScale = 0;
            //goUp = true;
            jumpNumber++;
            if(jumpNumber == 2)
            {
                canJump = false;
                jumpNumber = 0;
            }
        }
    }

    public void Down()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, -downSpeed);
        goDown = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            canJump = true;
            jumpNumber = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            if (goDown)
            {
                other.collider.isTrigger = true;
                goDown = false;
                rb2d.velocity = new Vector2(rb2d.velocity.x, -downSpeed);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            other.isTrigger = false;
        }
    }

    private void CheckPlayerPosition()
    {
        Vector3 tmpPos = Camera.main.WorldToScreenPoint(transform.position);
        if (tmpPos.x < (Screen.width / 3) && tmpPos.y < (Screen.height / 3))
        {
            playerOnJoystickPosition = true;
        }
        else playerOnJoystickPosition = false;
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
