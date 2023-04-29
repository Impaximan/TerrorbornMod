using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class NovagoldBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Novagold Alloy");
            // Tooltip.SetDefault("Enchanted with the power of a star");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ModContent.ItemType<NovagoldOre>(), 6)
                .AddIngredient(ItemID.FallenStar, 1)
                .AddTile(TileID.Furnaces)
                .Register();
        }
    }
}