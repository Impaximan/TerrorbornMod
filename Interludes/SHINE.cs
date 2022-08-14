using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;
using System;
using Terraria.ModLoader.IO;
using ReLogic.Content;

namespace TerrorbornMod.Interludes
{
    class SHINE : ModSystem
    {
        public static bool PlayedSong = false;
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("PlayedSong", PlayedSong);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            PlayedSong = tag.GetBool("PlayedSong");
        }

        public static SoundEffect music;
        public static SoundEffectInstance musicInstance;
        public static bool playingSong = false;
        public override void Load()
        {
            base.Load();
            music = ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Music/SHINE", AssetRequestMode.ImmediateLoad).Value;
        }

        public override void PostWorldGen()
        {
            PlayedSong = false;
        }

        public override void PostUpdateEverything()
        {
            if (!PlayedSong)
            {
                music = ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Music/SHINE", AssetRequestMode.ImmediateLoad).Value;
                musicInstance = music.CreateInstance();
                musicInstance.Play();
                PlayedSong = true;
            }

            playingSong = false;
            if (musicInstance != null && musicInstance.State == SoundState.Playing)
            {
                playingSong = true;
                musicInstance.Volume = Main.musicVolume * 0.75f;
                //for (int i = 0; i < Main.musicFade.Length; i++)
                //{
                //    if (i >= Main.musicFade.Length)
                //    {
                //        break;
                //    }
                //    Main.musicFade[i] = 0f;
                //}
            }
        }

        public override void OnWorldUnload()
        {
            if (musicInstance != null && musicInstance.State == SoundState.Playing)
            {
                musicInstance.Stop();
            }
        }

        public override void OnModUnload()
        {
            if (music != null) music.Dispose();
            if (musicInstance != null) musicInstance.Dispose();
        }
    }

    //This literally exists only to make vanilla music not play lol
    class SHINEBiome : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int Music => MusicLoader.GetMusicSlot("TerrorbornMod/Sounds/Music/Silence");

        public override bool IsSceneEffectActive(Player player)
        {
            return SHINE.playingSong;
        }
    }
}
