using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private Animator anim;

    private float speedInin;
    float move;
    bool isJumping = false;

    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 14f;

    private void Start()
    {
        speedInin = speed;
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        // MOVE and Flip left-right
        Walking();

        // JUMP
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        isJumping = true;
        anim.SetBool("isJumping", true);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetBool("isJumping", false);
        isJumping = false;
    }
    private void Walking()
    {
        move = Input.GetAxisRaw("Horizontal");
        // Walking
        body.velocity = new Vector2(move * speed, body.velocity.y);
        float dir = 0;

        if (dir == 0 && move !=0)
        {
            dir = move;
            //Flip 
            if (dir < 0f)
                sprite.flipX = true;
            else
                sprite.flipX = false;
        }
        Running();
        //Animation walking move
        if (move != 0 && !isJumping)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }
    private void Running()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && move != 0)
        {
            anim.SetBool("isRunning", true);
            speed += 4;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || move==0)
        {
            anim.SetBool("isRunning", false);
            speed = speedInin;
        }

    }
}
