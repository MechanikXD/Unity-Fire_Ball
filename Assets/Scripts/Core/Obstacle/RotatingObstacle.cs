using UnityEngine;

namespace Core.Obstacle {
    public class RotatingObstacle : MonoBehaviour {
        [SerializeField] private Vector3 _rotateSpeed;
        [SerializeField] private Transform _pivot;
        [SerializeField] private Vector3 _startingPosition;

        private Vector3 _pivotPosition;
        
        private void Awake() => Initialize();
        
        private void Update() => RotateObstacle();
        
        private void Initialize() {
            _pivotPosition = _pivot.position;
            transform.localPosition = _startingPosition;
        }

        private void RotateObstacle() {
            transform.RotateAround(_pivotPosition, _rotateSpeed.normalized,
                _rotateSpeed.magnitude);
        }
    }
}