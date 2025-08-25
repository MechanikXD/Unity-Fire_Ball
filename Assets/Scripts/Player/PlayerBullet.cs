using Core.Tower;
using UnityEngine;

namespace Player {
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerBullet : MonoBehaviour {
        private Rigidbody _body;
        [SerializeField] private float _timeToLive; // Time after which object will be destroyed.
        [SerializeField] private Vector3 _moveSpeed;
        private float _currentLiveTime;

        private void Awake() => Initialize();

        private void Update() => UpdateTimeToLive();
        // Placeholder function. Similar wrapper that is used in TowerElement.cs
        private void DestroySelf() => Destroy(gameObject);

        private void OnCollisionEnter(Collision other) => TryDestroyTowerElement(other);
        
        private void Initialize() {
            _currentLiveTime = 0f;
            _body = GetComponent<Rigidbody>();
            _body.linearVelocity = _moveSpeed;
        }

        private void UpdateTimeToLive() {
            _currentLiveTime += Time.deltaTime;
            if (_currentLiveTime > _timeToLive) DestroySelf();
        }

        private void TryDestroyTowerElement(Collision other) {
            if (other.gameObject.TryGetComponent<TowerElement>(out var element)) element.TakeHit();
            
            DestroySelf();
        }
    }
}