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
using TerrorbornMod.TBUtils;

namespace TerrorbornMod.NPCs.Bosses.InfectedIncarnate
{
    class MemorialCoffin : ModNPC
    {
        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Vector2 originPoint = npc.Center;
            Vector2 center = arena.Center.ToVector2().findCeilingAbove(ModContent.TileType<Tiles.MemorialBrick>());
            Vector2 distToProj = originPoint - center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            Texture2D texture = ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/InfectedIncarnate/Chainlink");

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
            return base.PreDraw(spriteBatch, drawColor);
        }

        public override void SetDefaults()
        {
            npc.width = 48;
            npc.height = 74;
            npc.friendly = false;
            npc.dontTakeDamage = true;
            npc.damage = 0;
            npc.lifeMax = 5;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        Rectangle arena;
        void SetArenaPosition()
        {
            Vector2 arenaPos = TerrorbornWorld.IIShrinePosition * 16;
            arenaPos += new Vector2(-37 * 16, 92 * 16);
            arena = new Rectangle((int)arenaPos.X, (int)arenaPos.Y, 75 * 16, 35 * 16);
        }

        bool spawningAnimation = false;
        int shakeCounter = 0;
        int shakesLeft = 3;
        const int totalSpawningTime = 510;
        double timeAlive = 0;
        public override void AI()
        {
            SetArenaPosition();

            timeAlive++;
            float rotation = MathHelper.ToRadians(94f + (float)Math.Sin(timeAlive / 50) * 4f);

            npc.position = arena.Center.ToVector2().findCeilingAbove(ModContent.TileType<Tiles.MemorialBrick>()) + rotation.ToRotationVector2() * arena.Height / 2;
            npc.rotation = rotation - MathHelper.ToRadians(94f);

            if (TerrorbornPlayer.modPlayer(Main.LocalPlayer).ShriekTime > 0 && arena.Contains(Main.LocalPlayer.getRect()) && !spawningAnimation)
            {
                spawningAnimation = true;
                TerrorbornMod.SetScreenToPosition(60, totalSpawningTime, arena.Center.ToVector2(), 0.9f);
            }
            
            if (spawningAnimation)
            {
                music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/InfectedIncarnate");
                shakeCounter++;
                if (shakeCounter >= totalSpawningTime / 3)
                {
                    shakeCounter = 0;
                    shakesLeft--;
                    TerrorbornMod.ScreenShake(10f);
                    if (shakesLeft == 1)
                    {
                        TerrorbornMod.ScreenShake(15f);
                    }
                    Main.PlaySound(SoundID.Item62, npc.Center);

                    if (shakesLeft <= 0)
                    {
                        npc.active = false;
                        NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<InfectedIncarnate>());
                    }
                }
            }
        }
    }

    class InfectedIncarnate : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;
            Main.npcFrameCount[npc.type] = 26;
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
            npc.width = 25;
            npc.height = 48;
            npc.damage = 30;
            npc.defense = 5;
            npc.friendly = false;
            npc.boss = true;
            npc.lifeMax = 1750;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/InfectedIncarnate");
            npc.knockBackResist = 0f;
            npc.aiStyle = -1;
            npc.HitSound = SoundID.NPCHit18;

            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(npc);
            modNPC.BossTitle = "Infected Incarnate";
            modNPC.BossSubtitle = "Prototype III";
            modNPC.BossTitleColor = new Color(255, 116, 39);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 3500;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "Stiria";
            potionType = ItemID.HealingPotion;
        }

        public override void NPCLoot()
        {
            foreach (Projectile projectile in Main.projectile)
            {
                if (player.heldProj != projectile.whoAmI)
                {
                    projectile.active = false;
                }
            }
            TerrorbornWorld.downedInfectedIncarnate = true;
            TerrorbornMod.ScreenDarknessAlpha = 0f;


            bool spawnBD = !TerrorbornPlayer.modPlayer(Main.player[Main.myPlayer]).unlockedAbilities.Contains(8);
            for (int i = 0; i < 1000; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.type == ModContent.ProjectileType<Abilities.BlinkDash>())
                {
                    spawnBD = false;
                }
            }

            if (spawnBD)
            {
                Projectile.NewProjectile(arena.Center.ToVector2(), Vector2.Zero, ModContent.ProjectileType<Abilities.BlinkDash>(), 0, 0, Main.myPlayer);
                Projectile.NewProjectile(arena.Center.ToVector2(), Vector2.Zero, ModContent.ProjectileType<TeleportLight>(), 0, 0);
            }

            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Placeable.Furniture.InfectedIncarnateTrophy>());
            }

            if (Main.expertMode)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.TreasureBags.II_TreasureBag>());
            }
            else
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Equipable.Armor.SilentHelmet>());
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Equipable.Armor.SilentBreastplate>());
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Equipable.Armor.SilentGreaves>());

                switch (Main.rand.Next(3))
                {
                    case 0:
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Weapons.Melee.NighEndSaber>());
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Weapons.Magic.Infectalanche>());
                        break;
                    case 1:
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Weapons.Ranged.GraveNeedle>());
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Weapons.Magic.Infectalanche>());
                        break;
                    case 2:
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Weapons.Melee.NighEndSaber>());
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Weapons.Ranged.GraveNeedle>());
                        break;
                }

                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Vanity.BossMasks.UnkindledAnekronianMask>());
                }
            }
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
            npcTeleportPos = NPCPosition;
            playerTeleportPos = playerPosition;

            foreach (Projectile projectile in Main.projectile)
            {
                if (player.heldProj != projectile.whoAmI)
                {
                    projectile.active = false;
                }
            }

            player.noFallDmg = true;
        }

        List<float> rings = new List<float>();
        int ringCounter = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (npc.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 offset = new Vector2(0, -4);
            spriteBatch.Draw(ModContent.GetTexture(Texture), npc.Center - Main.screenPosition + offset, npc.frame, drawColor, npc.rotation, new Vector2(80, 62) / 2, npc.scale, effects, 0f);
            spriteBatch.Draw(ModContent.GetTexture(Texture + "_Glow"), npc.Center - Main.screenPosition + offset, npc.frame, Color.White, npc.rotation, new Vector2(80, 62) / 2, npc.scale, effects, 0f);
            spriteBatch.Draw(ModContent.GetTexture(Texture + "_Sword"), npc.Center - Main.screenPosition + offset, npc.frame, Color.White, npc.rotation, new Vector2(80, 62) / 2, npc.scale, effects, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            for (float i = 1f; i < 1.2f; i += 0.05f)
            {
                spriteBatch.Draw(ModContent.GetTexture(Texture + "_Glow"), npc.Center - Main.screenPosition + offset, npc.frame, Color.White * 0.5f, npc.rotation, new Vector2(80, 62) / 2, npc.scale * i, effects, 0f);
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
                    Texture2D texture = ModContent.GetTexture("TerrorbornMod/Effects/Textures/Ring1");
                    spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, Color.Orange * 0.25f, 0f, texture.Size() / 2, rings[i] / texture.Width, SpriteEffects.None, 0f);

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
                    Utils.DrawLine(spriteBatch, new Vector2(targetPosition.X, arena.Center.Y - arena.Height), new Vector2(targetPosition.X, arena.Center.Y + arena.Height), Color.Orange * 0.1f, Color.Orange * 0.35f, 10f);
                }
                else
                {
                    Utils.DrawLine(spriteBatch, new Vector2(targetPosition.X, arena.Center.Y - arena.Height), new Vector2(targetPosition.X, arena.Center.Y + arena.Height), Color.Orange * 0.35f, Color.Orange * 0.1f, 10f);
                }
            }
            return false;
        }

        void SetArenaPosition()
        {
            Vector2 arenaPos = TerrorbornWorld.IIShrinePosition * 16;
            arenaPos += new Vector2(-37 * 16, 92 * 16);
            arena = new Rectangle((int)arenaPos.X, (int)arenaPos.Y, 75 * 16, 35 * 16);
        }

        int frame = 1;
        public override void FindFrame(int frameHeight)
        {
            TerrorbornMod.screenFollowTime = 2;
            npc.frame.Y = frameHeight * frame;
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
        Vector2 npcTeleportPos;
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

            npc.TargetClosest(false);
            player = Main.player[npc.target];

            if (teleportCounter > 0)
            {
                teleportCounter--;
                npc.position -= npc.velocity;
                player.velocity = Vector2.Zero;
                TerrorbornMod.ScreenDarknessAlpha = 1f;

                if (teleportCounter <= 0)
                {
                    if (teleportNPC)
                    {
                        npc.position = npcTeleportPos - npc.Size / 2;
                    }
                    if (teleportPlayer)
                    {
                        player.position = playerTeleportPos - player.Size / 2;
                    }
                    if (player.dead || player.active == false)
                    {
                        npc.active = false;
                        TerrorbornMod.ScreenDarknessAlpha = 0f;
                    }
                }

                foreach (Projectile projectile in Main.projectile)
                {
                    if (player.heldProj != projectile.whoAmI)
                    {
                        projectile.active = false;
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
                        if (npc.life >= npc.lifeMax * 0.66f) TeleportingSlashes(4, 60);
                        else if (npc.life >= npc.lifeMax * 0.33f) TeleportingSlashes(5, 45);
                        else TeleportingSlashes(7, 30);
                        break;
                    case 4:
                        if (npc.life >= npc.lifeMax * 0.66f) VerticalDashes(7, 34f);
                        else if (npc.life >= npc.lifeMax * 0.25f) VerticalDashes(8, 34f);
                        else VerticalDashes(10, 42f);
                        break;
                    default:
                        break;
                }

                if (teleportCounter > 0)
                {
                    npc.position -= npc.velocity;
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
                npc.noGravity = false;
                npc.noTileCollide = true;
                int direction = 1;
                if (Main.rand.NextBool()) direction = -1;
                Teleport(new Vector2(arena.Center.X + arena.Width / 10 * direction, arena.Y + 60), arena.Center().findGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, player.height / 2));
                npc.velocity = new Vector2(2 * direction, -10);
                attackCounter1 = delay;
                frame = 23;
                npc.rotation = 0f;
                attackCounter2 = 0;
                phaseStart = false;
            }
            if (attackCounter1 > 0)
            {
                npc.spriteDirection = Math.Sign(npc.Center.X - player.Center.X);

                if (npc.life <= npc.lifeMax * 0.5f)
                {
                    if ((int)attackCounter1 == (int)(delay / 2))
                    {
                        Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<TeleportLight>(), 0, 0f);
                        npc.position.X = npc.position.X + (arena.Center.X - npc.Center.X) * 2;
                        Main.PlaySound(SoundID.Item6, npc.Center);
                    }
                }

                attackCounter1--;
                if (attackCounter1 <= 0)
                {
                    attackCounter2 = timeUntilTeleport;
                    npc.velocity = npc.DirectionTo(player.Center) * speed;
                    npc.rotation = npc.DirectionTo(player.Center).ToRotation();
                    if (npc.spriteDirection == 1)
                    {
                        npc.rotation = npc.DirectionTo(player.Center).ToRotation() + MathHelper.ToRadians(180);
                    }
                    Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 71, 2.5f, -0.25f);
                    meleeOn = true;
                    npc.noGravity = true;
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
                Teleport((arena.Center() + new Vector2(distanceFromCenter * direction, 0)).findGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, npc.height / 2), (arena.Center() + new Vector2(distanceFromCenter * -direction, 0)).findGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, player.height / 2));
                npc.noTileCollide = false;
                npc.noGravity = false;
                npc.rotation = 0f;
                frame = 2;
                attackCounter1 = time;
                meleeOn = false;
                attackCounter2 = 0;
                phaseStart = false;
                npc.velocity = Vector2.Zero;
            }

            npc.spriteDirection = Math.Sign(npc.Center.X - player.Center.X);
            npc.velocity.X += acceleration * -npc.spriteDirection;

            if (npc.velocity.X > maxSpeed)
            {
                npc.velocity.X = maxSpeed;
            }

            if (npc.velocity.X < -maxSpeed)
            {
                npc.velocity.X = -maxSpeed;
            }

            WalkAnimation(Math.Abs(npc.velocity.X));

            if (npc.life <= npc.lifeMax * 0.65f)
            {
                attackCounter2++;
                if (attackCounter2 >= 60)
                {
                    attackCounter2 = 0;
                    Vector2 position = player.Center.findCeilingAbove(ModContent.TileType<Tiles.MemorialBrick>());
                    Vector2 velocity = new Vector2(0, 10);
                    Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<InfectedBoulder>(), 50 / 4, 0f);
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
                Teleport((arena.Center() + new Vector2(distanceFromCenter * direction, 0)).findGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, npc.height / 2), (arena.Center() + new Vector2(distanceFromCenter * -direction, 0)).findGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, player.height / 2));
                npc.noTileCollide = false;
                npc.noGravity = false;
                npc.rotation = 0f;
                npc.velocity = Vector2.Zero;
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
                int projCount = 7 + (int)(5f * (1f - ((float)npc.life / (float)npc.lifeMax)));
                float offset = Main.rand.NextFloat(0f, 360f / projCount);
                for (float i = 0f; i < 1f; i += 1f / (float)projCount)
                {
                    Vector2 direction = MathHelper.ToRadians(360f * i + offset).ToRotationVector2();
                    float distance = 1000f;
                    float speed = distance / (float)time;
                    Vector2 velocity = -speed * direction;
                    Vector2 position = npc.Center + distance * direction;
                    Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<IncarnateLaser>(), 40 / 4, 0f);
                }
                CombatText.NewText(npc.getRect(), new Color(255, 116, 39), "Shriek of Horror");
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
                Main.PlaySound(SoundID.Item103, npc.Center);
            }

            player.position.X += Math.Sign(npc.Center.X - player.Center.X) * 1.5f;

            npc.spriteDirection = Math.Sign(npc.Center.X - player.Center.X);
            TerrorbornMod.ScreenShake(2f);
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
                Teleport((arena.Center() + new Vector2(distanceFromCenter * direction, 0)).findCeilingAbove(ModContent.TileType<Tiles.MemorialBrick>()) + new Vector2(0, npc.height / 2), (arena.Center() + new Vector2(distanceFromCenter * -direction, 0)).findGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, player.height / 2));
                npc.noTileCollide = false;
                npc.noGravity = true;
                npc.rotation = 0f;
                npc.velocity = Vector2.Zero;
                frame = 18;
                attackCounter1 = amount;
                meleeOn = false;
                attackCounter2 = 0;
                phaseStart = false;
                slashingFrameCounter = 0;
                npc.spriteDirection = -Math.Sign(npc.Center.X - player.Center.X);
                npc.rotation = MathHelper.ToRadians(180);
            }

            if (npc.position.Y > arena.Center.Y)
            {
                npc.spriteDirection = Math.Sign(npc.Center.X - player.Center.X);
            }
            else
            {
                npc.spriteDirection = -Math.Sign(npc.Center.X - player.Center.X);
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
                Vector2 velocity = npc.DirectionTo(player.Center) * speed;
                Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<SlashAttack>(), 50 / 4, 0f);
                Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 71, 2.5f, 0.25f);

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
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<TeleportLight>(), 0, 0f);

                    Vector2 position = FindTeleportPosition(350);

                    while (player.Distance(position) <= 300)
                    {
                        position = FindTeleportPosition(450);
                    }

                    if (position.Y > arena.Center.Y)
                    {
                        npc.position = position - new Vector2(npc.width / 2, npc.height);
                        npc.spriteDirection = Math.Sign(npc.Center.X - player.Center.X);
                        npc.rotation = 0f;
                    }
                    else
                    {
                        npc.position = position - new Vector2(npc.width / 2, -6);
                        npc.spriteDirection = -Math.Sign(npc.Center.X - player.Center.X);
                        npc.rotation = MathHelper.ToRadians(180);
                    }
                    frame = 18;
                    Main.PlaySound(SoundID.Item6, npc.Center);
                }
            }
        }

        Vector2 FindTeleportPosition(int maxDistance)
        {
            bool above = Main.rand.NextBool();
            if (above)
            {
                return new Vector2(arena.Center.X + Main.rand.Next(-maxDistance, maxDistance), arena.Center.Y).findCeilingAbove(ModContent.TileType<Tiles.MemorialBrick>());
            }
            else
            {
                return new Vector2(arena.Center.X + Main.rand.Next(-maxDistance, maxDistance), arena.Center.Y).findGroundUnder(ModContent.TileType<Tiles.MemorialBrick>());
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
                Teleport((arena.Center() + new Vector2(distanceFromCenter * teleportDirection, 0)).findCeilingAbove(ModContent.TileType<Tiles.MemorialBrick>()) + new Vector2(0, npc.height / 2), (arena.Center().findGroundUnder(ModContent.TileType<Tiles.MemorialBrick>()) - new Vector2(0, player.height / 2)));
                npc.noTileCollide = true;
                npc.noGravity = true;
                npc.rotation = 0f;
                frame = 24;
                attackCounter1 = amount;
                attackCounter2 = 0;
                attackCounter3 = 0;
                meleeOn = false;
                phaseStart = false;
                slashingFrameCounter = 0;
                npc.spriteDirection = -1;
                npc.rotation = MathHelper.ToRadians(90);
                npc.velocity = new Vector2(0, 18f);
                bottom = true;
                hasStartedYet = false;
            }

            if (teleportCounter <= 0 && !hasStartedYet)
            {
                hasStartedYet = true;
                Projectile.NewProjectile(npc.Center + new Vector2(0, 2000), new Vector2(0, -1), ModContent.ProjectileType<InfectedSlash>(), 65 / 4, 0f);
                Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 71, 2.5f, -0.25f);
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
                    Projectile.NewProjectile(targetPosition + new Vector2(-5, 2000), new Vector2(0, -1), ModContent.ProjectileType<InfectedSlash>(), 40 / 4, 0f);
                    attackCounter2 = 0;
                    npc.position.X = targetPosition.X - npc.width / 2;
                    if (bottom)
                    {
                        bottom = false;
                        npc.rotation = MathHelper.ToRadians(-90);
                        npc.velocity = new Vector2(0, -speed);
                        npc.position.Y = arena.Center.Y + 1000f;
                    }
                    else
                    {
                        bottom = true;
                        npc.rotation = MathHelper.ToRadians(90);
                        npc.velocity = new Vector2(0, speed);
                        npc.position.Y = arena.Center.Y - 1000f;
                    }
                    Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 71, 2.5f, -0.25f);

                    targetPosition = player.Center;
                    if (attackCounter1 == (int)(amount * 0.65f) && npc.life <= npc.lifeMax * 0.75f)
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
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 500;
            projectile.tileCollide = false;
        }

        int timeUntilCollide = 20;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 position = projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, position, new Rectangle(0, 0, projectile.width, projectile.height), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), 1f, SpriteEffects.None, 0);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            for (float i = 1f; i < 1.2f; i += 0.05f)
            {
                Main.spriteBatch.Draw(texture, position, new Rectangle(0, 0, projectile.width, projectile.height), projectile.GetAlpha(Color.White) * 0.5f, projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), i, SpriteEffects.None, 0);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        bool start = true;
        public override void AI()
        {
            if (start)
            {
                projectile.rotation = MathHelper.ToRadians(Main.rand.Next(360));
                start = false;
            }
            projectile.rotation += MathHelper.ToRadians(5f);

            timeUntilCollide--;
            if (timeUntilCollide <= 0)
            {
                projectile.tileCollide = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<TeleportLight>(), 0, 0);
            Main.PlaySound(SoundID.Item14, projectile.Center);
            TerrorbornMod.ScreenShake(3f);
        }
    }

    class IncarnateLaser : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/TarSwarm";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            BezierCurve bezier = new BezierCurve();
            bezier.Controls.Clear();
            foreach (Vector2 pos in projectile.oldPos)
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
                    Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
                    Color color = projectile.GetAlpha(Color.Lerp(Color.OrangeRed, Color.Orange, mult)) * mult;
                    Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(25f * mult), color);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
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
            projectile.rotation = projectile.velocity.ToRotation();
        }
    }

    class SlashAttack : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 42;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
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
            projectile.width = defaultSize;
            projectile.height = defaultSize;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.localNPCHitCooldown = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.timeLeft = timeLeft;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Graphics.DrawGlow_1(spriteBatch, projectile.Center - Main.screenPosition, currentSize, Color.Orange);
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
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.hide = false;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.timeLeft = 3600;
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
