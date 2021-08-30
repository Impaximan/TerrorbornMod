using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class IncendiaryCurse : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Incendiary Warfare");
            Description.SetDefault("Greatly increased spawnrates");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
    }
}

