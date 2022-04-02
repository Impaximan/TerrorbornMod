using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Placeable.Furniture
{
    class DeimostoneChest : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.createTile = ModContent.TileType<Tiles.DeimostoneChestTile>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Blocks.SmoothDeimostoneBlock>(), 8)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
