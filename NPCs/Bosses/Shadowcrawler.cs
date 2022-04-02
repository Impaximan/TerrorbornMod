using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.NPCs.Bosses
{
    [AutoloadBossHead]
    class Shadowcrawler : ModNPC
    {
        public override void NPCLoot()
        {
            TerrorbornSystem.downedShadowcrawler = true;
            Item.NewItem(NPC.position, NPC.width, NPC.height, ModContent.ItemType<Items.PermanentUpgrades.AnekronianApple>());
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem(NPC.getRect(), ModContent.ItemType<Items.Placeable.Furniture.ShadowcrawlerTrophy>());
            }
            if (Main.expertMode)
            {
                Item.NewItem(NPC.position, NPC.width, NPC.height, ModContent.ItemType<Items.TreasureBags.SC_TreasureBag>());
            }
            else
            {
                Item.NewItem(NPC.position, NPC.width, NPC.height, ModContent.ItemType<Items.Materials.SoulOfPlight>(), Stack: Main.rand.Next(25, 41));
                int choice = Main.rand.Next(3);
                if (choice == 0)
                {
                    Item.NewItem(NPC.position, NPC.width, NPC.height, ModContent.ItemType<Items.Shadowcrawler.BladeOfShade>());
                }
                else if (choice == 1)
                {
                    Item.NewItem(NPC.position, NPC.width, NPC.height, ModContent.ItemType<Items.Shadowcrawler.Nightbrood>());
                }
                else if (choice == 2)
                {
                    Item.NewItem(NPC.position, NPC.width, NPC.height, ModContent.ItemType<Items.Shadowcrawler.BoiledBarrageWand>());
                }
                int armorChoice = Main.rand.Next(3);
                if (armorChoice == 0)
                {
                    Item.NewItem(NPC.position, NPC.width, NPC.height, ModContent.ItemType<Items.Equipable.Armor.TenebrisMask>());
                    Item.NewItem(NPC.position, NPC.width, NPC.height, ModContent.ItemType<Items.Equipable.Armor.TenebrisChestplate>());
                }
                if (armorChoice == 1)
                {
                    Item.NewItem(NPC.position, NPC.width, NPC.height, ModContent.ItemType<Items.Equipable.Armor.TenebrisLeggings>());
                    Item.NewItem(NPC.position, NPC.width, NPC.height, ModContent.ItemType<Items.Equipable.Armor.TenebrisChestplate>());
                }
                if (armorChoice == 2)
                {
                    Item.NewItem(NPC.position, NPC.width, NPC.height, ModContent.ItemType<Items.Equipable.Armor.TenebrisMask>());
                    Item.NewItem(NPC.position, NPC.width, NPC.height, ModContent.ItemType<Items.Equipable.Armor.TenebrisLeggings>());
                }
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Equipable.Vanity.BossMasks.ShadowcrawlerMask>());
                }
            }
        }

        public override bool CheckActive()
        {
            return false;
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

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !Jumping;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 15;
            DisplayName.SetDefault("Shadowcrawler");
            NPCID.Sets.TrailCacheLength[NPC.type] = 3;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.width = 158;
            NPC.height = 126;
            NPC.damage = 60;
            NPC.HitSound = SoundID.NPCHit29;
            NPC.defense = 15;
            NPC.DeathSound = SoundID.NPCDeath31;
            NPC.frame.Width = 388;
            music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/8LeggedTerror");
            NPC.boss = true;
            NPC.frame.Height = 254;
            NPC.lifeMax = 27000;
            NPC.knockBackResist = 0;

            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(NPC);
            modNPC.BossTitle = "Shadowcrawler";
            modNPC.BossSubtitle = "Titan of the Night";
            modNPC.BossTitleColor = new Color(29, 189, 49);
        }
        bool Attacking = false;
        int Frame = 0;
        int FrameWait = 6;
        //|--------Frames--------|
        //Jump = 8
        //Standing Still = 0-7
        //Attacking = 9-12
        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = Frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            if (NPC.direction == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(NPC.width / 2, NPC.height / 2);

            Vector2 drawPos = NPC.position - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY) + new Vector2(0, 4);
            Color color = NPC.GetAlpha(drawColor);
            Texture2D Texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/Shadowcrawler");
            if (phase == 2)
            {
                Texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/Shadowcrawler_2");
            }
            if (phase == 3)
            {
                Texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/Shadowcrawler_3");
            }
            spriteBatch.Draw(Texture, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);

            if (phase == 3)
            {
                for (int i = 0; i < NPC.oldPos.Length; i++)
                {
                    drawPos = NPC.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY) + new Vector2(0, 4);
                    color = NPC.GetAlpha(Color.White) * ((float)(NPC.oldPos.Length - i) / (float)NPC.oldPos.Length);
                    Texture2D glowTexture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/Shadowcrawler_Glow");
                    if (phase == 2)
                    {
                        glowTexture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/Shadowcrawler_2_Glow");
                    }
                    if (phase == 3)
                    {
                        glowTexture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/Shadowcrawler_3_Glow");
                    }
                    spriteBatch.Draw(glowTexture, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
                }
            }
            else
            {
                drawPos = NPC.position - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY) + new Vector2(0, 4);
                color = NPC.GetAlpha(Color.White);
                Texture2D glowTexture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/Shadowcrawler_Glow");
                if (phase == 2)
                {
                    glowTexture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/Shadowcrawler_2_Glow");
                }
                if (phase == 3)
                {
                    glowTexture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/Shadowcrawler_3_Glow");
                }
                spriteBatch.Draw(glowTexture, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
            }
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //if (!Jumping)
            //{
            //    Color mainColor = Color.LightGreen;
            //    mainColor *= 0.25f;
            //    float fadeOffset = 2f;
            //    Utils.DrawLine(spriteBatch, NPC.position + new Vector2(0, NPC.height), NPC.position + new Vector2(NPC.width, NPC.height), mainColor, mainColor, 3f);
            //    Utils.DrawLine(spriteBatch, NPC.position + new Vector2(-20, NPC.height), NPC.position + new Vector2(fadeOffset, NPC.height), Color.Transparent, mainColor, 3f);
            //    Utils.DrawLine(spriteBatch, NPC.position + new Vector2(NPC.width - fadeOffset, NPC.height), NPC.position + new Vector2(NPC.width + 20, NPC.height), mainColor, Color.Transparent, 3f);
            //}
        }

        public void WalkingAnimation(int frameCooldown)
        {
            FrameWait--;
            if (Frame < 7)
            {
                Frame = 7;
            }

            if (FrameWait <= 0)
            {
                FrameWait = frameCooldown;
                Frame++;
            }

            if (Frame > 13)
            {
                Frame = 7;
            }
        }
        public void BackwardsWalkingAnimation(int frameCooldown)
        {
            FrameWait--;
            if (Frame < 7)
            {
                Frame = 13;
            }

            if (FrameWait <= 0)
            {
                FrameWait = frameCooldown;
                Frame--;
            }

            if (Frame > 13)
            {
                Frame = 13;
            }
        }

        public void StillAnimation(int frameCooldown)
        {
            FrameWait--;

            if (FrameWait <= 0)
            {
                FrameWait = frameCooldown;
                Frame++;
            }

            if (Frame > 6)
            {
                Frame = 0;
            }
        }

        public void JumpAnimation()
        {
            Frame = 14;
        }

        public void ResetAnimations()
        {
            FrameWait = 1;
        }

        public void lockToScreen()
        {
            lockedToScreen = true;
            positionOnScreen = NPC.position - Main.screenPosition;
        }

        const float fallingSpeed = 1f;
        Vector2 jumpTarget;
        public void SetJump(Vector2 targetPosition, bool screenLocked)
        {
            jumpTarget = targetPosition;
            Jumping = true;
            jumpCounter = 60;

            if (screenLocked)
            {
                velocity.X = (targetPosition.X - positionOnScreen.X) / 60;
                velocity.Y = (targetPosition.Y - (460f * 4f * fallingSpeed) - positionOnScreen.Y) / 60 + fallingSpeed;

            }
            else
            {
                velocity.X = (targetPosition.X - NPC.position.X) / 60;
                velocity.Y = (targetPosition.Y - (460f * 4f * fallingSpeed) - NPC.position.Y) / 60 + fallingSpeed;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.85);
            NPC.defense = 25;
        }

        int beepCounter;
        int beepCount;
        public void BeepingSounds(int numberOfBeeps)
        {
            beepCount = numberOfBeeps;
            beepCounter = 0;
        }

        public void DiagonalCharge(Player player)
        {
            if (phaseStart)
            {
                phaseStart = false;
                lockToScreen();
                NPC.ai[1] = 1;
                phaseCounter1 = 60;
                phaseCounter2 = 75;
                ProjectileCounter1 = 0;
                phaseCounter3 = 0;
                autofacePlayer = true;
                if (Main.rand.NextBool())
                {
                    NPC.ai[1] = -1;
                }
                int xOffset = 400;
                SetJump(centerOfScreen + new Vector2(xOffset * NPC.ai[1] - NPC.width / 2, xOffset - NPC.height / 2), true);
            }
            if (phaseCounter1 > 0)
            {
                phaseCounter1--;
                if (phaseCounter1 == 1)
                {
                    velocity.Y = -18;
                    velocity.X = -NPC.ai[1] * 18;
                    if (phase == 3)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, NPC.Center);
                        Vector2 direction = velocity;
                        direction.Normalize();
                        float speed = 12;
                        int damage = 65 / 4;
                        int proj1 = Projectile.NewProjectile(NPC.Center, direction.RotatedBy(MathHelper.ToRadians(90)) * speed, ModContent.ProjectileType<Projectiles.MidnightFireball>(), damage, 0);
                        Main.projectile[proj1].ai[0] = 120;
                        Main.projectile[proj1].ai[1] = 3f;
                        int proj2 = Projectile.NewProjectile(NPC.Center, direction.RotatedBy(MathHelper.ToRadians(-90)) * speed, ModContent.ProjectileType<Projectiles.MidnightFireball>(), damage, 0);
                        Main.projectile[proj2].ai[0] = 120;
                        Main.projectile[proj2].ai[1] = 3f;
                    }
                    autofacePlayer = false;
                }
                StillAnimation(3);
            }
            else if (phaseCounter3 == 0)
            {
                lockedToScreen = false;
                WalkingAnimation(2);
                spriteDirection = (int)-NPC.ai[1];
                phaseCounter2--;
                if (phaseCounter2 <= 0)
                {
                    phaseCounter3 = 60;
                    ResetAnimations();
                }
                ProjectileCounter1--;
                if (ProjectileCounter1 <= 0)
                {
                    ProjectileCounter1 = 8;
                    if (phase >= 2)
                    {
                        ProjectileCounter1 = 7;
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot, NPC.Center);
                    Vector2 direction = NPC.velocity;
                    direction.Normalize();
                    float speed = 1;
                    int damage = 65 / 4;
                    int proj1 = Projectile.NewProjectile(NPC.Center, direction.RotatedBy(MathHelper.ToRadians(90)) * speed, ModContent.ProjectileType<NightmareFlame>(), damage, 0);
                    int proj2 = Projectile.NewProjectile(NPC.Center, direction.RotatedBy(MathHelper.ToRadians(-90)) * speed, ModContent.ProjectileType<NightmareFlame>(), damage, 0);
                    //Main.projectile[proj].ai[0] = 200f;
                    //Main.projectile[proj].ai[1] = 0.5f;
                    //if (phase == 2)
                    //{
                    //    Main.projectile[proj].ai[1] = 0.65f;
                    //}
                    //if (phase == 3)
                    //{
                    //    Main.projectile[proj].ai[1] = 1.5f;
                    //}
                }
            }
            else
            {
                StillAnimation(5);
                velocity = Vector2.Zero;
                lockedToScreen = false;
                phaseCounter3--;
                if (phaseCounter3 <= 0)
                {
                    DecideNextAttack();
                }
            }
        }

        public void EggSpew(Player player, int numberOfEggs, int delayBetweenEggs)
        {
            if (phaseStart)
            {
                phaseStart = false;
                lockedToScreen = false;
                ResetAnimations();
                phaseCounter1 = numberOfEggs;
                phaseCounter2 = delayBetweenEggs;
                phaseCounter3 = 110;
                int direction1 = 0;
                if (Main.rand.NextBool())
                {
                    direction1 = 1;
                }
                else
                {
                    direction1 = -1;
                }
                int direction2 = 0;
                if (Main.rand.NextBool())
                {
                    direction2 = 1;
                }
                else
                {
                    direction2 = -1;
                }
                SetJump(player.Center + new Vector2(Main.rand.Next(300, 600) * direction1, Main.rand.Next(200, 400) * direction2), false);
                autofacePlayer = true;
            }
            else if (phaseCounter1 > 0)
            {
                if (phaseCounter2 > 0)
                {
                    phaseCounter2--;
                    StillAnimation(3);
                }
                else
                {
                    phaseCounter1--;
                    phaseCounter2 = delayBetweenEggs;
                    float distance = Main.rand.Next(100, 350);
                    Vector2 position = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * distance + player.Center + player.velocity * 90;
                    int proj = Projectile.NewProjectile(NPC.Center, Vector2.Zero, ModContent.ProjectileType<ShadowEgg>(), 0, 0);
                    Main.projectile[proj].ai[0] = position.X;
                    Main.projectile[proj].ai[1] = position.Y;

                    Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath13, NPC.Center);
                }
            }
            else
            {
                phaseCounter3--;
                if (phaseCounter3 <= 0)
                {
                    DecideNextAttack();
                }
            }
        }

        public void MultipleJumpsWithFireballs(Player player, int jumpCount)
        {
            int ProjectileCount = 8;
            if (phase >= 3)
            {
                ProjectileCount = 10;
            }
            if (phaseStart)
            {
                phaseStart = false;
                lockToScreen();
                ResetAnimations();
                phaseCounter1 = jumpCount;
                phaseCounter2 = 45;
                if (phase >= 2)
                {
                    phaseCounter2 = 32;
                }
                int direction1 = 0;
                if (Main.rand.NextBool())
                {
                    direction1 = 1;
                }
                else
                {
                    direction1 = -1;
                }
                int direction2 = 0;
                if (Main.rand.NextBool())
                {
                    direction2 = 1;
                }
                else
                {
                    direction2 = -1;
                }
                SetJump(centerOfScreen + new Vector2(Main.rand.Next(300, 600) * direction1, Main.rand.Next(200, 400) * direction2), true);
                autofacePlayer = true;
            }
            else
            {
                if (phaseCounter2 > 0)
                {
                    phaseCounter2--;
                    StillAnimation(3);
                    float direction = 0;
                    float speed = 30;
                    for (int i = 0; i < ProjectileCount; i++)
                    {
                        direction += MathHelper.ToRadians(360 / ProjectileCount);
                        Vector2 directionVector = direction.ToRotationVector2();
                        Dust dust = Dust.NewDustPerfect(NPC.Center, 74);
                        dust.velocity = directionVector * speed + player.velocity;
                        dust.noLight = true;
                        dust.noGravity = true;
                    }
                }
                else
                {
                    phaseCounter1--;
                    phaseCounter2 = 45;
                    if (phase >= 2)
                    {
                        phaseCounter2 = 32;
                    }
                    float direction = 0;
                    float speed = 17;
                    if (phase >= 2)
                    {
                        speed = 23;
                    }
                    int timeLeft = 120;
                    for (int i = 0; i < ProjectileCount; i++)
                    {
                        direction += MathHelper.ToRadians(360 / ProjectileCount);
                        Vector2 directionVector = direction.ToRotationVector2();
                        int proj = Projectile.NewProjectile(NPC.Center, directionVector * speed, ModContent.ProjectileType<Projectiles.MidnightFireball>(), 90 / 4, 0);
                        Main.projectile[proj].ai[0] = timeLeft;
                        if (phase == 3)
                        {
                            Main.projectile[proj].ai[1] = 0.45f;
                        }
                        else
                        {
                            Main.projectile[proj].ai[1] = 0.65f;
                        }
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, NPC.Center);
                    if (phaseCounter1 <= 0)
                    {
                        DecideNextAttack();
                    }
                    else
                    {
                        int direction1 = 0;
                        if (Main.rand.NextBool())
                        {
                            direction1 = 1;
                        }
                        else
                        {
                            direction1 = -1;
                        }
                        int direction2 = 0;
                        if (Main.rand.NextBool())
                        {
                            direction2 = 1;
                        }
                        else
                        {
                            direction2 = -1;
                        }
                        SetJump(centerOfScreen + new Vector2(Main.rand.Next(300, 600) * direction1, Main.rand.Next(200, 400) * direction2), true);
                    }
                }
            }
        }

        public void PredictiveJump(Player player)
        {
            if (phaseStart)
            {
                phaseStart = false;
                lockToScreen();
                ResetAnimations();
                phaseCounter1 = 30;
                phaseCounter2 = 60;
                if (phase >= 2)
                {
                    phaseCounter2 = 30;
                }
                phaseCounter3 = 45;
                SetJump(centerOfScreen + new Vector2(0 - NPC.width / 2, 300 - NPC.height / 2), true);
                autofacePlayer = true;
            }
            else if (phaseCounter1 > 0)
            {
                StillAnimation(3);
                phaseCounter1--;
                if (phaseCounter1 == 1)
                {
                    BeepingSounds(3);
                    ResetAnimations();
                }
            }
            else if (phaseCounter2 > 0)
            {
                lockedToScreen = false;
                if (phase >= 2)
                {
                    BackwardsWalkingAnimation(2);
                }
                else
                {
                    BackwardsWalkingAnimation(5);
                }
                if (phase >= 2)
                {
                    velocity.X = spriteDirection * -6f;
                }
                else
                {
                    velocity.X = spriteDirection * -3f;
                }
                phaseCounter2--;
                if (phaseCounter2 == 1)
                {
                    SetJump(player.Center + player.velocity * 60 + new Vector2(0 - NPC.width / 2, 0 - NPC.height / 2), false);
                }
                if (phaseCounter2 == 0)
                {
                    int ProjectileCount = 12;
                    float direction = 0;
                    float speed = 25;
                    int timeLeft = 45;
                    if (phase == 3)
                    {
                        timeLeft *= 4;
                    }
                    int rotationMultiplier = 15;
                    for (int i = 0; i < ProjectileCount; i++)
                    {
                        direction += MathHelper.ToRadians(360 / ProjectileCount);
                        Vector2 directionVector = direction.ToRotationVector2();
                        int proj = Projectile.NewProjectile(NPC.Center, directionVector * speed, ModContent.ProjectileType<LargeNightmareFlame>(), 90 / 4, 0);
                        Main.projectile[proj].timeLeft = timeLeft;
                        Main.projectile[proj].ai[0] = rotationMultiplier;
                    }

                    if (phase == 3)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot, NPC.Center);
                        Vector2 direction2 = NPC.DirectionTo(player.Center);
                        float speed2 = 22;
                        int damage = 65 / 4;
                        int proj2 = Projectile.NewProjectile(NPC.Center, direction2 * speed2, ModContent.ProjectileType<Projectiles.MidnightFireball>(), damage, 0);
                        Main.projectile[proj2].ai[0] = 200f;
                        Main.projectile[proj2].ai[1] = 0.5f;
                    }
                }
            }
            else
            {
                velocity.X = 0;
                StillAnimation(5);
                phaseCounter3--;
                if (phaseCounter3 <= 0)
                {
                    DecideNextAttack();
                }
            }
        }

        public void HorizontalCharge(Player player)
        {
            if (phaseStart)
            {
                phaseStart = false;
                lockToScreen();
                NPC.ai[1] = 1;
                phaseCounter1 = 60;
                phaseCounter2 = 70;
                ProjectileCounter1 = 0;
                phaseCounter3 = 0;
                autofacePlayer = true;
                if (Main.rand.NextBool())
                {
                    NPC.ai[1] = -1;
                }
                int xOffset = 525;
                SetJump(centerOfScreen + new Vector2(xOffset * NPC.ai[1] - NPC.width / 2, 0 - NPC.height / 2), true);
            }
            if (phaseCounter1 > 0)
            {
                phaseCounter1--;
                if (phaseCounter1 == 1)
                {
                    velocity.Y = player.velocity.Y;
                    autofacePlayer = false;
                }
                StillAnimation(3);
            }
            else if (phaseCounter3 == 0)
            {
                lockedToScreen = false;
                WalkingAnimation(2);
                velocity.X = -NPC.ai[1] * 18;
                spriteDirection = (int)-NPC.ai[1];
                phaseCounter2--;
                if (phaseCounter2 <= 0)
                {
                    phaseCounter3 = 120;
                    ResetAnimations();
                }
                ProjectileCounter1--;
                if (ProjectileCounter1 <= 0)
                {
                    ProjectileCounter1 = 15;
                    if (phase >= 2)
                    {
                        ProjectileCounter1 = 10;
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot, NPC.Center);
                    Vector2 direction = NPC.DirectionTo(player.Center);
                    float speed = 15;
                    int damage = 65 / 4;
                    int proj = Projectile.NewProjectile(NPC.Center, direction * speed, ModContent.ProjectileType<Projectiles.MidnightFireball>(), damage, 0);
                    Main.projectile[proj].ai[0] = 200f;
                    Main.projectile[proj].ai[1] = 0.5f;
                    if (phase == 2)
                    {
                        Main.projectile[proj].ai[1] = 0.65f;
                    }
                    if (phase == 3)
                    {
                        Main.projectile[proj].ai[1] = 1.5f;
                    }
                }
            }
            else
            {
                StillAnimation(5);
                velocity = Vector2.Zero;
                lockedToScreen = false;
                phaseCounter3--;
                if (phaseCounter3 <= 0)
                {
                    DecideNextAttack();
                }
            }
        }

        public void OverheadFireballDropping(Player player)
        {
            if (phaseStart)
            {
                phaseStart = false;
                lockToScreen();
                NPC.ai[1] = 1;
                phaseCounter1 = 70;
                if (phase == 3)
                {
                    phaseCounter1 = 50;
                }
                phaseCounter2 = 45;
                ProjectileCounter1 = 0;
                phaseCounter3 = 0;
                autofacePlayer = true;
                if (Main.rand.NextBool())
                {
                    NPC.ai[1] = -1;
                }
                SetJump(centerOfScreen + new Vector2(350 * NPC.ai[1] - NPC.width / 2, -350), true);
            }
            if (phaseCounter1 > 0)
            {
                phaseCounter1--;
                StillAnimation(3);
            }
            else if (phaseCounter3 == 0)
            {

                autofacePlayer = false;
                WalkingAnimation(2);
                velocity.X = -NPC.ai[1] * 15;
                spriteDirection = (int)-NPC.ai[1];
                phaseCounter2--;
                if (phaseCounter2 <= 0)
                {
                    phaseCounter3 = 105;
                    ResetAnimations();
                }
                ProjectileCounter1--;
                if (ProjectileCounter1 <= 0)
                {
                    ProjectileCounter1 = 5;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot, NPC.Center);
                    Vector2 direction = NPC.DirectionTo(player.Center);
                    float speed = 14;
                    if (phase >= 2)
                    {
                        speed = 16;
                    }
                    int damage = 65 / 4;
                    int proj = Projectile.NewProjectile(NPC.Center, direction * speed, ModContent.ProjectileType<Projectiles.MidnightFireball>(), damage, 0);
                    Main.projectile[proj].ai[0] = 120f;
                    Main.projectile[proj].ai[1] = 2.5f;
                }
            }
            else
            {
                StillAnimation(5);
                velocity = Vector2.Zero;
                lockedToScreen = false;
                phaseCounter3--;
                if (phaseCounter3 <= 0)
                {
                    DecideNextAttack();
                }
            }
        }

        const float secondPhaseHealth = 0.60f;
        const float desparationPhaseHealth = 0.2f;

        public void DecideNextAttack()
        {
            if (NextAttacks.Count <= 0)
            {
                WeightedRandom<int> listOfAttacks = new WeightedRandom<int>();
                listOfAttacks.Add(0);
                listOfAttacks.Add(1);
                listOfAttacks.Add(2);
                listOfAttacks.Add(3);
                if (phase >= 2)
                {
                    listOfAttacks.Add(4);
                }
                listOfAttacks.Add(5);
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
                phaseTransitionCounter = 90;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath10, NPC.Center);
                ResetAnimations();
                NextAttacks.Add(4);
            }
            else if (phase == 2 && NPC.life <= desparationPhaseHealth * NPC.lifeMax)
            {
                phaseTransitionCounter = 90;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath10, NPC.Center);
                ResetAnimations();
            }
            else
            {
                AIPhase = NextAttacks[0];
                LastAttack = AIPhase;
                NextAttacks.RemoveAt(0);
            }
            phaseStart = true;
        }

        List<int> NextAttacks = new List<int>();
        int AIPhase = 69;
        int LastAttack = -1;
        bool lockedToScreen = false;
        bool Jumping = false;
        int jumpCounter = 0;
        bool phaseStart;
        int phaseCounter1;
        int phaseCounter2;
        int phaseCounter3;
        int ProjectileCounter1;
        int spriteDirection;
        bool autofacePlayer = true;
        int phase = 1;
        int phaseTransitionCounter = 0;

        Vector2 positionOnScreen;
        Vector2 velocity = Vector2.Zero;
        Vector2 centerOfScreen = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);

        public override void AI()
        {
            Player player = Main.player[NPC.target];

            if (player.dead || Main.dayTime)
            {
                NPC.active = false;
                DustExplosion(NPC.Center, 0, 25, 25, 74, 1.5f, true);
                return;
            }

            if ((phase == 1 && NPC.life <= secondPhaseHealth * NPC.lifeMax) || phaseTransitionCounter > 0 || (phase == 2 && NPC.life <= desparationPhaseHealth * NPC.lifeMax))
            {
                NPC.dontTakeDamage = true;
            }
            else
            {
                NPC.dontTakeDamage = false;
            }

            if (autofacePlayer)
            {
                if (player.Center.X > NPC.Center.X)
                {
                    spriteDirection = 1;
                }
                else
                {
                    spriteDirection = -1;
                }
            }
            NPC.TargetClosest(false);
            NPC.direction = spriteDirection;

            if (beepCount > 0)
            {
                if (beepCounter > 0)
                {
                    beepCounter--;
                }
                else
                {
                    beepCount--;
                    beepCounter = 5;
                    ModContent.GetSound("TerrorbornMod/Sounds/Effects/undertalewarning").Play(Main.soundVolume, 0f, 0f);
                }
            }

            if (Jumping)
            {
                JumpAnimation();
                velocity.Y += fallingSpeed;
                jumpCounter--;
                if (jumpCounter <= 0)
                {
                    Jumping = false;
                    velocity = Vector2.Zero;
                    if (lockedToScreen)
                    {
                        positionOnScreen = jumpTarget;
                    }
                    else
                    {
                        NPC.position = jumpTarget;
                    }
                    ResetAnimations();
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, NPC.Center);
                    for (int i = 0; i < 30; i++)
                    {
                        int dust = Dust.NewDust(NPC.position + new Vector2(0, NPC.height), NPC.width, 10, 74);
                        if (lockedToScreen)
                        {
                            Main.dust[dust].velocity = player.velocity;
                        }
                        Main.dust[dust].noGravity = true;
                    }
                }
            }
            else
            {
                if (phaseTransitionCounter > 0)
                {
                    phaseTransitionCounter--;
                    StillAnimation(2);
                    TerrorbornSystem.ScreenShake(10);
                    if (phaseTransitionCounter <= 0)
                    {
                        phase++;
                        DecideNextAttack();
                    }
                }
                else
                {
                    switch (AIPhase)
                    {
                        case 0:
                            OverheadFireballDropping(player);
                            break;
                        case 1:
                            HorizontalCharge(player);
                            break;
                        case 2:
                            PredictiveJump(player);
                            break;
                        case 3:
                            MultipleJumpsWithFireballs(player, 3);
                            break;
                        case 4:
                            if (phase == 3)
                            {
                                EggSpew(player, 12, 7);
                            }
                            else
                            {
                                EggSpew(player, 5, 10);
                            }
                            break;
                        case 5:
                            DiagonalCharge(player);
                            break;
                        default:
                            DecideNextAttack();
                            break;
                    }
                }
            }

            if (lockedToScreen)
            {
                positionOnScreen += velocity;
                NPC.velocity = Vector2.Zero;
                Vector2 relativeToPlayer = centerOfScreen - Main.screenPosition;
                NPC.position = player.Center - centerOfScreen + positionOnScreen;
            }
            else
            {
                NPC.velocity = velocity;
            }
        }
    }

    class ShadowEgg : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 46;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 90;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, 60, Color.LimeGreen * 0.5f);
            return base.PreDraw(spriteBatch, lightColor);
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(5);
            Vector2 targetPosition = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Projectile.velocity = (targetPosition - Projectile.Center) / 10;
        }

        public override void Kill(int timeLeft)
        {
            Vector2 rotation = Projectile.DirectionTo(Main.player[Main.myPlayer].Center);
            float speed = 10f;
            Projectile.NewProjectile(Projectile.Center, rotation * speed, ModContent.ProjectileType<GhostHatchling>(), 95 / 4, 0);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);
            DustExplosion(Projectile.Center, 0, 12, 7, 74, 2f, true);
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

    class GhostHatchling : ModProjectile
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 2);
            Projectile.timeLeft = 1;
        }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 34;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 120;
        }

        public override void AI()
        {
            float rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(180);
            Projectile.rotation = rotation - MathHelper.ToRadians(90);
            float Speed = 0.2f;
            Projectile.velocity += new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)].Center).ToRotation(), MathHelper.ToRadians(0.8f * (Projectile.velocity.Length() / 20))).ToRotationVector2() * Projectile.velocity.Length();
        }
        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 0, 12, 7, 74, 2f, true);
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
    
    class NightmareFlame : ModProjectile
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 3);
            Projectile.timeLeft = 1;
        }
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 200;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 74, 0f, 0f, 100, Scale: 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity;
            float rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(180);
            float Speed = 0.5f;
            Projectile.velocity += new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, 15, Color.LimeGreen * 1f);
            return false;
        }
    }
    class LargeNightmareFlame : ModProjectile
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 3);
            Projectile.timeLeft = 1;
        }
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 360;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, 45, Color.LimeGreen * 1f);
            return false;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 74, 0f, 0f, 100, Scale: 2.25f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity;
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(0.4f * Projectile.ai[0]));
        }
    }
    class NightmareSpawn : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.hide = true;
            Projectile.timeLeft = 200;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        int ProjectileWait = 3;
        public override void AI()
        {
            ProjectileWait--;
            if (ProjectileWait <= 0)
            {
                ProjectileWait = 9;
                int damage = 80;
                damage = Main.expertMode ? (damage / 4) : (damage / 2);
                Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.position.Y), new Vector2(0, -1), ModContent.ProjectileType<NightmareFlame>(), damage, 0);
                Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.position.Y), new Vector2(0, 1), ModContent.ProjectileType<NightmareFlame>(), damage, 0);
            }
        }
    }
}