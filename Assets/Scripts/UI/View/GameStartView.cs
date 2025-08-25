using UnityEngine;

namespace UI.View {
    public class GameStartView : CanvasBase {
        public override void EnableCanvas() {
            base.EnableCanvas();
            Time.timeScale = 0f;
        }

        public override void DisableCanvas() {
            base.DisableCanvas();
            Time.timeScale = 1f;
        }
    }
}