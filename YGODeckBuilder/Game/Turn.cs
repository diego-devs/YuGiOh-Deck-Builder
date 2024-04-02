namespace YGODeckBuilder.Game
{
    public class Turn
    {
        public Turn(Player p)
        {
            ActivePlayer = p;
        }

        public Player ActivePlayer { get; set; }
        public void DrawPhase()
        {
            this.ActivePlayer.Draw();
        }
        public void MainPhase()
        {

        }
        public void BattlePhase()
        {

        }
        public void MainPhase_2()
        {

        }
        public void EndPhase()
        {

        }
    }
}
