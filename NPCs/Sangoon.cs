using System.IO;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;

namespace TerrorbornMod.NPCs
{
    class Sangoon : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 46;
            npc.height = 30;
            npc.damage = 25;
            npc.defense = 7;
            npc.lifeMax = 50;
            npc.HitSound = SoundID.NPCHit18;
            npc.DeathSound = SoundID.NPCDeath32;
            npc.value = 250;
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            if (Main.hardMode)
            {
                npc.lifeMax = 285;
                npc.damage = 80;
                npc.defense = 17;
            }
        }

        int Frame = 0;
        int FrameWait = 4;
        public override void FindFrame(int frameHeight)
        {
            FrameWait--;
            if (FrameWait <= 0)
            {
                FrameWait = 6;
                Frame++;
                if (Frame >= 5)
                {
                    Frame = 0;
                }
            }
            npc.frame.Y = Frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1 && Main.bloodMoon)
            {
                return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
            }
            else
            {
                return 0f;
            }
        }

        bool charging = false;
        int attackCounter = 60;
        public override void AI()
        {
            npc.TargetClosest(true);
            if (Main.player[npc.target].dead)
            {
                float speed = -0.2f;
                Vector2 velocity = npc.DirectionTo(Main.player[npc.target].Center) * speed;
                npc.velocity += velocity;

                npc.velocity *= 0.99f;
                if (npc.Distance(Main.player[npc.target].Center) > 4500)
                {
                    npc.active = false;
                }
            }
            else
            {
                if (attackCounter > 0)
                {
                    attackCounter--;
                }
                else
                {
                    if (charging)
                    {
                        charging = false;
                        attackCounter = 180;
                    }
                    else
                    {
                        charging = true;
                        attackCounter = 15;
                        float speed = 10f;
                        npc.velocity = npc.DirectionTo(Main.player[npc.target].Center) * speed;
                    }
                }

                if (!charging)
                {
                    if (Main.player[npc.target].Center.X > npc.Center.X)
                    {
                        npc.spriteDirection = 1;
                    }
                    else
                    {
                        npc.spriteDirection = -1;
                    }

                    float speed = 0.1f;
                    Vector2 velocity = npc.DirectionTo(Main.player[npc.target].Center) * speed;
                    npc.velocity += velocity;

                    npc.velocity *= 0.98f;
                }
            }
            npc.rotation = MathHelper.ToRadians(npc.velocity.X * 2);
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(5) == 0)
            {
                Item.NewItem(npc.position, npc.width, npc.height, ItemID.Heart);
            }
            Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.Materials.SanguineFang>(), Main.rand.Next(1, 3));
        }
    }
}
