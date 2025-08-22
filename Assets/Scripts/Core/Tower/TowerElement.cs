using System;
using UnityEngine;

namespace Core.Tower {
    public class TowerElement : MonoBehaviour {
        [SerializeField] private int _hitsToDestroy;
        [SerializeField] private ParticleSystem _destroyParticles;
        private int _hitsTaken;
        [SerializeField] private float _objectHeight;
        private bool _isFinalElement;

        public static event Action<TowerElementDestroyedEventArgs> TowerElementDestroyed;
        public static event Action<TowerElementDestroyedEventArgs> FinalTowerElementDestroyed; 

        public float Height => _objectHeight;

        public void SetIsLastElement() => _isFinalElement = true;

        private void TakeHit() {
            _hitsTaken++;
            if (_hitsTaken >= _hitsToDestroy) DestroySelf();
        }

        private void DestroySelf() {
            var destroyedData = new TowerElementDestroyedEventArgs(_objectHeight, _isFinalElement);
            
            if (_isFinalElement) FinalTowerElementDestroyed?.Invoke(destroyedData);
            else TowerElementDestroyed?.Invoke(destroyedData);
            
            if (_destroyParticles != null) _destroyParticles.Play();
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other) {
            // if (other.gameObject.TryGetComponent<PlayerBullet>(out _)) TakeHit();
        }
    }
}
