using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod
{
    class GraniteSparkData
    {
        public static float speed = 7.5f;
        public static void Transform(Player player)
        {
            if (!player.HasBuff(ModContent.BuffType<Buffs.Debuffs.GraniteSparkCooldown>()))
            {
                player.AddBuff(ModContent.BuffType<Buffs.GraniteSpark>(), 60 * 5);
                player.AddBuff(ModContent.BuffType<Buffs.Debuffs.GraniteSparkCooldown>(), 60 * 35);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item72, player.Center);
            }
        }
    }

    class astralSparkData
    {
        public static float speed = 8.5f;
        public static void Transform(Player player)
        {
            if (!player.HasBuff(ModContent.BuffType<Buffs.Debuffs.AstralSparkCooldown>()))
            {
                player.AddBuff(ModContent.BuffType<Buffs.AstralSparkBuff>(), 60 * 5);
                player.AddBuff(ModContent.BuffType<Buffs.Debuffs.AstralSparkCooldown>(), 60 * 20);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item72, player.Center);
            }
        }
    }
}
