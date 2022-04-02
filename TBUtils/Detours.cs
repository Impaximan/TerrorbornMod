using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;
using TerrorbornMod.ForegroundObjects;

namespace TerrorbornMod.TBUtils
{
    static class Detours
    {
        public static void Initialize()
        {
            On.Terraria.Main.DrawInterface += DrawOvertopGraphics;
            On.Terraria.Main.DrawNPCs += DrawNPCs;
            IL.Terraria.Player.Update += HookUpdate;
            //IL.Terraria.Item.Prefix += HookPrefix;
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

        private static void HookPrefix(ILContext il)
        {
            var c = new ILCursor(il);
            if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdfld("Terraria.Item", "rare"), i => i.MatchLdcI4(11)))
            {
                ErrorLogger.Log("AHAHALHGKJLHDFGKJHDSGHJ NO RARITY INSTRUCTION GRRR");
                return;
            }

            c.Next.Next.Operand = int.MaxValue;
        }

        public static void Unload()
        {
            On.Terraria.Main.DrawInterface -= DrawOvertopGraphics;
            On.Terraria.Main.DrawNPCs -= DrawNPCs;
            IL.Terraria.Player.Update -= HookUpdate;
            //IL.Terraria.Item.Prefix -= HookPrefix;
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
                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/WhitePixel"), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * TerrorbornMod.ScreenDarknessAlpha);
            }

            Main.spriteBatch.End();

            orig(self, gameTime);
        }

        public static void DrawWireFriendly(SpriteBatch spriteBatch, Color drawColor, Texture2D texture, Texture2D endTexture, Vector2 startPosition, Vector2 endPosition, int fallAmount, int segmentCount = 25)
        {
            BezierCurve chain = new BezierCurve(new List<Vector2>()
            {
                { startPosition },
                { Vector2.Lerp(startPosition, endPosition, 0.5f) + new Vector2(0, fallAmount) },
                { endPosition }
            });

            List<Vector2> positions = chain.GetPoints(segmentCount);

            for (int i = 0; i < positions.Count - 1; i++)
            {
                spriteBatch.Draw(texture, new Rectangle((int)(positions[i].X - Main.screenPosition.X), (int)(positions[i].Y - Main.screenPosition.Y), texture.Width, (int)(positions[i + 1] - positions[i]).Length() + 2), null, drawColor, (positions[i + 1] - positions[i]).RotatedBy(MathHelper.ToRadians(90)).ToRotation(), texture.Size() / 2, SpriteEffects.None, 0);
            }

            spriteBatch.Draw(texture, new Rectangle((int)(positions[positions.Count - 1].X - Main.screenPosition.X), (int)(positions[positions.Count - 1].Y - Main.screenPosition.Y), texture.Width, (int)(endPosition - positions[positions.Count - 1]).Length() + 2), null, drawColor, (endPosition - positions[positions.Count - 1]).RotatedBy(MathHelper.ToRadians(90)).ToRotation(), texture.Size() / 2, SpriteEffects.None, 0);

            spriteBatch.Draw(endTexture, endPosition - Main.screenPosition, null, drawColor, (endPosition - positions[positions.Count - 1]).RotatedBy(MathHelper.ToRadians(90)).ToRotation(), new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0);
        }

        private static void DrawNPCs(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behindTiles)
        {
            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.HexedConstructor.HexedConstructor>()))
            {
                NPC NPC = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<NPCs.Bosses.HexedConstructor.HexedConstructor>())];
                foreach (NPC claw in Main.npc)
                {
                    if (claw.type == ModContent.NPCType<NPCs.Bosses.HexedConstructor.HexedClaw>() && claw.active)
                    {
                        NPCs.Bosses.HexedConstructor.HexedConstructor.DrawWire(Main.spriteBatch, Color.White, (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/HexedConstructor/HexedWire"), (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/HexedConstructor/HexedWireEnd"), NPC.Center - new Vector2(0, 20), claw.Center, (int)(Math.Abs(Math.Sin((double)(NPC.ai[0] / 60f))) * 50) + 20, 25);
                    }
                }
            }

            foreach (Projectile Projectile in Main.projectile)
            {
                if (Projectile.active && Projectile.type == ModContent.ProjectileType<Items.Equipable.Accessories.ConstructorsDestructorsClaw>())
                {
                    DrawWireFriendly(Main.spriteBatch, Color.White, (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Items/Equipable/Accessories/ConstructorsDestructorsWire"), (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Items/Equipable/Accessories/ConstructorsDestructorsWireEnd"), Main.player[Projectile.owner].Center, Projectile.Center, 30, 35);
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
                    Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Items/Weapons/Restless/DeimosChain");

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
