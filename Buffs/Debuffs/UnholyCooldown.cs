using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class UnholyCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unholy Cooldown");
            Description.SetDefault("You cannot be revived again");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
    }
}