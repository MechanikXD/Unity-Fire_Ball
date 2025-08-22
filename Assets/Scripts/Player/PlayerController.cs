using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour {
        [SerializeField] private PlayerBullet _bulletPrefab;
        [SerializeField] private Transform _shootOrigin;
        [SerializeField] private float _delayBetweenShots;
        private bool _isShooting;
        private PlayerInput _playerInput;
        private InputAction _pressAction;
        private const string PressActionKey = "Press";

        private Coroutine _shootCoroutine;
        private Action _unSubscriber;

        private void Awake() {
            _playerInput = GetComponent<PlayerInput>();
            _pressAction = _playerInput.actions[PressActionKey];

            void StartShooting(InputAction.CallbackContext _) {
                _isShooting = true;
                StartCoroutine(ShootWhilePressed());
            }

            void StopShooting(InputAction.CallbackContext _) => _isShooting = false;

            _pressAction.started += StartShooting;
            _pressAction.canceled += StopShooting;

            _unSubscriber = () => {
                _pressAction.started -= StartShooting;
                _pressAction.canceled -= StopShooting;
            };
        }

        private void OnDisable() {
            UnsubscribeFromEvents();
            StopCoroutineOnDisable();
        }

        private IEnumerator ShootWhilePressed() {
            while (_isShooting) {
                Instantiate(_bulletPrefab, _shootOrigin.position, _shootOrigin.rotation, _shootOrigin);
                yield return new WaitForSeconds(_delayBetweenShots);
            }
        }

        private void UnsubscribeFromEvents() => _unSubscriber();

        private void StopCoroutineOnDisable() {
            if (_shootCoroutine == null) return;

            StopCoroutine(_shootCoroutine);
            _shootCoroutine = null;
        }
    }
}