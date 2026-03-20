namespace Assets.Project
{
    public class GlobalDI : AbstractDI
    {
        public bool IsAllInitiallize { get; private set; }
        public ContainerDI Container { get; private set; }

        public bool Initialize()
        {
            Container = new();

            foreach (var installer in installers)
            {
                installer.Initialize(Container);
                installer.Installize();
                installer.InstallizeMono();
            }

            IsAllInitiallize = true;

            DontDestroyOnLoad(gameObject);

            Message($"Installize {this}");

            return IsAllInitiallize;
        }

    }
}