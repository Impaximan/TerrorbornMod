using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Liquid;
using Terraria.World.Generation;
using Terraria.Utilities;

namespace TerrorbornMod.NPCs.Bosses.TidalTitan
{
    class MysteriousCrab : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 17;
        }

        public override void SetDefaults()
        {
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.width = 128;
            npc.height = 78;
            npc.damage = 0;
            npc.HitSound = SoundID.NPCHit38;
            npc.defense = 9;
            npc.DeathSound = SoundID.NPCDeath41;
            Main.raining = true;
            npc.frame.Width = 388;
            npc.frame.Height = 254;
            npc.lifeMax = 500;
            npc.rarity = 10;
            npc.knockBackResist = 0;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Ichor] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 300;
            
        }

        public override void AI()
        {
            if (npc.wet)
            {
                if (npc.velocity.Y > -10)
                {
                    npc.velocity.Y -= 0.1f;
                }
                npc.noGravity = true;
            }
            else
            {
                npc.noGravity = false;
            }
            if (npc.velocity.Y == 0)
            {
                npc.frame.Y = 0;
            }
            else
            {
                npc.frame.Y = 7 * 78;
            }

            npc.velocity.Y *= 0.95f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                npc.life = 1;
                npc.dontTakeDamage = true;
                NPC.NewNPC((int)npc.Center.X + 700, (int)npc.Center.Y + 600, ModContent.NPCType<TidalTitan>());
                Main.NewText("Tidal Titan has awoken!", new Color(175, 75, 255));
                Main.PlaySound(SoundID.Roar, (int)npc.position.X, (int)npc.position.Y, 0);
                TerrorbornMod.ScreenShake(10f);
            }
        }

        public override bool PreNPCLoot()
        {
            return false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.dayTime && !NPC.AnyNPCs(mod.NPCType("TidalTitan")) && !NPC.AnyNPCs(mod.NPCType("TidalTitanIdle")))
            {
                if (NPC.downedBoss2 && !TerrorbornWorld.downedTidalTitan)
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
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/TidalTitan");
            npc.width = 278;
            npc.height = 88;
            npc.damage = 60;
            npc.HitSound = SoundID.DD2_WitherBeastCrystalImpact;
            npc.defense = 5;
            npc.DeathSound = SoundID.NPCDeath14;
            Main.raining = true;
            npc.frame.Width = 388;
            npc.frame.Height = 254;
            npc.lifeMax = 2500;
            npc.knockBackResist = 0;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Ichor] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
            npc.aiStyle = -1;
            npc.alpha = 255;

            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(npc);
            modNPC.BossTitle = "Tidal Titan";
            modNPC.BossSubtitle = "Terror of the Ocean";
            modNPC.BossTitleColor = Color.SkyBlue;
        }

        public override void NPCLoot()
        {
            if (!TerrorbornWorld.downedTidalTitan)
            {
                TerrorbornWorld.downedTidalTitan = true;
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
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Placeable.Furniture.TidalTitanTrophy>());
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AzuriteOre"), Main.rand.Next(15, 26));
            if (Main.expertMode)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TT_TreasureBag"));
            }
            else
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AzuriteOre"), Main.rand.Next(15, 26));
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CrackedShell"), Main.rand.Next(7, 11));
                int choice = Main.rand.Next(4);
                if (choice == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BubbleBow"));
                }
                else if (choice == 1)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TidalClaw"), Stack: 750);
                }
                else if (choice == 2)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SightForSoreEyes"));
                }
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Vanity.BossMasks.TidalTitanMask>());
                }
            }
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frame * frameHeight;
        }

        int FrameWait = 6;
        private void WalkingAnimation(int FrameSpeed)
        {
            FrameWait--;
            if (FrameWait <= 0)
            {
                FrameWait = FrameSpeed;
                frame++;
            }
            if (frame > 6)
            {
                frame = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            Vector2 origin = new Vector2(35, 44);
            if (npc.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
                origin = Main.npcTexture[npc.type].Size() - origin;
            }
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, npc.frame, npc.GetAlpha(drawColor), npc.rotation, origin, npc.scale, effects, 0f);
            spriteBatch.Draw(ModContent.GetTexture(Texture + "_Glow"), npc.Center - Main.screenPosition, npc.frame, npc.GetAlpha(Color.White), npc.rotation, origin, npc.scale, effects, 0f);
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
            phaseWait = timeBetween;
            NextAIPhase = NextAttacks[0];
            AIPhase = -1;
            LastAttack = NextAttacks[0];
            NextAttacks.RemoveAt(0);
            phaseStart = true;
            if (player.Center.X > realPosition.X) npc.spriteDirection = 1;
            else npc.spriteDirection = -1;
        }

        public float GetDirectionTo(Vector2 position, bool forVelocity = false)
        {
            float direction =  (position - realPosition).ToRotation();
            if (npc.spriteDirection == -1 && !forVelocity)
            {
                direction += MathHelper.ToRadians(180f);
                direction = MathHelper.WrapAngle(direction);
            }
            return direction;
        }

        public void InBetweenAttacks()
        {
            npc.rotation = npc.rotation.AngleLerp(GetDirectionTo(player.Center), 0.1f);

            if (!inWater)
            {
                npc.velocity.Y += 0.4f;
            }
            npc.velocity *= 0.95f;

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
        int attackDirection2 = 0;

        int phaseWait = 0;

        public override void AI()
        {
            if (start)
            {
                realPosition = npc.Center;
                start = false;
                inPhaseTransition = true;
                phaseTransitionTime = 0;
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                npc.dontTakeDamage = true;
            }

            int hitboxSize = 60;
            inWater = Collision.WetCollision(realPosition - new Vector2(hitboxSize / 2, hitboxSize / 2), hitboxSize, hitboxSize) || Collision.SolidCollision(realPosition - new Vector2(hitboxSize / 2, hitboxSize / 2), hitboxSize, hitboxSize);

            if (inPhaseTransition)
            {
                if (phase == 0)
                {
                    if (!NPC.AnyNPCs(ModContent.NPCType<MysteriousCrab>()))
                    {
                        phase++;
                        inPhaseTransition = false;
                        npc.dontTakeDamage = false;
                        npc.alpha = 0;
                        DecideNextAttack(240);
                    }
                    else
                    {
                        NPC target = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<MysteriousCrab>())];
                        npc.spriteDirection = -1;
                        npc.rotation = npc.rotation.AngleLerp(GetDirectionTo(target.Center), 0.05f);
                        if (inWater)
                        {
                            npc.velocity.Y -= 0.2f;
                        }
                        else
                        {
                            npc.velocity.Y += 0.5f;
                        }

                        if (npc.alpha > 0)
                        {
                            npc.alpha -= 255 / 60;
                        }
                        else
                        {
                            npc.alpha = 0;
                        }

                        phaseTransitionTime++;
                        if (phaseTransitionTime > 180)
                        {
                            float speed = 20f;
                            npc.velocity = GetDirectionTo(target.Center, true).ToRotationVector2() * speed;

                            Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 10, 1, 0.5f);
                            TerrorbornMod.ScreenShake(15f);
                            if (npc.Distance(target.Center) < target.width)
                            {
                                target.active = false;
                                Main.PlaySound(target.DeathSound, target.Center);
                                Main.NewText("Mysterious Crab has been defeated...", new Color(175, 75, 255));

                                for (int i = 0; i <= 6; i++)
                                {
                                    float goreSpeed = Main.rand.NextFloat(10f, 20f);
                                    Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * goreSpeed + npc.velocity;
                                    Gore.NewGore(target.Center, velocity, mod.GetGoreSlot("Gores/MCrabGore" + i.ToString()));
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                npc.dontTakeDamage = true;
                if (npc.Distance(player.Center) < 1000)
                {
                    npc.dontTakeDamage = false;
                }

                switch (AIPhase)
                {
                    case -1:
                        InBetweenAttacks();
                        break;
                    case 0:
                        BubbleBurping(3, 30);
                        break;
                    case 1:
                        GeyserRoar(5, 20, 60);
                        break;
                    case 2:
                        Leap(30, 15f, 30f);
                        break;
                    default:
                        break;
                }
            }


            realPosition += npc.velocity;

            float hitRotation = npc.rotation;
            if (npc.spriteDirection == -1)
            {
                hitRotation += MathHelper.ToRadians(180);
            }
            npc.position = realPosition + (hitRotation.ToRotationVector2() * hitboxDistance) - npc.Hitbox.Size() / 2;
            npc.Hitbox = new Rectangle((int)npc.Center.X - hitboxSize / 2, (int)npc.Center.Y - hitboxSize / 2, hitboxSize, hitboxSize);
        }

        public void BubbleBurping(int burps, int timeBetweenBurps)
        {
            if (phaseStart)
            {
                attackCounter1 = burps;
                attackCounter2 = 0;
                attackCounter3 = 0;
                phaseStart = false;
                if (player.Center.X > realPosition.X) npc.spriteDirection = 1;
                else npc.spriteDirection = -1;
            }
            else
            {
                npc.rotation = npc.rotation.AngleLerp(GetDirectionTo(player.Center), 0.2f);

                Vector2 targetPosition = player.Center + new Vector2(Math.Sign(player.Center.X - realPosition.X) * -300, 350);
                if (inWater)
                {
                    npc.velocity.Y += Math.Sign(targetPosition.Y - realPosition.Y) * 0.3f;
                }
                else
                {
                    npc.velocity.Y += 0.6f;
                }
                npc.velocity.X += Math.Sign(targetPosition.X - realPosition.X) * 0.5f;
                npc.velocity *= 0.98f;

                attackCounter3++;
                if (Math.Abs(targetPosition.X - realPosition.X) < 200 || attackCounter3 > 180)
                {
                    attackCounter2++;
                    if (attackCounter2 > timeBetweenBurps)
                    {
                        attackCounter2 = 0;
                        attackCounter1--;
                        NPC bubble = Main.npc[NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<TidalCrabBubble>())];
                        bubble.velocity = GetDirectionTo(player.Center, true).ToRotationVector2() * 3f;
                        npc.velocity -= GetDirectionTo(player.Center, true).ToRotationVector2() * 5f;
                        Main.PlaySound(SoundID.NPCDeath13, npc.Center);
                        if (attackCounter1 <= 0)
                        {
                            DecideNextAttack(180);
                        }
                    }
                }
            }
        }

        public void GeyserRoar(int geyserCount, int timeBetweenGeysers, int timeUntilErupt)
        {
            if (phaseStart)
            {
                attackCounter1 = geyserCount;
                attackCounter2 = 0;
                phaseStart = false;
                if (player.Center.X > realPosition.X) npc.spriteDirection = 1;
                else npc.spriteDirection = -1;

                Main.PlaySound(SoundID.Roar, (int)npc.position.X, (int)npc.position.Y, 0, 1, -0.5f);
                TerrorbornMod.ScreenShake(10f);
                npc.velocity -= GetDirectionTo(player.Center, true).ToRotationVector2() * 10f;
            }

            npc.rotation = npc.rotation.AngleLerp(GetDirectionTo(player.Center), 0.1f);

            if (!inWater)
            {
                npc.velocity.Y += 0.4f;
            }
            npc.velocity *= 0.95f;

            attackCounter2++;
            if (attackCounter2 >= timeBetweenGeysers)
            {
                attackCounter2 = 0;
                attackCounter1--;
                if (attackCounter1 <= 0)
                {
                    DecideNextAttack(timeUntilErupt * 3);
                }

                int proj = Projectile.NewProjectile(new Vector2(player.Center.X + (player.velocity.X * timeUntilErupt), player.Center.Y), Vector2.Zero, ModContent.ProjectileType<GeyserTelegraph>(), 60 / 4, 0f);
                Main.projectile[proj].ai[0] = timeUntilErupt;
            }
        }

        public void Leap(int timeBetweenProjectiles, float projectileSpeed, float leapSpeed)
        {
            if (phaseStart)
            {
                attackCounter1 = 0;
                attackCounter2 = 0;
                phaseStart = false;
                if (player.Center.X > realPosition.X) npc.spriteDirection = -1;
                else npc.spriteDirection = 1;

                attackDirection1 = -npc.spriteDirection;

                Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 10, 1, 1f);
                TerrorbornMod.ScreenShake(10f);
                npc.velocity -= GetDirectionTo(player.Center, true).ToRotationVector2() * 10f;
            }
            else if (attackCounter2 == 0)
            {
                npc.velocity *= 0.98f;
                npc.velocity.Y -= 0.3f;
                npc.rotation = npc.rotation.AngleLerp(GetDirectionTo(player.Center), 0.1f);

                if (!inWater)
                {
                    npc.velocity.Y = -leapSpeed;
                    attackCounter2++;
                }
            }
            else
            {
                npc.rotation = npc.rotation.AngleLerp(GetDirectionTo(player.Center), 0.2f);

                npc.velocity *= 0.98f;
                npc.velocity.Y += 0.3f;

                float targetX = player.Center.X + 500f * attackDirection1;
                npc.velocity.X += Math.Sign(targetX - realPosition.X) * 0.5f;

                if (npc.velocity.Y > 0 && inWater)
                {
                    DecideNextAttack(90);
                }
            }
        }
    }

    class TidalBubble : ModNPC
    {
        Vector2 trueVelocity = new Vector2(0, 0);
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
            DisplayName.SetDefault("Bubble");
        }
        public override bool PreNPCLoot()
        {
            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), mod.ProjectileType("TidalBubbleSmall"), npc.damage / 3, 0);
            }
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Heart);
            }
            return false;
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 30;
            npc.height = 30;
            npc.damage = 30;
            npc.HitSound = SoundID.Item54;
            npc.defense = 0;
            npc.DeathSound = SoundID.Item54;
            npc.frame.Width = 388;
            npc.frame.Height = 254;
            npc.lifeMax = 20;
            npc.knockBackResist = 0;
            npc.dontTakeDamage = true;
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
            npc.frame.Y = frame * 30;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 55;
        }

        int invincTime = 30;
        public override void AI()
        {
            if (invincTime <= 0)
            {
                npc.dontTakeDamage = false;
            }
            else
            {
                invincTime--;
            }
            npc.TargetClosest(true);
            trueVelocity *= 0.98f;
            npc.velocity = trueVelocity;
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
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 70;
            projectile.penetrate = 1;
            projectile.hide = false;
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item54, projectile.position);
        }
    }

    class TidalCrabBubble : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
            DisplayName.SetDefault("Bubble");
        }
        public override bool PreNPCLoot()
        {
            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), mod.ProjectileType("TidalBubbleSmall"), npc.damage / 3, 0);
            }
            NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("TidalCrab"));
            return false;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 30;
            npc.height = 30;
            npc.damage = 32;
            npc.HitSound = SoundID.Item54;
            npc.defense = 0;
            npc.DeathSound = SoundID.Item54;
            npc.frame.Width = 388;
            npc.frame.Height = 254;
            npc.lifeMax = 14;
            npc.knockBackResist = 0;
            npc.aiStyle = -1;
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
            npc.frame.Y = frame * 30;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 35;
        }

        public override void AI()
        {
            npc.TargetClosest(true);
            Vector2 targetPosition = Main.player[npc.target].position;
            float speed = 0.4f;
            npc.velocity += npc.DirectionTo(targetPosition) * speed;
            npc.velocity *= 0.98f;
        }
    }

    class TidalCrab : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.width = 26;
            npc.height = 24;
            npc.damage = 35;
            npc.HitSound = SoundID.NPCHit1;
            npc.defense = 0;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.frame.Width = 388;
            npc.frame.Height = 254;
            npc.lifeMax = 37;
            npc.aiStyle = -1;
            npc.knockBackResist = 0.01f;
        }

        int frame = 0;
        int frameWait = 8;
        public override void NPCLoot()
        {
            if (Main.rand.Next(7) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Heart);
            }

            if (Main.rand.Next(7) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.DarkEnergy>());
            }
        }

        public override void FindFrame(int frameHeight)
        {
            frameWait--;
            if (frameWait <= 0)
            {
                frameWait = 8;
                frame++;
                if (frame >= 4)
                {
                    frame = 0;
                }
            }
            npc.frame.Y = frame * frameHeight;
        }

        bool MovingFoward = true;
        int ChangeDirectionWait = 0;
        public override void AI()
        {
            ChangeDirectionWait--;
            if (ChangeDirectionWait <= 0)
            {
                if (MovingFoward)
                {
                    MovingFoward = false;
                    ChangeDirectionWait = Main.rand.Next(10, 60);
                }
                else
                {
                    MovingFoward = true;
                    ChangeDirectionWait = Main.rand.Next(80, 150);
                }
            }

            if (npc.wet)
            {
                npc.velocity.Y -= 0.2f;
            }
            else
            {
                npc.velocity.Y += 0.3f;
            }

            if (npc.Center.X < Main.player[npc.target].position.X)
            {
                if (MovingFoward)
                {
                    npc.velocity.X += 0.25f;
                }
                else
                {
                    npc.velocity.X -= 0.25f;
                }
            }

            if (npc.Center.X > Main.player[npc.target].position.X)
            {
                if (!MovingFoward)
                {
                    npc.velocity.X += 0.25f;
                }
                else
                {
                    npc.velocity.X -= 0.25f;
                }
            }

            npc.velocity.X *= 0.93f;
            npc.velocity.Y *= 0.97f;
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

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Color color = Color.LightSkyBlue;
            color.A = (int)(255 * 0.75f);
            Utils.DrawLine(spriteBatch, projectile.Center, projectile.Center + new Vector2(0, -telegraphLength), color * telegraphAlpha, Color.Transparent, 3);
            return false;
        }

        public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
        public override void SetDefaults()
        {
            projectile.width = 2;
            projectile.height = 2;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.hide = false;
            projectile.tileCollide = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.alpha = 255;
        }

        bool start = true;

        public override void AI()
        {
            Vector2 position = new Vector2(projectile.Center.X, projectile.position.Y - 60);
            while (!(Collision.SolidCollision(position, 1, 1) || Collision.WetCollision(position, 1, 1)))
            {
                position.Y++;
            }
            projectile.position.Y = position.Y;
            if (start)
            {
                start = false;
                FireWait = projectile.ai[0];
            }
            FireWait--;
            if (FireWait == 0)
            {
                Main.PlaySound(SoundID.Item34, projectile.Center);
                TerrorbornMod.ScreenShake(3f);
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
                        projectile.active = false;
                    }
                    TerrorbornMod.ScreenShake(0.5f);
                    Projectile.NewProjectile(new Vector2(projectile.position.X, projectile.position.Y), new Vector2(0, 0), ModContent.ProjectileType<TideFire>(), 18, projectile.knockBack);
                    projectile.velocity.X = 0;
                }
            }
            else
            {
                telegraphLength += 6;
                if (telegraphAlphaDirection == 1)
                {
                    telegraphAlpha += 2f / projectile.ai[0];
                    if (telegraphAlpha >= 1)
                    {
                        telegraphAlphaDirection = -1;
                    }
                }
                else
                {
                    telegraphAlpha -= 2f / projectile.ai[0];
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
            projectile.width = 100;
            projectile.height = 500;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.hide = false;
            projectile.tileCollide = false;
            projectile.penetrate = 1;
            projectile.timeLeft = timeLeft;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
        }

        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
                projectile.position -= new Vector2(0, projectile.height / 2);
                projectile.alpha = 0;
            }

            projectile.alpha += (int)(255f / (float)timeLeft);
            projectile.scale += 0.3f / timeLeft;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            spriteBatch.Draw(ModContent.GetTexture(Texture), projectile.position + new Vector2(projectile.width / 2, projectile.height) - Main.screenPosition, null, projectile.GetAlpha(Color.Azure), 0f, new Vector2(ModContent.GetTexture(Texture).Width / 2, ModContent.GetTexture(Texture).Height), new Vector2(((float)projectile.width / (float)ModContent.GetTexture(Texture).Width) * projectile.scale, ((float)projectile.height / (float)ModContent.GetTexture(Texture).Height) * projectile.scale), SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }
    }
}