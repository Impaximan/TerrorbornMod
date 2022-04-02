using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class DeimosteelBar : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 30, 0);
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<Placeable.Blocks.DeimosteelOreItem>(3)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }
}

