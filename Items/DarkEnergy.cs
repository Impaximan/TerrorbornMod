using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items
{
    class DarkEnergy : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 34;
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
        }

        public override bool OnPickup(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.TerrorTonic && modPlayer.DarkEnergyStored < 5)
            {
                modPlayer.DarkEnergyStored++;
                Main.PlaySound(SoundID.Item3, player.Center);
                CombatText.NewText(player.getRect(), Color.Orange, modPlayer.DarkEnergyStored, true, false);
                return false;
            }
            Main.PlaySound(SoundID.Item4, player.Center);
            modPlayer.GainTerror(10f, false, false);
            return false;
        }
    }
}
