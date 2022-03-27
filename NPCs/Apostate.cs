using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TerrorbornMod.NPCs
{
    class Apostate : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 6;
            NPCID.Sets.TrailCacheLength[npc.type] = 1;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 94;
            npc.height = 408 / 6;
            npc.damage = 75;
            npc.defense = 45;
            npc.lifeMax = 8500;
            npc.HitSound = SoundID.NPCHit21;
            npc.DeathSound = SoundID.NPCDeath39;
            npc.value = Item.buyPrice(0, 5, 0, 0);
            npc.aiStyle = -1;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(npc);
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TorturedEssence>());
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!NPC.downedMoonlord)
            {
                return 0f;
            }
            return SpawnCondition.Underworld.Chance * 0.15f;
        }

        int frame = 0;
        bool dashing = false;

        public override void FindFrame(int frameHeight)
        {
            if (!dashing)
            {
                npc.frameCounter++;
                if (npc.frameCounter > 4)
                {
                    npc.frameCounter = 0;
                    frame++;
                }
                if (frame >= 5)
                {
                    frame = 0;
                }
            }
            else
            {
                frame = 5;
            }
            npc.frame.Y = frame * frameHeight;
        }

        int AIPhase = 0;
        int attackCounter = 0;
        int aliveCounter = 0;
        bool start = true;
        Player player;
        public override void AI()
        {
            if (start)
            {
                npc.TargetClosest();
                player = Main.player[npc.target];
                start = false;
            }

            aliveCounter++;
            if (aliveCounter % 600 == 120)
            {
                WeightedRandom<string> messages = new WeightedRandom<string>();
                messages.Add("Hisssss...");
                messages.Add("*cursing*");
                messages.Add("Murrderrrr...");
                messages.Add("...");
                messages.Add("Death to... Uri...");
                CombatText.NewText(npc.getRect(), Color.Red, messages, true);
            }

            if (AIPhase == 0)
            {
                attackCounter++;
                npc.rotation = MathHelper.ToRadians(npc.velocity.X);
                npc.spriteDirection = Math.Sign(player.Center.X - npc.Center.X);
                npc.velocity += npc.DirectionTo(player.Center) * 0.5f;
                npc.velocity *= 0.96f;
                if (attackCounter > 180)
                {
                    attackCounter = 0;
                    AIPhase = 1;
                    npc.velocity = npc.DirectionTo(player.Center) * 20f;
                    npc.rotation = npc.DirectionTo(player.Center).ToRotation();
                    if (npc.spriteDirection == -1) npc.rotation += MathHelper.ToRadians(180);
                    dashing = true;
                }
            }
            else if (AIPhase == 1)
            {
                attackCounter++;
                npc.velocity *= 0.98f;
                npc.velocity.Y += 0.1f;
                if (attackCounter > 60)
                {
                    AIPhase = 2;
                    attackCounter = 0;
                    dashing = false;
                }
            }
            else if (AIPhase == 2)
            {
                attackCounter++;
                npc.rotation = MathHelper.ToRadians(npc.velocity.X);
                npc.spriteDirection = Math.Sign(player.Center.X - npc.Center.X);
                if (attackCounter > 60)
                {
                    attackCounter = 0;
                    AIPhase = 0;
                }
                npc.velocity *= 0.93f;
            }
        }
    }
}