using UnityEngine;

namespace UI.View {
    public class GameStartView : CanvasBase {
        public override void EnableCanvas() {
            base.EnableCanvas();
            // NOTE: TimeScale should be handled within UIManager
            Time.timeScale = 0f;
        }

        public override void DisableCanvas() {
            base.DisableCanvas();
            // NOTE: TimeScale should be handled within UIManager
            Time.timeScale = 1f;
        }
    }
}