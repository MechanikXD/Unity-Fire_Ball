using UnityEngine;

namespace Core.Tower {
    public class TowerElement : MonoBehaviour {
        [SerializeField] private int _hitsToDestroy;
        private int _hitsTaken;
        [SerializeField] private float _objectHeight;

        public float Height => _objectHeight;

        public void TakeHit() {
            _hitsTaken++;
            if (_hitsTaken >= _hitsToDestroy) DestroySelf();
        }

        public void DestroySelf() {
            Destroy(gameObject);
        }
    }
}
