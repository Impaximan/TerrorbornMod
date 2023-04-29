using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class DeimosteelCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Increases the amount of bleaks that spawn in deimostone caves" +
                "\nDoubles the chance that certain enemies will Drop terror samples"); */
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 34;
            Item.accessory = true;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.useAnimation = 5;
            Item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.DeimosteelCharm = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 4)
                .AddIngredient(ModContent.ItemType<Materials.TerrorSample>(), 1)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }
    }
}


