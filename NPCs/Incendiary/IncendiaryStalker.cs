using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs.Incendiary
{
    class IncendiaryStalker : ModNPC
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.width = 54;
            NPC.height = 38;
            NPC.damage = 45;
            NPC.defense = 20;
            NPC.lifeMax = 1200;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
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


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("A former Orumian bodyguard, converted and ruined by the Sisyphean Islands and then reinforced with Skullmound Alloy.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Magic.IncendiaryGazeblaster>(), 15));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornPlayer.modPlayer(spawnInfo.Player).ZoneIncendiary && NPC.downedGolemBoss)
            {
                return SpawnCondition.Sky.Chance * 0.06f;
            }
            else
            {
                return 0f;
            }
        }

        bool start = true;
        int ProjectileWait = -1;
        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            if (start)
            {
                start = false;
                ProjectileWait = 60;
            }

            if ((Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) || NPC.life <= NPC.lifeMax * 0.25f) && NPC.Distance(player.Center) < 1000)
            {
                ProjectileWait--;
                if (ProjectileWait <= 0)
                {
                    ProjectileWait = 20 + (int)(20f / ((float)NPC.lifeMax / (float)NPC.life));
                    float speed = 18f;
                    Vector2 velocity = NPC.DirectionTo(player.Center + player.velocity * (NPC.Distance(player.Center) / speed)) * speed;
                    NPC.velocity += NPC.DirectionFrom(player.Center) * 3;
                    SoundExtensions.PlaySoundOld(SoundID.Item33, NPC.Center);
                    float offset = MathHelper.ToRadians(30);
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity, ModContent.ProjectileType<Projectiles.HellbornLaser>(), 120 / 4, 0);
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity.RotatedBy(offset), ModContent.ProjectileType<Projectiles.HellbornLaser>(), 120 / 4, 0);
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity.RotatedBy(-offset), ModContent.ProjectileType<Projectiles.HellbornLaser>(), 120 / 4, 0);
                }
            }
            else
            {
                float moveSpeed = 0.4f;
                NPC.velocity += NPC.DirectionTo(player.Center) * moveSpeed;
            }

            if (player.Center.X > NPC.Center.X)
            {
                NPC.spriteDirection = 1;
            }
            else
            {
                NPC.spriteDirection = -1;
            }

            NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 1.5f);

            NPC.velocity *= 0.96f;
        }
    }
}