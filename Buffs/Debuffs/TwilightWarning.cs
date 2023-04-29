using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class TwilightWarning : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Twilight Warning");
            // Description.SetDefault("Significantly worsened stats and disabled life regen... get terror!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {

        }
    }
}