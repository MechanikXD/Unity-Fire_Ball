﻿using System;
using Core.Game;
using TMPro;
using UnityEngine;

namespace UI.View {
    public class HudView : CanvasBase {
        [SerializeField] private TMP_Text _countdown;

        private void Update() {
            _countdown.SetText(TimeSpan.FromSeconds(GameManager.Instance.CurrentTime).ToString());
        }
    }
}