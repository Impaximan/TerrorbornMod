using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class ParryCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Parry Recharge");
            Description.SetDefault("You cannot currently parry");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
    }
}