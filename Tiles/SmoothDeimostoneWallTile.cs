using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Tiles
{
    class SmoothDeimostoneWallTile : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            ItemDrop = ModContent.ItemType<Items.Placeable.Walls.SmoothDeimostoneWall>();
            AddMapEntry(new Color(9, 19, 29));
        }
    }
}

