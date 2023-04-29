using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Events;
using System.Collections.Generic;
using Terraria.WorldBuilding;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using TerrorbornMod.Utils;

namespace TerrorbornMod.NPCs.Bosses
{
    [AutoloadBossHead]
    class Dunestock : ModNPC
    {
        int FrameWait = 10;
        int frame = 1;
        bool Flying = false;
        bool start = true;

        public Vector2 findTileUnderPosition(int positionX, int positionY)
        {
            return new Vector2(positionX, positionY).FindGroundUnder();
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dunestock");
            Main.npcFrameCount[NPC.type] = 12;
            NPCID.Sets.TrailCacheLength[NPC.type] = 8;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                PortraitPositionYOverride = 0,
                CustomTexturePath = "TerrorbornMod/NPCs/Bosses/Dunestock_Bestiary"
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return Charging;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.defense = 13;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/BlightOfTheDunes");
            NPC.lifeMax = 7000;
            NPC.damage = 40;
            NPC.width = 100;
            NPC.height = 96;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.boss = true;
            NPC.HitSound = SoundID.NPCHit6;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 110000f;
            NPC.alpha = 250;
            NPC.knockBackResist = 0.00f;

            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(NPC);
            modNPC.BossTitle = "Dunestock";
            modNPC.BossSubtitle = "Amalgamation of the Dead";
            modNPC.BossDefeatTitle = "Supreme Undead";
            modNPC.BossTitleColor = Color.SandyBrown;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.defense = 19;
        }


        public override void OnKill()
        {
            bool spawnSS = !TerrorbornPlayer.modPlayer(Main.player[Main.myPlayer]).unlockedAbilities.Contains(5);
            for (int i = 0; i < 1000; i++)
            {
                Projectile Projectile = Main.projectile[i];
                if (Projectile.active && Projectile.type == ModContent.ProjectileType<Abilities.StarvingStorm>())
                {
                    spawnSS = false;
                }
            }

            if (spawnSS)
            {
                Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Abilities.StarvingStorm>(), 0, 0, Main.myPlayer);
            }

            if (!TerrorbornSystem.downedDunestock)
            {
                TerrorbornSystem.downedDunestock = true;
                Item.NewItem(NPC.GetSource_Loot(), NPC.Center, ModContent.ItemType<Items.Lore.JournalEntries.Raphael.Raphael_Dunestock>());
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Sandstorm,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

                new FlavorTextBestiaryInfoElement("Long ago a mysterious beast in the desert halted trade routes between kingdoms. Eventually, they sent some of their mightiest soldiers to take it out. They did not return.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.PermanentUpgrades.GoldenTooth>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.DunestockTrophy>(), 10));
            npcLoot.Add(ItemDropRule.ByCondition(new Terraria.GameContent.ItemDropRules.Conditions.IsExpert(), ModContent.ItemType<Items.TreasureBags.DS_TreasureBag>()));

            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Terraria.GameContent.ItemDropRules.Conditions.NotExpert());

            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<Items.Dunestock.NeedleClawStaff>(),
                ModContent.ItemType<Items.Dunestock.Dunesting>(),
                ModContent.ItemType<Items.Dunestock.HungryWhirlwind>()));

            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<Items.Equipable.Accessories.AntlionShell>(),
                ModContent.ItemType<Items.Equipable.Accessories.DryScarf>(),
                ModContent.ItemType<Items.Equipable.Accessories.AntlionClaw>(),
                ModContent.ItemType<Items.Equipable.Accessories.Wings.AntlionWings>()));
            npcLoot.Add(notExpertRule);

            npcLoot.Add(ItemDropRule.ByCondition(new Terraria.GameContent.ItemDropRules.Conditions.NotExpert(), ModContent.ItemType<Items.Equipable.Accessories.CloakOfTheWind>()));
            npcLoot.Add(ItemDropRule.ByCondition(new Terraria.GameContent.ItemDropRules.Conditions.NotExpert(), ModContent.ItemType<Items.Equipable.Vanity.BossMasks.DunestockMask>(), 7));
        }

        public override void FindFrame(int frameHeight)
        {
            FrameWait--;
            if (FrameWait <= 0)
            {
                frame++;
                if (!Flying)
                {
                    FrameWait = 5;
                }
                if (Flying)
                {
                    FrameWait = 5;
                }
                if (frame >= 4 && !Flying)
                {
                    frame = 0;
                }
                if (frame <= 8 && Flying)
                {
                    frame = 9;
                }
                if (frame >= 12 && Flying)
                {
                    frame = 9;
                }
                NPC.frame.Y = frame * frameHeight;
            }
        }

        Vector2 LineEnd;
        bool DrawLine = false;
        float NextAttackAngle;
        private void SetLine(float TimeOffset)
        {
            LineEnd = NPC.Center + NPC.AngleTo(Main.player[NPC.target].Center + (Main.player[NPC.target].velocity * TimeOffset)).ToRotationVector2() * 2200f;
            DrawLine = true;
            NextAttackAngle = NPC.AngleTo((Main.player[NPC.target].Center + (Main.player[NPC.target].velocity * TimeOffset)));
        }

        private void StopLine()
        {
            DrawLine = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                return true;
            }
            if (DrawLine)
            {
                Terraria.Utils.DrawLine(spriteBatch, NPC.Center, LineEnd, Color.LightYellow, Color.LightYellow, 3);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                return;
            }
            if (NPC.life > NPC.lifeMax * 0.40f)
            {
                return;
            }
            SpriteEffects effects = new SpriteEffects();
            if (NPC.direction == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(NPC.width / 2, NPC.height / 2);
            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                Vector2 drawPos = NPC.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY) + new Vector2(-2, 4);
                Color color = NPC.GetAlpha(Color.White) * ((float)(NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/DunestockEyesTrail"), drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
            }
        }

        public void shootBurstOfProjectiles(int numProjectiles, float speedX, float speedY, int Type, Vector2 position, int Damage, int knockback)
        {
            for (int i = 0; i < numProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15)); // 15 degree spread.
                float scale = 1f - (Main.rand.NextFloat() * .3f);
                perturbedSpeed = perturbedSpeed * scale;
                int num54 = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), position, perturbedSpeed, Type, Damage, 0f, 0);
            }
        }

        float DeathAnimVelocityX = 0;
        int InvincibilityCounter = 300;

        //Grounded Variables
        int WarningParticlesCounter = 0;
        int AIPhase = 0;
        int LastAI = 0;
        int PhaseTimeLeft = 180;
        int BurstWait = 30;
        int ProjectilesLeft;
        int ProjectileWait;
        int NoGravNeedles = 5;
        int AttacksUntilFlight;
        float NeedleSpeed = 0;
        List<int> NextAttacks = new List<int>();

        //Flight variables
        int AttacksUntilLand;
        bool Charging = false;
        int Direction = 0;
        float BarageDirection = 0;
        Vector2 MovementTarget;

        float predictMultiplier = 0f;
        int phaseTimeLeftMax;

        int pullBackTime = 0;
        public override void AI()
        {
            NPC.TargetClosest(true);
            if (Charging)
            {
                int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GoldFlame, Scale: 2.5f);
                Main.dust[dust].velocity = NPC.velocity;
                Main.dust[dust].noGravity = true;
            }
            phaseTimeLeftMax = 280 - 220 * (NPC.lifeMax - NPC.life) / (int)(NPC.lifeMax * 0.60f);
            Sandstorm.TimeLeft = 60;
            if (start)
            {
                Sandstorm.StartSandstorm();
                start = false;
                NPC.position.X = Main.player[NPC.target].position.X - NPC.width / 2;
                NPC.position.Y = Main.player[NPC.target].position.Y - 200;
                AttacksUntilFlight = Main.rand.Next(3, 6);
            }
            NPC.noGravity = Flying;
            NPC.noTileCollide = Flying;
            if (!Main.player[NPC.target].ZoneDesert)
            {
                if (InvincibilityCounter <= 0)
                {
                    NPC.dontTakeDamage = true;
                }
                else
                {
                    InvincibilityCounter--;
                }
            }
            else
            {
                InvincibilityCounter = 300;
                NPC.dontTakeDamage = false;
            }
            if (Main.player[NPC.target].Distance(NPC.Center) > 1500)
            {
                pullBackTime = 120;
            }
            if (Main.player[NPC.target].Distance(NPC.Center) < 500)
            {
                pullBackTime = 0;
            }
            if (pullBackTime > 0)
            {
                pullBackTime--;
                int SuckSpeed = 12;
                if (Main.player[NPC.target].Center.X > NPC.Center.X)
                {
                    Main.player[NPC.target].position.X -= SuckSpeed;
                    int newDust = Dust.NewDust(Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height, DustID.GoldFlame, -50, 0, Scale: 1.75f);
                    Main.dust[newDust].noGravity = true;
                }
                else
                {
                    Main.player[NPC.target].position.X += SuckSpeed;
                    int newDust = Dust.NewDust(Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height, DustID.GoldFlame, 50, 0, Scale: 1.75f);
                    Main.dust[newDust].noGravity = true;
                }
            }
            if (!Main.player[NPC.target].active || Main.player[NPC.target].dead)
            {
                Flying = true;
                NPC.velocity.Y -= 0.1f;
                NPC.velocity.X = DeathAnimVelocityX;
                if (NPC.position.Y <= Main.player[NPC.target].position.Y - 1500)
                {
                    NPC.active = false;
                }
            }
            else
            {
                if (Main.player[NPC.target].Center.X > NPC.Center.X)
                {
                    NPC.spriteDirection = 1;
                }
                else
                {
                    NPC.spriteDirection = -1;
                }

                if (Flying)
                {
                    NPC.ai[1] = 1;
                    if (!NPC.AnyNPCs(ModContent.NPCType<Dunestock_Lower>()) && AIPhase < 9)
                    {
                        NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.Center.X, (int)NPC.Center.Y + NPC.width / 4, ModContent.NPCType<Dunestock_Lower>());
                    }
                }
                else
                {
                    NPC.ai[1] = 0;
                    NPC.velocity.X *= 0.93f;
                }
                DeathAnimVelocityX = NPC.velocity.X;
                if (NPC.alpha > 0)
                {
                    NPC.alpha -= 15;
                }
                if (NPC.alpha < 0)
                {
                    NPC.alpha = 0;
                }
                if (AIPhase == 0)
                {
                    PhaseTimeLeft--;
                    WarningParticlesCounter--;
                    if (PhaseTimeLeft <= 31 && WarningParticlesCounter <= 0)
                    {
                        DustExplosion(NPC.Center, 0, 50, 20, 60, DustScale: 2f, NoGravity: true);
                        WarningParticlesCounter = 10;
                    }
                    if (PhaseTimeLeft <= 0)
                    {
                        if (NextAttacks.Count <= 0)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                int choice = Main.rand.Next(1, 5);
                                while (NextAttacks.Contains(choice))
                                {
                                    choice = Main.rand.Next(1, 5);
                                }
                                NextAttacks.Add(choice);
                            }
                        }
                        AIPhase = NextAttacks[0];
                        NextAttacks.RemoveAt(0);
                        if (AIPhase == 1)
                        {
                            BurstWait = 32 + (NPC.life / 700);
                            if (!Main.expertMode)
                            {
                                BurstWait = 30 + (NPC.life / 700);
                            }
                            SetLine(BurstWait);
                            SoundExtensions.PlaySoundOld(SoundID.Item103, NPC.Center);
                            ProjectilesLeft = Main.rand.Next(15, 30);
                            ProjectileWait = 2;
                        }
                        if (AIPhase == 2)
                        {
                            ProjectileWait = 5;
                            ProjectilesLeft = 24;
                            NoGravNeedles = 10;
                            NeedleSpeed = 1;
                            predictMultiplier = 0f;
                        }
                        if (AIPhase == 3)
                        {
                            ProjectileWait = 30;
                            ProjectilesLeft = 3;
                        }
                        if (AIPhase == 4)
                        {
                            ProjectileWait = 5;
                            ProjectilesLeft = 25;
                            NeedleSpeed = 1;
                        }
                        LastAI = AIPhase;
                        AttacksUntilFlight--;
                    }
                    else
                    {
                        if (AttacksUntilFlight <= 0)
                        {
                            Flying = true;
                            NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.Center.X, (int)NPC.Center.Y + NPC.width / 4, ModContent.NPCType<Dunestock_Lower>());
                            AttacksUntilLand = 5;
                            AIPhase = 5;
                            PhaseTimeLeft = 240;
                            ProjectileWait = 30;
                        }
                    }
                }
                if (AIPhase == 1) //Attack where it aims where the player will be before firing sand bolts.
                {
                    if (BurstWait <= 0)
                    {
                        StopLine();
                        ProjectileWait--;
                        if (ProjectileWait <= 0)
                        {
                            ProjectilesLeft--;
                            if (ProjectilesLeft <= 0)
                            {
                                AIPhase = 0;
                                PhaseTimeLeft = phaseTimeLeftMax;
                            }

                            ProjectileWait = 2;
                            SoundExtensions.PlaySoundOld(SoundID.Item42, NPC.Center);
                            float ProjSpeed = 3f;

                            int damage = 15;
                            int type = ModContent.ProjectileType<SandBolt>();
                            float rotation = NextAttackAngle;
                            Vector2 rotationVector = rotation.ToRotationVector2() * ProjSpeed;
                            rotationVector = rotationVector.RotatedByRandom(MathHelper.ToRadians(10));
                            Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center.X, NPC.Center.Y, rotationVector.X, rotationVector.Y, type, damage, 0f, 0);
                        }
                    }
                    else
                    {
                        BurstWait--;
                    }
                }
                if (AIPhase == 2) //Needle barrage.
                {
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        ProjectileWait = 8;
                        ProjectilesLeft--;
                        if (ProjectilesLeft <= 0)
                        {
                            AIPhase = 0;
                            PhaseTimeLeft = phaseTimeLeftMax;
                        }
                        SoundExtensions.PlaySoundOld(SoundID.Item42, NPC.Center);
                        NeedleSpeed++;
                        float realProjSpeed = NeedleSpeed;
                        NoGravNeedles--;
                        int damage = 20;
                        int type = ModContent.ProjectileType<TumblerNeedle>();
                        float rotation = NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation();

                        if (NoGravNeedles <= 0)
                        {
                            rotation = NPC.DirectionTo(Main.player[NPC.target].Center + (NPC.Distance(Main.player[NPC.target].Center) / realProjSpeed) * Main.player[NPC.target].velocity * predictMultiplier).ToRotation();
                        }
                        else
                        {
                            realProjSpeed /= 2;
                        }
                        Vector2 speed = (rotation.ToRotationVector2() * realProjSpeed).RotatedByRandom(MathHelper.ToRadians(10));

                        if (NoGravNeedles <= 0)
                        {
                            NoGravNeedles = 3;
                            predictMultiplier += 1f / 3f;
                            Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center.X, NPC.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 0, 240);
                        }
                        else
                        {
                            Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center.X, NPC.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 1, 180);
                        }
                    }
                }
                if (AIPhase == 3) //Air blast but now
                {
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        SoundExtensions.PlaySoundOld(SoundID.Item71, NPC.Center);
                        ProjectilesLeft--;
                        ProjectileWait = 30;
                        float ProjSpeed = 5;
                        if (ProjectilesLeft == 1)
                        {
                            ProjSpeed = 10;
                        }
                        if (ProjectilesLeft == 0)
                        {
                            ProjSpeed = 15;
                            AIPhase = 0;
                            PhaseTimeLeft = phaseTimeLeftMax;
                        }
                        Vector2 rotation = NPC.DirectionTo(Main.player[NPC.target].Center);
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, rotation * ProjSpeed, ModContent.ProjectileType<WindBlast>(), 60 / 2, 0);
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, (rotation * ProjSpeed).RotatedBy(MathHelper.ToRadians(-50)), ModContent.ProjectileType<WindBlast>(), 60 / 2, 0);
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, (rotation * ProjSpeed).RotatedBy(MathHelper.ToRadians(50)), ModContent.ProjectileType<WindBlast>(), 60 / 2, 0);
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, (rotation * ProjSpeed).RotatedBy(MathHelper.ToRadians(-25)), ModContent.ProjectileType<WindBlast>(), 60 / 2, 0);
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, (rotation * ProjSpeed).RotatedBy(MathHelper.ToRadians(25)), ModContent.ProjectileType<WindBlast>(), 60 / 2, 0);
                    }
                }
                if (AIPhase == 4) //Blood spew
                {
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        ProjectileWait = 5;
                        ProjectilesLeft--;
                        if (ProjectilesLeft <= 0)
                        {
                            AIPhase = 0;
                            PhaseTimeLeft = phaseTimeLeftMax;
                        }
                        SoundExtensions.PlaySoundOld(SoundID.Item42, NPC.Center);
                        NeedleSpeed++;
                        int damage = 20;
                        int type = ModContent.ProjectileType<ClotBlood>();
                        Vector2 rotation = NPC.DirectionTo(Main.player[NPC.target].Center);
                        Vector2 SpeedVector = (rotation * NeedleSpeed).RotatedByRandom(MathHelper.ToRadians(15));
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center.X, NPC.Center.Y, SpeedVector.X, SpeedVector.Y, type, damage, 0f, 0);
                    }
                }
                if (AIPhase == 5) //Fly towards the player
                {
                    float maxSpeed = 12;
                    if (NPC.Center.X > Main.player[NPC.target].Center.X && NPC.velocity.X > -maxSpeed)
                    {
                        NPC.velocity.X -= 0.15f + (NPC.Center.X - Main.player[NPC.target].Center.X) / 3500; //Moves faster the farther the player is from the boss.
                        if (NPC.velocity.X > 10f)
                        {
                            NPC.velocity.X -= 0.9f;
                        }
                    }
                    if (NPC.Center.X < Main.player[NPC.target].Center.X && NPC.velocity.X < maxSpeed)
                    {
                        NPC.velocity.X += 0.15f + (Main.player[NPC.target].Center.X - NPC.Center.X) / 3500;
                        if (NPC.velocity.X < -10f)
                        {
                            NPC.velocity.X += 0.9f;
                        }
                    }
                    if (NPC.Center.Y > Main.player[NPC.target].Center.Y)
                    {
                        NPC.velocity.Y -= 0.10f + (NPC.Center.Y - Main.player[NPC.target].Center.Y) / 3000;
                        if (NPC.velocity.Y > 7.5f)
                        {
                            NPC.velocity.Y -= 0.6f;
                        }
                    }
                    if (NPC.Center.Y < Main.player[NPC.target].Center.Y)
                    {
                        NPC.velocity.Y += 0.10f + (Main.player[NPC.target].Center.Y - NPC.Center.Y) / 3000;
                        if (NPC.velocity.Y < -7.5f)
                        {
                            NPC.velocity.Y += 0.6f;
                        }
                    }
                    NPC.velocity.X = NPC.velocity.X;
                    NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 1.5f);
                    NPC.velocity.Y *= 0.98f;
                    NPC.velocity.X *= 0.995f;
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        ProjectileWait = Main.rand.Next(20, 41);
                        SoundExtensions.PlaySoundOld(SoundID.Item42, NPC.Center);
                        float ProjSpeed = Main.rand.Next(15, 20);
                        int damage = 20;
                        int type = ModContent.ProjectileType<TumblerNeedle>();
                        Vector2 rotation = NPC.DirectionTo(Main.player[NPC.target].Center);
                        Vector2 speed = rotation * ProjSpeed;
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center.X, NPC.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 0, 200);
                    }
                    PhaseTimeLeft--;
                    if (PhaseTimeLeft <= 0)
                    {
                        AIPhase = 6;
                        PhaseTimeLeft = 50;
                        Charging = false;
                        ProjectileWait = 25;
                        if (NPC.Center.X > Main.player[NPC.target].Center.X)
                        {
                            Direction = -1;
                            MovementTarget = new Vector2(300, 0);
                        }
                        else
                        {
                            Direction = 1;
                            MovementTarget = new Vector2(-300, 0);
                        }
                    }
                }
                if (AIPhase == 6) //Horizontal charge
                {
                    NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 1.5f);
                    if (Charging)
                    {
                        NPC.velocity.X *= 0.95f;
                        NPC.velocity.Y = 0;
                        NPC.velocity.X += Direction;
                        ProjectileWait--;
                        if (NPC.velocity.X > 40)
                        {
                            NPC.velocity.X = 40;
                        }
                        if (NPC.velocity.X < -40)
                        {
                            NPC.velocity.X = -40;
                        }
                        if (ProjectileWait <= 0)
                        {
                            ProjectileWait = 25;
                            SoundExtensions.PlaySoundOld(SoundID.Item71, NPC.Center);
                            float ProjSpeed = 12;
                            int damage = 20;
                            int type = ModContent.ProjectileType<DuneClaw>();
                            Vector2 rotation = NPC.DirectionTo(Main.player[NPC.target].Center);
                            Vector2 SpeedVector = rotation * ProjSpeed;
                            Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center.X, NPC.Center.Y, SpeedVector.X, SpeedVector.Y, type, damage, 0f, 0);
                        }
                        PhaseTimeLeft--;
                        if (PhaseTimeLeft <= 0)
                        {
                            AIPhase = 7;
                            Charging = false;
                            ProjectileWait = 30;
                            ProjectilesLeft = Main.rand.Next(18, 32);
                            BarageDirection = NPC.DirectionTo(Main.player[NPC.target].Center).ToRotation();
                        }
                    }
                    else
                    {
                        NPC.TargetClosest(true);
                        Vector2 targetPosition = Main.player[NPC.target].Center + MovementTarget;
                        float speed = 4f;
                        Vector2 move = targetPosition - NPC.Center;
                        float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                        move *= speed / magnitude;
                        NPC.velocity.Y += move.Y;
                        NPC.velocity.X += move.X;
                        NPC.velocity.X *= 0.8f;
                        NPC.velocity.Y *= 0.8f;
                        if (NPC.Distance(targetPosition) <= 50)
                        {
                            Charging = true;
                        }
                    }
                    NPC.velocity.X = NPC.velocity.X;
                    NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 1.5f);
                }
                if (AIPhase == 7) //Airborn needle barrage
                {
                    NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 1.5f);
                    NPC.velocity.X = NPC.velocity.X;
                    NPC.velocity.X *= 0.93f;
                    NPC.velocity.Y *= 0.93f;
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        ProjectileWait = Main.rand.Next(1, 4);
                        ProjectilesLeft--;
                        if (ProjectilesLeft <= 0)
                        {

                            MovementTarget = Main.player[NPC.target].Center;
                            while (!WorldUtils.Find(MovementTarget.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                                {
        new Terraria.WorldBuilding.Conditions.IsSolid()
                                }), out _))
                            {
                                MovementTarget.Y++;
                            }
                            MovementTarget.Y -= NPC.height;
                            AIPhase = 8;
                        }
                        SoundExtensions.PlaySoundOld(SoundID.Item42, NPC.Center);
                        float ProjSpeed = Main.rand.Next(20, 29);
                        int damage = 20;
                        int type = ModContent.ProjectileType<TumblerNeedle>();
                        float rotation = BarageDirection;
                        Vector2 speed = BarageDirection.ToRotationVector2() * ProjSpeed;
                        NoGravNeedles = Main.rand.Next(4, 7);
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center.X, NPC.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 0, 60);
                    }
                }
                if (AIPhase == 8) //Fly to the land under the player
                {
                    MovementTarget = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Dunestock_Lower>())].position;
                    MovementTarget.Y -= NPC.height / 2;
                    Vector2 move;
                    NPC.TargetClosest(true);
                    float speed = 14f;
                    move = MovementTarget - NPC.Center;
                    float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                    move *= speed / magnitude;
                    NPC.velocity = move;
                    float Xdif = MovementTarget.X - (NPC.position.X + NPC.width / 2);
                    float Ydif = MovementTarget.Y - (NPC.position.Y + NPC.height / 2);
                    float distanceToTarget = (float)Math.Sqrt((Xdif * Xdif) + (Ydif * Ydif));
                    if (Math.Abs(distanceToTarget) <= speed)
                    {
                        AIPhase = 0;
                        PhaseTimeLeft = phaseTimeLeftMax;
                        Flying = false;
                        AttacksUntilFlight = 5;
                    }
                }
                if (NPC.life <= NPC.lifeMax * 0.40f && AIPhase <= 8)
                {
                    TerrorbornSystem.ScreenShake(75f);
                    SoundExtensions.PlaySoundOld(SoundID.NPCDeath10, NPC.Center);
                    DustExplosion(NPC.Center, 0, 50, 40, 60, DustScale: 2f, NoGravity: true);
                    Flying = true;
                    AIPhase = 9;
                    PhaseTimeLeft = 240;
                    ProjectileWait = 30;
                    StopLine();
                    Main.NewText("The wind accelerates around you...", 238, 45, 45);
                    Music = Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/DreadInTheDustwind");
                }
                if (AIPhase == 9) //Phase two flying
                {
                    float maxSpeed = 15;
                    if (NPC.Center.X > Main.player[NPC.target].Center.X && NPC.velocity.X > -maxSpeed)
                    {
                        NPC.velocity.X -= 0.16f + (NPC.Center.X - Main.player[NPC.target].Center.X) / 3500; //Moves faster the farther the player is from the boss.
                        if (NPC.velocity.X > 10f)
                        {
                            NPC.velocity.X -= 0.9f;
                        }
                    }
                    if (NPC.Center.X < Main.player[NPC.target].Center.X && NPC.velocity.X < maxSpeed)
                    {
                        NPC.velocity.X += 0.16f + (Main.player[NPC.target].Center.X - NPC.Center.X) / 3500;
                        if (NPC.velocity.X < -10f)
                        {
                            NPC.velocity.X += 0.9f;
                        }
                    }
                    if (NPC.Center.Y > Main.player[NPC.target].Center.Y)
                    {
                        NPC.velocity.Y -= 0.11f + (NPC.Center.Y - Main.player[NPC.target].Center.Y) / 3000;
                        if (NPC.velocity.Y > 7.5f)
                        {
                            NPC.velocity.Y -= 0.6f;
                        }
                    }
                    if (NPC.Center.Y < Main.player[NPC.target].Center.Y)
                    {
                        NPC.velocity.Y += 0.11f + (Main.player[NPC.target].Center.Y - NPC.Center.Y) / 3000;
                        if (NPC.velocity.Y < -7.5f)
                        {
                            NPC.velocity.Y += 0.6f;
                        }
                    }
                    NPC.velocity.X = NPC.velocity.X;
                    NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 1.5f);
                    NPC.velocity.Y *= 0.98f;
                    NPC.velocity.X *= 0.995f;
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        ProjectileWait = 45;
                        SoundExtensions.PlaySoundOld(SoundID.Item42, NPC.Center);
                        float ProjSpeed = 15;
                        int damage = 20;
                        int type = ModContent.ProjectileType<TumblerNeedle>();
                        Vector2 rotation = NPC.DirectionTo(Main.player[NPC.target].Center + (NPC.Distance(Main.player[NPC.target].Center) / ProjSpeed) * Main.player[NPC.target].velocity);
                        Vector2 speed = (rotation * ProjSpeed).RotatedByRandom(MathHelper.ToRadians(10));
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center.X, NPC.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 0, 200);
                    }
                    PhaseTimeLeft--;
                    if (PhaseTimeLeft <= 0)
                    {
                        AIPhase = 10;
                        PhaseTimeLeft = 45;
                        Charging = false;
                        ProjectileWait = 15;
                        if (NPC.Center.X > Main.player[NPC.target].Center.X)
                        {
                            Direction = -1;
                            MovementTarget = new Vector2(400, 0);
                        }
                        else
                        {
                            Direction = 1;
                            MovementTarget = new Vector2(-400, 0);
                        }
                    }
                }
                if (AIPhase == 10) //Phase two charge
                {
                    NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 1.5f);
                    if (Charging)
                    {
                        NPC.velocity.X *= 0.95f;
                        NPC.velocity.Y = 0;
                        NPC.velocity.X += Direction * 1.5f;
                        ProjectileWait--;
                        if (NPC.velocity.X > 55)
                        {
                            NPC.velocity.X = 55;
                        }
                        if (NPC.velocity.X < -55)
                        {
                            NPC.velocity.X = -55;
                        }
                        if (ProjectileWait <= 0)
                        {
                            ProjectileWait = 15;
                            SoundExtensions.PlaySoundOld(SoundID.Item71, NPC.Center);
                            float ProjSpeed = 14;
                            int damage = 20;
                            int type = ModContent.ProjectileType<DuneClaw>();
                            Vector2 rotation = NPC.DirectionTo(Main.player[NPC.target].Center);
                            Vector2 SpeedVector = rotation * ProjSpeed;
                            Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center.X, NPC.Center.Y, SpeedVector.X, SpeedVector.Y, type, damage, 0f, 0);
                        }
                        PhaseTimeLeft--;
                        if (PhaseTimeLeft <= 0)
                        {
                            AIPhase = 11;
                            PhaseTimeLeft = 180;
                            Charging = false;
                        }
                    }
                    else
                    {
                        NPC.TargetClosest(true);
                        Vector2 targetPosition = Main.player[NPC.target].Center + MovementTarget;
                        float speed = 4f;
                        Vector2 move = targetPosition - NPC.Center;
                        float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                        move *= speed / magnitude;
                        NPC.velocity.Y += move.Y;
                        NPC.velocity.X += move.X;
                        NPC.velocity.X *= 0.8f;
                        NPC.velocity.Y *= 0.8f;
                        if (NPC.Distance(targetPosition) <= 50)
                        {
                            Charging = true;
                        }
                    }
                    NPC.velocity.X = NPC.velocity.X;
                    NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 1.5f);
                }
                if (AIPhase == 11) //Clot bomb rain
                {
                    NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 1.5f);
                    MovementTarget = new Vector2(0, -300);
                    NPC.TargetClosest(true);
                    Vector2 targetPosition = Main.player[NPC.target].Center + MovementTarget;
                    float speed = 2.5f;
                    Vector2 move = targetPosition - NPC.Center;
                    float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                    move *= speed / magnitude;
                    if (NPC.Distance(Main.player[NPC.target].Center + MovementTarget) >= 45)
                    {
                        NPC.velocity.Y += move.Y;
                        NPC.velocity.X += move.X;
                    }
                    NPC.velocity.X *= 0.9f;
                    NPC.velocity.Y *= 0.9f;
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        ProjectileWait = Main.rand.Next(7, 27);
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), new Vector2(NPC.position.X + Main.rand.Next(NPC.width + 1), NPC.position.Y + Main.rand.Next(NPC.height + 1)), new Vector2(0, 0), ModContent.ProjectileType<ClotBomb>(), 30, 0);
                    }
                    NPC.velocity.X = NPC.velocity.X;
                    PhaseTimeLeft--;
                    if (PhaseTimeLeft <= 0)
                    {
                        PhaseTimeLeft = 420;
                        AIPhase = 12;
                        ProjectileWait = 60;
                        ProjectilesLeft = 4;
                        MovementTarget = Main.player[NPC.target].Center;
                    }
                }
                if (AIPhase == 12) //Tornado
                {
                    NPC.rotation = MathHelper.ToRadians(NPC.velocity.X * 1.5f);
                    NPC.velocity.X *= 0.9f;
                    NPC.velocity.Y *= 0.9f;
                    ProjectileWait--;
                    if (ProjectileWait <= 0 && ProjectilesLeft > 0)
                    {
                        ProjectilesLeft--;
                        ProjectileWait = 15;
                        if (ProjectilesLeft == 0)
                        {
                            ProjectileWait = 45;
                            SoundExtensions.PlaySoundOld(SoundID.NPCDeath10, NPC.Center);
                            DustExplosion(NPC.Center, 0, 50, 40, 60, DustScale: 2f, NoGravity: true);
                            while (!WorldUtils.Find(MovementTarget.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                                {
        new Terraria.WorldBuilding.Conditions.IsSolid()
                                }), out _))
                            {
                                MovementTarget.Y++;
                            }
                            Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), MovementTarget, Vector2.Zero, ModContent.ProjectileType<SandnadoSpawn>(), 30, 0);
                        }
                        else
                        {
                            DustExplosion(NPC.Center, 0, 50, 20, 60, DustScale: 2f, NoGravity: true);
                        }
                    }
                    if (ProjectileWait <= 0 && ProjectilesLeft <= 0)
                    {
                        SoundExtensions.PlaySoundOld(SoundID.Item71, NPC.Center);
                        ProjectileWait = 60;
                        float speed = 20;
                        Vector2 velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * speed;
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity, ModContent.ProjectileType<WindBlast>(), 60 / 4, 0);
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity.RotatedBy(MathHelper.ToRadians(-60)), ModContent.ProjectileType<WindBlast>(), 60 / 4, 0);
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity.RotatedBy(MathHelper.ToRadians(60)), ModContent.ProjectileType<WindBlast>(), 60 / 4, 0);
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity.RotatedBy(MathHelper.ToRadians(-30)), ModContent.ProjectileType<WindBlast>(), 60 / 4, 0);
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity.RotatedBy(MathHelper.ToRadians(30)), ModContent.ProjectileType<WindBlast>(), 60 / 4, 0);
                    }
                    NPC.velocity.X = NPC.velocity.X;

                    PhaseTimeLeft--;
                    if (PhaseTimeLeft <= 0)
                    {
                        AIPhase = 9;
                        PhaseTimeLeft = 240;
                        ProjectileWait = 30;
                    }
                }
            }
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }
    }
    class Dunestock_Lower : ModNPC
    {
        public override string Texture => "TerrorbornMod/NPCs/Bosses/Dunestock_Lower";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Dunestock");
            Main.npcFrameCount[NPC.type] = 7;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.defense = 999;
            NPC.lifeMax = 7000;
            NPC.damage = 0;
            NPC.width = 60;
            NPC.height = 44;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit6;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 110000f;
            NPC.knockBackResist = 0.00f;
            NPC.hide = false;
            NPC.chaseable = false;
        }
        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }
        public override bool? CanBeHitByProjectile(Projectile Projectile)
        {
            return false;
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            damage = 0;
            crit = false;
        }
        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            damage = 0;
            crit = false;
        }
        int frame = 0;
        int FrameWait = 0;
        private void WalkingAnimation(int FrameSpeed)
        {
            FrameWait--;
            if (FrameWait <= 0)
            {
                FrameWait = FrameSpeed;
                frame++;
            }
            if (frame > 5 || frame < 1)
            {
                frame = 1;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frame * frameHeight;
        }
        bool Start = true;
        int AIPhase = 0;
        int PhaseCounter = 60;
        int JumpDir;
        int NeedleCounter = 60;
        public override void AI()
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Dunestock>()))
            {
                int MainBoss = NPC.FindFirstNPC(ModContent.NPCType<Dunestock>());
                if (Main.npc[MainBoss].life <= Main.npc[MainBoss].lifeMax * 0.40f)
                {
                    NPC.active = false;
                }
                if (Main.npc[MainBoss].ai[1] == 0 || Main.player[NPC.target].dead)
                {
                    NPC.active = false;
                }
                if (Start)
                {
                    Start = false;
                }
                NPC.life = Main.npc[MainBoss].life;
                if (AIPhase == 0) //Standing still
                {
                    NPC.TargetClosest(true);
                    frame = 0;
                    PhaseCounter--;
                    if (PhaseCounter <= 0)
                    {
                        PhaseCounter = Main.rand.Next(120, 300);
                        AIPhase = 1;
                    }
                }
                if (AIPhase == 1)
                {
                    if (Main.player[NPC.target].Center.Y > NPC.position.Y + 30 && NPC.velocity.Y == 0)
                    {
                        NPC.position.Y++;
                    }
                    NPC.TargetClosest(true);
                    float WalkSpeed = 0.35f;
                    if (NPC.life <= NPC.lifeMax * 0.40f)
                    {
                        WalkSpeed = 0.45f;
                    }
                    WalkingAnimation(4);
                    if (NPC.Center.X <= Main.player[NPC.target].Center.X)
                    {
                        NPC.velocity.X += WalkSpeed;
                    }
                    else
                    {
                        NPC.velocity.X -= WalkSpeed;
                    }
                    PhaseCounter--;
                    if (PhaseCounter <= 0 && NPC.velocity.Y == 0)
                    {
                        if (Main.player[NPC.target].Center.X >= NPC.Center.X)
                        {
                            JumpDir = 1;
                        }
                        else
                        {
                            JumpDir = -1;
                        }
                        NPC.velocity.X = 10 * JumpDir;
                        NPC.velocity.Y = -14;
                        AIPhase = 2;
                    }
                }
                if (AIPhase == 2)
                {
                    frame = 6;
                    NPC.TargetClosest(false);
                    float speed = 0.5f;
                    if (Main.player[NPC.target].Center.X >= NPC.Center.X)
                    {
                        NPC.velocity.X += speed;
                        if (JumpDir == -1 && Main.player[NPC.target].Center.Y > NPC.position.Y + NPC.height && NPC.life <= NPC.lifeMax * 0.75f)
                        {
                            NPC.velocity.Y += 1;
                        }
                    }
                    else
                    {
                        NPC.velocity.X -= speed;
                        if (JumpDir == 1 && Main.player[NPC.target].Center.Y > NPC.position.Y + NPC.height && NPC.life <= NPC.lifeMax * 0.75f)
                        {
                            NPC.velocity.Y += 1;
                        }
                    }
                    if (NPC.velocity.Y == 0)
                    {
                        AIPhase = 0;
                        PhaseCounter = Main.rand.Next(45, 75);
                    }
                }
            }
            else
            {
                NPC.active = false;
            }
        }
    }
    class SandBolt : ModProjectile
    {
        public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.hide = true;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 1500;
        }
        int DustWait = 5;
        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 ProjectilePosition = Projectile.position;
                    ProjectilePosition -= Projectile.velocity * ((float)i * 0.25f);
                    Projectile.alpha = 255;
                    DustWait--;
                    if (DustWait <= 0)
                    {
                        int dust = Dust.NewDust(ProjectilePosition, Projectile.width, Projectile.height, 0, 0);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].position = ProjectilePosition;
                        DustWait = 5;
                        Main.dust[dust].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                        Main.dust[dust].velocity *= 0.2f;
                    }
                }
            }
        }
    }
    class TumblerNeedle : ModProjectile
    {
        bool Stick = false;
        int trueTimeleft = 235;
        float telegraphAlpha = 0f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] != 1 && !Stick && Projectile.ai[1] > 0)
            {
                Color color = Color.LightYellow;
                color.A = (int)(255 * 0.75f);
                Terraria.Utils.DrawLine(Main.spriteBatch, Projectile.Center, Projectile.Center + Projectile.velocity.ToRotation().ToRotationVector2() * 2200f, color * telegraphAlpha, Color.Transparent, 3);
            }
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 12000;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Dunestock>()))
            {
                fallThrough = Main.player[Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Dunestock>())].target].position.Y - 30 > Projectile.position.Y || Projectile.ai[0] == 0;
            }
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Stick = true;
            return false;
        }
        public override void AI()
        {
            if (telegraphAlpha < 1)
            {
                telegraphAlpha += 0.02f;
            }
            if (Projectile.timeLeft == 12000)
            {
                Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15));
            }
            if (Projectile.ai[1] <= 0)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha >= 255)
                {
                    Projectile.active = false;
                }
            }
            if (Stick)
            {
                Projectile.velocity *= 0;
                Projectile.ai[1]--;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
                if (Projectile.ai[0] == 1 && !Stick)
                {
                    Projectile.velocity.Y += 0.3f;
                }
            }
        }
    }
    class Tumbleweed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        int trueTimeleft = 180;
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 12000;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = Main.player[Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Dunestock>())].target].position.Y - 30 > Projectile.position.Y;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            SoundExtensions.PlaySoundOld(SoundID.Run, Projectile.Center);
            return false;
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.25f;
            trueTimeleft--;
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);
            if (trueTimeleft <= 0)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha >= 255)
                {
                    Projectile.active = false;
                }
            }
        }
    }
    class DuneClaw : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        int CollideCounter = 0;
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 150;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            //SoundExtensions.PlaySoundOld(SoundID.Run, Projectile.Center);
            CollideCounter += 1;
            if (CollideCounter >= 5)
            {
                Projectile.timeLeft = 0;
            }
            return false;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }
        public override void Kill(int timeLeft)
        {
            if (timeLeft <= 0)
            {
                Player targetPlayer = Main.player[Player.FindClosest(Projectile.Center, Projectile.width, Projectile.height)];
                if (Main.expertMode)
                {
                    for (int i = 0; i < Main.rand.Next(4,6); i++)
                    {
                        SoundExtensions.PlaySoundOld(SoundID.Item42, Projectile.Center);
                        float ProjSpeed = 15f;
                        int damage = 35 / 2; // /2 undoes the expert increase
                        int type = ModContent.ProjectileType<TumblerNeedle>();
                        float rotation = Projectile.DirectionTo(targetPlayer.Center).ToRotation();
                        Vector2 speed = (rotation.ToRotationVector2() * ProjSpeed).RotatedByRandom(MathHelper.ToRadians(6));
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 0, 180);
                    }
                    Projectile.active = false;
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        SoundExtensions.PlaySoundOld(SoundID.Item42, Projectile.Center);
                        float ProjSpeed = 10f;
                        int damage = 20;
                        int type = ModContent.ProjectileType<TumblerNeedle>();
                        float rotation = Projectile.DirectionTo(targetPlayer.Center).ToRotation();
                        Vector2 speed = (rotation.ToRotationVector2() * ProjSpeed).RotatedByRandom(MathHelper.ToRadians(3));
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 0, 120);
                    }
                    Projectile.active = false;
                }
                
            }
        }
    }
 
    class ClotBomb : ModProjectile
    {
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            damage = (int)(damage * 0.75f);
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }
        int RotationDirection = 0;
        public override void AI()
        {
            if (Projectile.timeLeft == 599)
            {
                if (Main.rand.NextBool(2))
                {
                    RotationDirection = 1;
                }
                else
                {
                    RotationDirection = -1;
                }
            }
            Projectile.rotation += MathHelper.ToRadians(5 * RotationDirection);
            Projectile.velocity.Y += 0.5f;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = Main.player[Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Dunestock>())].target].position.Y - 30 > Projectile.position.Y;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                float Speed = Main.rand.Next(7, 10);
                Vector2 ProjectileSpeed = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * Speed;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, ProjectileSpeed, ModContent.ProjectileType<ClotBlood>(), Projectile.damage / 2, 0);
            }
        }
    }
    class ClotBlood : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            Projectile.hide = true;
            Projectile.timeLeft = 30;
        }
        public override string Texture => "TerrorbornMod/placeholder";
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity;
            int targetPlayer = Player.FindClosest(Projectile.position, Projectile.width, Projectile.height);
            //HOME IN
            float speed = .35f;
            Vector2 move = Main.player[targetPlayer].Center - Projectile.Center;
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            move *= speed / magnitude;
            Projectile.velocity += move;
        }
    }
    class SandnadoSpawn : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            Projectile.hide = true;
            Projectile.timeLeft = 420;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override string Texture => "TerrorbornMod/placeholder";
        int Counter = 0;
        int NumPlaced = 0;
        public override void AI()
        {
            Counter--;
            if (Counter <= 0)
            {
                Counter = 5;
                if (NumPlaced <= 8)
                {
                    int num54 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.position.Y - 7 - (38 * NumPlaced/* * (1 + (NumPlaced * .18f))*/)), new Vector2(0, 0), ModContent.ProjectileType<Sandnado>(), Projectile.damage, Projectile.knockBack, Projectile.owner, NumPlaced);
                }
                NumPlaced++;
            }
            int SuckSpeed = 2;
            Player targetPlayer = Main.player[Player.FindClosest(Projectile.Center, 0, 0)];
            if (targetPlayer.Center.X > Projectile.Center.X)
            {
                targetPlayer.position.X -= SuckSpeed;
                int newDust = Dust.NewDust(targetPlayer.position, targetPlayer.width, targetPlayer.height, DustID.GoldFlame, -35, 0, Scale: 1.75f);
                Main.dust[newDust].noGravity = true;
            }
            else
            {
                targetPlayer.position.X += SuckSpeed;
                int newDust = Dust.NewDust(targetPlayer.position, targetPlayer.width, targetPlayer.height, DustID.GoldFlame, 35, 0, Scale: 1.75f);
                Main.dust[newDust].noGravity = true;
            }
        }

    }
    class Sandnado : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 10;
        }
        void FindFrame(int FrameHeight)
        {
            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frame++;
                Projectile.frameCounter = 1;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }
        public override void SetDefaults()
        {
            Projectile.width = 148;
            Projectile.height = 26;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.hostile = true;
            Projectile.damage = 0;
            Projectile.timeLeft = 10000;
            Projectile.alpha = 255;
        }
        int TrueTimeLeft = 300;
        float TornadoXVelocity = 10;
        int TornadoDirection = 1;
        public override void AI()
        {
            if ((TornadoXVelocity >= 10 && TornadoDirection == 1) || (TornadoXVelocity <= -10 && TornadoDirection == -1))
            {
                TornadoDirection *= -1;
                TornadoXVelocity += TornadoDirection;
            }
            else
            {
                TornadoXVelocity += TornadoDirection;
            }
            Projectile.velocity.X = TornadoXVelocity;

            FindFrame(Projectile.height);
            TrueTimeLeft--;
            if (TrueTimeLeft <= 0)
            {
                Projectile.alpha += 5;
                if (Projectile.alpha >= 255)
                {
                    Projectile.active = false;
                }
            }
            else
            {
                Projectile.alpha = 140 - (int)(Projectile.ai[0] * 15);
                Projectile.scale = 1.5f + (Projectile.ai[0] * .18f);
            }
        }
    }
    class WindBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 42;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }
    }
}
