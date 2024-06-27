using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerBaseState currentState;
    public PlayerIdleState playerIdleState = new PlayerIdleState();
    public PlayerRunState playerRunState = new PlayerRunState();
    public PlayerJumpState playerJumpState = new PlayerJumpState();

    private void Start()
    {
        currentState = playerIdleState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);

        // if detect left right movement - enter walk

        // if detected sprint - enter run

        // if detected jump - enter jump
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        currentState.OnCollisionEnter2D(this, collision2D);
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
