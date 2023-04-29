using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class AzuriteBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("'You'd think it would melt back into water'");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 35, 0);
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AzuriteOre>(3)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }
}
