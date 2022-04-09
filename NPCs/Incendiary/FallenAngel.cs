using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Projectiles;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;

namespace TerrorbornMod.NPCs.Incendiary
{
    class FallenAngel : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("An angel that used to guard the depths of the underworld, which has been cursed and converted by the Sisyphean Islands.")
            });
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.width = 166;
            NPC.height = 152;
            NPC.damage = 45;
            NPC.defense = 13;
            NPC.lifeMax = 1000;
            NPC.HitSound = SoundID.NPCHit37;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.value = 250;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter--;
            if (NPC.frameCounter <= 0)
            {
                frame++;
                NPC.frameCounter = 3;
            }
            if (frame >= Main.npcFrameCount[NPC.type])
            {
                frame = 0;
            }
            NPC.frame.Y = frame * frameHeight;
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

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.CursedJavelin>(), 5, 125, 175));
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (spawningLaser)
            {
                TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, NPC.Center - Main.screenPosition, 200, Color.LightPink * 0.5f);
                Utils.DrawLine(spriteBatch, laserPosition + new Vector2(0, -3000), laserPosition + new Vector2(0, 3000), Color.LightPink * 0.5f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        float direction = 0f;
        bool start = true;
        int ProjectileWait = -1;
        bool spawningLaser = false;
        int laserTime = 60;
        Vector2 laserPosition;
        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            if (start)
            {
                start = false;
                direction = NPC.DirectionFrom(player.Center).ToRotation();
                ProjectileWait = Main.rand.Next(60, 240);
            }

            if (spawningLaser)
            {
                NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 1.5f);
                NPC.velocity *= 0.92f;
                laserPosition = Vector2.Lerp(laserPosition, player.Center + laserTime * player.velocity, 0.05f);
                laserTime--;
                if (laserTime <= 0)
                {
                    spawningLaser = false;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item68, laserPosition);
                    TerrorbornSystem.ScreenShake(10);
                    Projectile proj = Main.projectile[Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), laserPosition + new Vector2(0, 3000), new Vector2(0, -1), ModContent.ProjectileType<AngelBeam>(), 120 / 4, 0f)];
                    proj.velocity.Normalize();
                }
            }

            else
            {
                direction.AngleTowards(NPC.DirectionFrom(player.Center).ToRotation(), MathHelper.ToRadians(2f));

                float moveSpeed = 0.5f;
                float distance = 300f / ((float)NPC.lifeMax / (float)NPC.life);
                NPC.velocity += NPC.DirectionTo(player.Center + direction.ToRotationVector2() * distance) * moveSpeed;
                NPC.velocity *= 0.97f;

                if (player.Center.X > NPC.Center.X)
                {
                    NPC.spriteDirection = 1;
                }
                else
                {
                    NPC.spriteDirection = -1;
                }

                NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 1.5f);

                ProjectileWait--;
                if (ProjectileWait <= 0)
                {
                    ProjectileWait = 60 * 3;
                    spawningLaser = true;
                    laserTime = 90;
                    laserPosition = player.Center + new Vector2(player.velocity.X * laserTime, 0);
                    float speed = 15f;
                    Vector2 velocity = NPC.DirectionTo(player.Center) * speed;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, NPC.Center);
                    float rotation = MathHelper.ToRadians(30);
                    Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center, velocity, ModContent.ProjectileType<CursedJavelin>(), 120 / 4, 0);
                    Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center, velocity.RotatedBy(rotation), ModContent.ProjectileType<CursedJavelin>(), 120 / 4, 0);
                    Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center, velocity.RotatedBy(-rotation), ModContent.ProjectileType<CursedJavelin>(), 120 / 4, 0);
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
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = false;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = timeLeft;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            MoveDistance = 20f;
            RealMaxDistance = 6000f;
            bodyRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
            headRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
            tailRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
            FollowPosition = false;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
        }
    }
}

