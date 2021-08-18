
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Placeable.Furniture
{
    class DeimostoneWorkbenchItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deimostone Workbench");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 18;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.DeimostoneWorkbench>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Blocks.SmoothDeimostoneBlock>(), 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}
