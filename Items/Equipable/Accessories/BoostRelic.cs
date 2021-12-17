using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class BoostRelic : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases item use speed by 10%" +
                "\nIncreases manueverability while airborne" +
                "\nIncreases jump speed");
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 34;
            item.accessory = true;
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.allUseSpeed *= 1.1f;
            player.jumpSpeedBoost += 1.5f;
            
            if (player.velocity.Y != 0)
            {
                player.runAcceleration += 0.2f;
            }
        }
    }
}

