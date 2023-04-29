using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs
{
    class IncendiaryRevival : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Incendiary Revival");
            // Description.SetDefault("You have been revived by an unholy force");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            player.lifeRegen += 5;
            if (modPlayer.iFrames < 2)
            {
                modPlayer.iFrames = 2;
            }
        }
    }
}

