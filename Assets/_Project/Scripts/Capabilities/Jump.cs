using UnityEngine;

[RequireComponent(typeof(Controller))]
public class Jump : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] float _jumpHeight = 3f;
    [SerializeField, Range(0f, 5f)] int _maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] float _downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] float _upwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0f, 0.3f)] float _coyoteTime = 0.2f;
    [SerializeField, Range(0f, 0.3f)] float _jumpBufferTime = 0.2f;

    Controller _controller;
    Rigidbody2D _rigidbody;
    Ground _ground;
    Vector2 _velocity;

    int _jumpPhase;
    float _defaultGravityScale, _jumpSpeed, _coyoteCounter, _jumpBufferCounter;
    bool _desiredJump, _onGround, _isJumping;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
        _controller = GetComponent<Controller>();

        _defaultGravityScale = 1f;
    }

    void Update()
    {
        _desiredJump |= _controller.input.RetrieveJumpInput();
    }

    void FixedUpdate()
    {
        _onGround = _ground.OnGround;
        _velocity = _rigidbody.velocity;

        if (_onGround && _rigidbody.velocity.y == 0) 
        {
            _jumpPhase = 0;
            _coyoteCounter = _coyoteTime;
            _isJumping = false;
        }
        else
        {
            _coyoteCounter -= Time.deltaTime;
        }

        if (_desiredJump)
        {
            _desiredJump = false;
            _jumpBufferCounter = _jumpBufferTime;
        }
        else if (!_desiredJump && _jumpBufferCounter > 0)
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        if (_jumpBufferCounter > 0)
        {
            JumpAction();
        }

        // Change gravity scale depending on falling or jumping or default gravity if we are on the ground
        if (_controller.input.RetrieveJumpHoldInput() && _rigidbody.velocity.y > 0)
        {
            _rigidbody.gravityScale = _upwardMovementMultiplier;
        }
        else if (!_controller.input.RetrieveJumpHoldInput() && _rigidbody.velocity.y < 0)
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
        if (_coyoteCounter > 0f || (_jumpPhase < _maxAirJumps && _isJumping))
        {
            if (_isJumping)
            {
                _jumpPhase++;
            }
            _jumpBufferCounter = 0;
            _coyoteCounter = 0;
            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);
            _isJumping = true;

            if (_velocity.y > 0)
            {
                _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
            }
            else if (_velocity.y < 0)
            {
                _jumpSpeed += Mathf.Abs(_rigidbody.velocity.y);
            }
            _velocity.y += _jumpSpeed;
        }
    }

}
