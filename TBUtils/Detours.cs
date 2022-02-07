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
using System.Reflection;

namespace TerrorbornMod.TBUtils
{
    static class Detours
    {
        public static void Initialize()
        {
            On.Terraria.Main.DrawInterface += DrawOvertopGraphics;
            On.Terraria.Main.DrawNPCs += DrawNPCs;
            IL.Terraria.Player.Update += HookUpdate;
        }

        private static void HookUpdate(ILContext il)
        {
            var c = new ILCursor(il);
            if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdfld("Terraria.Player", "statManaMax2"), i => i.MatchLdcI4(400)))
            {
                ErrorLogger.Log("Mana patchy no worky... missing instruction UWU");
                return;
            }

            c.Next.Next.Operand = int.MaxValue;
        }

        public static void Unload()
        {
            On.Terraria.Main.DrawInterface -= DrawOvertopGraphics;
            On.Terraria.Main.DrawNPCs -= DrawNPCs;
            IL.Terraria.Player.Update -= HookUpdate;
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
                        if (foregroundObject2.distance < foregroundObject.distance)
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
            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

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

            if (modPlayer.deimosChained.Count > 0)
            {
                for (int i = 0; i < modPlayer.deimosChained.Count; i++)
                {
                    Vector2 originPoint = modPlayer.deimosChained[i].Center;
                    Vector2 center = player.Center;
                    if (i > 0)
                    {
                        center = modPlayer.deimosChained[i - 1].Center;
                    }
                    Vector2 distToProj = originPoint - center;
                    float projRotation = distToProj.ToRotation() - 1.57f;
                    float distance = distToProj.Length();
                    Texture2D texture = ModContent.GetTexture("TerrorbornMod/Items/Weapons/Restless/DeimosChain");

                    while (distance > texture.Height && !float.IsNaN(distance))
                    {
                        distToProj.Normalize();
                        distToProj *= texture.Height;
                        center += distToProj;
                        distToProj = originPoint - center;
                        distance = distToProj.Length();


                        //Draw chain
                        Main.spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                            new Rectangle(0, 0, texture.Width, texture.Height), Color.White, projRotation,
                            new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
                    }
                }
            }
            
            orig(self, behindTiles);
        }
    }
}
