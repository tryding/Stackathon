using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    public float velX = 5f;
    public float velY = 1f;
    public Rigidbody2D rb;
    public bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        Vector3 currentPos = transform.position;
        rb.velocity = new Vector2(velX, velY);
        Flip(currentPos.x);
    }
}
