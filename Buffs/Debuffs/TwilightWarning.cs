using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class TwilightWarning : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Twilight Warning");
            Description.SetDefault("Significantly worsened stats and disabled life regen... get terror!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {

        }
    }
}