using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs
{
    class TarSludge : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.width = 32;
            NPC.height = 46;
            NPC.damage = 45;
            NPC.defense = 6;
            NPC.lifeMax = 170;
            NPC.HitSound = SoundID.NPCHit8;
            NPC.DeathSound = SoundID.NPCDeath13;
            NPC.value = 250;
            NPC.knockBackResist = 0.1f;
            NPC.aiStyle = 1;
            
            NPC.lavaImmune = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
                new FlavorTextBestiaryInfoElement("A pot possessed by the living tar within the desert, which uses javelins made with other pot scraps.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.ShriekOfHorrorUnlockedCondition(), ModContent.ItemType<Items.Materials.TarOfHunger>(), minimumDropped: 5, maximumDropped: 10));
            npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.ShriekOfHorrorUnlockedCondition(), ModContent.ItemType<Items.Equipable.Accessories.TheLiesOfNourishment>(), minimumDropped: 5, maximumDropped: 10, chanceNumerator: 1, chanceDenominator: 50));
        }

        int JavelinCounter = 69;
        public override void PostAI()
        {
            Player player = Main.player[NPC.target];
            if (NPC.velocity == Vector2.Zero && (Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height) || NPC.life <= NPC.lifeMax * 0.30f))
            {
                if (NPC.life <= NPC.lifeMax * 0.30f)
                {
                    JavelinCounter -= 3;
                }
                else
                {
                    JavelinCounter--;
                }
                if (JavelinCounter <= 0)
                {
                    JavelinCounter = Main.rand.Next(60, 80);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, NPC.Center);
                    Vector2 Rotation = NPC.DirectionTo(player.Center);
                    float Speed = 10;
                    if (NPC.life <= NPC.lifeMax * 0.30f)
                    {
                        Speed = 16;
                    }
                    Vector2 Velocity = Rotation * Speed;
                    Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center, Velocity, ModContent.ProjectileType<TarSludgeJavelin>(), 20, 0);
                }
            }
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y == 0)
            {
                NPC.frameCounter--;
                if (NPC.frameCounter <= 0)
                {
                    frame++;
                    NPC.frameCounter = 30;
                }
                if (frame >= 2)
                {
                    frame = 0;
                }
            }
            else
            {
                frame = 1;
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss3)
            {
                return SpawnCondition.DesertCave.Chance * 0.22f;
            }
            else
            {
                return 0f;
            }
        }
    }

    class TarSludgeJavelin : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Projectile.rotation =
                Projectile.velocity.ToRotation() +
                MathHelper.ToRadians(45f);
            Projectile.velocity.Y += 0.05f;
        }
    }
}
