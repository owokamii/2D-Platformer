using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 100f)] private float jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0f, 0.3f)] private float coyoteTime = 0.2f;
    [SerializeField, Range(0f, 0.3f)] private float jumpBufferTime = 0.2f;

    //private Controller controller;
    private Rigidbody2D rigidBody;
    private CollisionDataRetriever collisionDataRetriever;
    private Vector2 velocity;

    private int jumpPhase;
    private float defaultGravityScale;
    private float jumpSpeed;
    private float coyoteCounter;
    private float jumpBufferCounter;

    private bool desiredJump;
    private bool onGround;
    private bool isJumping;

    public Vector2 GetVelocity { get; private set; }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collisionDataRetriever = GetComponent<CollisionDataRetriever>();

        defaultGravityScale = 1f;
    }

    private void Update()
    {
        desiredJump |= input.RetrieveJumpInput();
    }

    private void FixedUpdate()
    {
        onGround = collisionDataRetriever.GetOnGround();
        velocity = rigidBody.velocity;

        if(onGround && rigidBody.velocity.y == 0)
        {
            jumpPhase = 0;
            coyoteCounter = coyoteTime;
            isJumping = false;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if(desiredJump)
        {
            desiredJump = false;
            jumpBufferCounter = jumpBufferTime;
        }
        else if(!desiredJump && jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if(jumpBufferCounter > 0)
        {
            JumpAction();
        }

        if(input.RetrieveJumpHoldInput() && rigidBody.velocity.y > 0)
        {
            rigidBody.gravityScale = upwardMovementMultiplier;
        }
        else if(!input.RetrieveJumpHoldInput() || rigidBody.velocity.y < 0)
        {
            rigidBody.gravityScale = downwardMovementMultiplier;
        }
        else if(rigidBody.velocity.y == 0)
        {
            rigidBody.gravityScale = defaultGravityScale;
        }

        rigidBody.velocity = velocity;
    }

    private void JumpAction()
    {
        if(coyoteCounter > 0 || (jumpPhase < maxAirJumps && isJumping))
        {
            if(isJumping)
            {
                jumpPhase++;
            }

            jumpBufferCounter = 0;
            coyoteCounter = 0;
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            isJumping = true;

            if(velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if(velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(rigidBody.velocity.y);
            }

            velocity.y += jumpSpeed;
        }
    }
}
