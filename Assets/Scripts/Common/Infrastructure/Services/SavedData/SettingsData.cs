using System;
using UnityEngine;

namespace Common.Infrastructure.Services.SavedData
{
    [Serializable]
    public class SettingsData
    {
        [SerializeField] public bool SoundOn;
        [SerializeField] public bool MusicOn;

        public SettingsData()
        {
            SoundOn = true;
            MusicOn = true;
        }
    }
}