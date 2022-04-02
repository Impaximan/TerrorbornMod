using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Placeable.Furniture
{
	public class DeimostonePlatform : ModItem
	{

		public override void SetDefaults()
		{
			Item.width = 8;
			Item.height = 10;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.DeimostonePlatformTile>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(2)
				.AddIngredient(ModContent.ItemType<Blocks.SmoothDeimostoneBlock>(), 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}