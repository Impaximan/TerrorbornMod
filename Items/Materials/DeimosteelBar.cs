using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class DeimosteelBar : ModItem
    {
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 0, 30, 0);
            item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Placeable.Blocks.DeimosteelOreItem>(), 2);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}

