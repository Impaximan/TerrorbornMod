using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs
{
    class Adrenaline : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Adrenaline");
            Description.SetDefault("Increased attack speed at low health");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.statLife <= player.statLifeMax2 / 4)
            {
                TerrorbornPlayer.modPlayer(player).allUseSpeed *= 1.3f;
            }
        }
    }
}