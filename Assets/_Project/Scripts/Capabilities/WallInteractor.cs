using UnityEngine;

public class WallInteractor : MonoBehaviour
{
    public bool WallJumping { get; private set; }

    [Header("Wall Slide")]
    [SerializeField][Range(0.1f, 5f)] float _wallSlideMaxSpeed = 2f;

    [Header("Wall Jump")]
    [SerializeField] Vector2 _wallJumpClimb = new Vector2(4f, 12f);
    [SerializeField] Vector2 _wallJumpBounce = new Vector2(10.7f, 10f);

    CollisionDataRetriever _collisionDataRetriever;
    Rigidbody2D _rigidbody;
    Controller _controller;

    Vector2 _velocity;
    bool _onWall, _onGround, _desiredJump;
    float _wallDirectionX;

    void Awake()
    {
        _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _controller = GetComponent<Controller>();
    }

    void Update()
    {
        if (_onWall && !_onGround)
        {
            _desiredJump |= _controller.input.RetrieveJumpInput();
        }
    }

    void FixedUpdate()
    {
        _velocity = _rigidbody.velocity;
        _onWall = _collisionDataRetriever.OnWall;
        _onGround = _collisionDataRetriever.OnGround;
        _wallDirectionX = _collisionDataRetriever.ContactNormal.x;


        #region Wall Slide
        if (_onWall )
        {
            if (_velocity.y < -_wallSlideMaxSpeed)
            {
                _velocity.y = _wallSlideMaxSpeed;
            }
        }
        #endregion

        #region Wall Jump
        if ((_onWall && _velocity.x == 0) || _onGround)
        {
            WallJumping = false;
        }

        if (_desiredJump) 
        { 
            if (-_wallDirectionX == _controller.input.RetrieveMoveInput())
            {
                _velocity = new Vector2(_wallJumpClimb.x * _wallDirectionX, _wallJumpClimb.y);
                WallJumping = true;
                _desiredJump = false;
            }
            else if (_controller.input.RetrieveMoveInput() == 0)
            {
                _velocity = new Vector2(_wallJumpBounce.x * _wallDirectionX, _wallJumpBounce.y);
                WallJumping = true;
                _desiredJump = false;
            }
        }
        #endregion

        _rigidbody.velocity = _velocity;
    }
}
