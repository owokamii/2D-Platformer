using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private Transform sideAttackTransform;
    [SerializeField] private Vector2 sideAttackArea;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private PlayerLedgeGrab playerLedgeGrab;

    private float attackTime = 0.3f;
    private bool isAttacking = false;
    private bool canAttack = true;

    public bool GetisAttacking { get; private set; }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerLedgeGrab = GetComponent<PlayerLedgeGrab>();
    }
    private void Update()
    {
        HandleAttack();

        if (isAttacking) return;
    }

    private void HandleAttack()
    {
        GetisAttacking = isAttacking;

        if (!playerLedgeGrab.GetisGrabbing)
        {
            if (Input.GetMouseButtonDown(0) && canAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        isAttacking = true;

        rigidBody.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(attackTime); // need to be adjusted according to attack 1 - 3's total time frame

        isAttacking = false;
        canAttack = true;
    }
}
