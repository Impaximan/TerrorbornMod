using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.World.Generation;
using System.Collections.Generic;
using System;

namespace TerrorbornMod
{
    class TerrorbornTile : GlobalTile
    {
        public override void PlaceInWorld(int i, int j, Item item)
        {
            DestroyBadGrass(i + 1, j);
            DestroyBadGrass(i - 1, j);
            DestroyBadGrass(i, j + 1);
            DestroyBadGrass(i, j - 1);
            DestroyBadGrass(i + 1, j + 1);
            DestroyBadGrass(i + 1, j - 1);
            DestroyBadGrass(i - 1, j + 1);
            DestroyBadGrass(i - 1, j - 1);
        }

        public void DestroyBadGrass(int i, int j)
        {
            if (Main.tile[i, j].type == ModContent.TileType<Tiles.Incendiary.KindlingGrass>())
            {
                if (!TerrorbornUtils.TileShouldBeGrass(i, j))
                {
                    Main.tile[i, j].type = (ushort)ModContent.TileType<Tiles.Incendiary.KindlingSoil>();
                }
            }
        }
    }
}
