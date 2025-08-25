using System;
using UnityEngine;

namespace Core.Tower {
    public class TowerElement : MonoBehaviour {
        [SerializeField] private int _hitsToDestroy;
        private int _hitsTaken;
        [SerializeField] private float _objectHeight; // Needed for correct tower falling
        private bool _isFinalElement;
        [SerializeField] private ParticleSystem _destroyParticles;

        public static event Action<TowerElementDestroyedEventArgs> TowerElementDestroyed;
        public static event Action<TowerElementDestroyedEventArgs> FinalTowerElementDestroyed; 

        public float Height => _objectHeight;

        public void SetIsLastElement() => _isFinalElement = true;

        public void TakeHit() {
            _hitsTaken++;
            if (_hitsTaken >= _hitsToDestroy) DestroySelf();
        }
        /// <summary>
        /// Small wrapper function that does additional things alongside destroying the object.
        /// </summary>
        private void DestroySelf() {
            var destroyedData = new TowerElementDestroyedEventArgs(_objectHeight, _isFinalElement);
            
            if (_isFinalElement) FinalTowerElementDestroyed?.Invoke(destroyedData);
            else TowerElementDestroyed?.Invoke(destroyedData);
            
            if (_destroyParticles != null) _destroyParticles.Play();
            Destroy(gameObject);
        }
    }
}
