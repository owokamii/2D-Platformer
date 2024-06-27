using UnityEngine;

public class CollisionDataRetriever : MonoBehaviour
{
    private PhysicsMaterial2D material;

    public Vector2 ContactNormal { get; private set; }
    public bool OnGround { get; private set; }
    public bool OnWall { get; private set; }
    public float Friction { get; private set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnGround = false;
        OnWall = false;
        Friction = 0.0f;
    }

    public void EvaluateCollision(Collision2D collision)
    {
        for(int i = 0; i < collision.contactCount; i++)
        {
            ContactNormal = collision.GetContact(i).normal;
            OnGround |= ContactNormal.y >= 0.9f;
            OnWall = Mathf.Abs(ContactNormal.x) >= 0.9f;
        }
    }

    private void RetrieveFriction(Collision2D collision)
    {
        material = collision.rigidbody.sharedMaterial;

        Friction = 0;

        if(material != null)
        {
            Friction = material.friction;
        }
    }

    public bool GetOnGround()
    {
        return OnGround;
    }

    public float GetFriction()
    {
        return Friction;
    }
}
