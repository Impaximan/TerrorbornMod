using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Abilities;
using Terraria.Utilities;
using Microsoft.Xna.Framework.Audio;
using TerrorbornMod.Projectiles;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;


namespace TerrorbornMod.NPCs.Bosses.HexedConstructor
{
    [AutoloadBossHead]
    class HexedConstructor : ModNPC
    {
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
        public override void OnKill()
        {
            if (!TerrorbornSystem.downedIncendiaryBoss)
            {
                TerrorbornSystem.downedIncendiaryBoss = true;
                Vector2 terrorMasterPosition = new Vector2(Main.spawnTileX * 16, Main.spawnTileY * 16);
                NPC.NewNPC(NPC.GetSource_OnHit(NPC), (int)terrorMasterPosition.X, (int)terrorMasterPosition.Y, ModContent.NPCType<NPCs.TownNPCs.Heretic>());
                Main.NewText("Gabrielle the Heretic has invited herself to your town!", new Color(50, 125, 255));

                ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/HexedConstructorDeath").Value.Play(Main.musicVolume, 0f, 0f);
                Main.musicFade[Music] = 0f;

                bool spawnTF = !TerrorbornPlayer.modPlayer(Main.player[Main.myPlayer]).unlockedAbilities.Contains(7);
                for (int i = 0; i < 1000; i++)
                {
                    Projectile Projectile = Main.projectile[i];
                    if (Projectile.active && Projectile.type == ModContent.ProjectileType<TimeFreeze>())
                    {
                        spawnTF = false;
                    }
                }

                if (spawnTF)
                {
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<TimeFreeze>(), 0, 0, Main.myPlayer);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.HexedConstructorTrophy>(), 10));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<Items.TreasureBags.HC_TreasureBag>()));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<Items.Materials.HexingEssence>(), 1, 15, 20));
            npcLoot.Add(new LeadingConditionRule(new Conditions.NotExpert()).OnSuccess(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<Items.Weapons.Ranged.MirageBow>(),
                ModContent.ItemType<Items.Weapons.Magic.SongOfTime>(),
                ModContent.ItemType<Items.Weapons.Melee.IcarusShred>())));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<Items.Equipable.Vanity.BossMasks.HexedConstructorMask>(), 7));
            npcLoot.Add(new LeadingConditionRule(new Conditions.NotExpert()).OnSuccess(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<Items.Equipable.Armor.HexDefenderMask>(),
                ModContent.ItemType<Items.Equipable.Armor.HexDefenderBreastplate>(),
                ModContent.ItemType<Items.Equipable.Armor.HexDefenderGreaves>())));
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 10;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
            name = "A hexed constructor";
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("An Orumian construct originally built to construct buildings for the city, which was later repurposed for combat and used as a bodyguard for the team sent to build the seal. Upon the dread catastrophe, these machines went missing, however, it seems they have reappeared among the sky islands.")
            });
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 68;
            NPC.height = 152;
            NPC.damage = 60;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.defense = 10;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.boss = true;
            NPC.frame.Height = 254;
            NPC.lifeMax = 20000;
            NPC.knockBackResist = 0;
            NPC.aiStyle = -1;
            NPC.alpha = 255;
            NPC.dontTakeDamage = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/HexedConstructor");

            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(NPC);
            modNPC.BossTitle = "Hexed Constructor";
            modNPC.BossSubtitle = "Possessed Builder of the Seal";
            modNPC.BossTitleColor = new Color(191, 82, 58);
        }

        int frame = 0;
        int frameTarget = 0;
        int targetChangeCounter = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter--;
            if (NPC.frameCounter <= 0)
            {
                NPC.frameCounter = 5;

                if (frame > frameTarget)
                {
                    frame--;
                }

                if (frame < frameTarget)
                {
                    frame++;
                }
            }

            if (frame == frameTarget)
            {
                targetChangeCounter--;
                if (targetChangeCounter <= 0)
                {
                    targetChangeCounter = Main.rand.Next(15);
                    frameTarget = 0 + (Main.rand.Next(3) * 2);
                }
            }

            if (phase == 1)
            {
                NPC.frame.Y = frame * frameHeight;
            }
            else
            {
                NPC.frame.Y = (frame + 5) * frameHeight;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit54, NPC.Center);
        }

        public void MoveTowardsPosition(Vector2 position, float speed, float maxSpeed, float dragMultiplier = 0.98f)
        {
            NPC.velocity += NPC.DirectionTo(position) * speed;

            if (NPC.velocity.Length() > maxSpeed)
            {
                NPC.velocity = NPC.velocity.ToRotation().ToRotationVector2() * maxSpeed;
            }

            NPC.velocity *= dragMultiplier;
        }

        bool doingMelee = false;
        int baseDamage;
        bool start = true;
        int AIPhase = 0;
        int phase = 1;
        bool enraged = false;
        int enrageCounter = 0;
        Player player;
        TerrorbornPlayer modPlayer;
        public void SetStats()
        {
            NPC.TargetClosest(false);
            if (doingMelee)
            {
                NPC.damage = baseDamage;
            }
            else
            {
                NPC.damage = 0;
            }

            player = Main.player[NPC.target];
            modPlayer = TerrorbornPlayer.modPlayer(player);

            if (player.Center.X > NPC.Center.X)
            {
                NPC.spriteDirection = -1;
            }
            else
            {
                NPC.spriteDirection = 1;
            }

            if (modPlayer.ZoneIncendiary)
            {
                enrageCounter = 0;
                enraged = false;
            }
            else
            {
                enrageCounter++;
                if (enrageCounter > 180)
                {
                    enraged = true;
                }
            }

            if (enraged)
            {
                enrageDamageMultiplier = 3f;
            }
            else
            {
                enrageDamageMultiplier = 1f;
            }
        }

        public void OnStart()
        {
            NPC.TargetClosest(false);
            player = Main.player[NPC.target];

            baseDamage = NPC.damage;
            NPC.position = player.Center + new Vector2(0, -250) - NPC.Size / 2;
            NPC.dontTakeDamage = false;
            transitioning = true;
            NPC.velocity = new Vector2(0, -5);
            TerrorbornMod.SetScreenToPosition(60 * 3, 20, NPC.Center + new Vector2(0, -75), 0.8f);
        }

        bool phaseStart;
        List<int> NextAttacks = new List<int>();
        int LastAttack = -1;
        float secondPhaseHealth = 0.4f;
        bool transitioning = true;
        int transitionTime = 120;
        int attackWait;
        int attackWait2;
        int attackCounter;
        float attackDirection = 0f;
        int ProjectileCounter = 0;
        int timeAlive = 0;
        Vector2 attackTarget;
        float enrageDamageMultiplier = 1f;
        bool firstTransition = true;

        float wingRotation = 0f;
        int wingRotateDirection = 1;

        float transitionCloneVelocity = 2.5f;

        NPC claw1;
        NPC claw2;

        Vector2 claw1Offset;
        Vector2 claw2Offset;

        public void DecideNextAttack()
        {
            if (NextAttacks.Count <= 0)
            {
                WeightedRandom<int> listOfAttacks = new WeightedRandom<int>();
                listOfAttacks.Add(0);
                listOfAttacks.Add(1);
                listOfAttacks.Add(2);
                listOfAttacks.Add(3);
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
            if (phase == 1 && NPC.life <= secondPhaseHealth * NPC.lifeMax)
            {
                int totalTime = 30 * 4;
                transitioning = true;
                transitionTime = totalTime;
                NPC.velocity = Vector2.Zero;
            }
            else
            {
                AIPhase = NextAttacks[0];
                LastAttack = AIPhase;
                NextAttacks.RemoveAt(0);
            }
            phaseStart = true;
        }

        public void RotateWings(float speed, float maxRotation)
        {
            wingRotation += wingRotateDirection * speed;
            if (Math.Abs(wingRotation) >= maxRotation)
            {
                wingRotateDirection *= -1;
                wingRotation += wingRotateDirection * (speed + MathHelper.ToRadians(1f));
            }
        }

        bool hasStartedLeave = false;
        public override void AI()
        {
            NPC.ai[0]++;

            if (start)
            {
                OnStart();
                start = false;
            }

            SetStats();
            RotateWings(MathHelper.ToRadians(3), MathHelper.ToRadians(25));

            if (player.dead || !player.active)
            {
                if (!hasStartedLeave)
                {
                    hasStartedLeave = true;
                    claw1Offset = claw1.position - NPC.position;
                    claw2Offset = claw2.position - NPC.position;
                }
                claw1.position = NPC.position + claw1Offset;
                claw2.position = NPC.position + claw2Offset;
                NPC.velocity.Y -= 0.2f;
                if (NPC.position.Y <= 0)
                {
                    NPC.active = false;
                }
                return;
            }

            if (phase == 2)
            {
                TerrorbornPlayer.modPlayer(player).HexedMirage = true;
            }

            if (transitioning)
            {
                if (firstTransition)
                {
                    if (NPC.alpha > 0)
                    {
                        NPC.alpha -= 5;
                        NPC.velocity *= 0.98f;
                    }
                    else
                    {
                        if (!NPC.AnyNPCs(ModContent.NPCType<HexedClaw>()))
                        {
                            claw1 = Main.npc[NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.Center.X, (int)NPC.Center.Y - 20, ModContent.NPCType<HexedClaw>())];
                            claw2 = Main.npc[NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.Center.X, (int)NPC.Center.Y - 20, ModContent.NPCType<HexedClaw>())];

                            claw1.realLife = NPC.whoAmI;
                            claw2.realLife = NPC.whoAmI;

                            claw1.ai[0] = 4;
                            claw2.ai[0] = 4;

                            claw1.velocity.X = -15;
                            claw2.velocity.X = 15;
                        }
                        NPC.velocity *= 0.93f;
                        claw1.rotation += MathHelper.ToRadians(15);
                        claw2.rotation += MathHelper.ToRadians(-15);
                        claw1.velocity *= 0.93f;
                        claw2.velocity *= 0.93f;

                        transitionTime--;
                        if (transitionTime <= 0)
                        {
                            DecideNextAttack();
                            transitioning = false;
                            firstTransition = false;
                        }
                    }
                }
                else
                {
                    if (transitionTime > 0)
                    {
                        NPC.velocity = Vector2.Zero;
                        transitionTime--;

                        claw1.ai[0] = 4;
                        claw2.ai[0] = 4;
                        claw1.rotation = 0f;
                        claw2.rotation = 0f;
                        claw1.velocity = (new Vector2(NPC.Center.X + 300, NPC.Center.Y) - claw1.Center) / 10;
                        claw2.velocity = (new Vector2(NPC.Center.X - 300, NPC.Center.Y) - claw2.Center) / 10;

                        if (transitionTime == 5 * (int)(transitionTime / 5))
                        {
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14);
                        }
                        Dust.NewDust(NPC.Center + new Vector2(-10, -NPC.height / 2 + 10), 20, 20, 6);
                        TerrorbornSystem.ScreenShake(2f);
                    }
                    else
                    {
                        phase++;
                        transitioning = false;
                        NPC.dontTakeDamage = false;
                        DecideNextAttack();
                        DustExplosion(NPC.Center + new Vector2(0, -NPC.height / 2 + 20), 0, 25, 20f, 6, 1.5f, true);
                        TerrorbornSystem.ScreenShake(10f);
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.Center.X, (int)NPC.Center.Y, 105, 1, 0);
                    }
                }
            }
            else
            {
                if (phase == 1 && NPC.life <= secondPhaseHealth * NPC.lifeMax)
                {
                    NPC.dontTakeDamage = true;
                }

                switch (AIPhase)
                {
                    case 0:
                        if (phase == 2)
                        {
                            UppercutAttack((int)(60 * 0.8f), 30, 1000, 6);
                        }
                        else
                        {
                            UppercutAttack((int)(60 * 1.3f), 30, 800, 5);
                        }
                        break;
                    case 1:
                        ArmSpinAttack(13f, 1f);
                        break;
                    case 2:
                        if (phase == 2)
                        {
                            LaserHell(8, MathHelper.ToRadians(60f * 0.75f), MathHelper.ToRadians(7.5f * 0.75f), (int)(60 * 3.5f));
                        }
                        else
                        {
                            LaserHell(8, MathHelper.ToRadians(60f), MathHelper.ToRadians(7.5f), (int)(60 * 4f));
                        }
                        break;
                    case 3:
                        if (phase == 2)
                        {
                            PredictiveDeathray(0.32f, 80, 90, 45);
                        }
                        else
                        {
                            PredictiveDeathray(0.25f, 120, 60);
                        }
                        break;
                    default:
                        DecideNextAttack();
                        break;
                }
            }

        }

        bool drawingLine;
        Vector2 lineStart;
        float lineDistance;
        float LineRotation;
        Color colorStart;
        Color colorEnd;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            int wingCount = 4;
            for (int i = -wingCount / 2; i < wingCount / 2; i++)
            {
                float rotation = MathHelper.ToRadians(30 * i) - (wingRotation * ((-i + (wingCount / 2) + 1) / 4f));
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/HexedConstructor/HexedWing");
                spriteBatch.Draw(texture, NPC.Center - new Vector2(10, 20) + new Vector2(10 * NPC.spriteDirection, 0) - Main.screenPosition, null, Color.FromNonPremultiplied(255, 255, 255, 255 - NPC.alpha), rotation, new Vector2(texture.Width - 6, texture.Height - 6), 1f, SpriteEffects.None, 0f);
            }

            for (int i = -wingCount / 2; i < wingCount / 2; i++)
            {
                float rotation = -MathHelper.ToRadians(30 * i) + (wingRotation * ((-i + (wingCount / 2) + 1) / 4f));
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/HexedConstructor/HexedWing");
                spriteBatch.Draw(texture, NPC.Center - new Vector2(-10, 20) + new Vector2(10 * NPC.spriteDirection, 0) - Main.screenPosition, null, Color.FromNonPremultiplied(255, 255, 255, 255 - NPC.alpha), rotation, new Vector2(6, texture.Height - 6), 1f, SpriteEffects.FlipHorizontally, 0f);
            }

            SpriteEffects effects = SpriteEffects.FlipHorizontally;
            if (NPC.spriteDirection == -1)
            {
                effects = SpriteEffects.None;
            }
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture), NPC.Center - Main.screenPosition, NPC.frame, Color.FromNonPremultiplied(255, 255, 255, 255 - NPC.alpha), NPC.rotation, NPC.Size / 2, NPC.scale, effects, 0f);

            if (drawingLine)
            {
                Utils.DrawLine(spriteBatch, lineStart, lineStart + lineDistance * LineRotation.ToRotationVector2(), colorStart, colorEnd, 3);
            }
            return false;
        }

        public static void DrawWire(SpriteBatch spriteBatch, Color drawColor, Texture2D texture, Texture2D endTexture, Vector2 startPosition, Vector2 endPosition, int fallAmount, int segmentCount = 25)
        {
            BezierCurve chain = new BezierCurve(new List<Vector2>()
            {
                { startPosition },
                { Vector2.Lerp(startPosition, endPosition, 0.5f) + new Vector2(0, fallAmount) },
                { endPosition }
            });

            List<Vector2> positions = chain.GetPoints(segmentCount);

            for (int i = 0; i < positions.Count - 1; i++)
            {
                spriteBatch.Draw(texture, new Rectangle((int)(positions[i].X - Main.screenPosition.X), (int)(positions[i].Y - Main.screenPosition.Y), texture.Width, (int)(positions[i + 1] - positions[i]).Length() + 2), null, drawColor, (positions[i + 1] - positions[i]).RotatedBy(MathHelper.ToRadians(90)).ToRotation(), texture.Size() / 2, SpriteEffects.None, 0);
            }

            spriteBatch.Draw(texture, new Rectangle((int)(positions[positions.Count - 1].X - Main.screenPosition.X), (int)(positions[positions.Count - 1].Y - Main.screenPosition.Y), texture.Width, (int)(endPosition - positions[positions.Count - 1]).Length() + 2), null, drawColor, (endPosition - positions[positions.Count - 1]).RotatedBy(MathHelper.ToRadians(90)).ToRotation(), texture.Size() / 2, SpriteEffects.None, 0);

            spriteBatch.Draw(endTexture, endPosition - Main.screenPosition, null, drawColor, (endPosition - positions[positions.Count - 1]).RotatedBy(MathHelper.ToRadians(90)).ToRotation(), new Vector2(texture.Width / 2, 0), 1f, SpriteEffects.None, 0);
        }

        float lerpAmount = 0f;
        public void UppercutAttack(int timeUntilAttack, int upperCutTime = 30, float upperCutDistance = 400, int ProjectileCount = 4)
        {
            if (phaseStart)
            {
                attackDirection = -1;
                if (player.Center.X > NPC.Center.X)
                {
                    attackDirection = 1;
                }
                attackTarget = new Vector2(200 * attackDirection, 300);
                phaseStart = false;
                attackWait = (int)(timeUntilAttack * 1.5f);
                lerpAmount = 0f;
                attackWait2 = upperCutTime;

                claw1Offset = claw1.Center - NPC.Center;
                claw2Offset = claw2.Center - NPC.Center;

                ProjectileCounter = upperCutTime / ProjectileCount;

                attackCounter = 2;
            }
            else if (attackWait > 0)
            {
                attackWait--;
                lerpAmount += 1f / (float)timeUntilAttack;
                NPC.velocity = ((player.Center + attackTarget) - NPC.Center) * lerpAmount;

                Vector2 claw1Target = new Vector2(attackDirection * 150, -25);
                claw1.ai[0] = 4;
                claw1.rotation += MathHelper.ToRadians(15 * attackDirection);
                claw1Offset = Vector2.Lerp(claw1Offset, claw1Target, lerpAmount);
                claw1.position = claw1Offset + NPC.Center - (claw1.Size / 2);

                Vector2 claw2Target = new Vector2(attackDirection * -150, 25);
                claw2.ai[0] = 0;
                claw2.rotation = claw2.DirectionTo(player.Center).RotatedBy(MathHelper.ToRadians(90)).ToRotation();
                claw2Offset = Vector2.Lerp(claw2Offset, claw2Target, lerpAmount);
                claw2.position = claw2Offset + NPC.Center - (claw1.Size / 2);

                if (attackWait <= 0)
                {
                    NPC.velocity = NPC.DirectionTo(player.Center) * (upperCutDistance / upperCutTime) * 0.5f;
                    claw2.velocity = claw2.DirectionTo(player.Center) * (upperCutDistance / upperCutTime);
                    claw2.ai[1]++;
                    claw1.ai[1]++;
                }

            }
            else
            {
                claw1.position = claw1Offset + NPC.Center - (claw1.Size / 2);
                claw1.rotation += MathHelper.ToRadians(15 * attackDirection);

                ProjectileCounter--;
                if (ProjectileCounter <= 0)
                {
                    ProjectileCounter = upperCutTime / ProjectileCount;
                    float speed = 20f;
                    if (enraged) 
                        speed *= 1.5f;
                    Vector2 direction = claw1.DirectionTo(player.Center + ((NPC.Distance(player.Center) / speed) * player.velocity));
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), claw1.Center, speed * direction, ModContent.ProjectileType<Projectiles.HellbornLaser>(), 90 / 4, 0);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, claw1.Center);
                }

                attackWait2--;
                if (attackWait2 <= 0)
                {
                    claw2.ai[1] = 0;
                    claw1.ai[1] = 0;
                    attackCounter--;
                    if (attackCounter <= 0)
                    {
                        DecideNextAttack();
                    }
                    else
                    {
                        attackDirection *= -1;

                        attackTarget = new Vector2(150 * attackDirection, 300);
                        phaseStart = false;
                        attackWait = timeUntilAttack;
                        lerpAmount = 0f;
                        attackWait2 = upperCutTime;

                        claw1Offset = claw1.Center - NPC.Center;
                        claw2Offset = claw2.Center - NPC.Center;

                        ProjectileCounter = upperCutTime / ProjectileCount;
                    }
                }
            }
        }

        float movementRotation = 0f;
        int movementSpinDirection = 1;
        float spinSpeedMultiplier = 1f;
        public void ArmSpinAttack(float spinSpeed, float distanceMultiplier, float moveSpeed = 7.5f)
        {
            int baseDistance = 500;
            if (phaseStart)
            {
                attackDirection = 1;
                if (player.Center.X > NPC.Center.X)
                {
                    attackDirection = -1;
                }
                attackTarget = new Vector2(baseDistance * -0.5f * distanceMultiplier * attackDirection, 0);
                phaseStart = false;
                attackWait = 60;
                attackWait2 = 180;
                lerpAmount = 0f;

                ProjectileCounter = 119;

                movementRotation = 0f;

                spinSpeedMultiplier = 1f;

                movementSpinDirection = 1;
                if (Main.rand.NextBool()) movementSpinDirection = -1;

                claw1Offset = claw1.Center - NPC.Center;
                claw2Offset = claw2.Center - NPC.Center;

                attackCounter = 2;
            }
            else if (attackWait > 0)
            {
                attackWait--;
                lerpAmount += 1f / 60f;
                NPC.velocity = ((player.Center + attackTarget) - NPC.Center) * lerpAmount;

                Vector2 claw1Target = new Vector2(baseDistance * distanceMultiplier, -25);
                claw1.ai[0] = 4;
                claw1.rotation += MathHelper.ToRadians(20 * attackDirection);
                claw1Offset = Vector2.Lerp(claw1Offset, claw1Target, lerpAmount);
                claw1.position = claw1Offset + NPC.Center - (claw1.Size / 2);

                Vector2 claw2Target = new Vector2(-baseDistance * distanceMultiplier, -25);
                claw2.ai[0] = 4;
                claw2.rotation += MathHelper.ToRadians(20 * attackDirection);
                claw2Offset = Vector2.Lerp(claw2Offset, claw2Target, lerpAmount);
                claw2.position = claw2Offset + NPC.Center - (claw1.Size / 2);

                if (attackWait <= 0)
                {
                    claw1.ai[1]++;
                    claw2.ai[1]++;

                    claw2.rotation = 0f;
                    claw1.rotation = MathHelper.ToRadians(180f);

                    if (attackDirection == -1)
                    {
                        claw1.rotation += MathHelper.ToRadians(180f);
                        claw2.rotation = MathHelper.ToRadians(180f);
                    }

                    NPC.velocity = new Vector2(moveSpeed * attackDirection, 0f);

                    Vector2 position = NPC.Center - new Vector2(0, NPC.height / 2 - 20);
                    Vector2 direction = (player.Center - position);
                    direction.Normalize();
                    float speed = 5f;
                    if (enraged) speed = 15f;
                    Vector2 velocity = direction * speed;
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), position, velocity, ModContent.ProjectileType<Projectiles.HellbornLaser>(), (int)(85 * enrageDamageMultiplier) / 4, 0);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, position);
                }
            }
            else
            {
                attackWait2--;

                claw1Offset = claw1Offset.RotatedBy(MathHelper.ToRadians(spinSpeed * spinSpeedMultiplier) * attackDirection);
                claw1Offset = (claw1Offset.Length() - 2.5f) * claw1Offset.ToRotation().ToRotationVector2();
                claw2Offset = claw2Offset.RotatedBy(MathHelper.ToRadians(spinSpeed * spinSpeedMultiplier) * attackDirection);
                claw2Offset = (claw2Offset.Length() - 2.5f) * claw2Offset.ToRotation().ToRotationVector2();

                claw2.position = claw2Offset + NPC.Center - (claw1.Size / 2);
                claw1.position = claw1Offset + NPC.Center - (claw1.Size / 2);

                claw1.ai[0] = 1;
                claw2.ai[0] = 1;

                claw1.rotation += MathHelper.ToRadians(spinSpeed * spinSpeedMultiplier) * attackDirection;
                claw2.rotation += MathHelper.ToRadians(spinSpeed * spinSpeedMultiplier) * attackDirection;


                if (enraged)
                {
                    spinSpeedMultiplier -= 0.25f / 180f;
                }
                else
                {
                    spinSpeedMultiplier -= 0.75f / 180f;
                }

                movementRotation += MathHelper.ToRadians(90f / 180f) * movementSpinDirection;

                if (enraged)
                {
                    MoveTowardsPosition(player.Center + new Vector2(100 * distanceMultiplier * attackDirection, 0).RotatedBy(movementRotation), moveSpeed / 15f, moveSpeed * 2, 0.98f);
                }
                else
                {
                    MoveTowardsPosition(player.Center + new Vector2(baseDistance * distanceMultiplier * attackDirection, 0).RotatedBy(movementRotation), moveSpeed / 15f, moveSpeed, 0.98f);
                }

                ProjectileCounter--;
                if (ProjectileCounter <= 0)
                {
                    ProjectileCounter = 20;
                    Vector2 position = NPC.Center - new Vector2(0, NPC.height / 2 - 20);
                    Vector2 direction = (player.Center - position);
                    direction.Normalize();
                    float speed = 15f;
                    if (enraged) speed = 25f;
                    Vector2 velocity = direction * speed;
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), position, velocity, ModContent.ProjectileType<Projectiles.HellbornLaser>(), (int)(85 * enrageDamageMultiplier) / 4, 0);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, position);
                }

                if (attackWait2 <= 0)
                {
                    claw1.ai[1] = 0;
                    claw2.ai[1] = 0;
                    DecideNextAttack();
                }
            }
        }

        float rotation = 0f;
        int armRotationDirection = -1;
        public void LaserHell(int laserCooldown, float maxRotation, float rotationSpeed, int time)
        {
            float clawDistance = 150f;
            if (phaseStart)
            {
                attackDirection = 1;
                if (player.Center.X > NPC.Center.X)
                {
                    attackDirection = -1;
                }
                attackTarget = new Vector2(500 * attackDirection, -400);
                phaseStart = false;
                attackWait = 60;
                attackWait2 = time;
                ProjectileCounter = attackWait / 2;
                lerpAmount = 0f;
                rotation = maxRotation;
                armRotationDirection = -1;
                movementRotation = 0f;

                movementSpinDirection = 1;
                if (Main.rand.NextBool()) movementSpinDirection = -1;

                claw1Offset = claw1.Center - NPC.Center;
                claw2Offset = claw2.Center - NPC.Center;

                attackCounter = 2;
            }
            else if (attackWait > 0)
            {
                attackWait--;

                lerpAmount += 1f / 60f;
                NPC.velocity = ((player.Center + attackTarget) - NPC.Center) * lerpAmount;

                Vector2 claw1Target = NPC.DirectionTo(player.Center).RotatedBy(rotation) * clawDistance;
                claw1.ai[0] = 2;
                claw1Offset = Vector2.Lerp(claw1Offset, claw1Target, lerpAmount);
                claw1.position = claw1Offset + NPC.Center - (claw1.Size / 2);

                Vector2 claw2Target = NPC.DirectionTo(player.Center).RotatedBy(-rotation) * clawDistance;
                claw2.ai[0] = 2;
                claw2Offset = Vector2.Lerp(claw2Offset, claw2Target, lerpAmount);
                claw2.position = claw2Offset + NPC.Center - (claw1.Size / 2);

                claw1.rotation = NPC.DirectionTo(player.Center).RotatedBy(rotation).ToRotation() + MathHelper.ToRadians(90);
                claw2.rotation = NPC.DirectionTo(player.Center).RotatedBy(-rotation).ToRotation() + MathHelper.ToRadians(90);

                ProjectileCounter--;
                if (ProjectileCounter <= 0)
                {
                    ProjectileCounter = laserCooldown;
                    float speed = 15f;
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), claw1.Center, NPC.DirectionTo(player.Center).RotatedBy(rotation) * speed, ModContent.ProjectileType<Projectiles.HellbornLaser>(), (int)(82 * enrageDamageMultiplier) / 4, 0);
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), claw2.Center, NPC.DirectionTo(player.Center).RotatedBy(-rotation) * speed, ModContent.ProjectileType<Projectiles.HellbornLaser>(), (int)(82 * enrageDamageMultiplier) / 4, 0);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item125, NPC.Center);
                }

                if (attackWait <= 0)
                {
                    claw1.ai[1]++;
                    claw2.ai[1]++;
                }
            }
            else if (attackWait2 > 0)
            {
                attackWait2--;

                if (enraged)
                {
                    MoveTowardsPosition(player.Center + (NPC.DirectionFrom(player.Center) * clawDistance), 0.3f, 8.5f, 1f);
                }
                else
                {
                    MoveTowardsPosition(player.Center + (NPC.DirectionFrom(player.Center) * clawDistance), 0.5f, 4f, 0.99f);
                }

                rotation += rotationSpeed * armRotationDirection;

                if (Math.Abs(rotation) > maxRotation)
                {
                    armRotationDirection *= -1;
                    rotation += rotationSpeed * armRotationDirection;
                }

                claw1.rotation = NPC.DirectionTo(player.Center).RotatedBy(rotation).ToRotation() + MathHelper.ToRadians(90);
                claw2.rotation = NPC.DirectionTo(player.Center).RotatedBy(-rotation).ToRotation() + MathHelper.ToRadians(90);

                claw1Offset = NPC.DirectionTo(player.Center).RotatedBy(rotation) * clawDistance;
                claw2Offset = NPC.DirectionTo(player.Center).RotatedBy(-rotation) * clawDistance;

                claw1.position = claw1Offset + NPC.Center - (claw1.Size / 2);
                claw2.position = claw2Offset + NPC.Center - (claw1.Size / 2);

                ProjectileCounter--;
                if (ProjectileCounter <= 0)
                {
                    ProjectileCounter = laserCooldown;
                    float speed = 15f;
                    if (enraged) speed = 20f;
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), claw1.Center, NPC.DirectionTo(player.Center).RotatedBy(rotation) * speed, ModContent.ProjectileType<Projectiles.HellbornLaser>(), (int)(75 * enrageDamageMultiplier) / 4, 0);
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), claw2.Center, NPC.DirectionTo(player.Center).RotatedBy(-rotation) * speed, ModContent.ProjectileType<Projectiles.HellbornLaser>(), (int)(75 * enrageDamageMultiplier) / 4, 0);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item125, NPC.Center);
                }
            }
            else
            {
                claw1.ai[1] = 0;
                claw2.ai[1] = 0;
                DecideNextAttack();
            }
        }

        float deathrayRotation = 0f;
        public void PredictiveDeathray(float rotateSpeed, int timeUntilDeathray, int deathrayTime, int deathrayDistance = 60, float backOffSpeed = 15)
        {
            int clawDistance = 200;
            if (phaseStart)
            {
                phaseStart = false;
                attackWait = timeUntilDeathray;
                attackWait2 = deathrayTime;
                ProjectileCounter = timeUntilDeathray / 3;

                lerpAmount = 0f;

                claw1Offset = claw1.Center - NPC.Center;
                claw2Offset = claw2.Center - NPC.Center;

                NPC.velocity = NPC.DirectionFrom(player.Center) * backOffSpeed;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 0, 1, 2f);

                deathrayRotation = NPC.DirectionTo(player.Center).ToRotation();

            }
            else if (attackWait > 0)
            {
                MoveTowardsPosition(player.Center, 0.1f, backOffSpeed, 0.98f);

                lerpAmount += 1f / (float)timeUntilDeathray;

                ProjectileCounter--;
                if (ProjectileCounter <= 0)
                {
                    ProjectileCounter = timeUntilDeathray / 3;
                    DustExplosion(lineStart, 0, 25, 30, 235, 2f, true);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.MaxMana, (int)NPC.Center.X, (int)NPC.Center.Y, 0, 3f, -0.25f);
                }

                Vector2 claw1Target = new Vector2(clawDistance, -25);
                claw1.ai[0] = 4;
                claw1.rotation += MathHelper.ToRadians(20);
                claw1Offset = Vector2.Lerp(claw1Offset, claw1Target, lerpAmount);
                claw1.position = claw1Offset + NPC.Center - (claw1.Size / 2);

                Vector2 claw2Target = new Vector2(-clawDistance, -25);
                claw2.ai[0] = 4;
                claw2.rotation += MathHelper.ToRadians(20);
                claw2Offset = Vector2.Lerp(claw2Offset, claw2Target, lerpAmount);
                claw2.position = claw2Offset + NPC.Center - (claw1.Size / 2);

                lineStart = NPC.Center - new Vector2(0, NPC.height / 2 - 20);
                colorStart = Color.Red;
                deathrayRotation = deathrayRotation.AngleTowards(((player.Center + player.velocity * deathrayDistance) - lineStart).ToRotation(), MathHelper.ToRadians(6f));
                LineRotation = deathrayRotation;
                colorEnd = Color.Transparent;
                lineDistance = 2000f;
                drawingLine = true;

                attackWait--;
                if (attackWait <= 0)
                {
                    drawingLine = false;
                    claw1.ai[1]++;
                    claw2.ai[1]++;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie, (int)NPC.Center.X, (int)NPC.Center.Y, 104, 1, 2f);
                }
            }
            else if (attackWait2 > 0)
            {
                TerrorbornSystem.ScreenShake(2f);

                MoveTowardsPosition(player.Center, 0.1f, backOffSpeed, 0.98f);

                float clawRotateSpeed = 15f;
                claw1Offset = claw1Offset.RotatedBy(MathHelper.ToRadians(clawRotateSpeed));
                claw1.rotation += MathHelper.ToRadians(clawRotateSpeed * 2);
                claw1.position = claw1Offset + NPC.Center - (claw1.Size / 2);
                claw2Offset = claw2Offset.RotatedBy(MathHelper.ToRadians(clawRotateSpeed));
                claw2.rotation += MathHelper.ToRadians(clawRotateSpeed * 2);
                claw2.position = claw2Offset + NPC.Center - (claw2.Size / 2);

                Vector2 deathrayPosition = NPC.Center - new Vector2(0, NPC.height / 2 - 20);
                Projectile proj = Main.projectile[Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), deathrayPosition, deathrayRotation.ToRotationVector2(), ModContent.ProjectileType<ClockworkDeathray>(), (int)(80 * enrageDamageMultiplier) / 4, 0)];
                proj.ai[0] = NPC.whoAmI;
                proj.ai[1] = -(NPC.height / 2 - 20);
                deathrayRotation = deathrayRotation.AngleTowards((player.Center - deathrayPosition).ToRotation(), MathHelper.ToRadians(rotateSpeed));
                attackWait2--;
            }
            else
            {
                claw1.ai[1] = 0;
                claw2.ai[1] = 0;
                DecideNextAttack();
            }

            if (deathrayRotation.ToRotationVector2().X > 0)
            {
                NPC.spriteDirection = -1;
            }
            else
            {
                NPC.spriteDirection = 1;
            }
        }
    }

    public class HexedClaw : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 82;
            NPC.height = 76;
            NPC.damage = 40;
            NPC.HitSound = SoundID.NPCHit41;
            NPC.DeathSound = SoundID.NPCDeath39;
            NPC.defense = 30;
            NPC.frame.Height = 254;
            NPC.lifeMax = 22500;
            NPC.knockBackResist = 0;
            NPC.aiStyle = -1;
            NPC.dontTakeDamage = true;
        }

        public override void ModifyHitByProjectile(Projectile Projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage /= 2;

            if (Projectile.type == ModContent.ProjectileType<Items.Weapons.Magic.SpiralSoul>())
            {
                damage /= 4;
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.ai[1] != 0;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = (int)NPC.ai[0] * frameHeight;
        }

        public override void AI()
        {
            if (NPC.realLife != NPC.whoAmI && NPC.realLife != 0)
            {
                NPC boss = Main.npc[NPC.realLife];
                NPC.dontTakeDamage = boss.dontTakeDamage;

                if (NPC.life <= 0 || !boss.active)
                {
                    NPC.active = false;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.ai[0] == 4)
            {
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture), NPC.Center - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, new Vector2(NPC.width / 2, 43), NPC.scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture), NPC.Center - Main.screenPosition, NPC.frame, Color.White, NPC.rotation, new Vector2(NPC.width / 2, 55), NPC.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
    }

    class ClockworkDeathray : Deathray
    {
        public override string Texture => "TerrorbornMod/Projectiles/IncendiaryDeathray";
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 22;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = false;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 2;
            MoveDistance = 20f;
            RealMaxDistance = 3000f;
            bodyRect = new Rectangle(0, 24, 18, 22);
            headRect = new Rectangle(0, 0, 18, 22);
            tailRect = new Rectangle(0, 46, 18, 22);
        }

        public override Vector2 Position()
        {
            return Main.npc[(int)Projectile.ai[0]].Center + new Vector2(0, Projectile.ai[1]);
        }
    }
}
