using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] InputController _input = null;
    [SerializeField, Range(0f, 100f)] float _jumpHeight = 3f;
    [SerializeField, Range(0f, 5f)] int _maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] float _downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] float _upwardMovementMultiplier = 1.7f;

    Rigidbody2D _rigidbody;
    Ground _ground;
    Vector2 _velocity;

    int _jumpPhase;
    float _defaultGravityScale;
    bool _desiredJump;
    bool _onGround;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();

        _defaultGravityScale = 1f;
    }

    void Update()
    {
        _desiredJump |= _input.RetrieveJumpInput();
    }

    void FixedUpdate()
    {
        _onGround = _ground.GetOnGround();
        _velocity = _rigidbody.velocity;

        if (_onGround) 
        {
            _jumpPhase = 0;
        }
        if (_desiredJump)
        {
            _desiredJump = false;
            JumpAction();
        }

        // Change gravity scale depending on falling or jumping or default gravity if we are on the ground
        if (_rigidbody.velocity.y > 0)
        {
            _rigidbody.gravityScale = _upwardMovementMultiplier;
        }
        else if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.gravityScale = _downwardMovementMultiplier;
        }
        else if (_rigidbody.velocity.y == 0)
        {
            _rigidbody.gravityScale = _defaultGravityScale;
        }

        _rigidbody.velocity = _velocity;
    }

    void JumpAction()
    {
        if (_onGround || _jumpPhase < _maxAirJumps) 
        {
            _jumpPhase++;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);
            if (_velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - _velocity.y, 0f);
            }
            _velocity.y += jumpSpeed;
        }
    }

}
