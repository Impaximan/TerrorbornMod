using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items
{
    class TwilightMatrix : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("While this item is favorited in your inventory, vanilla bosses you have already defeated will enter their twilight forms" +
                "\nIn their twilight forms, bosses will have more difficult AI and sometimes new drops" +
                "\n'Dragged by fate...'");
        }

        public override void SetDefaults()
        {
            item.expert = true;
        }

        public override void UpdateInventory(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (item.favorited)
            {
                modPlayer.TwilightMatrix = true;
            }
        }
    }
}