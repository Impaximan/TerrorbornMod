using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class BrainDead : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Braindead");
            Description.SetDefault("15% decreased damage");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.allDamage *= 0.85f;
        }
    }
}
