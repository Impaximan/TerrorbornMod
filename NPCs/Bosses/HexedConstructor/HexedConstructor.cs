using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Effects;
using TerrorbornMod.Abilities;
using TerrorbornMod.ForegroundObjects;
using Terraria.Graphics.Shaders;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Input;
using Extensions;
using Terraria.Utilities;
using TerrorbornMod.Projectiles;


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

        public override void NPCLoot()
        {
            if (!TerrorbornWorld.downedIncendiaryBoss)
            {
                TerrorbornWorld.downedIncendiaryBoss = true;
                Vector2 terrorMasterPosition = new Vector2(Main.spawnTileX * 16, Main.spawnTileY * 16);
                NPC.NewNPC((int)terrorMasterPosition.X, (int)terrorMasterPosition.Y, ModContent.NPCType<NPCs.TownNPCs.Heretic>());
                Main.NewText("Gabrielle the Heretic has invited herself to your town!", new Color(50, 125, 255));
            }

            bool spawnTF = !TerrorbornPlayer.modPlayer(Main.player[Main.myPlayer]).unlockedAbilities.Contains(7);
            for (int i = 0; i < 1000; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.type == ModContent.ProjectileType<TimeFreeze>())
                {
                    spawnTF = false;
                }
            }

            if (spawnTF)
            {
                Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<TimeFreeze>(), 0, 0, Main.myPlayer);
            }

            if (Main.expertMode)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.TreasureBags.HC_TreasureBag>());
            }
            else
            {

            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 10;
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
            name = "A hexed constructor";
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 68;
            npc.height = 152;
            npc.damage = 60;
            npc.HitSound = SoundID.NPCHit41;
            npc.defense = 10;
            npc.DeathSound = SoundID.NPCDeath39;
            npc.boss = true;
            npc.frame.Height = 254;
            npc.lifeMax = 20000;
            npc.knockBackResist = 0;
            npc.aiStyle = -1;
            npc.alpha = 255;
            npc.dontTakeDamage = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/HexedConstructor");

            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(npc);
            modNPC.BossTitle = "Hexed Constructor";
            modNPC.BossSubtitle = "Possessed Builder of the Seal";
            modNPC.BossTitleColor = new Color(191, 82, 58);
        }

        int frame = 0;
        int frameTarget = 0;
        int targetChangeCounter = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter--;
            if (npc.frameCounter <= 0)
            {
                npc.frameCounter = 5;

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
                npc.frame.Y = frame * frameHeight;
            }
            else
            {
                npc.frame.Y = (frame + 5) * frameHeight;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            Main.PlaySound(SoundID.NPCHit54, npc.Center);
        }

        public void MoveTowardsPosition(Vector2 position, float speed, float maxSpeed, float dragMultiplier = 0.98f)
        {
            npc.velocity += npc.DirectionTo(position) * speed;

            if (npc.velocity.Length() > maxSpeed)
            {
                npc.velocity = npc.velocity.ToRotation().ToRotationVector2() * maxSpeed;
            }

            npc.velocity *= dragMultiplier;
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
            npc.TargetClosest(false);
            if (doingMelee)
            {
                npc.damage = baseDamage;
            }
            else
            {
                npc.damage = 0;
            }

            player = Main.player[npc.target];
            modPlayer = TerrorbornPlayer.modPlayer(player);

            if (player.Center.X > npc.Center.X)
            {
                npc.spriteDirection = -1;
            }
            else
            {
                npc.spriteDirection = 1;
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
            npc.TargetClosest(false);
            player = Main.player[npc.target];

            baseDamage = npc.damage;
            npc.position = player.Center + new Vector2(0, -250) - npc.Size / 2;
            npc.dontTakeDamage = false;
            transitioning = true;
            npc.velocity = new Vector2(0, -5);
            TerrorbornMod.SetScreenToPosition(60 * 3, 20, npc.Center + new Vector2(0, -75), 0.8f);
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
        int projectileCounter = 0;
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
            if (phase == 1 && npc.life <= secondPhaseHealth * npc.lifeMax)
            {
                int totalTime = 30 * 4;
                transitioning = true;
                transitionTime = totalTime;
                npc.velocity = Vector2.Zero;
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

        public override void AI()
        {
            npc.ai[0]++;

            if (start)
            {
                OnStart();
                start = false;
            }

            SetStats();
            RotateWings(MathHelper.ToRadians(3), MathHelper.ToRadians(25));

            if (phase == 2)
            {
                TerrorbornPlayer.modPlayer(player).HexedMirage = true;
            }

            if (transitioning)
            {
                if (firstTransition)
                {
                    if (npc.alpha > 0)
                    {
                        npc.alpha -= 5;
                        npc.velocity *= 0.98f;
                    }
                    else
                    {
                        if (!NPC.AnyNPCs(ModContent.NPCType<HexedClaw>()))
                        {
                            claw1 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 20, ModContent.NPCType<HexedClaw>())];
                            claw2 = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 20, ModContent.NPCType<HexedClaw>())];

                            claw1.realLife = npc.whoAmI;
                            claw2.realLife = npc.whoAmI;

                            claw1.ai[0] = 4;
                            claw2.ai[0] = 4;

                            claw1.velocity.X = -15;
                            claw2.velocity.X = 15;
                        }
                        npc.velocity *= 0.93f;
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
                        npc.velocity = Vector2.Zero;
                        transitionTime--;

                        claw1.ai[0] = 4;
                        claw2.ai[0] = 4;
                        claw1.rotation = 0f;
                        claw2.rotation = 0f;
                        claw1.velocity = (new Vector2(npc.Center.X + 300, npc.Center.Y) - claw1.Center) / 10;
                        claw2.velocity = (new Vector2(npc.Center.X - 300, npc.Center.Y) - claw2.Center) / 10;

                        if (transitionTime == 5 * (int)(transitionTime / 5))
                        {
                            Main.PlaySound(SoundID.Item14);
                        }
                        Dust.NewDust(npc.Center + new Vector2(-10, -npc.height / 2 + 10), 20, 20, DustID.Fire);
                        TerrorbornMod.ScreenShake(2f);
                    }
                    else
                    {
                        phase++;
                        transitioning = false;
                        npc.dontTakeDamage = false;
                        DecideNextAttack();
                        DustExplosion(npc.Center + new Vector2(0, -npc.height / 2 + 20), 0, 25, 20f, DustID.Fire, 1.5f, true);
                        TerrorbornMod.ScreenShake(10f);
                        Main.PlaySound(SoundID.Zombie, (int)npc.Center.X, (int)npc.Center.Y, 105, 1, 0);
                    }
                }
            }
            else
            {
                if (phase == 1 && npc.life <= secondPhaseHealth * npc.lifeMax)
                {
                    npc.dontTakeDamage = true;
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

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            int wingCount = 4;
            for (int i = -wingCount / 2; i < wingCount / 2; i++)
            {
                float rotation = MathHelper.ToRadians(30 * i) - (wingRotation * ((-i + (wingCount / 2) + 1) / 4f));
                Texture2D texture = ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/HexedConstructor/HexedWing");
                spriteBatch.Draw(texture, npc.Center - new Vector2(10, 20) + new Vector2(10 * npc.spriteDirection, 0) - Main.screenPosition, null, Color.FromNonPremultiplied(255, 255, 255, 255 - npc.alpha), rotation, new Vector2(texture.Width - 6, texture.Height - 6), 1f, SpriteEffects.None, 0f);
            }

            for (int i = -wingCount / 2; i < wingCount / 2; i++)
            {
                float rotation = -MathHelper.ToRadians(30 * i) + (wingRotation * ((-i + (wingCount / 2) + 1) / 4f));
                Texture2D texture = ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/HexedConstructor/HexedWing");
                spriteBatch.Draw(texture, npc.Center - new Vector2(-10, 20) + new Vector2(10 * npc.spriteDirection, 0) - Main.screenPosition, null, Color.FromNonPremultiplied(255, 255, 255, 255 - npc.alpha), rotation, new Vector2(6, texture.Height - 6), 1f, SpriteEffects.FlipHorizontally, 0f);
            }

            SpriteEffects effects = SpriteEffects.FlipHorizontally;
            if (npc.spriteDirection == -1)
            {
                effects = SpriteEffects.None;
            }
            spriteBatch.Draw(ModContent.GetTexture(Texture), npc.Center - Main.screenPosition, npc.frame, Color.FromNonPremultiplied(255, 255, 255, 255 - npc.alpha), npc.rotation, npc.Size / 2, npc.scale, effects, 0f);

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
        public void UppercutAttack(int timeUntilAttack, int upperCutTime = 30, float upperCutDistance = 400, int projectileCount = 4)
        {
            if (phaseStart)
            {
                attackDirection = -1;
                if (player.Center.X > npc.Center.X)
                {
                    attackDirection = 1;
                }
                attackTarget = new Vector2(200 * attackDirection, 300);
                phaseStart = false;
                attackWait = (int)(timeUntilAttack * 1.5f);
                lerpAmount = 0f;
                attackWait2 = upperCutTime;

                claw1Offset = claw1.Center - npc.Center;
                claw2Offset = claw2.Center - npc.Center;

                projectileCounter = upperCutTime / projectileCount;

                attackCounter = 2;
            }
            else if (attackWait > 0)
            {
                attackWait--;
                lerpAmount += 1f / (float)timeUntilAttack;
                npc.velocity = ((player.Center + attackTarget) - npc.Center) * lerpAmount;

                Vector2 claw1Target = new Vector2(attackDirection * 150, -25);
                claw1.ai[0] = 4;
                claw1.rotation += MathHelper.ToRadians(15 * attackDirection);
                claw1Offset = Vector2.Lerp(claw1Offset, claw1Target, lerpAmount);
                claw1.position = claw1Offset + npc.Center - (claw1.Size / 2);

                Vector2 claw2Target = new Vector2(attackDirection * -150, 25);
                claw2.ai[0] = 0;
                claw2.rotation = claw2.DirectionTo(player.Center).RotatedBy(MathHelper.ToRadians(90)).ToRotation();
                claw2Offset = Vector2.Lerp(claw2Offset, claw2Target, lerpAmount);
                claw2.position = claw2Offset + npc.Center - (claw1.Size / 2);

                if (attackWait <= 0)
                {
                    npc.velocity = npc.DirectionTo(player.Center) * (upperCutDistance / upperCutTime) * 0.5f;
                    claw2.velocity = claw2.DirectionTo(player.Center) * (upperCutDistance / upperCutTime);
                    claw2.ai[1]++;
                    claw1.ai[1]++;
                }

            }
            else
            {
                claw1.position = claw1Offset + npc.Center - (claw1.Size / 2);
                claw1.rotation += MathHelper.ToRadians(15 * attackDirection);

                projectileCounter--;
                if (projectileCounter <= 0)
                {
                    projectileCounter = upperCutTime / projectileCount;
                    float speed = 20f;
                    if (enraged) 
                        speed *= 1.5f;
                    Vector2 direction = claw1.DirectionTo(player.Center + ((npc.Distance(player.Center) / speed) * player.velocity));
                    Projectile.NewProjectile(claw1.Center, speed * direction, ModContent.ProjectileType<Projectiles.HellbornLaser>(), 90 / 4, 0);
                    Main.PlaySound(SoundID.Item33, claw1.Center);
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

                        claw1Offset = claw1.Center - npc.Center;
                        claw2Offset = claw2.Center - npc.Center;

                        projectileCounter = upperCutTime / projectileCount;
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
                if (player.Center.X > npc.Center.X)
                {
                    attackDirection = -1;
                }
                attackTarget = new Vector2(baseDistance * -0.5f * distanceMultiplier * attackDirection, 0);
                phaseStart = false;
                attackWait = 60;
                attackWait2 = 180;
                lerpAmount = 0f;

                projectileCounter = 119;

                movementRotation = 0f;

                spinSpeedMultiplier = 1f;

                movementSpinDirection = 1;
                if (Main.rand.NextBool()) movementSpinDirection = -1;

                claw1Offset = claw1.Center - npc.Center;
                claw2Offset = claw2.Center - npc.Center;

                attackCounter = 2;
            }
            else if (attackWait > 0)
            {
                attackWait--;
                lerpAmount += 1f / 60f;
                npc.velocity = ((player.Center + attackTarget) - npc.Center) * lerpAmount;

                Vector2 claw1Target = new Vector2(baseDistance * distanceMultiplier, -25);
                claw1.ai[0] = 4;
                claw1.rotation += MathHelper.ToRadians(20 * attackDirection);
                claw1Offset = Vector2.Lerp(claw1Offset, claw1Target, lerpAmount);
                claw1.position = claw1Offset + npc.Center - (claw1.Size / 2);

                Vector2 claw2Target = new Vector2(-baseDistance * distanceMultiplier, -25);
                claw2.ai[0] = 4;
                claw2.rotation += MathHelper.ToRadians(20 * attackDirection);
                claw2Offset = Vector2.Lerp(claw2Offset, claw2Target, lerpAmount);
                claw2.position = claw2Offset + npc.Center - (claw1.Size / 2);

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

                    npc.velocity = new Vector2(moveSpeed * attackDirection, 0f);

                    Vector2 position = npc.Center - new Vector2(0, npc.height / 2 - 20);
                    Vector2 direction = (player.Center - position);
                    direction.Normalize();
                    float speed = 5f;
                    if (enraged) speed = 15f;
                    Vector2 velocity = direction * speed;
                    Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<Projectiles.HellbornLaser>(), (int)(85 * enrageDamageMultiplier) / 4, 0);
                    Main.PlaySound(SoundID.Item33, position);
                }
            }
            else
            {
                attackWait2--;

                claw1Offset = claw1Offset.RotatedBy(MathHelper.ToRadians(spinSpeed * spinSpeedMultiplier) * attackDirection);
                claw1Offset = (claw1Offset.Length() - 2.5f) * claw1Offset.ToRotation().ToRotationVector2();
                claw2Offset = claw2Offset.RotatedBy(MathHelper.ToRadians(spinSpeed * spinSpeedMultiplier) * attackDirection);
                claw2Offset = (claw2Offset.Length() - 2.5f) * claw2Offset.ToRotation().ToRotationVector2();

                claw2.position = claw2Offset + npc.Center - (claw1.Size / 2);
                claw1.position = claw1Offset + npc.Center - (claw1.Size / 2);

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

                projectileCounter--;
                if (projectileCounter <= 0)
                {
                    projectileCounter = 20;
                    Vector2 position = npc.Center - new Vector2(0, npc.height / 2 - 20);
                    Vector2 direction = (player.Center - position);
                    direction.Normalize();
                    float speed = 15f;
                    if (enraged) speed = 25f;
                    Vector2 velocity = direction * speed;
                    Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<Projectiles.HellbornLaser>(), (int)(85 * enrageDamageMultiplier) / 4, 0);
                    Main.PlaySound(SoundID.Item33, position);
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
                if (player.Center.X > npc.Center.X)
                {
                    attackDirection = -1;
                }
                attackTarget = new Vector2(500 * attackDirection, -400);
                phaseStart = false;
                attackWait = 60;
                attackWait2 = time;
                projectileCounter = attackWait / 2;
                lerpAmount = 0f;
                rotation = maxRotation;
                armRotationDirection = -1;
                movementRotation = 0f;

                movementSpinDirection = 1;
                if (Main.rand.NextBool()) movementSpinDirection = -1;

                claw1Offset = claw1.Center - npc.Center;
                claw2Offset = claw2.Center - npc.Center;

                attackCounter = 2;
            }
            else if (attackWait > 0)
            {
                attackWait--;

                lerpAmount += 1f / 60f;
                npc.velocity = ((player.Center + attackTarget) - npc.Center) * lerpAmount;

                Vector2 claw1Target = npc.DirectionTo(player.Center).RotatedBy(rotation) * clawDistance;
                claw1.ai[0] = 2;
                claw1Offset = Vector2.Lerp(claw1Offset, claw1Target, lerpAmount);
                claw1.position = claw1Offset + npc.Center - (claw1.Size / 2);

                Vector2 claw2Target = npc.DirectionTo(player.Center).RotatedBy(-rotation) * clawDistance;
                claw2.ai[0] = 2;
                claw2Offset = Vector2.Lerp(claw2Offset, claw2Target, lerpAmount);
                claw2.position = claw2Offset + npc.Center - (claw1.Size / 2);

                claw1.rotation = npc.DirectionTo(player.Center).RotatedBy(rotation).ToRotation() + MathHelper.ToRadians(90);
                claw2.rotation = npc.DirectionTo(player.Center).RotatedBy(-rotation).ToRotation() + MathHelper.ToRadians(90);

                projectileCounter--;
                if (projectileCounter <= 0)
                {
                    projectileCounter = laserCooldown;
                    float speed = 15f;
                    Projectile.NewProjectile(claw1.Center, npc.DirectionTo(player.Center).RotatedBy(rotation) * speed, ModContent.ProjectileType<Projectiles.HellbornLaser>(), (int)(82 * enrageDamageMultiplier) / 4, 0);
                    Projectile.NewProjectile(claw2.Center, npc.DirectionTo(player.Center).RotatedBy(-rotation) * speed, ModContent.ProjectileType<Projectiles.HellbornLaser>(), (int)(82 * enrageDamageMultiplier) / 4, 0);
                    Main.PlaySound(SoundID.Item125, npc.Center);
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
                    MoveTowardsPosition(player.Center + (npc.DirectionFrom(player.Center) * clawDistance), 0.3f, 8.5f, 1f);
                }
                else
                {
                    MoveTowardsPosition(player.Center + (npc.DirectionFrom(player.Center) * clawDistance), 0.5f, 4f, 0.99f);
                }

                rotation += rotationSpeed * armRotationDirection;

                if (Math.Abs(rotation) > maxRotation)
                {
                    armRotationDirection *= -1;
                    rotation += rotationSpeed * armRotationDirection;
                }

                claw1.rotation = npc.DirectionTo(player.Center).RotatedBy(rotation).ToRotation() + MathHelper.ToRadians(90);
                claw2.rotation = npc.DirectionTo(player.Center).RotatedBy(-rotation).ToRotation() + MathHelper.ToRadians(90);

                claw1Offset = npc.DirectionTo(player.Center).RotatedBy(rotation) * clawDistance;
                claw2Offset = npc.DirectionTo(player.Center).RotatedBy(-rotation) * clawDistance;

                claw1.position = claw1Offset + npc.Center - (claw1.Size / 2);
                claw2.position = claw2Offset + npc.Center - (claw1.Size / 2);

                projectileCounter--;
                if (projectileCounter <= 0)
                {
                    projectileCounter = laserCooldown;
                    float speed = 15f;
                    if (enraged) speed = 20f;
                    Projectile.NewProjectile(claw1.Center, npc.DirectionTo(player.Center).RotatedBy(rotation) * speed, ModContent.ProjectileType<Projectiles.HellbornLaser>(), (int)(75 * enrageDamageMultiplier) / 4, 0);
                    Projectile.NewProjectile(claw2.Center, npc.DirectionTo(player.Center).RotatedBy(-rotation) * speed, ModContent.ProjectileType<Projectiles.HellbornLaser>(), (int)(75 * enrageDamageMultiplier) / 4, 0);
                    Main.PlaySound(SoundID.Item125, npc.Center);
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
                projectileCounter = timeUntilDeathray / 3;

                lerpAmount = 0f;

                claw1Offset = claw1.Center - npc.Center;
                claw2Offset = claw2.Center - npc.Center;

                npc.velocity = npc.DirectionFrom(player.Center) * backOffSpeed;
                Main.PlaySound(SoundID.Roar, (int)npc.Center.X, (int)npc.Center.Y, 0, 1, 2f);

                deathrayRotation = npc.DirectionTo(player.Center).ToRotation();

            }
            else if (attackWait > 0)
            {
                MoveTowardsPosition(player.Center, 0.1f, backOffSpeed, 0.98f);

                lerpAmount += 1f / (float)timeUntilDeathray;

                projectileCounter--;
                if (projectileCounter <= 0)
                {
                    projectileCounter = timeUntilDeathray / 3;
                    DustExplosion(lineStart, 0, 25, 30, 235, 2f, true);
                    Main.PlaySound(SoundID.MaxMana, (int)npc.Center.X, (int)npc.Center.Y, 0, 3f, -0.25f);
                }

                Vector2 claw1Target = new Vector2(clawDistance, -25);
                claw1.ai[0] = 4;
                claw1.rotation += MathHelper.ToRadians(20);
                claw1Offset = Vector2.Lerp(claw1Offset, claw1Target, lerpAmount);
                claw1.position = claw1Offset + npc.Center - (claw1.Size / 2);

                Vector2 claw2Target = new Vector2(-clawDistance, -25);
                claw2.ai[0] = 4;
                claw2.rotation += MathHelper.ToRadians(20);
                claw2Offset = Vector2.Lerp(claw2Offset, claw2Target, lerpAmount);
                claw2.position = claw2Offset + npc.Center - (claw1.Size / 2);

                lineStart = npc.Center - new Vector2(0, npc.height / 2 - 20);
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
                    Main.PlaySound(SoundID.Zombie, (int)npc.Center.X, (int)npc.Center.Y, 104, 1, 2f);
                }
            }
            else if (attackWait2 > 0)
            {
                TerrorbornMod.ScreenShake(2f);

                MoveTowardsPosition(player.Center, 0.1f, backOffSpeed, 0.98f);

                float clawRotateSpeed = 15f;
                claw1Offset = claw1Offset.RotatedBy(MathHelper.ToRadians(clawRotateSpeed));
                claw1.rotation += MathHelper.ToRadians(clawRotateSpeed * 2);
                claw1.position = claw1Offset + npc.Center - (claw1.Size / 2);
                claw2Offset = claw2Offset.RotatedBy(MathHelper.ToRadians(clawRotateSpeed));
                claw2.rotation += MathHelper.ToRadians(clawRotateSpeed * 2);
                claw2.position = claw2Offset + npc.Center - (claw2.Size / 2);

                Vector2 deathrayPosition = npc.Center - new Vector2(0, npc.height / 2 - 20);
                Projectile proj = Main.projectile[Projectile.NewProjectile(deathrayPosition, deathrayRotation.ToRotationVector2(), ModContent.ProjectileType<ClockworkDeathray>(), (int)(80 * enrageDamageMultiplier) / 4, 0)];
                proj.ai[0] = npc.whoAmI;
                proj.ai[1] = -(npc.height / 2 - 20);
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
                npc.spriteDirection = -1;
            }
            else
            {
                npc.spriteDirection = 1;
            }
        }
    }

    public class HexedClaw : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 82;
            npc.height = 76;
            npc.damage = 40;
            npc.HitSound = SoundID.NPCHit41;
            npc.DeathSound = SoundID.NPCDeath39;
            npc.defense = 30;
            npc.frame.Height = 254;
            npc.lifeMax = 22500;
            npc.knockBackResist = 0;
            npc.aiStyle = -1;
            npc.dontTakeDamage = true;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage /= 2;

            if (projectile.type == ModContent.ProjectileType<Items.Weapons.Magic.SpiralSoul>())
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
            return npc.ai[1] != 0;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = (int)npc.ai[0] * frameHeight;
        }

        public override void AI()
        {
            if (npc.realLife != npc.whoAmI && npc.realLife != 0)
            {
                NPC boss = Main.npc[npc.realLife];
                npc.dontTakeDamage = boss.dontTakeDamage;

                if (npc.life <= 0 || !boss.active)
                {
                    npc.active = false;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (npc.ai[0] == 4)
            {
                spriteBatch.Draw(ModContent.GetTexture(Texture), npc.Center - Main.screenPosition, npc.frame, Color.White, npc.rotation, new Vector2(npc.width / 2, 43), npc.scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(ModContent.GetTexture(Texture), npc.Center - Main.screenPosition, npc.frame, Color.White, npc.rotation, new Vector2(npc.width / 2, 55), npc.scale, SpriteEffects.None, 0f);
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
            projectile.width = 18;
            projectile.height = 22;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.hide = false;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.timeLeft = 2;
            MoveDistance = 20f;
            RealMaxDistance = 3000f;
            bodyRect = new Rectangle(0, 24, 18, 22);
            headRect = new Rectangle(0, 0, 18, 22);
            tailRect = new Rectangle(0, 46, 18, 22);
        }

        public override Vector2 Position()
        {
            return Main.npc[(int)projectile.ai[0]].Center + new Vector2(0, projectile.ai[1]);
        }
    }
}
