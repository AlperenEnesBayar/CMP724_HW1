using UnityEngine;

public class movement : MonoBehaviour
{
    public int health = 10;
    public float maxcharacterSpeed = 5.0f;
    public float characterAccelerationPositive = 1.0f;
    public float characterAccelerationNegative = 3.0f;
    public float characterSpeed = 0.0f;

    public float jumpForce = 5.0f;
    public Rigidbody2D rb;
    public float minVelForJump = 0.01f;
    public bool isFlipped = false;

    public Animator animator;

    public TMPro.TextMeshProUGUI healthText;

    public GameObject endText;
    public Canvas canvas;

    bool canJump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Flip()
    {
        isFlipped = !isFlipped;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // move character
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            characterSpeed = Mathf.MoveTowards(characterSpeed, maxcharacterSpeed, characterAccelerationPositive * Time.deltaTime);
        }
        else
        {
            characterSpeed = Mathf.MoveTowards(characterSpeed, 0, characterAccelerationNegative * Time.deltaTime);
        }

        transform.position += new Vector3(horizontal, vertical, 0) * characterSpeed * Time.deltaTime;

        // set animator parameters
        animator.SetFloat("Speed", Mathf.Abs(characterSpeed));

        // jump RigidBody2D, and prevent double jump
        if (Input.GetButtonDown("Jump") && canJump)
        {
            animator.SetBool("IsJumping", true);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }

        // flip character
        if (horizontal < 0 && !isFlipped)
        {
            Flip();
        }
        else if (horizontal > 0 && isFlipped)
        {
            Flip();
        }
       
    }

    public void onLanding()
    {
        animator.SetBool("IsJumping", false);
        canJump = true;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onLanding();
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Player hit enemy!");
            // knockback player up and away from enemy
            Vector2 difference = transform.position - collision.transform.position; 
            difference = difference.normalized * 4;
            rb.AddForce(difference, ForceMode2D.Impulse);

            // enemy plays attack animation
            collision.gameObject.GetComponent<Enemy>().Attack();

            // player takes damage
            health -= 1;
            if (health <= 0)
            {
                Die();
            }

            // Change text
            healthText.text = "Health: " + health.ToString();
        }

        if (collision.gameObject.tag == "Health")
        {
            Debug.Log("Player picked up health!");
            health += 1;
            Destroy(collision.gameObject);
            healthText.text = "Health: " + health.ToString();
        }

        if (collision.gameObject.tag == "EndChest")
        {
            Debug.Log("Player reached end chest!");

            // instantiate end text inside canvas
            Instantiate(endText, canvas.transform);

            // destroy all enemies
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }

            // destroy player
            Destroy(gameObject);

            Destroy(collision.gameObject);
        }
    }   

    void Die()
    {
        Debug.Log("Player died!");
        Destroy(gameObject);
    }
}   
