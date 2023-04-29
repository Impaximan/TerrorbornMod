﻿using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.Incense
{
    class EmeraldIncense : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Emerald, 2)
                .AddIngredient(ModContent.ItemType<Materials.TerrorSample>())
                .AddIngredient(ItemID.Bottle)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Makes your terror meter green");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1, silver: 25);
            Item.vanity = true;
            TerrorbornItem.modItem(Item).meterColor = Color.LimeGreen;
        }
    }
}