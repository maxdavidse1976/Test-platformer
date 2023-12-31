using UnityEngine;

namespace DragonspiritGames.TestPlatformer
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class Move : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f)] float _maxSpeed = 4f;
        [SerializeField, Range(0f, 100f)] float _maxAcceleration = 35f;
        [SerializeField, Range(0f, 100f)] float _maxAirAcceleration = 20f;

        Controller _controller;
        Vector2 _direction, _desiredVelocity, _velocity;
        Rigidbody2D _rigidbody;
        CollisionDataRetriever _collisionDataRetriever;
        WallInteractor _wallInteractor;

        float _maxSpeedChange, _acceleration, _wallStickCounter;
        bool _onGround;

        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _controller = GetComponent<Controller>();
            _wallInteractor = GetComponent<WallInteractor>();
        }

        void Update()
        {
            _direction.x = _controller.input.RetrieveMoveInput(this.gameObject);
            _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - _collisionDataRetriever.Friction, 0f);
        }

        void FixedUpdate()
        {
            _onGround = _collisionDataRetriever.OnGround;
            _velocity = _rigidbody.velocity;

            _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
            _maxSpeedChange = _acceleration * Time.deltaTime;
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

            _rigidbody.velocity = _velocity;
        }
    }
}