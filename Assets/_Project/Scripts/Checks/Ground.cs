using UnityEngine;

public class Ground : MonoBehaviour
{
    bool _onGround;
    float _friction;

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
        _onGround = false;
        _friction = 0f;
    }

    void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            _onGround |= normal.y >= 0.9f;
        }
    }

    void RetrieveFriction(Collision2D collision)
    {
        PhysicsMaterial2D material = collision.rigidbody.sharedMaterial;

        _friction = 0;
        if (material != null)
        {
            _friction = material.friction;
        }
    }

    public bool GetOnGround() => _onGround;

    public float GetFriction() => _friction;
}
