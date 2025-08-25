namespace Core.Tower {
    public struct TowerElementDestroyedEventArgs {
        public float Height { get; }
        public bool WasFinal { get; }

        public TowerElementDestroyedEventArgs(float height, bool wasFinal) {
            WasFinal = wasFinal;
            Height = height;
        }
    }
}