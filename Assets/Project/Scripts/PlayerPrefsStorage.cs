using System;
using System.Collections;

using UnityEngine;

namespace Assets.Project
{
    public class PlayerPrefsStorage : IStorageData
    {
        private const string Data = "data";

        private readonly GameStateProxy gameState;

        public PlayerPrefsStorage(GameStateProxy gameState)
        {
            this.gameState = gameState;
        }

        public void Save(object gameStateData, bool flush = false)
        {
            var data = JsonUtility.ToJson(gameStateData);
            PlayerPrefs.SetString(Data, data);
        }

        public void Load(Action<bool> sendback)
        {
            string jsonData = PlayerPrefs.GetString(Data);

            if (jsonData != "")
            {
                try
                {
                    gameState.Data = JsonUtility.FromJson<GameStateData>(jsonData);
                    sendback?.Invoke(true);
                }
                catch (Exception e)
                {
                    Debug.LogError("Cloud Load Error: " + e.Message);
                    sendback?.Invoke(false);
                }
            }
            else
            {
                sendback?.Invoke(false);
            }
        }
    }
}