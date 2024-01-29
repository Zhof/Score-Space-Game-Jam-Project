using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerRespawn : MonoBehaviour
{
    public int maxLives = 3;
    public int lives;

    [SerializeField]
    Text livesText;
    [SerializeField]
    ScoreManager scoreManager;
    //public bool isTempDead;

    [SerializeField]
    BoxCollider2D myCollider;
    [SerializeField]
    CircleCollider2D heartCollider;

    [SerializeField]
    GameObject gameOverCanvas;
    [SerializeField]
    Text finalScoreText;

    private void Start()
    {
        Time.timeScale = 1;
        gameOverCanvas.SetActive(false);
    
        lives = maxLives;
        livesText.text = ("Lives: " + (lives - 1).ToString());

        scoreManager.scoreAndDifficultyIncreaseDead = true;

        myCollider.enabled = false;
        heartCollider.enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
    }

    //player cant die twice in under 0.2 seconds aye
    public float deathTimer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Kill") && deathTimer < 0)
        {
            Death();
            Debug.Log("entered kill and collider was named" + collision.gameObject.name);
        }
    }

    public void Death()
    {
        deathTimer = 0.2f;

        if(lives > 1)
        {
            //transform.position = BlockCellsManager.Instance.blockCellsList[BlockCellsManager.Instance.blockCellsList.Count - 1].position + new Vector3(0, 0.51f, 0);

            //When the player dies, he is allowed to choose when to respawn by waiting for blocks to create a good platform.
            //To represent this, there is a red dot where the player will spawn at the top of the screen.
            //When in this death state, if space is pressed, the player is reactivited at that position.
            //Also in that state, the score doesn't go up, and neither does the difficulty of platforms. They're paused.
            scoreManager.scoreAndDifficultyIncreaseDead = true;

            myCollider.enabled = false;
            heartCollider.enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;


            //reenable eventually
            lives--;
            livesText.text = ("Lives: " + (lives - 1).ToString());
        }
        else
        {
            Time.timeScale = 0;
            gameOverCanvas.SetActive(true);
            finalScoreText.text = ("Score: " + Mathf.FloorToInt(scoreManager.score));
        }
    }

    private void Update()
    {
        if(scoreManager.scoreAndDifficultyIncreaseDead == true)
        {
            transform.position = new Vector3(0, 6, 0);

            if (Input.GetKeyDown(KeyCode.P))
            {
                scoreManager.scoreAndDifficultyIncreaseDead = false;

                myCollider.enabled = true;
                heartCollider.enabled = true;
                GetComponent<Rigidbody2D>().isKinematic = false;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                
            }
        }

        deathTimer -= Time.deltaTime;
    }
}
