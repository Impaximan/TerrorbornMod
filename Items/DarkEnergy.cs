using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items
{
    class DarkEnergy : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 34;
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }

        public override bool OnPickup(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.TerrorTonic && modPlayer.DarkEnergyStored < 5)
            {
                modPlayer.DarkEnergyStored++;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item3, player.Center);
                CombatText.NewText(player.getRect(), Color.Orange, modPlayer.DarkEnergyStored, true, false);
                return false;
            }
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item4, player.Center);
            modPlayer.GainTerror(10f, false, false);
            return false;
        }
    }
}
