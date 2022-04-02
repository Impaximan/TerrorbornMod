using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TerrorbornMod.Items.Placeable.Furniture
{
    class InfectedIncarnateTrophy : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 48;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.createTile = ModContent.TileType<InfectedIncarnateTrophyTile>();
		}
	}

	class InfectedIncarnateTrophyTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Trophy");
			AddMapEntry(Color.OrangeRed, name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { Type };
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<InfectedIncarnateTrophy>());
		}
	}
}


