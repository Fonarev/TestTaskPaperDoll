namespace Assets.Project
{
    public class GlobalInstaller : Installer
    {
        public override void Installize()
        {
            container.Reg<GameStateData>();
            container.Reg<GameStateProxy>();
        }
    }
}