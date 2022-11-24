using Terraria.ID;
using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;

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
