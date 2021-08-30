using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
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

        public override void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            if (ModContent.TextureExists(Main.tileTexture[type].ToString() + "_Glow"))
            {
                Tile tile = Main.tile[i, j];
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                int height = tile.frameY == 36 ? 18 : 16;
                Main.spriteBatch.Draw(ModContent.GetTexture(Main.tileTexture[type].ToString() + "_Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
