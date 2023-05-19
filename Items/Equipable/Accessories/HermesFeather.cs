using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class HermesFeather : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hermes' Feather");
            Tooltip.SetDefault("Allows you to use Shriek of Horror quicker");
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 75, 0);
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ShriekSpeed *= 0.65f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe()
                .AddIngredient(this)
                .AddRecipeGroup(RecipeGroupID.IronBar, 3)
                .AddIngredient(ItemID.Silk, 10)
                .AddTile(TileID.Loom);
            recipe.ReplaceResult(ItemID.HermesBoots);
            recipe.Register();
        }
    }
}
