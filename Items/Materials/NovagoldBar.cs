using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class NovagoldBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Novagold Alloy");
            Tooltip.SetDefault("Enchanted with the power of a star");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 0, 18, 0);
            item.rare = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<NovagoldOre>(), 6);
            recipe.AddIngredient(ItemID.FallenStar, 1);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this, 2);
            recipe.AddRecipe();
        }
    }
}