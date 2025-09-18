// Author: Julian Kim
// File Name: Audio.cs
// Project Name: PASS3
// Creation Date: June 11, 2024
// Modified Date: June 12, 2024
// Description: Static class that stores all sound effects and background music

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace PASS3
{
    static class Audio
    {
        /// <summary>
        /// Store all sound effects
        /// </summary>
        public static SoundEffect BusSfx { get; set; }
        public static SoundEffect ButtonClickSfx { get; set; }
        public static SoundEffect CollectSfx { get; set; }
        public static SoundEffect DoorSfx { get; set; }
        public static SoundEffect HitSplashSfx { get; set; }
        public static SoundEffect NextSlideSfx { get; set; }
        public static SoundEffect PourSfx { get; set; }
        public static SoundEffect StepSfx { get; set; }
        public static SoundEffect LockedSfx { get; set; }

        /// <summary>
        /// Store all background music
        /// </summary>
        public static Song BkgMusic { get; set; }
        public static Song EndSequenceMusic { get; set; }
        
        /// <summary>
        /// Initializes a new instance of <see cref="Audio"/>
        /// </summary>
        /// <param name="content">The content manager for loading content</param>
        public static void InitializeAudio(ContentManager content)
        {
            // Load sound effects
            BusSfx = content.Load<SoundEffect>("Audio/Sounds/BusSfx");
            ButtonClickSfx = content.Load<SoundEffect>("Audio/Sounds/ButtonClickSfx");
            CollectSfx = content.Load<SoundEffect>("Audio/Sounds/CollectSfx");
            DoorSfx = content.Load<SoundEffect>("Audio/Sounds/DoorSfx");
            HitSplashSfx = content.Load<SoundEffect>("Audio/Sounds/HitSplashSfx");
            NextSlideSfx = content.Load<SoundEffect>("Audio/Sounds/NextSlideSfx");
            PourSfx = content.Load<SoundEffect>("Audio/Sounds/PourSfx");
            StepSfx = content.Load<SoundEffect>("Audio/Sounds/StepSfx");
            LockedSfx = content.Load<SoundEffect>("Audio/Sounds/LockedSfx");

            // Load music
            BkgMusic = content.Load<Song>("Audio/Music/BkgMusic");
            EndSequenceMusic = content.Load<Song>("Audio/Music/EndBkgMusic");
        }
    }
}
