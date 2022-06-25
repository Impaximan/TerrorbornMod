using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace TerrorbornMod.Items.Equipable.Accessories
{
    class VampiricPendant : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Decreases your max life by 50%" +
                "\nIncreases your life regen by a massive amount");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(0, 8, 0, 0);
            Item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 -= player.statLifeMax / 2;
            player.lifeRegen += 15;
        }
    }
}
