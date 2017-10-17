using System;
using UnityEngine;

namespace BotArena {
    [Serializable]
    internal class Sound {
        public AudioClip audio;
        public string id;
        public float volume;
        public float pitch;
    }
}
