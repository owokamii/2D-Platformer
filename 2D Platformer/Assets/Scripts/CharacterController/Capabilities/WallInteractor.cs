using UnityEngine;
using UnityEngine.UIElements;

public class WallInteractor : MonoBehaviour
{
    public bool WallJumping { get; private set; }

    [SerializeField] private InputController input = null;
    [Header("Wall Slide")]
    [SerializeField][Range(0.1f, 5f)] private float wallSlideMaxSpeed = 2f;
    [Header("Wall Jump")]
    [SerializeField] private Vector2 wallJumpClimb = new Vector2(4f, 12f);
    [SerializeField] private Vector2 wallJumpBounce = new Vector2(10.7f, 10f);
    [SerializeField] private Vector2 wallJumpLeap = new Vector2(14f, 12f);

    private CollisionDataRetriever collisionDataRetriever;
    private Rigidbody2D rigidBody;
    //private Controller controller;

    private Vector2 velocity;
    private bool onWall;
    private bool onGround;
    private bool desiredJump;
    private float wallDirectionX;

    private void Start()
    {
        collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(onWall && !onGround)
        {
            desiredJump |= input.RetrieveJumpInput();
        }
    }

    private void FixedUpdate()
    {
        velocity = rigidBody.velocity;
        onWall = collisionDataRetriever.OnWall;
        onGround = collisionDataRetriever.OnGround;
        wallDirectionX = collisionDataRetriever.ContactNormal.x;

        #region Wall Slide
        if(onWall)
        {
            if (velocity.y < -wallSlideMaxSpeed)
            {
                velocity.y = -wallSlideMaxSpeed;
            }
        }
        #endregion

        #region Wall Jump
        if((onWall && velocity.x == 0) || onGround)
        {
            WallJumping = false;
        }

        if(desiredJump)
        {
            if(wallDirectionX == input.RetrieveMoveInput())
            {
                velocity = new Vector2(wallJumpClimb.x * wallDirectionX, wallJumpClimb.y);
                WallJumping = true;
                desiredJump = false;

            }
            else if(input.RetrieveMoveInput() == 0)
            {
                velocity = new Vector2(wallJumpBounce.x * wallDirectionX, wallJumpBounce.y);
                WallJumping = true;
                desiredJump = false;
            }
            else
            {
                velocity = new Vector2(wallJumpLeap.x * wallDirectionX, wallJumpLeap.y);
                WallJumping = true;
                desiredJump = false;

            }
        }
        #endregion

        rigidBody.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionDataRetriever.EvaluateCollision(collision);

        if (collisionDataRetriever.OnWall && collisionDataRetriever.OnGround && WallJumping)
        {
            rigidBody.velocity = Vector2.zero;
        }
    }
}
