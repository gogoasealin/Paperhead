using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHp : MonoBehaviour {

    public bool moved = true;
    public int hpBoss;
    public GameObject gameController;
    private GameController gameControllerScript;
    private PlayerControllerV2 playerControllerV2;
    public GameObject player;
    public bool getDmg = false;
    public bool DmgReceived;
    public SpriteRenderer spriteRenderer;
    public Vector3[] points;
    private int lastPosition;
    private CircleCollider2D circleCollider2d;
    public float timerDmg;
    public float timerAnimation;
    private float movingTimer;
    public float maxTimerDmg ;
    public float maxTimerAnimation = 3f;
    public float playerControllerTimer;
    public Sprite normal;
    public Sprite angry;
    public Sprite explosion1;
    public Sprite explosion2;
    public Sprite explosion3;
    public Sprite explosion4;
    public bool enemydestroy; // daca se distrug inamici sau nu
    public bool moving;
    public bool destroyBoss;
    private bool prepareOnce;
    private bool playerControllerStatus = true;
    private int magnitude = 500;
    private Vector3 force;
    private Vector2 nextPosition;
    private bool onceSwitchPosition = true;
    private int blinkNumber;
    private bool changeImage;
    private Animator anim;
    private int explosionCount;
    




    void Start () {
        hpBoss = 120;
        gameController = GameObject.FindGameObjectWithTag("GameController");
        gameControllerScript = gameController.GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerControllerV2 = player.GetComponent<PlayerControllerV2>();
        circleCollider2d = GetComponent<CircleCollider2D>();
        DmgReceived = false;
        timerDmg = 0;
        timerAnimation = 0;
        playerControllerTimer = 0;
        enemydestroy = true;
        moving = false;
        destroyBoss = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextPosition = Vector2.zero;
        points[0] = new Vector3(-11f, 5f, 0f);
        points[1] = new Vector3(0f, 5f, 0f);
        points[2] = new Vector3(11f, 5f, 0f);
        points[3] = new Vector3(-11f, 0f, 0f);
        points[4] = new Vector3(0f, 0f, 0f);
        points[5] = new Vector3(11f, 0f, 0f);
        blinkNumber = 0;
        lastPosition = 2;
        maxTimerDmg = 0.1f;
        changeImage = true;
        movingTimer = 0;
        anim = GetComponent<Animator>();
        explosionCount = 0;
    }

    private void Update()
    {
        if (DmgReceived)
        {
            timerDmg += Time.deltaTime;
            spriteRenderer.enabled = false;
            circleCollider2d.enabled = false;
            if (timerDmg >= maxTimerDmg)
            {
                blinkNumber++;
                if (blinkNumber == 1 && onceSwitchPosition)
                {
                    SwitchPosition();
                    onceSwitchPosition = false;
                }
                timerDmg = 0;
                AppearAndDisappear();
            }
        }
        if (getDmg)
        {
            timerAnimation += Time.deltaTime;
            if(timerAnimation >= maxTimerAnimation && !DmgReceived)
            {
                timerAnimation = 0;
                SwitchPosition();
                if (enemydestroy)
                {
                    EnemyDestroy();
                }
            }
        }else
        {
            timerAnimation = 0;
        }

        if (!playerControllerStatus)
        {
            playerControllerV2.enabled = false;           
            playerControllerTimer += Time.deltaTime;
            if(playerControllerTimer > 0.5f)
            {
                playerControllerV2.enabled = true;
                playerControllerStatus = true;
                playerControllerTimer = 0;
            }
        }

        if (moving)
        {
            movingTimer += Time.deltaTime;
            if (changeImage)
            {
                ChangeImage();
                changeImage = false;
            }
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), nextPosition, 3 * Time.deltaTime);
            if((Vector2)transform.position == nextPosition && movingTimer >= 2f)
            {
                moving = false;
                AfterMovePosition();
                AppearAndDisappear(true);
                ColliderOnAndOff(true);
                blinkNumber = 0;
                changeImage = true;
                movingTimer = 0;
            }
        }
        if(hpBoss == -1) {

            switch (explosionCount)
            {
                case 0:
                    spriteRenderer.sprite = explosion1;
                    explosionCount++;
                    break;
                case 10:
                    spriteRenderer.sprite = explosion2;
                    explosionCount++;
                    break;
                case 20:
                    spriteRenderer.sprite = explosion3;
                    explosionCount++;
                    break;
                case 30:
                    spriteRenderer.sprite = explosion4;
                    explosionCount = 0;
                    break;
                default:
                    explosionCount++;
                    break;
            }         
            if (destroyBoss)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
           if (getDmg)
           {
                DmgReceived = true;
                onceSwitchPosition = true;
                playerControllerStatus = false;
                ColliderOnAndOff(false);
                if (enemydestroy)
                {
                    EnemyDestroy();
                }
                other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                force = transform.position - other.transform.position;          
                force.Normalize();
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(-force * magnitude);
                hpBoss -= 10;
                if (hpBoss == 0)
                {
                    moved = true;
                    playerControllerV2.enabled = true;
                    gameControllerScript.end = true;           
                    hpBoss = -1;
                    gameObject.transform.localScale = new Vector3(3, 3, 1);
                }
            }
            else if (!getDmg)
            {
                if (GetComponent<BossHp>().enabled) {
                    other.gameObject.SetActive(false);
                    gameControllerScript.GameOver();
                }
            }


        }
        else return;
    }

    public void SwitchPosition()
    {

        if(hpBoss <= 120 && hpBoss > 90)
        {
            int random = UnityEngine.Random.Range(0, 5);
            if (lastPosition == random)
            {
                random++;
            }
            float nextPositionX = points[random].x - player.transform.position.x;
            if (Mathf.Abs(nextPositionX) < 6.5f)
            {
                switch (random)
                {
                    case 0:
                        random = 5;
                        
                        break;
                    case 1:
                        random = 2;
                        break;
                    case 2:
                        random = 3;
                        break;
                    case 3:
                        random = 2;
                        break;
                    case 4:
                        random = 3;
                        break;
                    case 5:
                        random = 0;
                        break;
                }
            }
            nextPosition = points[random];
            moving = true;
            lastPosition = random;
        }
        else if (hpBoss <= 90 && hpBoss > 60)
        {
            nextPosition = points[4];
            moving = true;
        }else if (hpBoss <= 60 && hpBoss > 30)
        {
            int random = UnityEngine.Random.Range(0, 5);
            if (lastPosition == random)
            {
                random++;
            }
            float nextPositionX = points[random].x - player.transform.position.x;
            if (Mathf.Abs(nextPositionX) < 6.5f)
            {
                switch (random)
                {
                    case 0:
                        random = 5;

                        break;
                    case 1:
                        random = 2;
                        break;
                    case 2:
                        random = 3;
                        break;
                    case 3:
                        random = 2;
                        break;
                    case 4:
                        random = 3;
                        break;
                    case 5:
                        random = 0;
                        break;
                }
            }
            nextPosition = points[random];
            moving = true;
            lastPosition = random;
        }
        else if(hpBoss <= 30)
        {
            if (prepareOnce)
            {
                prepareOnce = false;
                maxTimerAnimation += 1;
                movingTimer += 2;
            }
            nextPosition = points[4];
            moving = true;
        }
    }

    private void AfterSwitchPosition()
    {
        AppearAndDisappear();
        ColliderOnAndOff();
        timerAnimation = 0;
        timerDmg = 0;
    }

    private void AppearAndDisappear()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;

    }

    private void AppearAndDisappear(bool status)
    {
        spriteRenderer.enabled = status;
    }

    private void ColliderOnAndOff()
    {
        circleCollider2d.enabled = !circleCollider2d.enabled;
    }

    private void ColliderOnAndOff(bool status)
    {
        circleCollider2d.enabled = status;
    }

    private void AfterMovePosition()
    {
        moved = true;
        DmgReceived = false;
    }

    public void ChangeImage()
    {
        if(spriteRenderer.sprite == normal)
        {
            spriteRenderer.sprite = angry;
            getDmg = true;
        }else if(spriteRenderer.sprite == angry)
        {
            spriteRenderer.sprite = normal;
            getDmg = false;
        }
    }

    public void EnemyDestroy()
    {
        GameObject[] Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < Enemy.Length; i++)
        {
            Destroy(Enemy[i]);
        }

    }

}
