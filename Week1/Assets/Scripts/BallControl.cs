using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallControl : MonoBehaviour
{

    Rigidbody rb;

    public float speed = 3;
    float teleportCooldown = 1f;
    float teleportTimer = 0;
    bool isTeleported = false;

    Vector3 restartPoint;
    int score = 0;
    int lives = 3;
    [SerializeField] Text scoreText;
    [SerializeField] Text liveText;

    [SerializeField] float jumpForce = 10;

    bool IsGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        restartPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isTeleported)
        {
            teleportTimer += Time.deltaTime;
            if(teleportTimer > teleportCooldown)
            {
                teleportTimer = 0;
                isTeleported = false;
            }
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce , ForceMode.Impulse);
            IsGrounded = false;
        }

        Vector3 movement = new Vector3(h, 0, v);

        rb.AddForce(movement * speed);

        if(transform.position.y < -5)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)  //ignore physics
    {
        if(other.gameObject.CompareTag("PickUp"))
        {
            Destroy(other.gameObject);
            score = score + 3;
            UpdateScore(score);
            //other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Trap"))
        {
            //lives--;
            //UpdateLive(lives);
            Die();
        }
        else if (other.CompareTag("Checkpoint"))
        {
            Destroy(other.gameObject);
            restartPoint = other.transform.position;
        }
        else if (other.CompareTag("Upgrade"))
        {
            Destroy(other.gameObject);
            transform.localScale *= 1.2f;
        }
        else if (other.CompareTag("Teleport") && !isTeleported)
        {
            transform.position = other.GetComponent<Teleport>().linkTeleportRoom.position;
            rb.velocity *= -1;
            isTeleported = true;
        }
    }

    void UpdateScore(int score)
    {
        scoreText.text = "Score : " + score;
    }

    void UpdateLive(int lives)
    {
        liveText.text = "Lives x " + lives;
    }

    void Die()
    {
        rb.Sleep();
        transform.position = restartPoint;
        rb.WakeUp();
        lives--;
        UpdateLive(lives);

        if(lives == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnCollisionEnter(Collision collision) // with physics
    {
        IsGrounded = true;
    }
}
