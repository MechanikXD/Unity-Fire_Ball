using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Obstacle {
    public class MovingObstacle : MonoBehaviour {
        [SerializeField] private MoveType _type;
        private Dictionary<MoveType, Func<Action>> _initialize;
        private Action _moveAction;

        [Header("Bounce Movement")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Transform _firstPoint;
        [SerializeField] private Transform _secondPoint;
        [SerializeField] private float _margin;
        private bool _isMovingToSecond;
        private Vector3 _moveVector;

        [Header("Pivot")]
        [SerializeField] private Vector3 _rotateSpeed;
        [SerializeField] private Transform _pivot;
        [SerializeField] private Vector3 _radius;

        private void Awake() {
            _initialize = new Dictionary<MoveType, Func<Action>> {
                [MoveType.Pivot] = () => {
                    transform.localPosition = _radius;

                    return () => transform.RotateAround(_pivot.position, _rotateSpeed.normalized,
                        _rotateSpeed.magnitude);
                },
                [MoveType.Bounce] = () => {
                    var firstPosition = _firstPoint.position;
                    var secondPosition = _secondPoint.position;
                    
                    transform.localPosition = firstPosition;
                    _moveVector = firstPosition - secondPosition;

                    return () => {
                        if (_isMovingToSecond) {
                            transform.Translate(
                                -_moveVector.normalized * _moveSpeed * Time.deltaTime);

                            if (IsNearPoint(transform, secondPosition, _margin)) {
                                _isMovingToSecond = false;
                            }
                        }
                        else {
                            transform.Translate(_moveVector.normalized * _moveSpeed *
                                                Time.deltaTime);

                            if (IsNearPoint(transform, firstPosition, _margin)) {
                                _isMovingToSecond = true;
                            }
                        }
                    };
                }
            };
            _moveAction = _initialize[_type]();
        }

        private void Update() {
            _moveAction();
        }

        private static bool IsNearPoint(Transform obj, Vector3 point, float margin) {
            var dist = Vector3.Distance(obj.position, point);
            return dist < margin && dist > -margin;
        }
    }
}