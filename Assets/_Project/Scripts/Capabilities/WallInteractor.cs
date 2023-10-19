using UnityEngine;

namespace DragonspiritGames.TestPlatformer
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class WallInteractor : MonoBehaviour
    {
        public bool WallJumping { get; private set; }

        [Header("Wall Slide")]
        [SerializeField][Range(0.1f, 5f)] float _wallSlideMaxSpeed = 2f;

        [Header("Wall Jump")]
        [SerializeField] Vector2 _wallJumpClimb = new Vector2(4f, 12f);
        [SerializeField] Vector2 _wallJumpBounce = new Vector2(10.7f, 10f);
        [SerializeField] Vector2 _wallJumpLeap = new Vector2(14f, 12f);

        [Header("Wall Stick")]
        [SerializeField, Range(0.05f, 0.5f)] float _wallStickTime = 0.25f;


        CollisionDataRetriever _collisionDataRetriever;
        Rigidbody2D _rigidbody;
        Controller _controller;

        Vector2 _velocity;
        bool _onWall, _onGround, _desiredJump, _isJumpReset;
        float _wallDirectionX, _wallStickCounter;

        void Awake()
        {
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _controller = GetComponent<Controller>();
            _isJumpReset = true;
        }

        void Update()
        {
            _desiredJump = _controller.input.RetrieveJumpInput(this.gameObject);
        }

        void FixedUpdate()
        {
            _velocity = _rigidbody.velocity;
            _onWall = _collisionDataRetriever.OnWall;
            _onGround = _collisionDataRetriever.OnGround;
            _wallDirectionX = _collisionDataRetriever.ContactNormal.x;


            #region Wall Slide
            if (_onWall)
            {
                if (_velocity.y < -_wallSlideMaxSpeed)
                {
                    _velocity.y = _wallSlideMaxSpeed;
                }
            }
            #endregion

            #region Wall Stick
            if (_collisionDataRetriever.OnWall && !_collisionDataRetriever.OnGround && !WallJumping)
            {
                if (_wallStickCounter > 0)
                {
                    _velocity.x = 0;

                    if (_controller.input.RetrieveMoveInput(this.gameObject) != 0 &&
                        Mathf.Sign(_controller.input.RetrieveMoveInput(this.gameObject)) == Mathf.Sign(_collisionDataRetriever.ContactNormal.x))
                    {
                        _wallStickCounter -= Time.deltaTime;
                    }
                    else
                    {
                        _wallStickCounter = _wallStickTime;
                    }
                }
                else
                {
                    _wallStickCounter = _wallStickTime;
                }
            }
            #endregion


            #region Wall Jump
            if ((_onWall && _velocity.x == 0) || _onGround)
            {
                WallJumping = false;
            }

            if (_onWall && !_onGround)
            {
                if (_desiredJump && _isJumpReset)
                {
                    if (_controller.input.RetrieveMoveInput(this.gameObject) == 0)
                    {
                        _velocity = new Vector2(_wallJumpBounce.x * _wallDirectionX, _wallJumpBounce.y);
                        WallJumping = true;
                        _desiredJump = false;
                        _isJumpReset = false;
                    }
                    else if (Mathf.Sign(-_wallDirectionX) == Mathf.Sign(_controller.input.RetrieveMoveInput(this.gameObject)))
                    {
                        _velocity = new Vector2(_wallJumpClimb.x * _wallDirectionX, _wallJumpClimb.y);
                        WallJumping = true;
                        _desiredJump = false;
                        _isJumpReset = false;
                    }
                    else
                    {
                        _velocity = new Vector2(_wallJumpLeap.x * _wallDirectionX, _wallJumpLeap.y);
                        WallJumping = true;
                        _desiredJump = false;
                        _isJumpReset = false;
                    }
                }
                else if (!_desiredJump)
                {
                    _isJumpReset = true;
                }
            }
            #endregion

            _rigidbody.velocity = _velocity;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            _collisionDataRetriever.EvaluateCollision(collision);
            _isJumpReset = false;

            if (_collisionDataRetriever.OnWall && !_collisionDataRetriever.OnGround && WallJumping)
            {
                _rigidbody.velocity = Vector2.zero;
            }
        }
    }
}
