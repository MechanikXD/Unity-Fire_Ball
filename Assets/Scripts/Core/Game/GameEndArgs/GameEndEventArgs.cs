namespace Core.Game.GameEndArgs {
    public struct GameEndEventArgs {
        public float RemainingTime { get; }
        public GameEndState GameState { get; }
        public PlayerScore Score { get; }

        public GameEndEventArgs(float remainingTime, GameEndState state, PlayerScore score) {
            RemainingTime = remainingTime;
            GameState = state;
            Score = score;
        }
    }
}