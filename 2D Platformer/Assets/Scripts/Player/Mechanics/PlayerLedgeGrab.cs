using System.Collections;
using UnityEngine;

public class PlayerLedgeGrab : MonoBehaviour
{
    [Header("Ledge Grab")]
    [SerializeField] private float redXOffset;
    [SerializeField] private float redYOffset;
    [SerializeField] private float redXSize;
    [SerializeField] private float redYSize;
    [SerializeField] private float greenXOffset;
    [SerializeField] private float greenYOffset;
    [SerializeField] private float greenXSize;
    [SerializeField] private float greenYSize;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rigidBody;

    [Header("Hidden")]
    private PlayerMovement playerMovement;
    private float defaultGravity;
    private bool isGrabbing = false;
    private bool canGrab = true;
    private bool greenBox;
    private bool redBox;

    public bool GetisGrabbing { get; private set; }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        defaultGravity = rigidBody.gravityScale;
    }

    private void Update()
    {
        GetisGrabbing = isGrabbing;

        HandleLedgeGrab();
    }

    private void HandleLedgeGrab()
    {
        greenBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y + greenYOffset), new Vector2(greenXSize, greenYSize), 0f, groundLayerMask);
        redBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y + redYOffset), new Vector2(redXSize, redYSize), 0f, groundLayerMask);

        if (greenBox && !redBox && !isGrabbing && canGrab && !playerMovement.IsGrounded())
        {
            isGrabbing = true;
        }

        if (isGrabbing)
        {
            rigidBody.velocity = new Vector2(0f, 0f);
            rigidBody.gravityScale = 0f;

            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(HandleRelease());
            }
        }
    }

    private IEnumerator HandleRelease()
    {
        canGrab = false;
        isGrabbing = false;
        rigidBody.gravityScale = defaultGravity;

        yield return new WaitForSeconds(0.1f);

        canGrab = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y + redYOffset), new Vector2(redXSize, redYSize));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y + greenYOffset), new Vector2(greenXSize, greenYSize));
    }
}
