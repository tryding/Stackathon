using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Animator animator;

    public Text healthText;
    public int health;
    public LayerMask enemyLayer;
    public Transform attackPos;
    public int speed = 7;
    public Rigidbody2D rb;
    public bool facingRight;
    public int damage;
    public float attackRangeX;
    public float attackRangeY;
    public float attackDelay;
    private float lastAttackTime;

    // Start is called before the first frame update
    void Start()
    {
        healthText.text = "Health: " + health.ToString();
        health = 100;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        facingRight = true;
    }

    private void Movement(float horizontal, float vertical)
    {
        Vector3 tempVec = new Vector3(horizontal, vertical, 0);
        tempVec = tempVec.normalized * speed * Time.deltaTime;
        if (horizontal > 0 || vertical > 0 || vertical < 0 || horizontal < 0)
            animator.SetTrigger("playerRun");
        rb.MovePosition(rb.transform.position + tempVec);
        Flip(horizontal);
        if (horizontal == 0 && vertical == 0)
            animator.SetTrigger("playerIdle");
    }

    private void Flip(float horizontal)
    {
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public void takeDamage(int damage)
    {
        animator.SetTrigger("playerHurt");
        health -= damage;
        healthText.text = "Health: " + health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Movement(horizontal, vertical);
        //transform.rotation = Quaternion.identity;
        if (Input.GetMouseButtonDown(0))
            {
            if(Time.time > lastAttackTime + attackDelay)
            {
                animator.SetTrigger("playerAttack1");
                Collider2D[] enemies = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, enemyLayer);
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].GetComponent<Enemies>().takeDamage(damage);
                }
                lastAttackTime = Time.time;
            }
        }
        rb.velocity = new Vector2(0, 0);
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));
    }
}
