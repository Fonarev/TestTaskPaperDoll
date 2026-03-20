using System;

namespace Assets.Project
{
    public interface IStorageData
    {
        void Load(Action<bool> sendback);
        void Save(object gameStateData, bool flush = false);
       
    }
}