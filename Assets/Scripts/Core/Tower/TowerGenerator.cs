using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Tower {
    /// <summary>
    /// Must be present on the scene to create tower.
    /// </summary>
    public class TowerGenerator : MonoBehaviour {
        [SerializeField] private Transform _origin;
        [SerializeField] private int _towerSize;    // Number of tower elements
        [SerializeField] private TowerElement[] _elements;
        [SerializeField] private TowerElement _finalElement;
        [SerializeField] private GenerationType _type;
        
        private Dictionary<GenerationType, Func<TowerElement[]>> _towerElementGetter;
        private GameObject _towerObject;
        // Future proof: [SerializeField] private float _fallSpeed;
        private Action _unSubscriber;

        private void OnEnable() => SubscribeToEvents();
        
        private void Awake() {
            Initialize();
            GenerateTower();
        }

        private void OnDisable() => UnsubscribeFromEvents();
        /// <summary>
        /// Generates tower as well as initializes _towerObject.
        /// </summary>
        private void GenerateTower() {
            // All tower elements are parented to this one,
            // So only this object should be moved to lower tower.
            _towerObject = new GameObject("Tower") {
                transform = {
                    position = _origin.position,
                    rotation = _origin.rotation
                }
            };
            var towerTransform = _towerObject.transform;
            // Place tower elements
            var elements = _towerElementGetter[_type]();
            var totalHeight = elements[0].Height / 2f;
            
            for (var i = 0; i < _towerSize; i++) {
                var newPosition = new Vector3(0, totalHeight, 0);
                Instantiate(elements[i], newPosition, Quaternion.identity, towerTransform);
                totalHeight += elements[i].Height;
            }
            // Final tower element
            var lastPosition = new Vector3(0, totalHeight, 0);
            var instance = Instantiate(_finalElement, lastPosition, Quaternion.identity,
                towerTransform);
            instance.SetIsLastElement();
        }
        
        private void UnsubscribeFromEvents() => _unSubscriber();
        
        private void SubscribeToEvents() {
            void LowerTowerOnElementDestroyed(TowerElementDestroyedEventArgs args) {
                // TODO: Move object instead of teleporting it
                var pos = _towerObject.transform.position;
                pos.y -= args.Height;
                _towerObject.transform.position = pos;
            }

            TowerElement.TowerElementDestroyed += LowerTowerOnElementDestroyed;

            _unSubscriber = () => {
                TowerElement.TowerElementDestroyed -= LowerTowerOnElementDestroyed;
            };
        }
        
        private void Initialize() {
            // This dictionary returns an array of tower element and in which order they should be placed.
            // Fancy way to avoid using switch statement.  
            _towerElementGetter = new Dictionary<GenerationType, Func<TowerElement[]>> {
                [GenerationType.Random] = () => {
                    var elements = new TowerElement[_towerSize];
                    for (var i = 0; i < _towerSize; i++) {
                        elements[i] = _elements[Random.Range(0, _elements.Length)];
                    }

                    return elements;
                },
                [GenerationType.Cycling] = () => {
                    var elements = new TowerElement[_towerSize];
                    for (var i = 0; i < _towerSize; i++) {
                        elements[i] = _elements[i % _elements.Length];
                    }

                    return elements;
                },
                [GenerationType.PingPong] = () => {
                    var elements = new TowerElement[_towerSize];
                    var index = 0;
                    var currentIncrement = 1;
                    for (var i = 0; i < _towerSize; i++) {
                        elements[i] = _elements[index];

                        if (_elements.Length == 1) {
                            currentIncrement = 0;
                        }
                        else if (index + 1 >= _elements.Length) {
                            currentIncrement = -1;
                        }
                        else if (index - 1 < 0) {
                            currentIncrement = 1;
                        }

                        index += currentIncrement;
                    }

                    return elements;
                }
            };
        }
    }
}
