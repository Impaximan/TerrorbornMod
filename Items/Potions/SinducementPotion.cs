﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TerrorbornMod.Items.Materials;

namespace TerrorbornMod.Items.Potions
{
    public class SinducementPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Killing enemies causes them to explode into blood and turn into a soul that spins around your cursor");
        }
        public override void SetDefaults()
        {
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.maxStack = 30;
            item.consumable = true;
            item.rare = ItemRarityID.LightRed;
            item.autoReuse = false;
            item.UseSound = SoundID.Item3;
            item.useTurn = true;
            item.maxStack = 30;
            item.buffType = ModContent.BuffType<Buffs.Sinducement>();
            item.buffTime = 3600 * 4;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ModContent.ItemType<PyroclasticGemstone>(), 5);
            recipe.AddIngredient(ItemID.AshBlock, 10);
            recipe.AddIngredient(ItemID.Fireblossom);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

