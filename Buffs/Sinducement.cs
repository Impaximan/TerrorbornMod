using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs
{
    class Sinducement : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sinducement");
            Description.SetDefault("Your enemies will be punished.");
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
