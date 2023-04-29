using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.Utilities;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs
{
    class SlateBanshee : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 14;
            NPCID.Sets.TrailCacheLength[NPC.type] = 1;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 34;
            NPC.height = 896 / 14;
            NPC.damage = 25;
            NPC.defense = 6;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit37;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.value = 250;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("A mysterious wandering mass of terror that roams deimostone caves. Though its exact origin is unknown, it is suspected by many that they are the remaining souls of the fearmongerers.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.ShriekOfHorrorUnlockedCondition(), ModContent.ItemType<Items.Materials.TerrorSample>(), minimumDropped: 3, maximumDropped: 6));
        }

        public override void OnKill()
        {
            TerrorbornSystem.downedSlateBanshee = true;

            bool spawnHI = !TerrorbornPlayer.modPlayer(Main.player[Main.myPlayer]).unlockedAbilities.Contains(9);
            for (int i = 0; i < 1000; i++)
            {
                Projectile Projectile = Main.projectile[i];
                if (Projectile.active && Projectile.type == ModContent.ProjectileType<Abilities.HiddenInstinct>())
                {
                    spawnHI = false;
                }
            }

            if (spawnHI)
            {
                Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Abilities.HiddenInstinct>(), 0, 0, Main.myPlayer);
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(NPC.width / 2, NPC.height / 2);
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 drawPos = NPC.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY) + new Vector2(0, 4);
                Color color = NPC.GetAlpha(Color.White) * ((float)(NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/SlateBanshee_Glow"), drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
            }
        }

        bool charging = false;
        bool resting = false;
        int frame = 0;
        Vector2 velocity = Vector2.Zero;
        public override void FindFrame(int frameHeight)
        {
            if (!charging && !resting)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 4)
                {
                    frame++;
                    NPC.frameCounter = 0;
                }
                if (frame >= 4)
                {
                    frame = 0;
                }
            }
            if (charging)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 3)
                {
                    frame++;
                    NPC.frameCounter = 0;
                }
                if (frame < 4)
                {
                    frame = 4;
                    NPC.frameCounter = 0;
                }
                if (frame >= 8)
                {
                    frame = 5;
                }
            }
            if (resting)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 5)
                {
                    frame++;
                    NPC.frameCounter = 0;
                }
                if (frame < 8)
                {
                    frame = 8;
                    NPC.frameCounter = 0;
                }
                if (frame >= 14)
                {
                    frame = 9;
                }
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(spawnInfo.Player);
            if (modPlayer.ZoneDeimostone && TerrorbornSystem.obtainedShriekOfHorror)
            {
                if (modPlayer.DeimosteelCharm)
                {
                    return SpawnCondition.Cavern.Chance * 0.0325f;
                }
                else
                {
                    return SpawnCondition.Cavern.Chance * 0.015f;
                }
            }
            else
            {
                return 0f;
            }
        }

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (charging)
            {
                NPC.velocity = Vector2.Zero;
                charging = false;
                resting = true;
                restingCounter = 120;
            }
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (charging && Projectile.DamageType == DamageClass.Melee && NPC.Distance(Main.player[NPC.target].Center) <= 75)
            {
                NPC.velocity = Vector2.Zero;
                charging = false;
                resting = true;
                restingCounter = 120;
            }
        }

        int regularCounter = 120;
        int chargeCounter = 0;
        int restingCounter = 0;
        float offsetDirection = 0f;
        Vector2 targetOffset;
        bool start = true;
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

            if (start)
            {
                start = false;
                targetOffset = NPC.DirectionFrom(player.Center) * 300;
            }

            if (charging)
            {
                NPC.rotation = 0f;
                chargeCounter--;
                if (chargeCounter <= 0)
                {
                    charging = false;
                    resting = true;
                    restingCounter = 60;
                }
            }
            else if (resting)
            {
                NPC.velocity *= 0.95f;
                restingCounter--;
                if (restingCounter <= 0)
                {
                    resting = false;
                    targetOffset = NPC.DirectionFrom(player.Center) * 300;
                }
            }
            else
            {
                NPC.spriteDirection = 1;
                if (player.Center.X < NPC.Center.X)
                {
                    NPC.spriteDirection = -1;
                }
                float speed = 0.4f;
                NPC.velocity += NPC.DirectionTo(player.Center + targetOffset) * speed;
                NPC.velocity *= 0.95f;
                NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 2);

                if (NPC.Distance(player.Center) <= 600)
                {
                    regularCounter--;
                    if (regularCounter <= 0)
                    {
                        SoundExtensions.PlaySoundOld(SoundID.ScaryScream, (int)NPC.Center.X, (int)NPC.Center.Y, 2, 1f, 0.5f);
                        charging = true;
                        regularCounter = 240;
                        chargeCounter = 45;
                        speed = 10f;
                        NPC.velocity = NPC.DirectionTo(player.Center) * speed;
                    }
                }
            }
        }

        public void moveTowardsPosition(float speed, float velocityMultiplier, Vector2 position)
        {
            Vector2 direction = NPC.DirectionTo(position);
            velocity += speed * direction;
            velocity *= velocityMultiplier;
        }
    }
}