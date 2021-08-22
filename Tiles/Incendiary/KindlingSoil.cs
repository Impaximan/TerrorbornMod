using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace TerrorbornMod.Tiles.Incendiary
{
    public class KindlingSoil : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            //Main.tileShine[Type] = 1;
            Main.tileLighted[Type] = false;
            Main.tileSpelunker[Type] = true;
            soundType = 0;
            soundStyle = 1;
            //Main.soundDig[Type] =  21;

            Main.tileMerge[Type][ModContent.TileType<KindlingGrass>()] = true;

            minPick = 0;
            mineResist = 3;
            drop = ModContent.ItemType<Items.Placeable.Blocks.KindlingSoilBlock>();
            AddMapEntry(new Color(71, 72, 92));
        }

        public override bool HasWalkDust()
        {
            return false;
        }

        public override bool CanExplode(int i, int j)
        {
            return true;
        }

        public override void RandomUpdate(int i, int j)
        {
            if (TerrorbornUtils.TileShouldBeGrass(i, j))
            {
                Main.tile[i, j].type = (ushort)ModContent.TileType<KindlingGrass>();
                //return;
            }

            //if (Main.tile[i + 1, j].type == ModContent.TileType<KindlingGrass>())
            //{
            //    Main.tile[i, j].type = (ushort)ModContent.TileType<KindlingGrass>();
            //}
            //else if (Main.tile[i - 1, j].type == ModContent.TileType<KindlingGrass>())
            //{
            //    Main.tile[i, j].type = (ushort)ModContent.TileType<KindlingGrass>();
            //}
            //else if (Main.tile[i, j + 1].type == ModContent.TileType<KindlingGrass>())
            //{
            //    Main.tile[i, j].type = (ushort)ModContent.TileType<KindlingGrass>();
            //}
            //else if (Main.tile[i, j - 1].type == ModContent.TileType<KindlingGrass>())
            //{
            //    Main.tile[i, j].type = (ushort)ModContent.TileType<KindlingGrass>();
            //}
        }
    }

    public class KindlingGrass : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            //Main.tileShine[Type] = 1;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            soundType = 0;
            soundStyle = 1;
            //Main.soundDig[Type] =  21;

            Main.tileMerge[Type][ModContent.TileType<KindlingSoil>()] = true;

            minPick = 0;
            mineResist = 4.5f;
            drop = ModContent.ItemType<Items.Placeable.Blocks.KindlingSoilBlock>();
            AddMapEntry(new Color(204, 114, 98));
        }

        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            dustType = DustID.Fire;
            makeDust = true;
        }

        public override bool HasWalkDust()
        {
            return true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.1f;
            g = 0;
            b = 0f;
        }
    }
}


