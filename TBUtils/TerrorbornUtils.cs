using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.WorldBuilding;
using ReLogic.Graphics;
using Terraria.GameContent;
using TerrorbornMod.Abilities;
using System;
using ReLogic.Content;
using System.Threading;

namespace TerrorbornMod
{
    static class TerrorbornUtils
    {
        public static void InvokeOnMainThread(Action action)
        {
            if (!AssetRepository.IsMainThread)
            {
                ManualResetEvent evt = new(false);

                Main.QueueMainThreadAction(() => {
                    action();
                    evt.Set();
                });

                evt.WaitOne();
            }
            else
                action();
        }

        public static string AutoSortTooltip(string tooltip)
        {
            int lineAmount;
            string[] lines = Utils.WordwrapString(tooltip, FontAssets.MouseText.Value, (int)TerrorbornMod.LoreParagraphWidth, 25, out lineAmount);
            string newTooltip = "";
            for (int i = 0; i <= lineAmount; i++)
            {
                newTooltip = newTooltip + "\n" + lines[i];
            }
            return newTooltip;
        }

        public static float GetStringWidth(string text, SpriteFont spriteFont)
        {
            return spriteFont.MeasureString(text).X;
        }

        public static float GetStringWidth(string text, DynamicSpriteFont spriteFont)
        {
            return spriteFont.MeasureString(text).X;
        }

        public static bool TileShouldBeGrass(int i, int j)
        {
            if (WorldGen.TileEmpty(i + 1, j) || !Main.tileSolid[Main.tile[i + 1, j].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j) || !Main.tileSolid[Main.tile[i - 1, j].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j + 1) || !Main.tileSolid[Main.tile[i, j + 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j - 1) || !Main.tileSolid[Main.tile[i, j - 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i + 1, j + 1) || !Main.tileSolid[Main.tile[i + 1, j + 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i + 1, j - 1) || !Main.tileSolid[Main.tile[i + 1, j - 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j + 1) || !Main.tileSolid[Main.tile[i - 1, j + 1].TileType])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j - 1) || !Main.tileSolid[Main.tile[i - 1, j - 1].TileType])
            {
                return true;
            }

            return false;
        }

        public static AbilityInfo intToAbility(int integerValue)
        {
            if (integerValue == 0)
            {
                return new None();
            }
            if (integerValue == 1)
            {
                return new HorrificAdaptationInfo();
            }
            if (integerValue == 2)
            {
                return new TerrorWarpInfo();
            }
            if (integerValue == 3)
            {
                return new VoidBlinkInfo();
            }
            if (integerValue == 4)
            {
                return new NecromanticCurseInfo();
            }
            if (integerValue == 5)
            {
                return new StarvingStormInfo();
            }
            if (integerValue == 6)
            {
                return new GelatinArmorInfo();
            }
            if (integerValue == 7)
            {
                return new TimeFreezeInfo();
            }
            if (integerValue == 8)
            {
                return new BlinkDashInfo();
            }
            return new None();
        }

        public static int abilityToInt(AbilityInfo abilityType)
        {
            return abilityType.typeInt();
        }

        public static Vector2 findGroundUnder(this Vector2 position)
        {
            Vector2 returned = position;
            while (!WorldUtils.Find(returned.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                {
        new Conditions.IsSolid()
                }), out _))
            {
                returned.Y++;
            }

            return returned;
        }

        public static Vector2 findGroundUnder(this Vector2 position, int type)
        {
            Vector2 returned = position;

            int i = 5000;
            while (Main.tile[(int)(returned.X / 16), (int)(returned.Y / 16)].TileType != type)
            {
                returned.Y++;

                i--;
                if (i <= 0)
                {
                    return Vector2.Zero;
                }
            }

            return returned;
        }

        public static Vector2 findCeilingAbove(this Vector2 position)
        {
            Vector2 returned = position;
            while (!WorldUtils.Find(returned.ToTileCoordinates(), Searches.Chain(new Searches.Up(1), new GenCondition[]
                {
        new Conditions.IsSolid()
                }), out _))
            {
                returned.Y--;
            }

            return returned;
        }

        public static Vector2 findCeilingAbove(this Vector2 position, int type)
        {
            Vector2 returned = position;

            int i = 5000;
            while (Main.tile[(int)(returned.X / 16), (int)(returned.Y / 16)].TileType != type)
            {
                returned.Y--;

                i--;
                if (i <= 0)
                {
                    return Vector2.Zero;
                }
            }

            return returned;
        }

        public static bool mouseDown;
        public static bool mouseJustPressed;
        static bool canPressMouseAgain = false;
        public static void Update()
        {
            Player player = Main.player[Main.myPlayer];
            if (player.controlUseItem)
            {
                mouseDown = true;
            }
            else
            {
                mouseDown = false;
                canPressMouseAgain = true;
            }
            if (mouseJustPressed)
            {
                mouseJustPressed = false;
            }
            if (canPressMouseAgain && mouseDown)
            {
                mouseJustPressed = true;
                canPressMouseAgain = false;
            }
        }

        public static void RevealMapAroundPlayer(int distanceInTiles, Player player)
        {
            Point playerPosition = player.Center.ToTileCoordinates();
            for (int i = playerPosition.X - distanceInTiles; i < playerPosition.X + distanceInTiles; i++)
            {
                for (int j = playerPosition.Y - distanceInTiles; j < playerPosition.Y + distanceInTiles; j++)
                {
                    Point tile = new Point(i, j);
                    
                    if (Vector2.Distance(playerPosition.ToVector2(), tile.ToVector2()) <= distanceInTiles && WorldGen.InWorld(i, j))
                    {
                        Main.Map.Update(i, j, 255);
                    }
                }
            }
            Main.refreshMap = true;
        }
    }
}
