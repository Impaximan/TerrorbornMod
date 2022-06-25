using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.Utilities;
using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.UI;
using TerrorbornMod.ForegroundObjects;
using TerrorbornMod.UI.TerrorMeter;
using TerrorbornMod.UI.TerrorAbilityUnlock;
using TerrorbornMod.UI.TitleCard;
using TerrorbornMod.UI.TwilightEmpowerment;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;
using ReLogic.Utilities;

namespace TerrorbornMod
{
    static class SoundExtensions
    {
        public static void PlaySoundOld(SoundStyle style, int x, int y, int oldstyle, float volume, float pitch)
        {
            if (style == SoundID.Item)
            {
                style = new SoundStyle(style.SoundPath + oldstyle.ToString(), style.Variants, style.Type);
            }
            style.Pitch = pitch;
            style.Volume = volume;
            SoundEngine.PlaySound(style, new Vector2(x, y));
        }

        public static void PlaySoundOld(SoundStyle style, int x, int y, int oldstyle)
        {
            SoundEngine.PlaySound(style, new Vector2(x, y));
        }

        public static void PlaySoundOld(SoundStyle style, int x, int y)
        {
            SoundEngine.PlaySound(style, new Vector2(x, y));
        }

        public static void PlaySoundOld(SoundStyle style, Vector2 position)
        {
            SoundEngine.PlaySound(style, position);
        }

        public static void PlaySoundOld(SoundStyle style, Vector2 position, int oldstyle)
        {
            SoundEngine.PlaySound(style, position);
        }

        public static void PlaySoundOld(SoundStyle style)
        {
            SoundEngine.PlaySound(style);
        }

        internal static void PlaySoundOld(SoundStyle? style, Vector2 center)
        {
            throw new NotImplementedException();
        }
    }
}
