using System;
using System.IO;
using System.Threading.Tasks;
using Data;
using UnityEngine;

namespace Gameplay
{
    public class PlayerDataSaver
    {
        private readonly string _absoluteSavePath;
        public PlayerData PlayerData;

        public PlayerDataSaver(string path)
        {
            _absoluteSavePath = Path.Combine(Application.persistentDataPath, path);
        }
        
        public async Task Save()
        {
            string json = JsonUtility.ToJson(PlayerData);
            await File.WriteAllTextAsync(_absoluteSavePath, json);            
        }

        public void Load()
        {
            if (File.Exists(_absoluteSavePath))
            {
                try
                {
                    string json = File.ReadAllText(_absoluteSavePath);
                    PlayerData = JsonUtility.FromJson<PlayerData>(json);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    CreateSaveFile();
                }
            }
            else
            {
                CreateSaveFile();    
            }
        }

        private void CreateSaveFile()
        {
            if (File.Exists(_absoluteSavePath))
            {
                File.Delete(_absoluteSavePath);
            }

            PlayerData = new PlayerData();
            string json = JsonUtility.ToJson(PlayerData);
            File.WriteAllText(_absoluteSavePath, json);            
        }
    }
}