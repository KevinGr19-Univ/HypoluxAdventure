using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Utils
{
    internal static class SoundPlayer
    {
        private static Dictionary<string, SoundEffect> _soundEffects = new Dictionary<string, SoundEffect>();

        public static void LoadSound(ContentManager content, string soundPath)
        {
            if (_soundEffects.ContainsKey(soundPath)) return;
            _soundEffects.Add(soundPath, content.Load<SoundEffect>(soundPath));
        }

        public static void PlaySound(string soundPath, float volume = 0.5f, float pitch = 0)
        {
            if (!_soundEffects.TryGetValue(soundPath, out SoundEffect sound))
                throw new ArgumentException("This soundEffect is not loaded");

            sound.Play(volume, pitch, 0);
        }
    }
}
