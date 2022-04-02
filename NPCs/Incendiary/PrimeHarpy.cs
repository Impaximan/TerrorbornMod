using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.NPCs.Incendiary
{
    class PrimeHarpy : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.width = 96;
            NPC.height = 84;
            NPC.damage = 45;
            NPC.defense = 13;
            NPC.lifeMax = 375;
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

        public override void NPCLoot()
        {
            if (Main.rand.NextFloat() <= 0.2f)
            {
                Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Weapons.Ranged.CursedJavelin>(), Main.rand.Next(125, 176));
            }
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
        int ProjectileWait = -1;
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

            direction.AngleTowards(NPC.DirectionFrom(player.Center).ToRotation(), MathHelper.ToRadians(2f));

            float moveSpeed = 0.3f;
            float distance = 400f / ((float)NPC.lifeMax / (float)NPC.life);
            NPC.velocity += NPC.DirectionTo(player.Center + direction.ToRotationVector2() * distance) * moveSpeed;
            NPC.velocity *= 0.98f;

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
                float speed = 15f;
                Vector2 velocity = NPC.DirectionTo(player.Center) * speed;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, NPC.Center);
                Projectile.NewProjectile(NPC.Center, velocity, ModContent.ProjectileType<CursedJavelin>(), 80 / 4, 0);
            }
        }
    }

    class CursedJavelin : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 56;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
        }
    }
}
