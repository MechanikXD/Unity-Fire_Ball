using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Tower {
    public class TowerGenerator : MonoBehaviour {
        [SerializeField] private Transform _origin;
        [SerializeField] private int _towerSize;
        [SerializeField] private TowerElement[] _elements;
        [SerializeField] private TowerElement _finalElement;
        [SerializeField] private GenerationType _type;

        private Dictionary<GenerationType, Func<TowerElement[]>> _towerElementGetter;

        private void Awake() {
            Initialize();
            GenerateTower();
        }

        private void Initialize() {
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

        private void GenerateTower() {
            var towerObject = new GameObject("Tower") {
                transform = {
                    position = _origin.position,
                    rotation = _origin.rotation
                }
            };
            var towerTransform = towerObject.transform;
            
            var elements = _towerElementGetter[_type]();
            var totalHeight = elements[0].Height / 2f;
            
            for (var i = 0; i < _towerSize; i++) {
                var newPosition = new Vector3(0, totalHeight, 0);
                Instantiate(elements[i], newPosition, Quaternion.identity, towerTransform);
                totalHeight += elements[i].Height;
            }
            
            var lastPosition = new Vector3(0, totalHeight, 0);
            Instantiate(_finalElement, lastPosition, Quaternion.identity, towerTransform);
        }
    }
}
