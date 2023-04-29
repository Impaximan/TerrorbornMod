using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs
{
    class TortureBoost : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Torturer's Satisfaction");
            // Description.SetDefault("Increased life regen");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 10;
        }
    }
}

