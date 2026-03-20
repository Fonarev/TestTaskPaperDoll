namespace Assets.Project
{
    public class GameStateProxy
    {
        public  GameStateData Data;
        internal bool OpenedWindows;

        public GameStateProxy(GameStateData data)
        {
            Data = data;
        }
    }
}