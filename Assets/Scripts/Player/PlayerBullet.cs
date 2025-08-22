using Core.Tower;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerBullet : MonoBehaviour {
        private Rigidbody _body;
        [SerializeField] private float _timeToLive;
        [SerializeField] private Vector3 _moveSpeed;
        private float _currentLiveTime;

        private void Awake() {
            _currentLiveTime = 0f;
            _body = GetComponent<Rigidbody>();
            _body.linearVelocity = _moveSpeed;
        }

        private void Update() {
            _currentLiveTime += Time.deltaTime;
            if (_currentLiveTime > _timeToLive) DestroySelf();
        }

        private void DestroySelf() {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other) {
            if (!other.gameObject.TryGetComponent<TowerElement>(out var element)) return;

            element.TakeHit();
            DestroySelf();
        }
    }
}