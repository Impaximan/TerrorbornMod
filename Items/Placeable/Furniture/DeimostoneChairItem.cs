using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Placeable.Furniture
{
	public class DeimostoneChairItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deimostone Chair");
		}

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 150;
			item.createTile = ModContent.TileType<Tiles.DeimostoneChair>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Blocks.SmoothDeimostoneBlock>(), 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
