using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float speed;
    private Animator anim;
    private bool grounded;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float move = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(move * speed, body.velocity.y);

        // MOVE and Flip left-right
        if (move > 0.01f)
            transform.localScale = Vector3.one;
        else if (move > -0.01f)
            transform.localScale = new Vector3(1, 1, 1);


        // JUMP
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            Jump();
        }

        //set animator parameters
        anim.SetBool("isWalking", move != 0);
        anim.SetBool("isGrounded", grounded);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        anim.SetTrigger("isJumping");
        grounded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("colidiu");
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }

}
