using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
    public Text winText;
    public Text loseText;
    public Text createdBy;
    public Text lives;

    public int scoreValue;
    public int livesValue;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    bool GameOver;
    private bool facingRight = true;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        lives.text = livesValue.ToString();
        SetScore();
        SetLives();
        winText.text = "";
        loseText.text = "";
        createdBy.text = "";
        scoreValue = 0;
        livesValue = 3;
        musicSource.clip = musicClipOne;
        musicSource.Play();
        GameOver = false;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            Destroy(collision.collider.gameObject);
            SetScore();
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.collider.gameObject);
            livesValue = livesValue - 1;
            SetLives();
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                anim.SetBool("IsJumping", true);
            }
            else
            {
                anim.SetBool("IsJumping", false);
            }
        }
    }


    void SetScore()
    {
        score.text = "Score: " + scoreValue.ToString();
        if (scoreValue == 4)
        {
            transform.position = new Vector3(104.4f, 2.2f, 0.0f);
            livesValue = 3;
            SetLives();
        }
        if (scoreValue >= 8 && GameOver == false)
        {
            winText.text = "You Made It!";
            createdBy.text = "Created by: Hallie Richardson";
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            GameOver = true;
        }
    }

    void SetLives()
    {
        lives.text = "Lives: " + livesValue.ToString();
        if (livesValue == 0 && GameOver == false)
        {
            loseText.text = "YOU LOSE!!";
            createdBy.text = "Created by: Hallie Richardson";
        }
    }
    
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}