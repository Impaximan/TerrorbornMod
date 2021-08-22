using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TerrorbornMod.Abilities;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Input;
using Extensions;

namespace TerrorbornMod
{
    static class TerrorbornUtils
    {
        public static bool TileShouldBeGrass(int i, int j)
        {
            if (WorldGen.TileEmpty(i + 1, j))
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j))
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j + 1))
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j - 1))
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i + 1, j + 1))
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i + 1, j - 1))
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j + 1))
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j - 1))
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

        static bool fullyCharged = false;
        public static void UpdateBurstJump(int chargeUpTime, int effectTime, Item item, Player player, Vector2 velocityRight, Color textColor, Terraria.Audio.LegacySoundStyle sound)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (player.controlUp)
            {
                modPlayer.BurstJumpChargingTime++;
                if (modPlayer.BurstJumpChargingTime > chargeUpTime)
                {
                    if (!fullyCharged)
                    {
                        CombatText.NewText(player.getRect(), textColor, item.Name + " charged...", true, false);
                        fullyCharged = true;
                        Main.PlaySound(SoundID.Item37, player.Center);
                    }
                    player.armorEffectDrawOutlines = true;
                }
            }
            else
            {
                modPlayer.BurstJumpChargingTime = 0;
                if (fullyCharged)
                {
                    fullyCharged = false;
                    Vector2 velocity = velocityRight;
                    player.direction = 1;
                    if (Main.MouseWorld.X < player.Center.X)
                    {
                        velocity.X *= -1;
                        player.direction = -1;
                    }
                    player.velocity = velocity;
                    modPlayer.BurstJumpTime = effectTime;
                    modPlayer.JustBurstJumped = true;
                    CombatText.NewText(player.getRect(), textColor, "..." + item.Name + " activated!", true, false);
                    Main.PlaySound(sound, player.Center);
                    TerrorbornMod.ScreenShake(5);
                    player.fallStart = (int)player.position.Y;
                    player.jumpAgainSandstorm = true;
                    player.jumpAgainSail = true;
                    player.jumpAgainFart = true;
                    player.jumpAgainCloud = true;
                    player.jumpAgainBlizzard = true;
                }
            }

            if (modPlayer.BurstJumpTime > 0)
            {
                modPlayer.BurstJumpTime--;
            }
        }

        public static string GetBurstJumpString(int chargeUpTime)
        {
            return "Hold UP for " + chargeUpTime / 60f + " seconds to charge up a burst jump" +
                "\nOnce fully charged, release UP to launch yourself left or right" +
                "\nThe direction of the launch depends on the position of your cursor";
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
