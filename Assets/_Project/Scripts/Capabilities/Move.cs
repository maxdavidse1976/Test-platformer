using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] InputController _input = null;
    [SerializeField, Range(0f, 100f)] float _maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] float _maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] float _maxAirAcceleration= 20f;

    Vector2 _direction;
    Vector2 _desiredVelocity;
    Vector2 _velocity;
    Rigidbody2D _rigidbody;
    Ground _ground;

    float _maxSpeedChange;
    float _acceleration;
    bool _onGround;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
    }

    void Update()
    {
        _direction.x = _input.RetrieveMoveInput();
        _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - _ground.GetFriction(), 0f);
    }

    void FixedUpdate()
    {
        _onGround = _ground.GetOnGround();
        _velocity = _rigidbody.velocity;

        _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

        _rigidbody.velocity = _velocity;
    }
}
