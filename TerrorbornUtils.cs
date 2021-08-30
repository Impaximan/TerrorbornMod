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
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TerrorbornMod.Abilities;
using TerrorbornMod.ForegroundObjects;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Input;
using Extensions;

namespace TerrorbornMod
{
    static class TerrorbornUtils
    {
        public static void Initialize()
        {
            On.Terraria.Main.DrawInterface += DrawOvertopGraphics;
        }

        public static void Unload()
        {
            On.Terraria.Main.DrawInterface -= DrawOvertopGraphics;
        }

        static Vector2 perlinPosition = Vector2.Zero;
        static Vector2 lastScreenPosition = Vector2.Zero;
        static int flipState = 1;
        static int currentSpriteEffects = 1;
        const float fogMovementMultiplier = 1.25f;

        private static void DrawOvertopGraphics(On.Terraria.Main.orig_DrawInterface orig, Main self, GameTime gameTime)
        {
            if (lastScreenPosition == Vector2.Zero)
            {
                lastScreenPosition = Main.screenPosition;
            }

            Vector2 difference = Main.screenPosition - lastScreenPosition;
            lastScreenPosition = Main.screenPosition;

            foreach (ForegroundObject foregroundObject in TerrorbornMod.foregroundObjects)
            {
                if (foregroundObject != null)
                {
                    foregroundObject.position -= difference;
                }
            }
            List<ForegroundObject> drawing = new List<ForegroundObject>();
            foreach (ForegroundObject foregroundObject in TerrorbornMod.foregroundObjects)
            {
                if (foregroundObject != null)
                {
                    bool foundSpot = false;
                    for (int i = 0; i < drawing.Count; i++)
                    {
                        ForegroundObject foregroundObject2 = drawing[i];
                        if (foregroundObject2.distance > foregroundObject.distance)
                        {
                            drawing.Insert(i, foregroundObject);
                            i = drawing.Count;
                            foundSpot = true;
                        }
                    }

                    if (!foundSpot)
                    {
                        drawing.Add(foregroundObject);
                    }
                }
            }

            Main.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (ForegroundObject foregroundObject in drawing)
            {
                if (foregroundObject != null)
                {
                    if (foregroundObject.PreDraw(Main.spriteBatch))
                    {
                        foregroundObject.Draw(Main.spriteBatch);
                        foregroundObject.PostDraw(Main.spriteBatch);
                    }
                }
            }

            //Texture2D texture = ModContent.GetTexture("TerrorbornMod/Perlin");
            //Texture2D texture2 = ModContent.GetTexture("TerrorbornMod/PerlinFlipped");
            //if (flipState == -1)
            //{
            //    texture = ModContent.GetTexture("TerrorbornMod/PerlinFlipped");
            //    texture2 = ModContent.GetTexture("TerrorbornMod/Perlin");
            //}
            ////spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/ScreenTintig"), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Crimson);
            //float widthRatio = texture.Width / Main.screenWidth;
            //SpriteEffects effects = SpriteEffects.None;
            //if (currentSpriteEffects == -1)
            //{
            //    effects = SpriteEffects.FlipHorizontally;
            //}
            //Main.spriteBatch.Draw(texture, new Rectangle((int)perlinPosition.X, (int)perlinPosition.Y, Main.screenWidth, Main.screenHeight), new Rectangle((int)(perlinPosition.X * widthRatio), 0, (int)(texture.Width - perlinPosition.X * widthRatio), texture.Height), Color.Red * 0.15f, 0, Vector2.Zero, effects, 0f);
            //Main.spriteBatch.Draw(texture2, new Rectangle((int)perlinPosition.X, (int)perlinPosition.Y + Main.screenHeight, Main.screenWidth, Main.screenHeight), new Rectangle((int)(perlinPosition.X * widthRatio), 0, (int)(texture.Width - perlinPosition.X * widthRatio), texture.Height), Color.Red * 0.15f, 0, Vector2.Zero, effects, 0f);
            //Main.spriteBatch.Draw(texture2, new Rectangle((int)perlinPosition.X, (int)perlinPosition.Y - Main.screenHeight, Main.screenWidth, Main.screenHeight), new Rectangle((int)(perlinPosition.X * widthRatio), 0, (int)(texture.Width - perlinPosition.X * widthRatio), texture.Height), Color.Red * 0.15f, 0, Vector2.Zero, effects, 0f);

            //effects = SpriteEffects.None;
            //if (currentSpriteEffects == 1)
            //{
            //    effects = SpriteEffects.FlipHorizontally;
            //}
            //int positionX2 = (int)perlinPosition.X - Main.screenWidth;
            //Main.spriteBatch.Draw(texture, new Rectangle(positionX2, (int)perlinPosition.Y, Main.screenWidth, Main.screenHeight), new Rectangle((int)(positionX2 * widthRatio), 0, (int)(texture.Width - positionX2 * widthRatio), texture.Height), Color.Red * 0.15f, 0, Vector2.Zero, effects, 0f);
            //Main.spriteBatch.Draw(texture2, new Rectangle(positionX2, (int)perlinPosition.Y + Main.screenHeight, Main.screenWidth, Main.screenHeight), new Rectangle((int)(positionX2 * widthRatio), 0, (int)(texture.Width - positionX2 * widthRatio), texture.Height), Color.Red * 0.15f, 0, Vector2.Zero, effects, 0f);
            //Main.spriteBatch.Draw(texture2, new Rectangle(positionX2, (int)perlinPosition.Y - Main.screenHeight, Main.screenWidth, Main.screenHeight), new Rectangle((int)(positionX2 * widthRatio), 0, (int)(texture.Width - positionX2 * widthRatio), texture.Height), Color.Red * 0.15f, 0, Vector2.Zero, effects, 0f);
            ////spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/Perlin"), new Rectangle(positionX - Main.screenWidth, 0, Main.screenWidth, Main.screenHeight), new Rectangle((int)(texture.Width * (1 - (positionX / Main.screenWidth))), 0, (int)(texture.Width * (float)(positionX / (float)Main.screenWidth)), texture.Height), Color.Red * 0.15f);

            Main.spriteBatch.End();

            orig(self, gameTime);
        }

        public static bool TileShouldBeGrass(int i, int j)
        {
            if (WorldGen.TileEmpty(i + 1, j) && !Main.tileFrameImportant[Main.tile[i + 1, j].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j) && !Main.tileFrameImportant[Main.tile[i - 1, j].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j + 1) && !Main.tileFrameImportant[Main.tile[i, j + 1].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i, j - 1) && !Main.tileFrameImportant[Main.tile[i, j - 1].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i + 1, j + 1) && !Main.tileFrameImportant[Main.tile[i + 1, j + 1].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i + 1, j - 1) && !Main.tileFrameImportant[Main.tile[i + 1, j - 1].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j + 1) && !Main.tileFrameImportant[Main.tile[i - 1, j + 1].type])
            {
                return true;
            }
            else if (WorldGen.TileEmpty(i - 1, j - 1) && !Main.tileFrameImportant[Main.tile[i - 1, j - 1].type])
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
