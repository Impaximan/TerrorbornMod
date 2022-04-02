using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TerrorbornMod.Tiles.Incendiary
{
    class IncendiaryRitual : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 18 };
			//TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<TEElementalPurge>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			//TileObjectData.newTile.CoordinateHeights = new[] { 18 };
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Incendiary Ritual");
			mineResist = 10;
			AddMapEntry(new Color(92 / 2, 111 / 2, 126 / 2), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			AdjTiles = new int[] { Type };
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 48, 32, ModContent.ItemType<Items.Placeable.Furniture.IncendiaryRitual>());
		}
	}
}