using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityStandardAssets.CrossPlatformInput;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
    public GameObject player;
    public GameObject platforma;
    public GameObject platformaInstatiate;
    public GameObject platformInstatiateMovingLeft;
    public GameObject platforms;
    public GameObject platforms2;
    public GameObject FirstPlatformForInstantiate;
    public GameObject[] buttons;
    public GameObject DashButton;
    public GameObject GameOverWindow;
    public GameObject PauseWindow;
    public GameObject boss;
    public GameObject backToMenuButton;
    private GameObject gameManager;
    private GameManager gameManagerScript;
    public Text scoreText;
    public Text gameOverText;
    public Text weaveCount;
    public Text highScoreText;
    public int count;
    public int highScore;
    public int numberOfAllGames;
    public bool gameOver;
    public Transform midPoint;
    public Transform[] points;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public float levelWait;
    public float upWait;
    public bool end = false;
    private PlatformReSize platformReSize;
    private PlatformMove platformMove;
    private PlatformMovingLeftInstantiate platformMovingLeftInstantiate;
    private PlayerController playerController;
    private PlayerControllerV2 playerControllerV2;
    private Mover mover;
    private EnemyInCircle enemyInCircle;
    private GameObject platformaActuala;
    private int missleCount;
    private int hazardCount;
    private Vector2 spawnPosition;
    private int spawnPositionx;
    private int spawnPositiony;
    private PlayAdd playAdd;
    public bool pause;

    void Awake()
    {
        weaveCount.text = "";
        highScoreText.text = "";
        gameOverText.text = "";
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        gameManagerScript.Load();
        playAdd = GetComponent<PlayAdd>();
        GameOverWindow.SetActive(false);
        PauseWindow.SetActive(false);
        DisableScoreText();
        PrepareGame();
        playAdd.InitializeAdd();
        //Debug.Log(numberOfAllGames);
        hazardCount = 4;//era 3
        missleCount = 5;
        spawnPositionx = 20;
        spawnPositiony = 15;
        gameOver = true;
        count = 1;
        UpdateScore(count);
        platformMove = platforma.GetComponent<PlatformMove>();
        platformMove.enabled = false;
        platformReSize = platforma.GetComponent<PlatformReSize>();
        platformReSize.enabled = false;
        platformMovingLeftInstantiate = GetComponent<PlatformMovingLeftInstantiate>();
        playerController = player.GetComponent<PlayerController>();
        playerControllerV2 = player.GetComponent<PlayerControllerV2>();
        platformMovingLeftInstantiate.enabled = false;
        StartCoroutine(StartWaves());
        pause = false;

    }


    IEnumerator StartWaves()
    {
        int levelNumber;
        levelNumber = UnityEngine.Random.Range(1, 10);
        if (levelNumber % 2 == 0)
        {
            yield return StartCoroutine(RightWaves());
            yield return StartCoroutine(LeftWaves());
        }
        else
        {
            yield return StartCoroutine(LeftWaves());
            yield return StartCoroutine(RightWaves());
        }

        yield return StartCoroutine(LeftAndRightWaves());
        hazardCount++;
        levelNumber = UnityEngine.Random.Range(1, 10);
        if (levelNumber % 2 == 0)
        {
            yield return StartCoroutine(PlatformMove());
            yield return StartCoroutine(MultiplePlatform());
        }
        else
        {
            yield return StartCoroutine(MultiplePlatform());
            yield return StartCoroutine(PlatformMove());
        }
        hazardCount++;
        yield return StartCoroutine(MultiplePlatformMoving());
        yield return StartCoroutine(UpWaves());
        yield return StartCoroutine(MissleWaves());
        yield return StartCoroutine(BossFight());

    }


    IEnumerator RightWaves()
    {

        spawnPositionx = 20;
        
        do
        {
            CheckGameOver();
            for (int i = 0; i < hazardCount; i++)
            {
                int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                GameObject hazard = hazards[type];

                Vector2 spawnPosition = new Vector2(spawnPositionx, UnityEngine.Random.Range(platforma.transform.position.y + 0.5f, player.transform.position.y + 1));
                Quaternion spawnRotation = Quaternion.identity;
                GameObject enemy = Instantiate(hazard, spawnPosition, spawnRotation) as GameObject;
                mover = enemy.GetComponent<Mover>();
                mover.speed = -600;
                yield return new WaitForSeconds(spawnWait);
            }
            count++;
            if (gameOver)
            {
                UpdateScore(count);
            }
            else break;
            yield return new WaitForSeconds(waveWait);
        } while (count % 6 != 0);
        CheckGameOver();
        yield return new WaitForSeconds(levelWait);
    }

    IEnumerator LeftWaves()
    {
        spawnPositionx = -20;      
        do
        {
            CheckGameOver();
            for (int i = 0; i < hazardCount; i++)
            {
                int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                GameObject hazard = hazards[type];
                Vector2 spawnPosition = new Vector2(spawnPositionx, UnityEngine.Random.Range(platforma.transform.position.y + 0.5f, player.transform.position.y + 1));
                Quaternion spawnRotation = Quaternion.identity;
                GameObject enemy = Instantiate(hazard, spawnPosition, spawnRotation) as GameObject;
                mover = enemy.GetComponent<Mover>();
                mover.speed = 600;

                yield return new WaitForSeconds(spawnWait);
            }

            count++;
            if (gameOver)
            {
                UpdateScore(count);
            }
            else break;
            yield return new WaitForSeconds(waveWait);

        } while (count % 6 != 0);
        CheckGameOver();
        yield return new WaitForSeconds(levelWait);
    }

    IEnumerator LeftAndRightWaves()
    {
        platformReSize.enabled = true;
        do
        {
            CheckGameOver();
            for (int i = 0; i < hazardCount; i++)
            {
                int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                GameObject hazard = hazards[type];
                int random = UnityEngine.Random.Range(1, 100);
                if (random % 2 == 0)
                {
                    spawnPositionx = -20;
                }
                else spawnPositionx = 20;
                Vector2 spawnPosition = new Vector2(spawnPositionx, UnityEngine.Random.Range(player.transform.position.y - 5f, player.transform.position.y + 5));
                Quaternion spawnRotation = Quaternion.identity;
                GameObject enemy = Instantiate(hazard, spawnPosition, spawnRotation) as GameObject;
                if (random % 2 == 0)
                {
                    mover = enemy.GetComponent<Mover>();
                    mover.speed = 600;
                }
                else
                {
                    mover = enemy.GetComponent<Mover>();
                    mover.speed = -600;
                }
                yield return new WaitForSeconds(spawnWait);
            }
            count++;
            if (gameOver)
            {
                UpdateScore(count);
            }
            else break;
            yield return new WaitForSeconds(waveWait);
        } while (count % 6 != 0);
        CheckGameOver();
        yield return new WaitForSeconds(levelWait);
    }

    IEnumerator PlatformMove()
    {
        if (platformMove.enabled == false)
        {
            platformMove.goToCenter = false;
            platformMove.enabled = true;

        }
        do
        {
            CheckGameOver();
            for (int i = 0; i < hazardCount; i++)
            {
                int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                GameObject hazard = hazards[type];
                int random = UnityEngine.Random.Range(1, 100);
                if (random % 2 == 0)
                {
                    spawnPositionx = -20;
                }
                else spawnPositionx = 20;
                Vector2 spawnPosition = new Vector2(spawnPositionx, UnityEngine.Random.Range(player.transform.position.y - 5f, player.transform.position.y + 5));
                Quaternion spawnRotation = Quaternion.identity;
                GameObject enemy = Instantiate(hazard, spawnPosition, spawnRotation) as GameObject;
                if (random % 2 == 0)
                {
                    mover = enemy.GetComponent<Mover>();
                    mover.speed = 600;
                }
                else
                {
                    mover = enemy.GetComponent<Mover>();
                    mover.speed = -600;
                }
                yield return new WaitForSeconds(spawnWait);
            }
            count++;
            if (gameOver)
            {
                UpdateScore(count);
            }
            else break;
            yield return new WaitForSeconds(waveWait);
        } while (count % 6 != 0);
        if (platforma.transform.position != midPoint.position)
        {
            platformMove.goToCenter = true;       
        }
        CheckGameOver();
        yield return new WaitForSeconds(levelWait);      
    }

    IEnumerator MultiplePlatform()
    {

        if (platformMove.enabled == false)
        {
            platformMove.goToCenter = true;
            platformMove.enabled = true;
        }
        GameObject platforms2;
        for (int i = 0; i < points.Length-1; i++) {
            platforms2 = Instantiate(platformaInstatiate, points[i].transform.position, Quaternion.identity);
            platforms2.transform.parent = platforms.transform;
        }

        do
        {
            CheckGameOver();
            for (int i = 0; i < hazardCount; i++)
            {
                int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                GameObject hazard = hazards[type];
                int random = UnityEngine.Random.Range(1, 100);
                if (random % 2 == 0)
                {
                    spawnPositionx = -20;
                }
                else spawnPositionx = 20;
                Vector2 spawnPosition = new Vector2(spawnPositionx, UnityEngine.Random.Range(player.transform.position.y - 5f, player.transform.position.y + 5));
                Quaternion spawnRotation = Quaternion.identity;
                GameObject enemy = Instantiate(hazard, spawnPosition, spawnRotation) as GameObject;
                if (random % 2 == 0)
                {
                    mover = enemy.GetComponent<Mover>();
                    mover.speed = 600;
                }
                else
                {
                    mover = enemy.GetComponent<Mover>();
                    mover.speed = -600;
                }
                yield return new WaitForSeconds(spawnWait);
            }
            count++;
            if (gameOver)
            {
                UpdateScore(count);
            }
            else break;
            yield return new WaitForSeconds(waveWait);
        } while (count %6 != 0);
        gameOverText.text = "WARNING !";
        yield return new WaitForSeconds(levelWait);
        gameOverText.text = "";
        Destroy(platforms);
    }

    IEnumerator MultiplePlatformMoving()
    {
  
        platformMovingLeftInstantiate.enabled = true;
        Destroy(platforma);
        GameObject platforms3;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 position = points[i].transform.position;
            position.x += 5;
            points[i].transform.position = position;
            platforms3 = Instantiate(platformInstatiateMovingLeft, points[i].transform.position, Quaternion.identity);
            platforms3.transform.parent = platforms2.transform;
            position = points[i].transform.position;
            position.x -= 5;
            points[i].transform.position = position;
        }
        do
        {
            CheckGameOver();
            for (int i = 0; i < hazardCount; i++)
            {
                int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                GameObject hazard = hazards[type];
                int random = UnityEngine.Random.Range(1, 100);
                if (random % 2 == 0)
                {
                    spawnPositionx = -20;
                }
                else spawnPositionx = 20;
                Vector2 spawnPosition = new Vector2(spawnPositionx, UnityEngine.Random.Range(player.transform.position.y - 5f, player.transform.position.y + 5));
                Quaternion spawnRotation = Quaternion.identity;
                GameObject enemy = Instantiate(hazard, spawnPosition, spawnRotation) as GameObject;
                if (random % 2 == 0)
                {
                    mover = enemy.GetComponent<Mover>();
                    mover.speed = 600;
                }
                else
                {
                    mover = enemy.GetComponent<Mover>();
                    mover.speed = -600;
                }
                yield return new WaitForSeconds(spawnWait);
            }
            count++;
            if (gameOver)
            {
                UpdateScore(count);
            }
            else break;
            yield return new WaitForSeconds(waveWait);
        } while (count % 6 != 0);
        gameOverText.text = "WARNING !";
        yield return new WaitForSeconds(levelWait);
        gameOverText.text = "";
        platformMovingLeftInstantiate.stop = true;
        Vector3 platformPosition = midPoint.transform.position;
        platformPosition.y -= 2.5f;
        platformaActuala = Instantiate(FirstPlatformForInstantiate, platformPosition, Quaternion.identity) as GameObject;
        Destroy(platforms2);

    }

    IEnumerator UpWaves()
    {
        ChangeButtons();
        gameOverText.text = "FUCK THE JUMP JUST DASH!";
        yield return new WaitForSeconds(levelWait);
        gameOverText.text = "";
        Quaternion spawnRotation = Quaternion.Euler(0, 0, 270);
        do
        {
            CheckGameOver();
            for (int i = 0; i < hazardCount; i++)
            {
                int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                GameObject hazard = hazards[type];
                Vector2 spawnPosition = new Vector2(player.transform.position.x, spawnPositiony);

                GameObject enemy = Instantiate(hazard, spawnPosition, spawnRotation) as GameObject;
                mover = enemy.GetComponent<Mover>();
                mover.speed = 600;

                Vector2 spawnPosition2 = spawnPosition + new Vector2(5, 0);
                GameObject enemy1 = Instantiate(hazard, spawnPosition2, spawnRotation) as GameObject;
                mover = enemy1.GetComponent<Mover>();
                mover.speed = 600;

                Vector2 spawnPosition3 = spawnPosition2 + new Vector2(5, 0);
                GameObject enemy2 = Instantiate(hazard, spawnPosition3, spawnRotation) as GameObject;
                mover = enemy2.GetComponent<Mover>();
                mover.speed = 600;
                yield return new WaitForSeconds(upWait);
            }
            count++;
            if (gameOver)
            {
                UpdateScore(count);
            }
            else break;
            yield return new WaitForSeconds(waveWait);
        } while (count % 6 != 0);
        CheckGameOver();
        gameOverText.text = "Fly like a Dragon";//fly like butterfly sting like a bee 
        yield return new WaitForSeconds(levelWait);
        gameOverText.text = "";
    }

    IEnumerator MissleWaves()
    {

        playerController.destroy = true;
        Destroy(platformaActuala);
        Quaternion spawnRotation = Quaternion.Euler(180, 180, 180);
        do
        {
            CheckGameOver();
            for (int i = 0; i < missleCount; i++)
            {
                int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                GameObject hazard = hazards[type];

                int random = UnityEngine.Random.Range(1, 100);
                if (random % 2 == 0)
                {
                    spawnPositionx = -25;
                }
                else
                {
                    spawnPositionx = 25;
                }
                Vector2 spawnPosition = new Vector2(spawnPositionx, player.transform.position.y);
                GameObject enemy = Instantiate(hazard, spawnPosition, spawnRotation) as GameObject;
                mover = enemy.GetComponent<Mover>();
                mover.rotateToPlayer = true;
                mover.speed = 600;
                yield return new WaitForSeconds(spawnWait);
            }
            count++;
            if (gameOver)
            {
                UpdateScore(count);
            }
            else break;
            yield return new WaitForSeconds(waveWait);
        } while (count < 50);
        CheckGameOver();
        yield return new WaitForSeconds(levelWait);
        yield return new WaitForSeconds(levelWait);
    }

    IEnumerator BossFight()
    {
        //playerController.destroy = true;
        //Destroy(platforma);
        //ChangeButtons();
        Vector3 spawnPositionBoss = new Vector3(0f, spawnPositiony, 0f);
        GameObject bossObject = Instantiate(boss, spawnPositionBoss, Quaternion.identity) as GameObject;
        BossHp bossHp = bossObject.GetComponent<BossHp>();
        Quaternion spawnRotation = Quaternion.Euler(180, 180, 180);
        yield return new WaitForSeconds(levelWait * 3);
        do
        {
            CheckGameOver();
            if (bossHp.hpBoss <= 120 && bossHp.hpBoss > 90)
            {
                for (int i = 0; i < 5; i++)
                {
                    spawnPositionBoss = bossObject.transform.position;
                    int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                    GameObject hazard = hazards[type];
                    enemyInCircle = hazard.GetComponent<EnemyInCircle>();
                    enemyInCircle.enabled = true;
                    Instantiate(hazard, spawnPositionBoss, spawnRotation);
                    yield return new WaitForSeconds(0.3f);
                }
                bossHp.ChangeImage();
                bossHp.moved = false;                
                while (!bossHp.moved) 
                {
                    yield return new WaitForSeconds(0.0000001f);
                }
                if (gameOver)
                {
                    UpdateScore(count);
                }
                yield return new WaitForSeconds(waveWait);
            
            }
            else if (bossHp.hpBoss <= 90 && bossHp.hpBoss > 60)
            {
               
                for (int i = 0; i < 5; i++)
                {
                    spawnPositionBoss = bossObject.transform.position;
                    int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                    GameObject hazard = hazards[type];
                    enemyInCircle = hazard.GetComponent<EnemyInCircle>();
                    enemyInCircle.enabled = true;
                    enemyInCircle.onPlayer = true;
                    Instantiate(hazard, spawnPositionBoss, spawnRotation);
                    yield return new WaitForSeconds(0.3f);
                }
                bossHp.ChangeImage();
                bossHp.moved = false;
                while (!bossHp.moved) // && animation running
                {
                    yield return new WaitForSeconds(0.0000001f);
                }
                if (gameOver)
                {
                    UpdateScore(count);
                }
                yield return new WaitForSeconds(waveWait);
            }
            else if (bossHp.hpBoss <= 60 && bossHp.hpBoss > 30)
            {
                for (int i = 0; i < 5; i++)
                {
                    spawnPositionBoss = bossObject.transform.position;
                    int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                    GameObject hazard = hazards[type];
                    enemyInCircle = hazard.GetComponent<EnemyInCircle>();
                    enemyInCircle.enabled = false;
                    mover = hazard.GetComponent<Mover>();
                    mover.rotateToPlayer = true;
                    Instantiate(hazard, spawnPositionBoss, spawnRotation);
                    yield return new WaitForSeconds(spawnWait);
                }

                bossHp.ChangeImage();
                bossHp.moved = false;
                while (!bossHp.moved) // && animation running
                {
                    yield return new WaitForSeconds(0.0000001f);
                }
                if (gameOver)
                {
                    UpdateScore(count);
                }
                yield return new WaitForSeconds(waveWait);             
            }
            else if (bossHp.hpBoss <= 30)
            {
                for (int i = 0; i < 5; i++)
                {
                    spawnPositionBoss = bossObject.transform.position;
                    int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                    GameObject hazard = hazards[type];
                    enemyInCircle = hazard.GetComponent<EnemyInCircle>();
                    enemyInCircle.enabled = true;
                    enemyInCircle.onPlayer = false;
                    mover = hazard.GetComponent<Mover>();
                    mover.rotateToPlayer = false;
                    Instantiate(hazard, spawnPositionBoss, spawnRotation);

                    yield return new WaitForSeconds(0.3f);
                }

                for (int i = 0; i < 5; i++)
                {
                    int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                    GameObject hazard = hazards[type];
                    enemyInCircle = hazard.GetComponent<EnemyInCircle>();
                    enemyInCircle.enabled = true;
                    enemyInCircle.onPlayer = true;
                    Instantiate(hazard, spawnPositionBoss, spawnRotation);
                    yield return new WaitForSeconds(0.3f);
                }

                for (int i = 0; i < 5; i++)
                {
                    int type = UnityEngine.Random.Range(0, hazards.Length - 1);
                    GameObject hazard = hazards[type];
                    enemyInCircle = hazard.GetComponent<EnemyInCircle>();
                    enemyInCircle.enabled = false;
                    mover = hazard.GetComponent<Mover>();
                    mover.rotateToPlayer = true;
                    Instantiate(hazard, spawnPositionBoss, spawnRotation);
                    yield return new WaitForSeconds(spawnWait);
                }

                bossHp.ChangeImage();
                bossHp.moved = false;

                while (!bossHp.moved)
                {
                    yield return new WaitForSeconds(0.0000001f);
                }
                if (gameOver)
                {
                    UpdateScore(count);
                }
                yield return new WaitForSeconds(waveWait);
            }
        } while (!end);
        playerControllerV2.boxCollider2d.enabled = false;
        spawnRotation = Quaternion.Euler(0, 0, 270);
        PrepareGame();
        highScore = 51;
        UpdateScore(highScore);
        gameManagerScript.Save();
        for (int i = 0; i < hazards.Length-1; i++)
        {  
            GameObject hazard = hazards[i];
            spawnPositionBoss = new Vector2(UnityEngine.Random.Range(-3, 3), 0);
            GameObject enemy = Instantiate(hazard, spawnPositionBoss, spawnRotation) as GameObject;
            yield return new WaitForSeconds(spawnWait);
        }
        bossHp.destroyBoss = true;

        SceneManager.LoadScene("End");
    }



    public void ChangeButtons()
    {
        for (int i = 0; i < buttons.Length; i++) {
            Destroy(buttons[i]);
        }
        GameObject dashButton = Instantiate(DashButton, DashButton.transform.position, DashButton.transform.rotation) as GameObject;
        dashButton.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        playerController.jumpDestroy = true;
    }

    public void UpdateScore(int count)
    {
        scoreText.text = "Waves: " + count;
    }

    public void CheckGameOver()
    {
        if (!gameOver)
        {
            StopAllCoroutines();
        }
    }

    public void GameOver()
    {
        if (gameOver)
        {
            gameOver = false;
            numberOfAllGames += 1;
            if (highScore < count)
            {
                //new record
                highScore = count;    
            }
            EnableScoreText();
            gameManagerScript.Save();
            if (platformMovingLeftInstantiate != null && platformMovingLeftInstantiate.enabled)
            {
                platformMovingLeftInstantiate.enabled = false;
            }

            if (numberOfAllGames >= 10)
            {
                numberOfAllGames = 0;
                playAdd.ShowAd();
            }else
            {
                GameOverWindow.SetActive(true);
            }

        }
    }

    public void EnableScoreText()
    {
        scoreText.text = "";
        weaveCount.text = "" + count;
        highScoreText.text = "" + highScore;
        backToMenuButton.SetActive(true);
    }

    public void DisableScoreText()
    {
        scoreText.text = "Waves: " + count;
        weaveCount.text = "";
        highScoreText.text = "";
        backToMenuButton.SetActive(false);
    }

    public void RestartButton()
    {
        GameOverWindow.SetActive(false);
        weaveCount.text = "";
        highScoreText.text = "";
        gameOverText.text = "";
        SceneManager.LoadScene("Main");

    }

    public void Pause()
    {
        Time.timeScale = 0;
        EnableScoreText();
        PauseWindow.SetActive(true);
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;   
        DisableScoreText();
        PauseWindow.SetActive(false);
    }

    public void SwitchPause()
    {
        if (playerController != null && playerController.enabled)
        {
            if (playerController.pause)
            {
                playerController.pause = false;
                playerController.resume = true;
            }
            else if (!playerController.pause)
            {
                playerController.pause = true;
            }
        }
        if (playerControllerV2 != null && playerControllerV2.enabled)
        {
            if (playerControllerV2.pause)
            {
                playerControllerV2.pause = false;
                playerControllerV2.resume = true;
            }
            else if (!playerControllerV2.pause)
            {
                playerControllerV2.pause = true;
            }
        }
    }

    public void PrepareGame()
    {
        Time.timeScale = 1;
        for (int i = 0; i <= hazards.Length - 1; i++)
        {
            GameObject hazard = hazards[i];
            mover = hazard.GetComponent<Mover>();
            mover.speed = 600;
            mover.rotateToPlayer = false;
            enemyInCircle = hazard.GetComponent<EnemyInCircle>();
            enemyInCircle.enabled = false;
            enemyInCircle.onPlayer = false;
        }
        

    }

}
