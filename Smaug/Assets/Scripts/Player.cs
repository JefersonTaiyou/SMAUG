using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float speed;
    private Animator anim;

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
        else if (move > 0.01f)
            transform.localScale = new Vector3(-1, 1, 1);


        // JUMP
        if (Input.GetKey(KeyCode.Space))
        {
            body.velocity = new Vector2(body.velocity.x, speed);
        }

        //set animator parameters
        anim.SetBool("isWalking", move != 0);
    }
}
