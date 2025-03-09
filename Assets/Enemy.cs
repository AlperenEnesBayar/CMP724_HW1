using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3;
    public float speed = 2.0f;
    public bool direction = true;
    public GameObject bEffect;

    public float patrolxmin = 0.0f;
    public float patrolxmax = 5.0f;

    bool isFlipped = false;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Patrol y until it reaches the max, then it will go back to the min
        if (direction)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (transform.position.x >= patrolxmax)
        {
            direction = false;
            Flip();
        }
        else if (transform.position.x <= patrolxmin)
        {
            direction = true;
            Flip();
        }

    }


    public void TakeDamage(int damage)
    {
        Debug.Log("Enemy took damage!");
        health -= damage;

        animator.SetTrigger("Damaged");

        Instantiate(bEffect, transform.position, Quaternion.identity);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }


    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }

    void Flip()
    {
        isFlipped = !isFlipped;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
}
