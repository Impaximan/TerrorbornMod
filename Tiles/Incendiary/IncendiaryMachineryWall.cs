using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Tiles.Incendiary
{
    class IncendiaryMachineryWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = false;
            ItemDrop = ModContent.ItemType<Items.Placeable.Walls.IncendiaryMachineryWall>();
            AddMapEntry(new Color(20, 20, 23));
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
