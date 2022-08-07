using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;

namespace TerrorbornMod.NPCs.Bosses.TidalTitan
{
    class MysteriousCrab : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 17;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.width = 128;
            NPC.height = 78;
            NPC.damage = 0;
            NPC.HitSound = SoundID.NPCHit38;
            NPC.defense = 9;
            NPC.DeathSound = SoundID.NPCDeath41;
            Main.raining = true;
            NPC.frame.Width = 388;
            NPC.frame.Height = 254;
            NPC.lifeMax = 500;
            NPC.rarity = 10;
            NPC.knockBackResist = 0;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Ichor] = true;
            NPC.buffImmune[BuffID.CursedInferno] = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = 300;
            
        }

        public override void AI()
        {
            if (NPC.wet)
            {
                if (NPC.velocity.Y > -10)
                {
                    NPC.velocity.Y -= 0.1f;
                }
                NPC.noGravity = true;
            }
            else
            {
                NPC.noGravity = false;
            }
            if (NPC.velocity.Y == 0)
            {
                NPC.frame.Y = 0;
            }
            else
            {
                NPC.frame.Y = 7 * 78;
            }

            NPC.velocity.Y *= 0.95f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                NPC.life = 1;
                NPC.dontTakeDamage = true;
                NPC.NewNPC(NPC.GetSource_OnHit(NPC), (int)NPC.Center.X + 700, (int)NPC.Center.Y + 600, ModContent.NPCType<TidalTitan>());
                Main.NewText("Azuredire has awoken!", new Color(175, 75, 255));
                SoundExtensions.PlaySoundOld(SoundID.Roar, (int)NPC.position.X, (int)NPC.position.Y, 0);
                TerrorbornSystem.ScreenShake(10f);
                TerrorbornSystem.downedMysteriousCrab = true;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<TidalTitan>()) && !NPC.AnyNPCs(ModContent.NPCType<MysteriousCrab>()))
            {
                if (NPC.downedBoss2 && !TerrorbornSystem.downedTidalTitan)
                {
                    return SpawnCondition.Ocean.Chance * 0.085f;
                }
                else
                {
                    return SpawnCondition.Ocean.Chance * 0.05f;
                }
            }
            else
            {
                return 0f;
            }
        }
    }

    [AutoloadBossHead]
    class TidalTitan : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 12;
            DisplayName.SetDefault("Azuredire");
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                PortraitPositionXOverride = 10,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/TidalTitan");
            NPC.width = 286;
            NPC.height = 90;
            NPC.damage = 40;
            NPC.HitSound = SoundID.DD2_WitherBeastCrystalImpact;
            NPC.defense = 5;
            NPC.DeathSound = SoundID.NPCDeath14;
            Main.raining = true;
            NPC.frame.Width = 388;
            NPC.frame.Height = 254;
            NPC.lifeMax = 3250;
            NPC.knockBackResist = 0;
            NPC.buffImmune[BuffID.OnFire] = true;
            NPC.buffImmune[BuffID.Ichor] = true;
            NPC.buffImmune[BuffID.CursedInferno] = true;
            NPC.aiStyle = -1;
            NPC.alpha = 255;

            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(NPC);
            modNPC.BossTitle = "Azuredire";
            modNPC.BossSubtitle = "Terrific Tidal Titan";
            modNPC.BossTitleColor = Color.SkyBlue;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement("A strange ancient beast from the depths of the ocean, capable of manipulating the water with terror. In the days of Anekronyx, these were studied to further understand the utilization of terror spells as applicable by incarnates.")
            });
        }

        public override void OnKill()
        {
            if (!TerrorbornSystem.downedTidalTitan)
            {
                TerrorbornSystem.downedTidalTitan = true;
                Main.NewText("Azurite Ore forms in caverns below the ocean", 37, 173, 255);
                Main.NewText("Lunar energy blesses the rain", 75, 253, 248);

                for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05) / 12; k++)
                {
                    int x = WorldGen.genRand.Next(20, 380);
                    int y = WorldGen.genRand.Next((int)(Main.maxTilesY * 0.3f), Main.maxTilesY);
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(3, 6), ModContent.TileType<Tiles.Azurite>(), false, 0f, 0f, false, true);
                }

                for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05) / 12; k++)
                {
                    int x = WorldGen.genRand.Next(Main.maxTilesX - 380, Main.maxTilesX - 20);
                    int y = WorldGen.genRand.Next((int)(Main.maxTilesY * 0.3f), Main.maxTilesY);
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(3, 6), ModContent.TileType<Tiles.Azurite>(), false, 0f, 0f, false, true);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.TidalTitanTrophy>(), 10));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<Items.TreasureBags.TT_TreasureBag>()));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<Items.Materials.AzuriteOre>(), 1, 18, 25));

            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Equipable.Vanity.BossMasks.TidalTitanMask>(), 7));

            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<Items.Weapons.Ranged.Jawvelin>(),
                ModContent.ItemType<Items.Weapons.Summons.Whips.AzuretoothWhip>(),
                ModContent.ItemType<Items.Weapons.Magic.BubbleBlaster>()));

            npcLoot.Add(notExpertRule);
        }

        int frame = 0;
        bool mouthOpen = false;
        int mouthTime = 0;
        int frameTime = 4;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter >= frameTime)
            {
                NPC.frameCounter = 0;
                frame++;
                if (frame >= 6)
                {
                    frame = 0;
                }
            }
            NPC.frame.Y = frame * frameHeight;
            if (mouthTime > 0)
            {
                mouthTime--;
            }
            if (!mouthOpen && mouthTime <= 0)
            {
                NPC.frame.Y += frameHeight * 6;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                return true;
            }
            SpriteEffects effects = SpriteEffects.None;
            Vector2 origin = new Vector2(35, 44);
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
                origin = NPC.frame.Size() - origin;
            }
            spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, NPC.Center - Main.screenPosition, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, effects, 0f);
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture + "_Glow"), NPC.Center - Main.screenPosition, NPC.frame, NPC.GetAlpha(Color.White), NPC.rotation, origin, NPC.scale, effects, 0f);
            return false;
        }

        List<int> NextAttacks = new List<int>();
        bool phaseStart;
        int AIPhase = 0;
        int NextAIPhase = 0;
        int LastAttack = 0;
        public void DecideNextAttack(int timeBetween)
        {
            if (NextAttacks.Count <= 0)
            {
                WeightedRandom<int> listOfAttacks = new WeightedRandom<int>();
                listOfAttacks.Add(0);
                listOfAttacks.Add(1);
                listOfAttacks.Add(2);
                listOfAttacks.Add(3);
                listOfAttacks.Add(4);
                for (int i = 0; i < listOfAttacks.elements.Count; i++)
                {
                    int choice = listOfAttacks.Get();
                    while (NextAttacks.Contains(choice) || (choice == LastAttack && NextAttacks.Count == 0))
                    {
                        choice = listOfAttacks.Get();
                    }
                    NextAttacks.Add(choice);
                }
            }
            phaseWait = (int)(timeBetween * MathHelper.Lerp(0.75f, 1.25f, (float)NPC.life / (float)NPC.lifeMax));
            frameTime = 8;
            NextAIPhase = NextAttacks[0];
            AIPhase = -1;
            LastAttack = NextAttacks[0];
            NextAttacks.RemoveAt(0);
            phaseStart = true;
            if (player.Center.X > realPosition.X) NPC.spriteDirection = 1;
            else NPC.spriteDirection = -1;
        }

        public float GetDirectionTo(Vector2 position, bool forVelocity = false)
        {
            float direction =  (position - realPosition).ToRotation();
            if (NPC.spriteDirection == -1 && !forVelocity)
            {
                direction += MathHelper.ToRadians(180f);
                direction = MathHelper.WrapAngle(direction);
            }
            return direction;
        }

        public void InBetweenAttacks()
        {
            NPC.rotation = NPC.rotation.AngleLerp(GetDirectionTo(player.Center), 0.1f);

            if (!inWater)
            {
                NPC.velocity.Y += 0.4f;
            }
            NPC.velocity *= 0.95f;

            phaseWait--;
            if (phaseWait <= 0)
            {
                AIPhase = NextAIPhase;
            }
        }

        bool inPhaseTransition = true;
        int phaseTransitionTime = 0;
        int phase = 0;
        float hitboxDistance = 100;
        Vector2 realPosition;
        bool inWater = false;
        bool start = true;
        Player player;

        int attackCounter1 = 0;
        int attackCounter2 = 0;
        int attackCounter3 = 0;

        int attackDirection1 = 0;

        int phaseWait = 0;

        public override void AI()
        {
            if (start)
            {
                realPosition = NPC.Center;
                start = false;
                inPhaseTransition = true;
                phaseTransitionTime = 0;
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                NPC.dontTakeDamage = true;
                mouthTime = 60;
            }

            int hitboxSize = 60;
            inWater = Collision.WetCollision(realPosition - new Vector2(hitboxSize / 2, hitboxSize / 2), hitboxSize, hitboxSize) || Collision.SolidCollision(realPosition - new Vector2(hitboxSize / 2, hitboxSize / 2), hitboxSize, hitboxSize);

            if (player.dead)
            {
                NPC.alpha += 255 / 120;
                if (NPC.alpha >= 255)
                {
                    NPC.active = false;
                }
            }

            if (inPhaseTransition)
            {
                if (phase == 0)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<MysteriousCrab>()))
                    {
                        phase++;
                        inPhaseTransition = false;
                        NPC.dontTakeDamage = false;
                        NPC.alpha = 0;
                        DecideNextAttack(240);
                    }
                    else
                    {
                        NPC target = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<MysteriousCrab>())];
                        NPC.spriteDirection = -1;
                        NPC.rotation = NPC.rotation.AngleLerp(GetDirectionTo(target.Center), 0.05f);
                        if (inWater)
                        {
                            NPC.velocity.Y -= 0.2f;
                        }
                        else
                        {
                            NPC.velocity.Y += 0.5f;
                        }

                        if (NPC.alpha > 0)
                        {
                            NPC.alpha -= 255 / 60;
                        }
                        else
                        {
                            NPC.alpha = 0;
                        }

                        phaseTransitionTime++;
                        if (phaseTransitionTime > 180)
                        {
                            mouthOpen = true;
                            float speed = 20f;
                            NPC.velocity = GetDirectionTo(target.Center, true).ToRotationVector2() * speed;

                            SoundExtensions.PlaySoundOld(SoundID.NPCDeath10, (int)NPC.position.X, (int)NPC.position.Y, 10, 1, 0.5f);
                            TerrorbornSystem.ScreenShake(15f);
                            if (NPC.Distance(target.Center) < target.width)
                            {
                                mouthOpen = false;
                                target.active = false;
                                Terraria.Audio.SoundEngine.PlaySound((Terraria.Audio.SoundStyle)target.DeathSound, target.Center);
                                Main.NewText("Mysterious Crab has been defeated...", new Color(175, 75, 255));

                                for (int i = 1; i <= 6; i++)
                                {
                                    float goreSpeed = Main.rand.NextFloat(10f, 20f);
                                    Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * goreSpeed + NPC.velocity;
                                    Gore.NewGore(target.GetSource_Death(), target.Center, velocity, Mod.Find<ModGore>("MCrabGore" + i.ToString()).Type);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                NPC.dontTakeDamage = true;
                if (NPC.Distance(player.Center) < 1000)
                {
                    NPC.dontTakeDamage = false;
                }

                switch (AIPhase)
                {
                    case -1:
                        InBetweenAttacks();
                        break;
                    case 0:
                        BubbleBurping((int)MathHelper.Lerp(4.75f, 3f, (float)NPC.life / (float)NPC.lifeMax), (int)MathHelper.Lerp(25, 35, (float)NPC.life / (float)NPC.lifeMax));
                        break;
                    case 1:
                        GeyserRoar((int)MathHelper.Lerp(8f, 5f, (float)NPC.life / (float)NPC.lifeMax), 20, 60);
                        break;
                    case 2:
                        Leap((int)MathHelper.Lerp(18f, 45f, (float)NPC.life / (float)NPC.lifeMax), 15f, 30f);
                        break;
                    case 3:
                        ChargeAttack(20f, 60, 30);
                        break;
                    case 4:
                        BubbleDash(20f, 5, 0.25f, 60, (int)MathHelper.Lerp(30f, 60f, (float)NPC.life / (float)NPC.lifeMax));
                        break;
                    default:
                        break;
                }
            }


            realPosition += NPC.velocity;

            float hitRotation = NPC.rotation;
            if (NPC.spriteDirection == -1)
            {
                hitRotation += MathHelper.ToRadians(180);
            }
            NPC.position = realPosition + (hitRotation.ToRotationVector2() * hitboxDistance) - NPC.Hitbox.Size() / 2;
            NPC.Hitbox = new Rectangle((int)NPC.Center.X - hitboxSize / 2, (int)NPC.Center.Y - hitboxSize / 2, hitboxSize, hitboxSize);
        }

        public void BubbleBurping(int burps, int timeBetweenBurps)
        {
            if (phaseStart)
            {
                attackCounter1 = burps;
                attackCounter2 = 0;
                attackCounter3 = 0;
                frameTime = 4;
                phaseStart = false;
                if (player.Center.X > realPosition.X) NPC.spriteDirection = 1;
                else NPC.spriteDirection = -1;
            }
            else
            {
                NPC.rotation = NPC.rotation.AngleLerp(GetDirectionTo(player.Center), 0.2f);

                Vector2 targetPosition = player.Center + new Vector2(Math.Sign(player.Center.X - realPosition.X) * -300, 350);
                if (inWater)
                {
                    NPC.velocity.Y += Math.Sign(targetPosition.Y - realPosition.Y) * 0.3f;
                }
                else
                {
                    NPC.velocity.Y += 0.6f;
                }
                NPC.velocity.X += Math.Sign(targetPosition.X - realPosition.X) * 0.5f;
                NPC.velocity *= 0.98f;

                float burpTimeMult = 1f;
                if (TerrorbornSystem.TwilightMode)
                {
                    burpTimeMult = 0.75f;
                    if (Main.masterMode)
                    {
                        burpTimeMult = 1f;
                    }
                }

                attackCounter3++;
                if (Math.Abs(targetPosition.X - realPosition.X) < 200 || attackCounter3 > 180)
                {
                    attackCounter2++;
                    if (attackCounter2 > (float)timeBetweenBurps * burpTimeMult)
                    {
                        mouthOpen = true;
                        attackCounter2 = 0;
                        attackCounter1--;
                        NPC bubble = Main.npc[NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PearlranhaBubble>())];
                        bubble.velocity = GetDirectionTo(player.Center, true).ToRotationVector2() * 3f;
                        NPC.velocity -= GetDirectionTo(player.Center, true).ToRotationVector2() * 5f;
                        SoundExtensions.PlaySoundOld(SoundID.NPCDeath13, NPC.Center);
                        if (attackCounter1 <= 0)
                        {
                            mouthOpen = false;
                            if (Main.masterMode)
                            {
                                DecideNextAttack(320);
                            }
                            else
                            {
                                DecideNextAttack(240);
                            }
                        }
                    }
                }
            }
        }

        public void GeyserRoar(int geyserCount, int timeBetweenGeysers, int timeUntilErupt)
        {
            if (phaseStart)
            {
                if (TerrorbornSystem.TwilightMode)
                {
                    geyserCount += 2;
                    if (Main.masterMode)
                    {
                        geyserCount++;
                    }
                }
                frameTime = 5;
                attackCounter1 = geyserCount;
                attackCounter2 = 0;
                phaseStart = false;
                if (player.Center.X > realPosition.X) NPC.spriteDirection = 1;
                else NPC.spriteDirection = -1;

                SoundExtensions.PlaySoundOld(SoundID.Roar, (int)NPC.position.X, (int)NPC.position.Y, 0, 1, -0.5f);
                mouthTime = 30;
                TerrorbornSystem.ScreenShake(10f);
                NPC.velocity -= GetDirectionTo(player.Center, true).ToRotationVector2() * 10f;
            }

            NPC.rotation = NPC.rotation.AngleLerp(GetDirectionTo(player.Center), 0.1f);

            if (!inWater)
            {
                NPC.velocity.Y += 0.4f;
            }
            NPC.velocity *= 0.95f;

            attackCounter2++;
            if (attackCounter2 >= timeBetweenGeysers)
            {
                frameTime = 3;
                attackCounter2 = 0;
                attackCounter1--;
                if (attackCounter1 <= 0)
                {
                    DecideNextAttack(timeUntilErupt * 3);
                }

                int proj = Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), new Vector2(player.Center.X + (player.velocity.X * timeUntilErupt), player.Center.Y), Vector2.Zero, ModContent.ProjectileType<GeyserTelegraph>(), 60 / 4, 0f);
                Main.projectile[proj].ai[0] = timeUntilErupt;
            }
        }

        public void Leap(int timeBetweenProjectiles, float ProjectileSpeed, float leapSpeed)
        {
            if (phaseStart)
            {
                attackCounter1 = 0;
                attackCounter2 = 0;
                phaseStart = false;
                if (player.Center.X > realPosition.X) NPC.spriteDirection = -1;
                else NPC.spriteDirection = 1;

                attackDirection1 = -NPC.spriteDirection;

                SoundExtensions.PlaySoundOld(SoundID.NPCDeath10, (int)NPC.position.X, (int)NPC.position.Y, 10, 1, 1f);
                mouthTime = 30;
                TerrorbornSystem.ScreenShake(10f);
            }
            else if (attackCounter2 == 0)
            {
                frameTime = 3;
                NPC.velocity *= 0.98f;
                NPC.velocity.Y -= 0.3f;
                NPC.rotation = NPC.rotation.AngleLerp(GetDirectionTo(player.Center), 0.05f);

                if (!inWater)
                {
                    frameTime = 4;
                    NPC.velocity.Y = -leapSpeed;
                    attackCounter2++;
                }
            }
            else
            {
                NPC.rotation = NPC.rotation.AngleLerp(GetDirectionTo(player.Center), 0.2f);

                NPC.velocity *= 0.98f;
                NPC.velocity.Y += 0.3f;

                float targetX = player.Center.X + 500f * attackDirection1;
                NPC.velocity.X += Math.Sign(targetX - realPosition.X) * 0.5f;

                float timeMult = 1f;
                if (TerrorbornSystem.TwilightMode)
                {
                    timeMult = 0.75f;
                    if (Main.masterMode)
                    {
                        timeMult = 0.5f;
                    }
                }

                attackCounter1++;
                if (attackCounter1 > timeBetweenProjectiles * timeMult)
                {
                    attackCounter1 = 0;
                    SoundExtensions.PlaySoundOld(SoundID.Item21, NPC.Center);
                    TerrorbornSystem.ScreenShake(2f);
                    Vector2 velocity = GetDirectionTo(player.Center, true).ToRotationVector2() * ProjectileSpeed;
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity, ModContent.ProjectileType<TidebornLaser>(), 50 / 4, 0f);

                }

                if (NPC.velocity.Y > 0 && inWater)
                {
                    DecideNextAttack(90);
                    ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/TTSplash").Value.Play(Main.soundVolume * 0.75f, 0f, 0f);
                }
            }
        }

        public void ChargeAttack(float leapSpeed, int chargeTime, int timeUntilCharge)
        {
            if (phaseStart)
            {
                attackCounter2 = 0;
                attackCounter1 = chargeTime;
                attackCounter3 = timeUntilCharge;

                phaseStart = false;
                if (player.Center.X > realPosition.X) NPC.spriteDirection = 1;
                else NPC.spriteDirection = -1;

            }
            else if (attackCounter2 == 0)
            {
                frameTime = 3;
                NPC.velocity *= 0.98f;
                NPC.velocity.Y -= 0.3f;
                if (NPC.life <= NPC.lifeMax * 0.65f || TerrorbornSystem.TwilightMode)
                {
                    NPC.rotation = NPC.rotation.AngleLerp(GetDirectionTo(player.Center + player.velocity * (NPC.Distance(player.Center) / leapSpeed)), 0.1f);
                }
                else
                {
                    NPC.rotation = NPC.rotation.AngleLerp(GetDirectionTo(player.Center), 0.1f);
                }

                if (!inWater)
                {
                    NPC.velocity.Y -= 10;
                    attackCounter2++;
                }
            }
            else if (attackCounter3 > 0)
            {
                frameTime = 4;
                NPC.velocity *= 0.98f;
                NPC.velocity.Y += 0.1f;
                if (NPC.life <= NPC.lifeMax * 0.65f)
                {
                    NPC.rotation = NPC.rotation.AngleLerp(GetDirectionTo(player.Center + player.velocity * (NPC.Distance(player.Center) / leapSpeed)), 0.1f);
                }
                else
                {
                    NPC.rotation = NPC.rotation.AngleLerp(GetDirectionTo(player.Center), 0.1f);
                }

                attackCounter3--;
                if (attackCounter3 <= 0)
                {
                    SoundExtensions.PlaySoundOld(SoundID.Roar, (int)NPC.position.X, (int)NPC.position.Y, 0, 1, 1f);
                    TerrorbornSystem.ScreenShake(10f);
                    //Do the charge itself
                    frameTime = 2;
                    mouthOpen = true;
                    if (NPC.life <= NPC.lifeMax * 0.65f)
                    {
                        NPC.rotation = GetDirectionTo(player.Center + player.velocity * (NPC.Distance(player.Center) / leapSpeed));
                        NPC.velocity = GetDirectionTo(player.Center + player.velocity * (NPC.Distance(player.Center) / leapSpeed), true).ToRotationVector2() * leapSpeed;
                    }
                    else
                    {
                        NPC.rotation = GetDirectionTo(player.Center);
                        NPC.velocity = GetDirectionTo(player.Center, true).ToRotationVector2() * leapSpeed;
                    }
                }
            }
            else if (attackCounter1 > 0)
            {
                if (inWater)
                {
                    attackCounter1 = 0;
                }
                attackCounter1--;
                if (attackCounter1 <= 0)
                {
                    frameTime = 4;
                    mouthOpen = false;
                    if (player.Center.X > realPosition.X) NPC.spriteDirection = 1;
                    else NPC.spriteDirection = -1;
                }
            }
            else
            {
                NPC.rotation = NPC.rotation.AngleLerp(GetDirectionTo(player.Center), 0.2f);

                NPC.velocity.Y *= 0.98f;
                NPC.velocity.X *= 0.95f;
                NPC.velocity.Y += 0.3f;


                if (NPC.velocity.Y > 0 && inWater)
                {
                    DecideNextAttack(90);
                    ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/TTSplash").Value.Play(Main.soundVolume * 0.75f, 0f, 0f);
                }
            }
        }

        public void BubbleDash(float dashSpeed, int timeBetweenBubbles, float bubbleAccelleration, int dashTime, int timeUntilDash = 60, int maxTimeUntilDash = 180)
        {
            if (phaseStart)
            {
                attackCounter1 = 0;
                attackCounter2 = 0;
                attackCounter3 = 0;
                phaseStart = false;
                if (player.Center.X > realPosition.X) NPC.spriteDirection = 1;
                else NPC.spriteDirection = -1;
                attackDirection1 = NPC.spriteDirection;
            }
            else if (attackCounter1 < maxTimeUntilDash)
            {
                NPC.rotation = NPC.rotation.AngleLerp(0f + MathHelper.ToRadians(NPC.velocity.Y * NPC.spriteDirection), 0.05f);

                Vector2 targetPosition = player.Center + new Vector2(attackDirection1 * -760, 300);
                if (inWater)
                {
                    NPC.velocity.Y += Math.Sign(targetPosition.Y - realPosition.Y) * 0.3f;
                }
                else
                {
                    NPC.velocity.Y += 0.6f;
                }
                NPC.velocity.X += Math.Sign(targetPosition.X - realPosition.X) * 0.5f;
                NPC.velocity *= 0.98f;

                attackCounter1++;
                if (Math.Abs(targetPosition.X - realPosition.X) < 200 && attackCounter1 >= timeUntilDash)
                {
                    attackCounter1 = maxTimeUntilDash;
                }
            }
            else
            {
                NPC.rotation = NPC.rotation.AngleLerp(0f + MathHelper.ToRadians(NPC.velocity.Y * NPC.spriteDirection), 0.2f);
                NPC.velocity.Y = 0;
                NPC.velocity.X = dashSpeed * attackDirection1;
                if (TerrorbornSystem.TwilightMode && Main.masterMode)
                {
                    NPC.velocity.X *= 1.5f;
                }

                frameTime = 3;

                attackCounter2++;
                if (attackCounter2 > timeBetweenBubbles)
                {
                    SoundExtensions.PlaySoundOld(SoundID.Item85, NPC.Center);
                    attackCounter2 = 0;
                    NPC bubble = Main.npc[NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<TidalBubble>())];
                    bubble.ai[0] = bubbleAccelleration;
                }

                if (TerrorbornSystem.TwilightMode)
                {
                    dashTime = (int)(dashTime * 1.25f);
                }
                attackCounter3++;
                if (attackCounter3 > dashTime)
                {
                    DecideNextAttack(120);
                }
            }
        }
    }

    class TidalBubble : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            DisplayName.SetDefault("Bubble");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void OnKill()
        {
            Projectile.NewProjectile(NPC.GetSource_OnHit(NPC), NPC.Center, new Vector2(5, 0), ModContent.ProjectileType<TidalBubbleSmall>(), NPC.damage / 4, 0);
            Projectile.NewProjectile(NPC.GetSource_OnHit(NPC), NPC.Center, new Vector2(-5, 0), ModContent.ProjectileType<TidalBubbleSmall>(), NPC.damage / 4, 0);
            Projectile.NewProjectile(NPC.GetSource_OnHit(NPC), NPC.Center, new Vector2(0, 5), ModContent.ProjectileType<TidalBubbleSmall>(), NPC.damage / 4, 0);
            Projectile.NewProjectile(NPC.GetSource_OnHit(NPC), NPC.Center, new Vector2(0, -5), ModContent.ProjectileType<TidalBubbleSmall>(), NPC.damage / 4, 0);

            if (Main.rand.NextBool(10))
            {
                Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Heart);
            }
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 38;
            NPC.height = 36;
            NPC.damage = 25;
            NPC.HitSound = SoundID.Item54;
            NPC.defense = 0;
            NPC.DeathSound = SoundID.Item54;
            NPC.frame.Width = 388;
            NPC.frame.Height = 254;
            NPC.lifeMax = 40;
            NPC.knockBackResist = 0;
            NPC.dontTakeDamage = true;
            NPC.aiStyle = -1;
        }

        int frame = 0;
        int frameWait = 10;
        public override void FindFrame(int frameHeight)
        {
            frameWait--;
            if (frameWait <= 0)
            {
                frameWait = 10;
                frame++;
                if (frame >= 4)
                {
                    frame = 0;
                }
            }
            NPC.frame.Y = frame * 36;
        }

        int invincTime = 30;
        public override void AI()
        {
            if (invincTime <= 0)
            {
                NPC.dontTakeDamage = false;
            }
            else
            {
                invincTime--;
            }
            NPC.TargetClosest(true);

            NPC.velocity.Y -= NPC.ai[0];

            if (NPC.Center.Y < Main.LocalPlayer.Center.Y - 1000)
            {
                NPC.active = false;
            }
        }
    }

    class TidalBubbleSmall : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Bubble";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble");
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 70;
            Projectile.penetrate = 1;
            Projectile.hide = false;
        }
        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.Item54, Projectile.position);
        }
    }

    class PearlranhaBubble : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
            DisplayName.SetDefault("Bubble");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }
        public override void OnKill()
        {
            Projectile.NewProjectile(NPC.GetSource_OnHit(NPC), NPC.Center, new Vector2(5, 0), ModContent.ProjectileType<TidalBubbleSmall>(), NPC.damage / 4, 0);
            Projectile.NewProjectile(NPC.GetSource_OnHit(NPC), NPC.Center, new Vector2(-5, 0), ModContent.ProjectileType<TidalBubbleSmall>(), NPC.damage / 4, 0);
            Projectile.NewProjectile(NPC.GetSource_OnHit(NPC), NPC.Center, new Vector2(0, 5), ModContent.ProjectileType<TidalBubbleSmall>(), NPC.damage / 4, 0);
            Projectile.NewProjectile(NPC.GetSource_OnHit(NPC), NPC.Center, new Vector2(0, -5), ModContent.ProjectileType<TidalBubbleSmall>(), NPC.damage / 4, 0);
            NPC.NewNPC(NPC.GetSource_OnHit(NPC), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Pearlranha>());
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 38;
            NPC.height = 36;
            NPC.damage = 25;
            NPC.HitSound = SoundID.Item54;
            NPC.defense = 0;
            NPC.DeathSound = SoundID.Item54;
            NPC.frame.Width = 388;
            NPC.frame.Height = 254;
            NPC.lifeMax = 14;
            NPC.knockBackResist = 0;
            NPC.aiStyle = -1;
        }

        int frame = 0;
        int frameWait = 10;
        public override void FindFrame(int frameHeight)
        {
            frameWait--;
            if (frameWait <= 0)
            {
                frameWait = 10;
                frame++;
                if (frame >= 4)
                {
                    frame = 0;
                }
            }
            NPC.frame.Y = frame * 36;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = 45;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Vector2 targetPosition = Main.player[NPC.target].Center;
            NPC.spriteDirection = Math.Sign(targetPosition.X - NPC.Center.X);
            float speed = 0.4f;
            NPC.velocity += NPC.DirectionTo(targetPosition) * speed;
            NPC.velocity *= 0.98f;
        }
    }

    class Pearlranha : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
        }


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

                new FlavorTextBestiaryInfoElement("Azuredire children were once considered an entirely different species due to their immense difference in nature. Highly adept in combat from the getgo, Pearlranhas are a viscious bunch.")
            });
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.width = 38;
            NPC.height = 28;
            NPC.damage = 20;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.defense = 0;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.frame.Width = 388;
            NPC.frame.Height = 254;
            NPC.lifeMax = 45;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0.01f;
            NPC.noTileCollide = true;
        }

        int frame = 0;
        int frameWait = 8;
        public override void OnKill()
        {
            if (Main.rand.NextBool(7))
            {
                Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Heart);
            }

            if (Main.rand.NextBool(7))
            {
                Item.NewItem(NPC.GetSource_Loot(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.DarkEnergy>());
            }
        }

        public override void FindFrame(int frameHeight)
        {
            frameWait--;
            if (frameWait <= 0)
            {
                frameWait = 8;
                frame++;
                if (frame >= 6)
                {
                    frame = 0;
                }
            }
            NPC.frame.Y = frame * frameHeight;
        }

        public bool CheckInWater(bool hasVelocityOffset = false)
        {
            if (hasVelocityOffset) inWater = Collision.WetCollision(NPC.position + NPC.velocity, NPC.width, NPC.height) || Collision.SolidCollision(NPC.position + NPC.velocity, NPC.width, NPC.height);
            else inWater = Collision.WetCollision(NPC.position, NPC.width, NPC.height) || Collision.SolidCollision(NPC.position, NPC.width, NPC.height);

            return inWater;
        }

        bool inWater = false;
        public override void AI()
        {
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);
            CheckInWater();

            if (inWater)
            {
                NPC.noGravity = true;
                if (player.wet)
                {
                    NPC.velocity += NPC.DirectionTo(player.Center);
                    NPC.velocity *= 0.95f;
                }
                else
                {
                    NPC.velocity.Y -= 0.5f;
                    NPC.velocity.X += Math.Sign(player.Center.X - NPC.Center.X) * 0.5f;
                    NPC.velocity *= 0.96f;
                    if (!CheckInWater(true))
                    {
                        NPC.velocity += NPC.DirectionTo(player.Center) * NPC.Distance(player.Center) / 45f;
                        if (NPC.velocity.Length() > 20) NPC.velocity *= 20 / NPC.velocity.Length();
                    }
                }
            }
            else
            {
                NPC.noGravity = false;
            }

            NPC.rotation = 0f + MathHelper.ToRadians(NPC.velocity.Y) * NPC.spriteDirection;
        }
    }

    class GeyserTelegraph : ModProjectile
    {
        float FireWait = 90;
        int FireWait2 = 6;
        int FiresLeft = 5;

        int telegraphLength = 60;
        float telegraphAlpha = 0;
        int telegraphAlphaDirection = 1;

        public override bool PreDraw(ref Color lightColor)
        {
            Color color = Color.LightSkyBlue;
            color.A = (int)(255 * 0.75f);
            Utils.DrawLine(Main.spriteBatch, Projectile.Center, Projectile.Center + new Vector2(0, -telegraphLength), color * telegraphAlpha, Color.Transparent, 3);
            return false;
        }

        public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.hide = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.alpha = 255;
        }

        bool start = true;

        public override void AI()
        {
            Vector2 position = new Vector2(Projectile.Center.X, Projectile.position.Y - 60);
            while (!(Collision.SolidCollision(position, 1, 1, true) || Collision.WetCollision(position, 1, 1)))
            {
                position.Y++;
            }
            Projectile.position.Y = position.Y;
            if (start)
            {
                start = false;
                FireWait = Projectile.ai[0];
            }
            FireWait--;
            if (FireWait == 0)
            {
                SoundExtensions.PlaySoundOld(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 34, 3, 0.15f);
                TerrorbornSystem.ScreenShake(3f);
            }
            if (FireWait <= 0)
            {
                FireWait2--;
                if (FireWait2 <= 0)
                {
                    FireWait2 = 1;
                    FiresLeft--;
                    if (FiresLeft <= 0)
                    {
                        Projectile.active = false;
                    }
                    TerrorbornSystem.ScreenShake(0.5f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X, Projectile.position.Y), new Vector2(0, 0), ModContent.ProjectileType<TideFire>(), 18, Projectile.knockBack);
                    Projectile.velocity.X = 0;
                }
            }
            else
            {
                telegraphLength += 6;
                if (telegraphAlphaDirection == 1)
                {
                    telegraphAlpha += 2f / Projectile.ai[0];
                    if (telegraphAlpha >= 1)
                    {
                        telegraphAlphaDirection = -1;
                    }
                }
                else
                {
                    telegraphAlpha -= 2f / Projectile.ai[0];
                }
            }
        }
    }

    class TideFire : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Effects/Textures/Geyser";

        int timeLeft = 30;
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 500;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.hide = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = timeLeft;
            Projectile.alpha = 255;
            Projectile.ignoreWater = true;
        }

        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
                Projectile.position -= new Vector2(0, Projectile.height / 2);
                Projectile.alpha = 0;
            }

            Projectile.alpha += (int)(255f / (float)timeLeft);
            Projectile.scale += 0.3f / timeLeft;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture), Projectile.position + new Vector2(Projectile.width / 2, Projectile.height) - Main.screenPosition, null, Projectile.GetAlpha(Color.Azure), 0f, new Vector2(ModContent.Request<Texture2D>(Texture).Width() / 2, ModContent.Request<Texture2D>(Texture).Height()), new Vector2(((float)Projectile.width / (float)ModContent.Request<Texture2D>(Texture).Width()) * Projectile.scale, ((float)Projectile.height / (float)ModContent.Request<Texture2D>(Texture).Height()) * Projectile.scale), SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }
    }

    class TidebornLaser : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Effects/Textures/Glow_1";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            BezierCurve bezier = new BezierCurve();
            bezier.Controls.Clear();
            foreach (Vector2 pos in Projectile.oldPos)
            {
                if (pos != Vector2.Zero && pos != null)
                {
                    bezier.Controls.Add(pos);
                }
            }

            if (bezier.Controls.Count > 1)
            {
                List<Vector2> positions = bezier.GetPoints(30);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.RoyalBlue, Color.Azure, mult)) * mult;
                    TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * mult), color);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int newDimensions = 15;
            Rectangle oldHitbox = hitbox;
            hitbox.Width = newDimensions;
            hitbox.Height = newDimensions;
            hitbox.X = oldHitbox.X - newDimensions / 2;
            hitbox.Y = oldHitbox.Y - newDimensions / 2;
        }

        bool gottenClose = false;
        public override void AI()
        {
            if (TerrorbornSystem.TwilightMode)
            {
                if (!gottenClose)
                {
                    Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.LocalPlayer.Center).ToRotation(), MathHelper.ToRadians(2f)).ToRotationVector2() * Projectile.velocity.Length();
                    if (Projectile.Distance(Main.LocalPlayer.Center) <= 200f)
                    {
                        gottenClose = true;
                    }
                }
            }
            else
            {
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.LocalPlayer.Center).ToRotation(), MathHelper.ToRadians(0.5f)).ToRotationVector2() * Projectile.velocity.Length();
            }
        }
    }
}