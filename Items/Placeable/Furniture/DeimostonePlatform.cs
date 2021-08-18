using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Placeable.Furniture
{
	public class DeimostonePlatform : ModItem
	{

		public override void SetDefaults()
		{
			item.width = 8;
			item.height = 10;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.DeimostonePlatformTile>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Blocks.SmoothDeimostoneBlock>());
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
			ModRecipe recipe2 = new ModRecipe(mod);
			recipe2.AddIngredient(this, 2);
			recipe2.AddTile(TileID.WorkBenches);
			recipe2.SetResult(ModContent.ItemType<Blocks.SmoothDeimostoneBlock>());
			recipe2.AddRecipe();
		}
	}
}