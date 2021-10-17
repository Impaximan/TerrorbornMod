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

namespace TerrorbornMod.TBUtils
{
    static class Detours
    {
        public static void Initialize()
        {
            On.Terraria.Main.DrawInterface += DrawOvertopGraphics;
            On.Terraria.Main.DrawNPCs += DrawNPCs;
        }

        public static void Unload()
        {
            On.Terraria.Main.DrawInterface -= DrawOvertopGraphics;
            On.Terraria.Main.DrawNPCs -= DrawNPCs;
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

            //Main.spriteBatch.End();
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

            if (TerrorbornMod.ScreenDarknessAlpha > 0f)
            {
                Main.spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/WhitePixel"), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * TerrorbornMod.ScreenDarknessAlpha);
            }

            Main.spriteBatch.End();

            orig(self, gameTime);
        }

        private static void DrawNPCs(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behindTiles)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.HexedConstructor.HexedConstructor>()))
            {
                NPC npc = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<NPCs.Bosses.HexedConstructor.HexedConstructor>())];
                foreach (NPC claw in Main.npc)
                {
                    if (claw.type == ModContent.NPCType<NPCs.Bosses.HexedConstructor.HexedClaw>() && claw.active)
                    {
                        NPCs.Bosses.HexedConstructor.HexedConstructor.DrawWire(Main.spriteBatch, Color.White, ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/HexedConstructor/HexedWire"), ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/HexedConstructor/HexedWireEnd"), npc.Center - new Vector2(0, 20), claw.Center, (int)(Math.Abs(Math.Sin((double)(npc.ai[0] / 60f))) * 50) + 20, 25);
                    }
                }
            }
            orig(self, behindTiles);
        }
    }
}
