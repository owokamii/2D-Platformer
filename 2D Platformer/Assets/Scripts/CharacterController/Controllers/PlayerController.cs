using UnityEngine;

[CreateAssetMenuAttribute(fileName = "PlayerController", menuName = "InputController/PlayerController")]
public class PlayerController : InputController
{
    public override bool RetrieveJumpHoldInput()
    {
        return Input.GetButton("Jump");
    }

    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }
}
