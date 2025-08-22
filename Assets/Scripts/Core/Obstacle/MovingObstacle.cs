using UnityEngine;

namespace Core.Obstacle {
    public class MovingObstacle : MonoBehaviour {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Transform _firstPoint;
        [SerializeField] private Transform _secondPoint;

        private Vector3 _firstPosition;
        private Vector3 _secondPosition;
        private bool _isMovingForward;
        private Vector3 _moveVector;
        [SerializeField] private float _distanceMargin;

        private void Awake() => Initialize();

        private void Update() => MoveBackAndForth();
        
        private void Initialize() {
            _firstPosition = _firstPoint.position;
            _secondPosition = _secondPoint.position;
            
            _isMovingForward = true;
            _moveVector = _secondPosition - _firstPosition;

            transform.position = _firstPosition;
        }

        private void MoveBackAndForth() {
            if (_isMovingForward) {
                transform.Translate(_moveVector * (_moveSpeed * Time.deltaTime));

                if (IsNearPoint(transform, _secondPosition, _distanceMargin))
                    _isMovingForward = false;
            }
            else {
                transform.Translate(-_moveVector * (_moveSpeed * Time.deltaTime));

                if (IsNearPoint(transform, _firstPosition, _distanceMargin))
                    _isMovingForward = true;
            }
        }

        private static bool IsNearPoint(Transform obj, Vector3 point, float margin) {
            var dist = Vector3.Distance(obj.position, point);
            return dist < margin && dist > -margin;
        }
    }
}