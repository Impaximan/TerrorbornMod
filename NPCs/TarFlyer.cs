using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Events;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.UI;

namespace TerrorbornMod.NPCs
{
    class TarFlyer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 62;
            npc.height = 36;
            npc.damage = 45;
            npc.defense = 6;
            npc.lifeMax = 160;
            npc.HitSound = SoundID.NPCHit31;
            npc.DeathSound = SoundID.NPCDeath39;
            npc.value = 250;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TarOfHunger"), Main.rand.Next(5, 11));
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            if (npc.direction == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2);
            for (int i = 0; i < npc.oldPos.Length; i++)
            {
                Vector2 drawPos = npc.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY) + new Vector2(0, 4);
                Color color = npc.GetAlpha(Color.White) * ((float)(npc.oldPos.Length - i) / (float)npc.oldPos.Length);
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/NPCs/TarFlyer_Glow"), drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
        }
        int frame = 0;
        float TrueVelocityX = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter--;
            if (npc.frameCounter <= 0)
            {
                frame++;
                npc.frameCounter = 2;
            }
            if (frame >= 3)
            {
                frame = 0;
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss3)
            {
                return SpawnCondition.DesertCave.Chance * 0.11f;
            }
            else
            {
                return 0f;
            }
        }
        public override void AI()
        {
            npc.TargetClosest();
            if (Main.player[npc.target].dead)
            {
                float speed = -0.2f;
                Vector2 move = Main.player[npc.target].Center - npc.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                move *= speed / magnitude;
                npc.velocity.Y += move.Y;
                TrueVelocityX += move.X;

                npc.velocity.Y *= 0.99f;
                TrueVelocityX *= 0.99f;
                if (npc.Distance(Main.player[npc.target].Center) > 4500)
                {
                    npc.active = false;
                }
            }
            else
            {
                float speed = 0.1f;
                if (!Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    speed = 0.04f;
                }
                else
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldFlame, Scale: 1f);
                    Main.dust[dust].velocity = npc.velocity;
                    Main.dust[dust].noGravity = true;
                }
                Vector2 move = Main.player[npc.target].Center - npc.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                move *= speed / magnitude;
                npc.velocity.Y += move.Y;
                TrueVelocityX += move.X;

                npc.velocity.Y *= 0.99f;
                TrueVelocityX *= 0.99f;
            }

            //Antlion swarmer AI - used to have an edited version
            //if (npc.collideX)
            //{
            //    TrueVelocityX = npc.oldVelocity.X * -0.5f;
            //    if (npc.direction == -1 && TrueVelocityX > 0f && TrueVelocityX < 2f)
            //    {
            //        TrueVelocityX = 2f;
            //    }
            //    if (npc.direction == 1 && TrueVelocityX < 0f && TrueVelocityX > -2f)
            //    {
            //        TrueVelocityX = -2f;
            //    }
            //}
            //if (npc.collideY)
            //{
            //    npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
            //    if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
            //    {
            //        npc.velocity.Y = 1f;
            //    }
            //    if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
            //    {
            //        npc.velocity.Y = -1f;
            //    }
            //}
            //float num707 = 0.06f; //Flying speed (?)
            //float num708 = 8f;
            //float num709 = Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2)));
            //float num710 = Main.player[npc.target].position.Y - (float)(npc.height / 2);
            //if (num709 > 30f)
            //{
            //    if (npc.direction == -1 && TrueVelocityX > -num708)
            //    {
            //        TrueVelocityX = TrueVelocityX - num707;
            //        if (TrueVelocityX > num708)
            //        {
            //            TrueVelocityX = TrueVelocityX - num707;
            //        }
            //        else
            //        {
            //            if (TrueVelocityX > 0f)
            //            {
            //                TrueVelocityX = TrueVelocityX - num707 / 2f;
            //            }
            //        }
            //        if (TrueVelocityX < -num708)
            //        {
            //            TrueVelocityX = -num708;
            //        }
            //    }
            //    else
            //    {
            //        if (npc.direction == 1 && TrueVelocityX < num708)
            //        {
            //            TrueVelocityX = TrueVelocityX + num707;
            //            if (TrueVelocityX < -num708)
            //            {
            //                TrueVelocityX = TrueVelocityX + num707;
            //            }
            //            else
            //            {
            //                if (TrueVelocityX < 0f)
            //                {
            //                    TrueVelocityX = TrueVelocityX + num707 / 2f;
            //                }
            //            }
            //            if (TrueVelocityX > num708)
            //            {
            //                TrueVelocityX = num708;
            //            }
            //        }
            //    }
            //}
            //if (num709 > 100f)
            //{
            //    num710 -= 50f;
            //}
            //if (npc.position.Y < num710)
            //{
            //    npc.velocity.Y = npc.velocity.Y + 0.01f;
            //    if (npc.velocity.Y < 0f)
            //    {
            //        npc.velocity.Y = npc.velocity.Y + 0.01f;
            //    }
            //}
            //else
            //{
            //    npc.velocity.Y = npc.velocity.Y - 0.01f;
            //    if (npc.velocity.Y > 0f)
            //    {
            //        npc.velocity.Y = npc.velocity.Y - 0.01f;
            //    }
            //}
            //if (npc.velocity.Y < -1f)
            //{
            //    npc.velocity.Y = -1f;
            //}
            //if (npc.velocity.Y > 1f)
            //{
            //    npc.velocity.Y = 1f;
            //    return;
            //}
            //if (Main.player[npc.target].Center.X > npc.Center.X)
            //{
            //    npc.direction = 1;
            //}
            //else
            //{
            //    npc.direction = -1;
            //}
            npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);
            npc.velocity.X = TrueVelocityX;
        }
    }
}
