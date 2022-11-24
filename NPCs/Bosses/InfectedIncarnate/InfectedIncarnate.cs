using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TerrorbornMod.Projectiles;
using TerrorbornMod.Utils;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs.Bosses.InfectedIncarnate
{
    class MemorialCoffin : ModNPC
    {
        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 originPoint = NPC.Center;
            Vector2 center = arena.Center.ToVector2().FindCeilingAbove(ModContent.TileType<Tiles.MemorialBrick>());
            Vector2 distToProj = originPoint - center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/NPCs/Bosses/InfectedIncarnate/Chainlink");

            while (distance > texture.Height && !float.IsNaN(distance))
            {
                distToProj.Normalize();
                distToProj *= texture.Height;
                center += distToProj;
                distToProj = originPoint - center;
                distance = distToProj.Length();


                //Draw chain
                spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, texture.Width, texture.Height), drawColor, projRotation,
                    new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 74;
            NPC.friendly = false;
            NPC.dontTakeDamage = true;
            NPC.damage = 0;
            NPC.lifeMax = 5;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        Rectangle arena;
        void SetArenaPosition()
        {
            Vector2 arenaPos = TerrorbornSystem.IIShrinePosition * 16;
            arenaPos += new Vector2(-37 * 16, 92 * 16);
            arena = new Rectangle((int)arenaPos.X, (int)arenaPos.Y, 75 * 16, 35 * 16);
        }

        bool spawningAnimation = false;
        int shakeCounter = 0;
        int shakesLeft = 3;
        const int totalSpawningTime = 1110;
        double timeAlive = 0;
        public override void AI()
        {
            SetArenaPosition();

            timeAlive++;
            float rotation = MathHelper.ToRadians(94f + (float)Math.Sin(timeAlive / 50) * 4f);

            NPC.position = arena.Center.ToVector2().FindCeilingAbove(ModContent.TileType<Tiles.MemorialBrick>()) + rotation.ToRotationVector2() * arena.Height / 2;
            NPC.rotation = rotation - MathHelper.ToRadians(94f);

            if (TerrorbornPlayer.modPlayer(Main.LocalPlayer).ShriekTime > 0 && arena.Contains(Main.LocalPlayer.getRect()) && !spawningAnimation)
            {
                spawningAnimation = true;
                TerrorbornMod.SetScreenToPosition(120, totalSpawningTime, arena.Center.ToVector2(), 0.9f);
            }
            
            if (spawningAnimation)
            {
                NPC.boss = true;
                Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/InfectedIncarnate");
                shakeCounter++;
                if (shakeCounter >= totalSpawningTime / 3)
                {
                    shakeCounter = 0;
                    shakesLeft--;
                    TerrorbornSystem.ScreenShake(10f);
                    if (shakesLeft == 1)
                    {
                        TerrorbornSystem.ScreenShake(15f);
                    }
                    SoundExtensions.PlaySoundOld(SoundID.Item62, NPC.Center);

                    if (shakesLeft <= 0)
                    {
                        NPC.active = false;
                        NPC.NewNPC(NPC.GetSource_ReleaseEntity(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<InfectedIncarnate>());
                    }
                }
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
    }

    class InfectedIncarnate : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 26;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override bool CheckActive()
        {
            return false;
        }

        bool meleeOn = false;
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return meleeOn;
        }

        public override void SetDefaults()
        {
            NPC.width = 25;
            NPC.height = 48;
            NPC.damage = 30;
            NPC.defense = 5;
            NPC.friendly = false;
            NPC.boss = true;
            NPC.lifeMax = 1750;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/InfectedIncarnate");
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit18;

            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(NPC);
            modNPC.BossTitle = "Infected Incarnate";
            modNPC.BossSubtitle = "Prototype III";
            modNPC.BossDefeatTitle = "Greater Undead";
            modNPC.BossTitleColor = new Color(255, 116, 39);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "Stiria";
            potionType = ItemID.HealingPotion;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,
                new FlavorTextBestiaryInfoElement("The atrocities committed during the creation of the infection are highly numerous, however, some of the most notable ones are the four prototypes. This one in particular was injected with an experimental strain of the infection to test its ability to consume terror.")
            });
        }

        public override void OnKill()
        {
            foreach (Projectile Projectile in Main.projectile)
            {
                if (player.heldProj != Projectile.whoAmI && !Projectile.minion && !Projectile.sentry)
                {
                    Projectile.active = false;
                }
            }
            TerrorbornSystem.downedInfectedIncarnate = true;
            TerrorbornMod.ScreenDarknessAlpha = 0f;


            bool spawnBD = !TerrorbornPlayer.modPlayer(Main.player[Main.myPlayer]).unlockedAbilities.Contains(8);
            for (int i = 0; i < 1000; i++)
            {
                Projectile Projectile = Main.projectile[i];
                if (Projectile.active && Projectile.type == ModContent.ProjectileType<Abilities.BlinkDash>())
                {
                    spawnBD = false;
                }
            }

            if (spawnBD)
            {
                Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), arena.Center.ToVector2(), Vector2.Zero, ModContent.ProjectileType<Abilities.BlinkDash>(), 0, 0, Main.myPlayer);
                Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), arena.Center.ToVector2(), Vector2.Zero, ModContent.ProjectileType<TeleportLight>(), 0, 0);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.InfectedIncarnateTrophy>(), 10));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<Items.TreasureBags.II_TreasureBag>()));
            
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Equipable.Vanity.BossMasks.UnkindledAnekronianMask>(), 7));
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<Items.Weapons.Ranged.Thrown.GraveNeedle>(),
                ModContent.ItemType<Items.Weapons.Magic.SpellBooks.Infectalanche>(),
                ModContent.ItemType<Items.Weapons.Melee.Swords.NighEndSaber>()));
            notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<Items.Equipable.Armor.SilentHelmet>(),
                ModContent.ItemType<Items.Equipable.Armor.SilentBreastplate>(),
                ModContent.ItemType<Items.Equipable.Armor.SilentGreaves>()));

            npcLoot.Add(notExpertRule);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (teleportCounter > 0)
            {
                return false;
            }
            return base.DrawHealthBar(hbPosition, ref scale, ref position);
        }

        void Teleport(Vector2 NPCPosition, Vector2 playerPosition)
        {
            teleportNPC = !(NPCPosition == Vector2.Zero);
            teleportPlayer = !(playerPosition == Vector2.Zero);

            teleportCounter = 15;
            NPCTeleportPos = NPCPosition;
            playerTeleportPos = playerPosition;

            foreach (Projectile Projectile in Main.projectile)
            {
                if (player.heldProj != Projectile.whoAmI && !Projectile.minion && !Projectile.sentry)
                {
                    Projectile.active = false;
                }
            }

            player.noFallDmg = true;
        }

        List<float> rings = new List<float>();
        int ringCounter = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
            {
                return true;
            }
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 offset = new Vector2(0, -4);
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture), NPC.Center - Main.screenPosition + offset, NPC.frame, drawColor, NPC.rotation, new Vector2(80, 62) / 2, NPC.scale, effects, 0f);
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture + "_Glow"), NPC.Center - Main.screenPosition + offset, NPC.frame, Color.White, NPC.rotation, new Vector2(80, 62) / 2, NPC.scale, effects, 0f);
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture + "_Sword"), NPC.Center - Main.screenPosition + offset, NPC.frame, Color.White, NPC.rotation, new Vector2(80, 62) / 2, NPC.scale, effects, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            for (float i = 1f; i < 1.2f; i += 0.05f)
            {
                spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>(Texture + "_Glow"), NPC.Center - Main.screenPosition + offset, NPC.frame, Color.White * 0.5f, NPC.rotation, new Vector2(80, 62) / 2, NPC.scale * i, effects, 0f);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            if (AIPhase == 2)
            {
                ringCounter++;
                if (ringCounter >= 10)
                {
                    ringCounter = 0;
                    rings.Add(4000f);
                }

                for (int i = 0; i < rings.Count; i++)
                {
                    Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Effects/Textures/Ring1");
                    spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, null, Color.Orange * 0.25f, 0f, texture.Size() / 2, rings[i] / texture.Width, SpriteEffects.None, 0f);

                    rings[i] -= 150f;
                    if (rings[i] <= 0f)
                    {
                        rings.RemoveAt(i);
                    }
                }
            }
            else if (rings.Count != 0)
            {
                rings.Clear();
            }

            if (AIPhase == 4 && hasStartedYet && !phaseStart)
            {
                if (bottom)
                {
                    Terraria.Utils.DrawLine(spriteBatch, new Vector2(targetPosition.X, arena.Center.Y - arena.Height), new Vector2(targetPosition.X, arena.Center.Y + arena.Height), Color.Orange * 0.1f, Color.Orange * 0.35f, 10f);
                }
                else
                {
                    Terraria.Utils.DrawLine(spriteBatch, new Vector2(targetPosition.X, arena.Center.Y - arena.Height), new Vector2(targetPosition.X, arena.Center.Y + arena.Height), Color.Orange * 0.35f, Color.Orange * 0.1f, 10f);
                }
            }
            return false;
        }

        void SetArenaPosition()
        {
            Vector2 arenaPos = TerrorbornSystem.IIShrinePosition * 16;
            arenaPos += new Vector2(-37 * 16, 92 * 16);
            arena = new Rectangle((int)arenaPos.X, (int)arenaPos.Y, 75 * 16, 35 * 16);
        }

        int frame = 1;
        public override void FindFrame(int frameHeight)
        {
            if (!NPC.IsABestiaryIconDummy)
            {
                TerrorbornMod.screenFollowTime = 2;
                TerrorbornMod.screenTransitionTime = 30;
            }
            NPC.frame.Y = frameHeight * frame;
        }

        List<int> NextAttacks = new List<int>();
        bool phaseStart;
        int AIPhase = 0;
        int LastAttack = 0;
        public void DecideNextAttack()
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
            AIPhase = NextAttacks[0];
            LastAttack = AIPhase;
            NextAttacks.RemoveAt(0);
            phaseStart = true;
        }

        Rectangle arena;
        int teleportCounter;
        bool teleportNPC;
        bool teleportPlayer;
        Vector2 NPCTeleportPos;
        Vector2 playerTeleportPos;
        bool start = true;

        int attackCounter1 = 0;
        int attackCounter2 = 0;
        int attackCounter3 = 0;

        Player player;

        public override void AI()
        {
            if (start)
            {
                SetArenaPosition();
                start = false;
                DecideNextAttack();
            }

            NPC.TargetClosest(false);
            player = Main.player[NPC.target];

            if (teleportCounter > 0)
            {
                teleportCounter--;
                NPC.position -= NPC.velocity;
                player.velocity = Vector2.Zero;
                TerrorbornMod.ScreenDarknessAlpha = 1f;

                if (teleportCounter <= 0)
                {
                    if (teleportNPC)
                    {
                        NPC.position = NPCTeleportPos - NPC.Size / 2;
                    }
                    if (teleportPlayer)
                    {
                        player.position = playerTeleportPos - player.Size / 2;
                    }
                    if (player.dead || player.active == false)
                    {
                        NPC.active = false;
                        TerrorbornMod.ScreenDarknessAlpha = 0f;
                    }
                }

                foreach (Projectile Projectile in Main.projectile)
                {
                    if (player.heldProj != Projectile.whoAmI && !Projectile.minion && !Projectile.sentry)
                    {
                        Projectile.active = false;
                    }
                }

                player.noFallDmg = true;
            }
            else
            {
                TerrorbornMod.ScreenDarknessAlpha = 0f;
                if (!player.getRect().Intersects(arena))
                {
                    Teleport(Vector2.Zero, arena.Center.ToVector2());
                    player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was gone in the blink of an eye..."), 120, 0);
                }

                switch (AIPhase)
                {
                    case 0:
                        TeleportLunge(15f, 75, 30);
                        break;
                    case 1:
                        Walk(60 * 4, 1.5f, 0.5f);
                        break;
                    case 2:
                        ShriekOfHorror(60 * 3);
                        break;
                    case 3:
                        if (NPC.life >= NPC.lifeMax * 0.66f) TeleportingSlashes(4, 60);
                        else if (NPC.life >= NPC.lifeMax * 0.33f) TeleportingSlashes(5, 45);
                        else TeleportingSlashes(7, 30);
                        break;
                    case 4:
                        if (NPC.life >= NPC.lifeMax * 0.66f) VerticalDashes(7, 34f);
                        else if (NPC.life >= NPC.lifeMax * 0.25f) VerticalDashes(8, 34f);
                        else VerticalDashes(10, 42f);
                        break;
                    default:
                        break;
                }

                if (teleportCounter > 0)
                {
                    NPC.position -= NPC.velocity;
                    TerrorbornMod.ScreenDarknessAlpha = 1f;
                    player.velocity = Vector2.Zero;
                    player.noFallDmg = true;
                }
            }
        }

        void TeleportLunge(float speed, int delay, int timeUntilTeleport)
        {
            if (phaseStart)
            {
                NPC.noGravity = false;
                NPC.noTileCollide = true;
                int direction = 1;
                if (Main.rand.NextBool()) direction = -1;
                Teleport(new Vector2(arena.Center.X + arena.Width / 10 * direction, arena.Y + 60), arena.Center().FindGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, player.height / 2));
                NPC.velocity = new Vector2(2 * direction, -10);
                attackCounter1 = delay;
                frame = 23;
                NPC.rotation = 0f;
                attackCounter2 = 0;
                phaseStart = false;
            }
            if (attackCounter1 > 0)
            {
                NPC.spriteDirection = Math.Sign(NPC.Center.X - player.Center.X);

                if (NPC.life <= NPC.lifeMax * 0.5f)
                {
                    if ((int)attackCounter1 == (int)(delay / 2))
                    {
                        Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<TeleportLight>(), 0, 0f);
                        NPC.position.X = NPC.position.X + (arena.Center.X - NPC.Center.X) * 2;
                        SoundExtensions.PlaySoundOld(SoundID.Item6, NPC.Center);
                    }
                }

                attackCounter1--;
                if (attackCounter1 <= 0)
                {
                    attackCounter2 = timeUntilTeleport;
                    NPC.velocity = NPC.DirectionTo(player.Center) * speed;
                    NPC.rotation = NPC.DirectionTo(player.Center).ToRotation();
                    if (NPC.spriteDirection == 1)
                    {
                        NPC.rotation = NPC.DirectionTo(player.Center).ToRotation() + MathHelper.ToRadians(180);
                    }
                    SoundExtensions.PlaySoundOld(SoundID.Item71, (int)NPC.Center.X, (int)NPC.Center.Y, 71, 2.5f, -0.25f);
                    meleeOn = true;
                    NPC.noGravity = true;
                    frame = 24;
                }
            }
            if (attackCounter2 > 0)
            {
                attackCounter2--;
                if (attackCounter2 <= 0)
                {
                    meleeOn = false;
                    DecideNextAttack();
                }
            }
        }

        float walkFrameCounter = 0f;
        void WalkAnimation(float speed)
        {
            walkFrameCounter += speed;
            float amount = 3f;
            while (walkFrameCounter >= amount)
            {
                walkFrameCounter -= amount;
                frame++;
            }
            if (frame < 2)
            {
                frame = 2;
            }
            if (frame > 15)
            {
                frame = 2;
            }
        }

        void Walk(int time, float maxSpeed, float acceleration)
        {
            if (phaseStart)
            {
                int direction = 1;
                if (Main.rand.NextBool()) direction = -1;
                int distanceFromCenter = 250;
                Teleport((arena.Center() + new Vector2(distanceFromCenter * direction, 0)).FindGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, NPC.height / 2), (arena.Center() + new Vector2(distanceFromCenter * -direction, 0)).FindGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, player.height / 2));
                NPC.noTileCollide = false;
                NPC.noGravity = false;
                NPC.rotation = 0f;
                frame = 2;
                attackCounter1 = time;
                meleeOn = false;
                attackCounter2 = 0;
                phaseStart = false;
                NPC.velocity = Vector2.Zero;
            }

            NPC.spriteDirection = Math.Sign(NPC.Center.X - player.Center.X);
            NPC.velocity.X += acceleration * -NPC.spriteDirection;

            if (NPC.velocity.X > maxSpeed)
            {
                NPC.velocity.X = maxSpeed;
            }

            if (NPC.velocity.X < -maxSpeed)
            {
                NPC.velocity.X = -maxSpeed;
            }

            WalkAnimation(Math.Abs(NPC.velocity.X));

            if (NPC.life <= NPC.lifeMax * 0.65f)
            {
                attackCounter2++;
                if (attackCounter2 >= 60)
                {
                    attackCounter2 = 0;
                    Vector2 position = player.Center.FindCeilingAbove(ModContent.TileType<Tiles.MemorialBrick>());
                    Vector2 velocity = new Vector2(0, 10);
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), position, velocity, ModContent.ProjectileType<InfectedBoulder>(), 50 / 4, 0f);
                }
            }

            attackCounter1--;
            if (attackCounter1 <= 0)
            {
                DecideNextAttack();
            }
        }

        bool spawnedProjectile = false;
        int shriekFrameCounter = 0;
        int shriekSoundCounter = 0;
        void ShriekOfHorror(int time)
        {
            if (phaseStart)
            {
                int direction = 1;
                if (Main.rand.NextBool()) direction = -1;
                int distanceFromCenter = 100;
                Teleport((arena.Center() + new Vector2(distanceFromCenter * direction, 0)).FindGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, NPC.height / 2), (arena.Center() + new Vector2(distanceFromCenter * -direction, 0)).FindGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, player.height / 2));
                NPC.noTileCollide = false;
                NPC.noGravity = false;
                NPC.rotation = 0f;
                NPC.velocity = Vector2.Zero;
                frame = 16;
                attackCounter1 = time;
                meleeOn = false;
                attackCounter2 = 0;
                phaseStart = false;
                spawnedProjectile = false;
                shriekSoundCounter = 0;
            }

            if (teleportCounter <= 0 && !spawnedProjectile)
            {
                spawnedProjectile = true;
                int projCount = 7 + (int)(5f * (1f - ((float)NPC.life / (float)NPC.lifeMax)));
                float offset = Main.rand.NextFloat(0f, 360f / projCount);
                for (float i = 0f; i < 1f; i += 1f / (float)projCount)
                {
                    Vector2 direction = MathHelper.ToRadians(360f * i + offset).ToRotationVector2();
                    float distance = 1000f;
                    float speed = distance / (float)time;
                    Vector2 velocity = -speed * direction;
                    Vector2 position = NPC.Center + distance * direction;
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), position, velocity, ModContent.ProjectileType<IncarnateLaser>(), 40 / 4, 0f);
                }
                CombatText.NewText(NPC.getRect(), new Color(255, 116, 39), "Shriek of Horror");
            }

            shriekFrameCounter--;
            if (shriekFrameCounter <= 0)
            {
                shriekFrameCounter = 30;
                if (frame == 16)
                {
                    frame = 17;
                }
                else
                {
                    frame = 16;
                }
            }

            shriekSoundCounter--;
            if (shriekSoundCounter <= 0 && teleportCounter <= 0)
            {
                shriekSoundCounter = 22;
                SoundExtensions.PlaySoundOld(SoundID.Item103, NPC.Center);
            }

            player.position.X += Math.Sign(NPC.Center.X - player.Center.X) * 1.5f;

            NPC.spriteDirection = Math.Sign(NPC.Center.X - player.Center.X);
            TerrorbornSystem.ScreenShake(2f);
            TerrorbornPlayer.modPlayer(player).LoseTerror(2f, true, true);

            attackCounter1--;
            if (attackCounter1 <= 0)
            {
                DecideNextAttack();
            }
        }

        int slashingFrameCounter = 0;
        void TeleportingSlashes(int amount, int delay)
        {
            if (phaseStart)
            {
                int direction = 1;
                if (Main.rand.NextBool()) direction = -1;
                int distanceFromCenter = 200;
                Teleport((arena.Center() + new Vector2(distanceFromCenter * direction, 0)).FindCeilingAbove(ModContent.TileType<Tiles.MemorialBrick>()) + new Vector2(0, NPC.height / 2), (arena.Center() + new Vector2(distanceFromCenter * -direction, 0)).FindGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, player.height / 2));
                NPC.noTileCollide = false;
                NPC.noGravity = true;
                NPC.rotation = 0f;
                NPC.velocity = Vector2.Zero;
                frame = 18;
                attackCounter1 = amount;
                meleeOn = false;
                attackCounter2 = 0;
                phaseStart = false;
                slashingFrameCounter = 0;
                NPC.spriteDirection = -Math.Sign(NPC.Center.X - player.Center.X);
                NPC.rotation = MathHelper.ToRadians(180);
            }

            if (NPC.position.Y > arena.Center.Y)
            {
                NPC.spriteDirection = Math.Sign(NPC.Center.X - player.Center.X);
            }
            else
            {
                NPC.spriteDirection = -Math.Sign(NPC.Center.X - player.Center.X);
            }

            slashingFrameCounter++;
            if (slashingFrameCounter >= delay / 5)
            {
                frame++;
                slashingFrameCounter = 0;
            }

            if (frame == 20 && slashingFrameCounter == 0)
            {
                float speed = 10f;
                Vector2 velocity = NPC.DirectionTo(player.Center) * speed;
                Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, velocity, ModContent.ProjectileType<SlashAttack>(), 50 / 4, 0f);
                SoundExtensions.PlaySoundOld(SoundID.Item71, (int)NPC.Center.X, (int)NPC.Center.Y, 71, 2.5f, 0.25f);

            }

            if (frame > 22)
            {
                attackCounter1--;
                if (attackCounter1 <= 0)
                {
                    DecideNextAttack();
                }
                else
                {
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<TeleportLight>(), 0, 0f);

                    Vector2 position = FindTeleportPosition(350);

                    while (player.Distance(position) <= 300)
                    {
                        position = FindTeleportPosition(450);
                    }

                    if (position.Y > arena.Center.Y)
                    {
                        NPC.position = position - new Vector2(NPC.width / 2, NPC.height);
                        NPC.spriteDirection = Math.Sign(NPC.Center.X - player.Center.X);
                        NPC.rotation = 0f;
                    }
                    else
                    {
                        NPC.position = position - new Vector2(NPC.width / 2, -6);
                        NPC.spriteDirection = -Math.Sign(NPC.Center.X - player.Center.X);
                        NPC.rotation = MathHelper.ToRadians(180);
                    }
                    frame = 18;
                    SoundExtensions.PlaySoundOld(SoundID.Item6, NPC.Center);
                }
            }
        }

        Vector2 FindTeleportPosition(int maxDistance)
        {
            bool above = Main.rand.NextBool();
            if (above)
            {
                return new Vector2(arena.Center.X + Main.rand.Next(-maxDistance, maxDistance), arena.Center.Y).FindCeilingAbove(ModContent.TileType<Tiles.MemorialBrick>());
            }
            else
            {
                return new Vector2(arena.Center.X + Main.rand.Next(-maxDistance, maxDistance), arena.Center.Y).FindGroundUnder(ModContent.TileType<Tiles.MemorialBrick>());
            }
        }

        bool bottom = false;
        Vector2 targetPosition;
        bool hasStartedYet = false;
        int teleportDirection = 1;
        void VerticalDashes(int amount, float speed)
        {
            float distance = arena.Height * 3f;
            if (phaseStart)
            {
                teleportDirection = 1;
                if (Main.rand.NextBool()) teleportDirection = -1;
                int distanceFromCenter = 200;
                Teleport((arena.Center() + new Vector2(distanceFromCenter * teleportDirection, 0)).FindCeilingAbove(ModContent.TileType<Tiles.MemorialBrick>()) + new Vector2(0, NPC.height / 2), (arena.Center().FindGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, player.height / 2)));
                NPC.noTileCollide = true;
                NPC.noGravity = true;
                NPC.rotation = 0f;
                frame = 24;
                attackCounter1 = amount;
                attackCounter2 = 0;
                attackCounter3 = 0;
                meleeOn = false;
                phaseStart = false;
                slashingFrameCounter = 0;
                NPC.spriteDirection = -1;
                NPC.rotation = MathHelper.ToRadians(90);
                NPC.velocity = new Vector2(0, 18f);
                bottom = true;
                hasStartedYet = false;
            }

            if (teleportCounter <= 0 && !hasStartedYet)
            {
                hasStartedYet = true;
                Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), NPC.Center + new Vector2(0, 2000), new Vector2(0, -1), ModContent.ProjectileType<InfectedSlash>(), 65 / 4, 0f);
                SoundExtensions.PlaySoundOld(SoundID.Item71, (int)NPC.Center.X, (int)NPC.Center.Y, 71, 2.5f, -0.25f);
                targetPosition = player.Center;
            }

            attackCounter2++;
            if (attackCounter2 > distance / speed)
            {
                attackCounter1--;
                if (attackCounter1 <= 0)
                {
                    hasStartedYet = false;
                    DecideNextAttack();
                }
                else
                {
                    Projectile.NewProjectile(NPC.GetSource_ReleaseEntity(), targetPosition + new Vector2(-5, 2000), new Vector2(0, -1), ModContent.ProjectileType<InfectedSlash>(), 40 / 4, 0f);
                    attackCounter2 = 0;
                    NPC.position.X = targetPosition.X - NPC.width / 2;
                    if (bottom)
                    {
                        bottom = false;
                        NPC.rotation = MathHelper.ToRadians(-90);
                        NPC.velocity = new Vector2(0, -speed);
                        NPC.position.Y = arena.Center.Y + 1000f;
                    }
                    else
                    {
                        bottom = true;
                        NPC.rotation = MathHelper.ToRadians(90);
                        NPC.velocity = new Vector2(0, speed);
                        NPC.position.Y = arena.Center.Y - 1000f;
                    }
                    SoundExtensions.PlaySoundOld(SoundID.Item71, (int)NPC.Center.X, (int)NPC.Center.Y, 71, 2.5f, -0.25f);

                    targetPosition = player.Center;
                    if (attackCounter1 == (int)(amount * 0.65f) && NPC.life <= NPC.lifeMax * 0.75f)
                    {
                        targetPosition.X += -teleportDirection * 200;
                    }
                }
            }
        }
    }

    class InfectedBoulder : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = false;
        }

        int timeUntilCollide = 20;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, position, new Rectangle(0, 0, Projectile.width, Projectile.height), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), 1f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            for (float i = 1f; i < 1.2f; i += 0.05f)
            {
                Main.spriteBatch.Draw(texture, position, new Rectangle(0, 0, Projectile.width, Projectile.height), Projectile.GetAlpha(Color.White) * 0.5f, Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), i, SpriteEffects.None, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        bool start = true;
        public override void AI()
        {
            if (start)
            {
                Projectile.rotation = MathHelper.ToRadians(Main.rand.Next(360));
                start = false;
            }
            Projectile.rotation += MathHelper.ToRadians(5f);

            timeUntilCollide--;
            if (timeUntilCollide <= 0)
            {
                Projectile.tileCollide = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<TeleportLight>(), 0, 0);
            SoundExtensions.PlaySoundOld(SoundID.Item14, Projectile.Center);
            TerrorbornSystem.ScreenShake(3f);
        }
    }

    class IncarnateLaser : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/MagicGuns/TarSwarm";

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
            Projectile.timeLeft = 600;
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
                List<Vector2> positions = bezier.GetPoints(50);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.OrangeRed, Color.Orange, mult)) * mult;
                    Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * mult), color);
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

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }

    class SlashAttack : ModProjectile
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
                Color color = Projectile.GetAlpha(Color.White) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
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

    class TeleportLight : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Effects/Textures/Glow_2";

        int timeLeft = 15;
        const int defaultSize = 100;
        int currentSize = defaultSize;
        public override void SetDefaults()
        {
            Projectile.width = defaultSize;
            Projectile.height = defaultSize;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = timeLeft;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, currentSize, Color.Orange);
            return false;
        }

        public override void AI()
        {
            currentSize -= defaultSize / timeLeft;
        }
    }

    class InfectedSlash : Deathray
    {
        const int timeLeft = 30;
        int timeUntilFinished = timeLeft;
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/LightBlast";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 6000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = false;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 3600;
            MoveDistance = 20f;
            RealMaxDistance = 4000f;
            bodyRect = new Rectangle(0, 0, 10, 10);
            headRect = new Rectangle(0, 0, 10, 10);
            tailRect = new Rectangle(0, 0, 10, 10);
            deathrayWidth = 0;
            FollowPosition = false;
        }

        public override void PostAI()
        {
            if (timeUntilFinished > 0)
            {
                timeUntilFinished--;
                deathrayWidth += 1f / (float)timeLeft;
            }
        }
    }
}
