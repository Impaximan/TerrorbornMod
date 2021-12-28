using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles
{
    public class Deimostone : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = false;
            soundType = SoundID.Tink;
            soundStyle = 1;

            minPick = 0;
            mineResist = 0.5f;
            drop = ModContent.ItemType<Items.Placeable.Blocks.DeimostoneBlock>();
            AddMapEntry(new Color(40, 55, 70));
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (Main.rand.NextFloat() <= 0.02f)
            {
                bool generate = true;
                int distance = 60;
                for (int checkI = -distance; checkI <= distance; checkI++)
                {
                    if (checkI + i < 0 || checkI + i > Main.maxTilesX)
                    {
                        break;
                    }
                    for (int checkJ = -distance; checkJ <= distance; checkJ++)
                    {
                        if (checkJ + j < 0 || checkJ + j > Main.maxTilesY)
                        {
                            break;
                        }
                        if (Main.tile[checkI + i, checkJ + j] != null)
                        {
                            if (Main.tile[checkI + i, checkJ + j].type == ModContent.TileType<MidnightFruit>())
                            {
                                generate = false;
                            }
                        }
                    }
                }

                if (generate)
                {
                    if (WorldGen.TileEmpty(i, j - 1) && WorldGen.TileEmpty(i, j - 2) && WorldGen.TileEmpty(i + 1, j - 2) && WorldGen.TileEmpty(i + 1, j - 1))
                    {
                        WorldGen.PlaceTile(i, j - 1, ModContent.TileType<MidnightFruit>(), true);
                    }
                    else if (WorldGen.TileEmpty(i, j - 1) && WorldGen.TileEmpty(i, j - 2) && WorldGen.TileEmpty(i - 1, j - 2) && WorldGen.TileEmpty(i - 1, j - 1))
                    {
                        WorldGen.PlaceTile(i - 1, j - 1, ModContent.TileType<MidnightFruit>(), true);
                    }
                }
            }
        }
    }
}
