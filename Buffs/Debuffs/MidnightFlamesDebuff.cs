using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class MidnightFlamesDebuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Midnight Flames");
            Description.SetDefault("Spiritual flames tear at your skin, decreasing your defense and damaging you over time");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= 45;
            Dust.NewDust(player.position, player.width, player.height, 74, Scale: 1.5f);
        }
    }
}
