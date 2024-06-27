using System.Collections;
using UnityEngine;

public class CameraTurn : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Flip Rotation Settings")]
    [SerializeField] private float flipYRotationTime = 0.5f;

    private Coroutine turnCoroutine;
    private PlayerMovement player1;
    //private PlayerAnimation player2;
    private bool facingRight;

    private void Awake()
    {
        player1 = playerTransform.gameObject.GetComponent<PlayerMovement>();
        //player2 = playerTransform.gameObject.GetComponent<PlayerAnimation>();
        facingRight = player1.GetisFacingRight;
        //facingRight = player2.GetFacingRight;
    }

    private void Update()
    {
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;
        float elapsedTime = 0f;

        while(elapsedTime < flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        facingRight = !facingRight;

        if(facingRight)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }
}
