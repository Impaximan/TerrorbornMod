using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.NPCs
{
    class TidalSquid : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.width = 20;
            NPC.height = 40;
            NPC.damage = 45;
            NPC.defense = 3;
            NPC.lifeMax = 130;
            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath25;
            NPC.knockBackResist = 0f;
        }
        
        float ChargeWait = 60;
        public override void AI()
        {
            NPC.rotation = MathHelper.ToRadians(NPC.velocity.X);
            NPC.TargetClosest(false);
            Player targetPlayer = Main.player[NPC.target];
            
            if (targetPlayer.wet || !NPC.wet)
            {
                NPC.velocity.Y += 0.1f;
            }
            else
            {
                NPC.velocity.Y *= 0.8f;
            }

            if (NPC.wet && targetPlayer.wet)
            {
                NPC.noTileCollide = false;
                ChargeWait--;
                if (Collision.CanHit(NPC.position, NPC.width, NPC.height, targetPlayer.position, targetPlayer.width, targetPlayer.height) && targetPlayer.position.Y < NPC.position.Y && ChargeWait <= 0)
                {
                    ChargeWait = Main.rand.Next(15, 75);
                    NPC.velocity.Y -= 10;
                    float XSpeed = 6;
                    if (targetPlayer.position.X < NPC.position.X)
                    {
                        NPC.velocity.X -= XSpeed;
                    }
                    else
                    {
                        NPC.velocity.X += XSpeed;
                    }
                }
            }
            else
            {

            }
            NPC.velocity.Y *= 0.98f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(9) == 0)
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, mod.ItemType("CrackedShell"), 1);
            }
        }

        int frame = 0;
        float TrueVelocityX = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter--;
            if (NPC.frameCounter <= 0)
            {
                frame++;
                NPC.frameCounter = 6;
            }
            if (frame >= 3)
            {
                frame = 0;
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornSystem.downedTidalTitan && Main.raining)
            {
                return SpawnCondition.Ocean.Chance * 0.9f;
            }
            else
            {
                return 0f;
            }
        }
    }
}
