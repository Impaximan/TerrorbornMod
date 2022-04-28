using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace TerrorbornMod.NPCs
{
    class Sangoon : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 46;
            NPC.height = 30;
            NPC.damage = 25;
            NPC.defense = 7;
            NPC.lifeMax = 50;
            NPC.HitSound = SoundID.NPCHit18;
            NPC.DeathSound = SoundID.NPCDeath32;
            NPC.value = 250;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            if (Main.hardMode)
            {
                NPC.lifeMax = 285;
                NPC.damage = 80;
                NPC.defense = 17;
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
            NPC.frame.Y = Frame * frameHeight;
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
            NPC.TargetClosest(true);
            if (Main.player[NPC.target].dead)
            {
                float speed = -0.2f;
                Vector2 velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * speed;
                NPC.velocity += velocity;

                NPC.velocity *= 0.99f;
                if (NPC.Distance(Main.player[NPC.target].Center) > 4500)
                {
                    NPC.active = false;
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
                        NPC.velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * speed;
                    }
                }

                if (!charging)
                {
                    if (Main.player[NPC.target].Center.X > NPC.Center.X)
                    {
                        NPC.spriteDirection = 1;
                    }
                    else
                    {
                        NPC.spriteDirection = -1;
                    }

                    float speed = 0.1f;
                    Vector2 velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * speed;
                    NPC.velocity += velocity;

                    NPC.velocity *= 0.98f;
                }
            }
            NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 2);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.SanguineFang>(), 1, 1, 2));
        }
    }
}
