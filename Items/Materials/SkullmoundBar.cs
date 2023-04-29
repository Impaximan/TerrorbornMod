using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class SkullmoundBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("'You mind shrieks as you touch it'");
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 65, 0);
            Item.rare = ItemRarityID.Yellow;
        }
        public override void AddRecipes()
        {
            int barCount = 5;
            CreateRecipe(barCount)
                .AddIngredient(ModContent.ItemType<SkullmoundOre>(), 3 * barCount)
                .AddIngredient(ModContent.ItemType<HellbornEssence>())
                .AddTile(TileID.AdamantiteForge)
                .Register();
        }
    }
}