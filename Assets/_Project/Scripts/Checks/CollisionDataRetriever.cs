using UnityEngine;

public class CollisionDataRetriever : MonoBehaviour
{
    public bool OnGround { get; private set; }
    public bool OnWall { get; private set; }
    public float Friction { get; private set; }
    public Vector2 ContactNormal { get; private set; }
    
    PhysicsMaterial2D _material;


    void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        OnGround = false;
        OnWall = false;
        Friction = 0f;
    }

    public void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactNormal = collision.GetContact(i).normal;
            OnGround |= ContactNormal.y >= 0.9f;
            OnWall = Mathf.Abs(ContactNormal.x) >= 0.9f;
        }
    }

    void RetrieveFriction(Collision2D collision)
    {
        _material = collision.rigidbody.sharedMaterial;

        Friction = 0;
        if (_material != null)
        {
            Friction = _material.friction;
        }
    }
}
