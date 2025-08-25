using UnityEngine;

namespace UI {
    [RequireComponent(typeof(Canvas))]
    public abstract class CanvasBase : MonoBehaviour {
        protected Canvas ThisCanvas;

        protected virtual void Awake() => GetCanvasReference();

        private void GetCanvasReference() => ThisCanvas = GetComponent<Canvas>();

        public virtual void EnableCanvas() => ThisCanvas.enabled = true;

        public virtual void DisableCanvas() => ThisCanvas.enabled = false;
    }
}