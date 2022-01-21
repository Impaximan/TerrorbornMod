﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Potions
{
    public class LesserTerrorPotion : ModItem
    {
        float terrorAmount = 2f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Tap the Shriek of Horror hotkey while in your inventory to consume" +
                "\nCauses you to gain " + terrorAmount.ToString() + "% terror per nearby enemy when consumed" +
                "\nHas a default cooldown of 10 seconds");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(0, 0, 0, 60);
            item.consumable = true;
            item.rare = ItemRarityID.White;
            item.maxStack = 999;
            item.width = 20;
            item.height = 26;
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.terrorPotionTerror = terrorAmount;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.TerrorSample>());
            recipe.AddIngredient(ItemID.Bottle, 3);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this, 3);
            recipe.AddRecipe();
        }
    }
}