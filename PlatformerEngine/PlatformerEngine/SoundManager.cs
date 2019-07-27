using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// manages sound instances and music
    /// </summary>
    public class SoundManager
    {
        /// <summary>
        /// list of all live sounds
        /// </summary>
        public List<SoundEffectInstance> LiveSounds;
        /// <summary>
        /// the currently playing music
        /// </summary>
        public SoundEffectInstance CurrentMusic;
        /// <summary>
        /// makes an instance of the sound manager
        /// </summary>
        public SoundManager()
        {
            LiveSounds = new List<SoundEffectInstance>();
            CurrentMusic = null;
        }
        /// <summary>
        /// plays the given music
        /// </summary>
        /// <param name="song">the sound effect to play on loop</param>
        /// <param name="volume">the volume of the music to play</param>
        public void PlayMusic(SoundEffect song, float volume = 0.1f)
        {
            StopMusic();
            if (song != null)
            {
                CurrentMusic = song.CreateInstance();
                CurrentMusic.Volume = volume;
                CurrentMusic.IsLooped = true;
                CurrentMusic.Play();
            }
        }
        /// <summary>
        /// plays a sound
        /// </summary>
        /// <param name="sound">the sound to play</param>
        /// <param name="volume">volume to play the sound at</param>
        public void PlaySound(SoundEffect sound, float volume = 1f)
        {
            if (sound == null) return;
            SoundEffectInstance inst = sound.CreateInstance();
            inst.Volume = volume;
            inst.Play();
            LiveSounds.Add(inst);
        }
        /// <summary>
        /// stops all sounds and music
        /// </summary>
        public void StopEverything()
        {
            for(int i = LiveSounds.Count - 1; i >= 0; i--)
            {
                SoundEffectInstance snd = LiveSounds[i];
                snd.Stop();
                snd.Dispose();
                LiveSounds.RemoveAt(i);
            }
            StopMusic();
        }
        /// <summary>
        /// stops the playing music
        /// </summary>
        public void StopMusic()
        {
            CurrentMusic?.Stop();
            CurrentMusic?.Dispose();
            CurrentMusic = null;
        }
    }
}
