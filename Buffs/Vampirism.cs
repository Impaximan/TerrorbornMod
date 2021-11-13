using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs
{
    class Vampirism : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Vampirism");
            Description.SetDefault("Increased chance to get hearts and dark energy from enemies that are close to you");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {

        }
    }
}