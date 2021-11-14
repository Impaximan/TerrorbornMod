using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.NPCs.Incendiary
{
    class FallenAngel : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 6;
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = -1;
            npc.width = 166;
            npc.height = 152;
            npc.damage = 45;
            npc.defense = 13;
            npc.lifeMax = 1000;
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
            if (TerrorbornPlayer.modPlayer(spawnInfo.player).ZoneIncendiary && NPC.downedGolemBoss)
            {
                return SpawnCondition.Sky.Chance * 0.15f;
            }
            else
            {
                return 0f;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (spawningLaser)
            {
                TBUtils.Graphics.DrawGlow_1(spriteBatch, npc.Center - Main.screenPosition, 200, Color.LightPink * 0.5f);
                Utils.DrawLine(spriteBatch, laserPosition + new Vector2(0, -3000), laserPosition + new Vector2(0, 3000), Color.LightPink * 0.5f);
            }
            return base.PreDraw(spriteBatch, drawColor);
        }

        float direction = 0f;
        bool start = true;
        int projectileWait = -1;
        bool spawningLaser = false;
        int laserTime = 60;
        Vector2 laserPosition;
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

            if (spawningLaser)
            {
                npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);
                npc.velocity *= 0.92f;
                laserPosition = Vector2.Lerp(laserPosition, player.Center + laserTime * player.velocity, 0.05f);
                laserTime--;
                if (laserTime <= 0)
                {
                    spawningLaser = false;
                    Main.PlaySound(SoundID.Item68, laserPosition);
                    TerrorbornMod.ScreenShake(10);
                    Projectile proj = Main.projectile[Projectile.NewProjectile(laserPosition + new Vector2(0, 3000), new Vector2(0, -1), ModContent.ProjectileType<AngelBeam>(), 120 / 4, 0f)];
                    proj.velocity.Normalize();
                }
            }

            else
            {
                direction.AngleTowards(npc.DirectionFrom(player.Center).ToRotation(), MathHelper.ToRadians(2f));

                float moveSpeed = 0.5f;
                float distance = 300f / ((float)npc.lifeMax / (float)npc.life);
                npc.velocity += npc.DirectionTo(player.Center + direction.ToRotationVector2() * distance) * moveSpeed;
                npc.velocity *= 0.97f;

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
                    spawningLaser = true;
                    laserTime = 90;
                    laserPosition = player.Center + new Vector2(player.velocity.X * laserTime, 0);
                    float speed = 15f;
                    Vector2 velocity = npc.DirectionTo(player.Center) * speed;
                    Main.PlaySound(SoundID.DD2_MonkStaffSwing, npc.Center);
                    float rotation = MathHelper.ToRadians(30);
                    Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<CursedJavelin>(), 120 / 4, 0);
                    Projectile.NewProjectile(npc.Center, velocity.RotatedBy(rotation), ModContent.ProjectileType<CursedJavelin>(), 120 / 4, 0);
                    Projectile.NewProjectile(npc.Center, velocity.RotatedBy(-rotation), ModContent.ProjectileType<CursedJavelin>(), 120 / 4, 0);
                }
            }
        }
    }


    class AngelBeam : Deathray
    {
        int timeLeft = 60;
        public override string Texture => "TerrorbornMod/NPCs/Incendiary/AngelBeam";
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.hide = false;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.timeLeft = timeLeft;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            MoveDistance = 20f;
            RealMaxDistance = 6000f;
            bodyRect = new Rectangle(0, 0, projectile.width, projectile.height);
            headRect = new Rectangle(0, 0, projectile.width, projectile.height); ;
            tailRect = new Rectangle(0, 0, projectile.width, projectile.height); ;
            FollowPosition = false;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
        }
    }
}

