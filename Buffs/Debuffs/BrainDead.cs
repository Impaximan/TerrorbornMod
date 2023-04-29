using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class BrainDead : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Braindead");
            // Description.SetDefault("15% decreased damage");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) *= 0.85f;
        }
    }
}
