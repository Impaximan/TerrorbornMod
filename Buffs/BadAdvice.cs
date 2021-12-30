using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs
{
    class BadAdvice : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bad Advice");
            Description.SetDefault("15% increased damage, -15% max life");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.allDamage += 0.15f;
            player.statLifeMax2 -= (int)(player.statLifeMax * 0.15f);
        }
    }
}
