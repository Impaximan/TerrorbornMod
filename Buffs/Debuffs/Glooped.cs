using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class Glooped : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Glooped");
            // Description.SetDefault("Significantly decreased wing flight time");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.Glooped = true;
        }
    }
}
