using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class TerrorSickness : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Terror Sickness");
            Description.SetDefault("Cannot consume any more terror potions");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
    }
}
