using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.NPCs
{
    class DuneRoller : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 14;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.aiStyle = -1;
            npc.width = 34;
            npc.height = 34;
            npc.damage = 15;
            npc.defense = 12;
            npc.lifeMax = 125;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 250;
            npc.knockBackResist = 0.75f;
            npc.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.ShellFragments>(), Main.rand.Next(2, 4));
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            if (npc.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2);
            for (int i = 0; i < npc.oldPos.Length; i++)
            {
                Vector2 drawPos = npc.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY) + new Vector2(0, 4);
                Color color = npc.GetAlpha(Color.White) * ((float)(npc.oldPos.Length - i) / (float)npc.oldPos.Length);
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/NPCs/CaveRoller_Glow"), drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (rolling)
            {
                npc.frame.Y = 13 * frameHeight;
                return;
            }

            if (npc.velocity.Y == 0)
            {
                npc.frameCounter--;
                if (npc.frameCounter <= 0)
                {
                    frame++;
                    npc.frameCounter = 3;
                }
                if (frame >= 12)
                {
                    frame = 0;
                }
            }
            else
            {
                frame = 12;
            }

            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.dayTime)
            {
                return SpawnCondition.OverworldDayDesert.Chance * 0.5f;
            }
            else
            {
                return SpawnCondition.DesertCave.Chance * 0.15f;
            }
        }

        bool rolling = false;
        public override void AI()
        {
            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(npc);
            if (rolling)
            {
                modNPC.ImprovedFighterAI(npc, 10, 0.1f, 0.995f, 10, true, 0, 600, 120);
                npc.rotation += MathHelper.ToRadians(npc.velocity.X);
            }
            else
            {
                modNPC.ImprovedFighterAI(npc, 2, 0.2f, 0.99f, 8, true, 0);
                if (npc.life <= npc.lifeMax / 3)
                {
                    rolling = true;
                    npc.knockBackResist /= 3;
                    npc.defense *= 2;
                    npc.damage *= 2;
                }
            }
        }
    }
}


