﻿
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Placeable.Furniture
{
    class DeimostoneWorkbenchItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Deimostone Workbench");
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 18;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.DeimostoneWorkbench>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Blocks.SmoothDeimostoneBlock>(), 10)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }
}
