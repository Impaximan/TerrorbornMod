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
            soundType = 0;
            soundStyle = 1;
            //Main.soundDig[Type] =  21;

            Main.tileMerge[Type][ModContent.TileType<KindlingSoil>()] = true;

            dustType = DustID.Fire;

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

        //public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        //{
        //    Tile tile = Main.tile[i, j];
        //    Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
        //    if (Main.drawToScreen)
        //    {
        //        zero = Vector2.Zero;
        //    }
        //    int height = tile.frameY == 36 ? 18 : 16;
        //    Main.spriteBatch.Draw(mod.GetTexture("Tiles/Incendiary/KindlingGrass_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        //}

        public override void RandomUpdate(int i, int j)
        {
            Vector2 position = new Vector2(i * 16, j * 16);
            Dust dust = Dust.NewDustPerfect(position, DustID.Fire);
            dust.velocity = new Vector2(0, -3);
            dust.noGravity = true;
            dust.scale = 1.5f;

            if (WorldGen.TileEmpty(i, j - 1) && Main.rand.NextFloat() <= 0.05f)
            {
                int distance = 10;
                for (int checkI = -distance; checkI <= distance; checkI++)
                {
                    for (int checkJ = -distance; checkJ <= distance; checkJ++)
                    {
                        if (Main.tile[checkI + i, checkJ + j].type == ModContent.TileType<PyroclasticGemstone>())
                        {
                            return;
                        }
                    }
                }
                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<PyroclasticGemstone>(), true, false);
            }
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


