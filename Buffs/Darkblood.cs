using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs
{
    class Darkblood : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkblood");
            Description.SetDefault("Allows you to attack while using Shriek of Horror");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
    }
}

