using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using ReLogic.Graphics;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.ModLoader.IO;
using TerrorbornMod.Abilities;
using Terraria.Graphics.Shaders;
using TerrorbornMod.ForegroundObjects;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Input;
using Extensions;

namespace TerrorbornMod
{
    static class TerrorbornUtils
    {
        public static string AutoSortTooltip(string tooltip)
        {
            string newTooltip = tooltip;
            for (int i = 0; i < tooltip.Length; i++)
            {
                string substring = tooltip.Substring(0, i);
                float width = GetStringWidth(substring, Main.fontMouseText);
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
            if (WorldGen.TileEmpty(i + 1, j) || !Main.tileSolid[Main.tile[i + 1, j].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j) && !Main.tileSolid[Main.tile[i - 1, j].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j + 1) || !Main.tileSolid[Main.tile[i, j + 1].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j - 1) || !Main.tileSolid[Main.tile[i, j - 1].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i + 1, j + 1) || !Main.tileSolid[Main.tile[i + 1, j + 1].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i + 1, j - 1) || !Main.tileSolid[Main.tile[i + 1, j - 1].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j + 1) || !Main.tileSolid[Main.tile[i - 1, j + 1].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j - 1) || !Main.tileSolid[Main.tile[i - 1, j - 1].type])
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
            return new None();
        }

        public static int abilityToInt(AbilityInfo abilityType)
        {
            return abilityType.typeInt();
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
