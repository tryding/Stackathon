using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{

    public LayerMask player;
    private Animator animator;
    public int speed;
    public int runSpeed;
    public int walkSpeed;
    public Rigidbody2D rb;
    public int health;
    public int attackDamage;
    private bool facingRight;
    private bool canAttack = true;
    private Transform target;
    public float attackRangeX;
    public float attackRangeY;
    public Transform attackPos;
    public bool moving;
    public float attackDelay;
    private float lastAttackTime;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        facingRight = true;
        speed = walkSpeed;
        moving = true;
    }

    public void takeDamage(int damage)
    {
        canAttack = false;
        animator.SetTrigger("rogueHit");
        health -= damage;
    }

    private void Flip(float horizontal)
    {
        if ((horizontal < transform.position.x && !facingRight) || (horizontal > transform.position.x && facingRight))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        Collider2D[] Player = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, player);

        if (Player.Length > 0)
        {
            if (Time.time > lastAttackTime + attackDelay)
            {
                animator.SetTrigger("rogueAttack1");
                Player[0].GetComponent<Player>().takeDamage(attackDamage);
                lastAttackTime = Time.time;
            }
        }
        if (distanceToPlayer < 4 && moving)
        {
            speed = runSpeed;
            animator.SetTrigger("rogueRun");
            Vector3 currentPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            //transform.rotation = Quaternion.identity;
            Flip(currentPos.x);
            rb.velocity = new Vector2(0, 0);
        }
        else if (distanceToPlayer < 7 && distanceToPlayer > 4 && moving)
        {
            speed = walkSpeed;
            animator.SetTrigger("rogueWalk");
            Vector3 currentPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            //transform.rotation = Quaternion.identity;
            Flip(currentPos.x);
            rb.velocity = new Vector2(0, 0);
        }
        if (health <= 0)
        {
            speed = 0;
            canAttack = false;
            moving = false;
            animator.SetTrigger("rogueDeath");
            Destroy(gameObject, 1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));
    }
}
