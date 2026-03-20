using System.Collections;

using UnityEngine;

namespace Assets.Project
{
    public class LocalDI : AbstractDI
    {
        private GlobalDI globalDI;

        public bool IsAllInitiallize { get; private set; }
        public ContainerDI Container { get; private set; }

        public bool IsLoadedData => IsAllInitiallize;

        private void Awake()
        {
            FindGlobalPacker();
            StartCoroutine(InitRoutine());
        }

        private IEnumerator InitRoutine()
        {
            while (!globalDI.IsAllInitiallize)
                yield return globalDI.Initialize();

            IsAllInitiallize = Initialize();
        }

        private bool Initialize()
        {
            Container = new(globalDI.Container);

            foreach (var installer in installers)
            {
                installer.Initialize(Container);
                installer.Installize();
                installer.InstallizeMono();
            }

            Message($"Installize {this}");

            return true;
        }

        private void FindGlobalPacker()
        {
            globalDI = FindAnyObjectByType<GlobalDI>();
            
            if (globalDI == null) globalDI = CreateGlobalPart();
        }

        private GlobalDI CreateGlobalPart()
        {
            var newGlobalPacker = Instantiate(new GameObject("GlobalDI"));
            var globalPacker = newGlobalPacker.AddComponent<GlobalDI>();
            globalPacker.Initialize();

            return globalPacker;
        }

    }
}