using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;
    [SerializeField, Range(0.05f, 0.5f)] private float wallStickTime = 0.25f;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D rigidBody;
    private CollisionDataRetriever collisionDataRetriever;
    private WallInteractor wallInteractor;

    private float maxSpeedChange;
    private float acceleration;
    private float wallStickCounter;
    private bool onGround;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        wallInteractor = GetComponent<WallInteractor>();
    }

    private void Update()
    {
        direction.x = input.RetrieveMoveInput();
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - collisionDataRetriever.GetFriction(), 0f);
    }

    private void FixedUpdate()
    {
        onGround = collisionDataRetriever.GetOnGround();
        velocity = rigidBody.velocity;

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        #region Wall Stick
        if(collisionDataRetriever.OnWall && !collisionDataRetriever.OnGround && !wallInteractor.WallJumping)
        {
            if(wallStickCounter > 0)
            {
                velocity.x = 0;

                if(input.RetrieveMoveInput() == collisionDataRetriever.ContactNormal.x)
                {
                    wallStickCounter -= Time.deltaTime;
                }
                else
                {
                    wallStickCounter = wallStickTime;
                }
            }
            else
            {
                wallStickCounter = wallStickTime;
            }
        }
        #endregion

        rigidBody.velocity = velocity;
    }
}
