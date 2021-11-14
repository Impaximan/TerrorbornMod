using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.NPCs.Incendiary
{
    class PrimeHarpy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = -1;
            npc.width = 96;
            npc.height = 84;
            npc.damage = 45;
            npc.defense = 13;
            npc.lifeMax = 375;
            npc.HitSound = SoundID.NPCHit37;
            npc.DeathSound = SoundID.NPCDeath39;
            npc.value = 250;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter--;
            if (npc.frameCounter <= 0)
            {
                frame++;
                npc.frameCounter = 3;
            }
            if (frame >= Main.npcFrameCount[npc.type])
            {
                frame = 0;
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornPlayer.modPlayer(spawnInfo.player).ZoneIncendiary && !NPC.downedGolemBoss)
            {
                return SpawnCondition.Sky.Chance * 0.15f;
            }
            else
            {
                return 0f;
            }
        }

        float direction = 0f;
        bool start = true;
        int projectileWait = -1;
        public override void AI()
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];

            if (start)
            {
                start = false;
                direction = npc.DirectionFrom(player.Center).ToRotation();
                projectileWait = Main.rand.Next(60, 240);
            }

            direction.AngleTowards(npc.DirectionFrom(player.Center).ToRotation(), MathHelper.ToRadians(2f));

            float moveSpeed = 0.3f;
            float distance = 400f / ((float)npc.lifeMax / (float)npc.life);
            npc.velocity += npc.DirectionTo(player.Center + direction.ToRotationVector2() * distance) * moveSpeed;
            npc.velocity *= 0.98f;

            if (player.Center.X > npc.Center.X)
            {
                npc.spriteDirection = 1;
            }
            else
            {
                npc.spriteDirection = -1;
            }

            npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);

            projectileWait--;
            if (projectileWait <= 0)
            {
                projectileWait = 60 * 3;
                float speed = 15f;
                Vector2 velocity = npc.DirectionTo(player.Center) * speed;
                Main.PlaySound(SoundID.DD2_MonkStaffSwing, npc.Center);
                Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<CursedJavelin>(), 80 / 4, 0);
            }
        }
    }

    class CursedJavelin : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 56;
            projectile.height = 56;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
        }
    }
}
