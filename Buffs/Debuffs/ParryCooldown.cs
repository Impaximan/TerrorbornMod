using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class ParryCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Parry Recharge");
            Description.SetDefault("You cannot currently parry");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
    }
}