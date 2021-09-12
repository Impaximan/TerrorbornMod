using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.NPCs.Incendiary
{
    class IncendiaryProbe : ModNPC
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = -1;
            npc.width = 40;
            npc.height = 28;
            npc.damage = 45;
            npc.defense = 20;
            npc.lifeMax = 425;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
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

        public override void NPCLoot()
        {
            if (Main.rand.NextFloat() <= 0.066f)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Weapons.Magic.IncendiaryGazeblaster>());
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornPlayer.modPlayer(spawnInfo.player).ZoneIncendiary)
            {
                return SpawnCondition.Sky.Chance * 0.06f;
            }
            else
            {
                return 0f;
            }
        }

        bool start = true;
        int projectileWait = -1;
        public override void AI()
        {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];

            if (start)
            {
                start = false;
                projectileWait = 60;
            }

            if ((Collision.CanHitLine(npc.position, npc.width, npc.height, player.position, player.width, player.height) || npc.life <= npc.lifeMax * 0.25f) && npc.Distance(player.Center) < 1000)
            {
                projectileWait--;
                if (projectileWait <= 0)
                {
                    projectileWait = 30 + (int)(30f / ((float)npc.lifeMax / (float)npc.life));
                    float speed = 15f;
                    Vector2 velocity = npc.DirectionTo(player.Center + player.velocity * (npc.Distance(player.Center) / speed)) * speed;
                    npc.velocity += npc.DirectionFrom(player.Center) * 3;
                    Main.PlaySound(SoundID.Item33, npc.Center);
                    Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<IncendiaryProbeLaser>(), 90 / 4, 0);
                }
            }
            else
            {
                float moveSpeed = 0.2f;
                npc.velocity += npc.DirectionTo(player.Center) * moveSpeed;
            }

            if (player.Center.X > npc.Center.X)
            {
                npc.spriteDirection = 1;
            }
            else
            {
                npc.spriteDirection = -1;
            }

            npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);

            npc.velocity *= 0.96f;
        }
    }

    class IncendiaryProbeLaser : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 6;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 60 * 5);
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Fire, Vector2.Zero);
            dust.noGravity = true;

        }
    }
}
