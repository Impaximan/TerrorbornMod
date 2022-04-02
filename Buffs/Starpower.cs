using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs
{
    class Starpower : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starpower");
            Description.SetDefault("40 increased max mana");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statManaMax2 += 40;
        }
    }
}