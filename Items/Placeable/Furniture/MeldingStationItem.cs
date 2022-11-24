using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Placeable.Furniture
{
    class MeldingStationItem : ModItem
	{
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Melding Station");
            Tooltip.SetDefault("Can be used to craft with deimosteel and create restless weapons");
        }
        public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 999;
			Item.useTurn = true;
            Item.rare = ItemRarityID.Blue;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.createTile = ModContent.TileType<Tiles.MeldingStation>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 1)
                .AddIngredient(ModContent.ItemType<Items.Materials.DeimosteelBar>(), 12)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}

