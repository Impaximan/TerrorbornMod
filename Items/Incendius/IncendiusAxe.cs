﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Incendius
{
    class IncendiusAxe : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(35 * TerrorbornMod.IncendiaryAlloyMultiplier));
            recipe.AddIngredient(ItemID.CobaltBar, 20);
            recipe.AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>());
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(35 * TerrorbornMod.IncendiaryAlloyMultiplier));
            recipe2.AddIngredient(ItemID.PalladiumBar, 20);
            recipe2.AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>());
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Incendius Waraxe");
        }
        public override void SetDefaults()
        {
            item.damage = 20;
            item.melee = true;
            item.width = 64;
            item.height = 64;
            item.useTime = 4;
            item.useAnimation = 15;
            item.axe = 17;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
    }
}
