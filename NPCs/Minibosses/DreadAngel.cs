using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TerrorbornMod.NPCs.Minibosses
{
    class DreadAngel : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 20;
            NPCID.Sets.TrailCacheLength[npc.type] = 1;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 60;
            npc.height = 90;
            npc.damage = 75;
            npc.defense = 70;
            npc.lifeMax = 35000;
            npc.HitSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Effects/DreadAngelHurt");
            npc.DeathSound = SoundID.NPCDeath39;
            npc.value = Item.buyPrice(0, 20, 0, 0);
            npc.aiStyle = -1;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(npc);
            modNPC.getsTitleCard = true;
            modNPC.BossTitle = "Dread Angel";
            modNPC.BossSubtitle = "Servant of Uriel";
            modNPC.BossTitleColor = Color.Goldenrod;
        }

        public override void NPCLoot()
        {

            TerrorbornWorld.downedDreadAngel = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornPlayer.modPlayer(spawnInfo.player).ZoneIncendiary && NPC.downedMoonlord && !NPC.AnyNPCs(ModContent.NPCType<DreadAngel>()))
            {
                return SpawnCondition.Sky.Chance * 0.02f;
            }
            else
            {
                return 0f;
            }
        }

        int frame = 0;
        bool attacking = false;
        bool hasWeapon = true;

        public override void FindFrame(int frameHeight)
        {
            if (!attacking)
            {
                if (hasWeapon)
                {
                    npc.frameCounter++;
                    if (npc.frameCounter > 4)
                    {
                        npc.frameCounter = 0;
                        frame++;
                    }
                    if (frame >= 5)
                    {
                        frame = 0;
                    }
                }
                else
                {
                    npc.frameCounter++;
                    if (npc.frameCounter > 4)
                    {
                        npc.frameCounter = 0;
                        frame++;
                    }
                    if (frame < 14 || frame >= 20)
                    {
                        frame = 14;
                    }
                }
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (spawningLaser)
            {
                TBUtils.Graphics.DrawGlow_1(spriteBatch, npc.Center - Main.screenPosition, 200, Color.LightPink * 0.5f);
                Utils.DrawLine(spriteBatch, laserPosition + new Vector2(0, -3000), laserPosition + new Vector2(0, 3000), Color.LightPink * 0.5f);
            }
            return base.PreDraw(spriteBatch, drawColor);
        }

        bool spawningLaser = false;
        Vector2 laserPosition;
        int AIPhase = 0;
        int attackCounter = 0;
        int aliveCounter = 0;
        bool start = true;
        Player player;
        public override void AI()
        {
            if (start)
            {
                npc.TargetClosest();
                player = Main.player[npc.target];
                start = false;
                spawningLaser = true;
                laserPosition = player.Center;
            }

            npc.spriteDirection = Math.Sign(player.Center.X - npc.Center.X);

            aliveCounter++;
            if (aliveCounter == 60 * 3)
            {
                WeightedRandom<string> messages = new WeightedRandom<string>();
                messages.Add("BEHOLD! The power of an ANGEL!");
                messages.Add("Prepare thyself, traitor!");
                messages.Add("Though shall repent, incarnate, though shall repent.");
                messages.Add("Thine end has come!");
                messages.Add("Is your software running slow?");
                CombatText.NewText(npc.getRect(), Color.Gold, messages, true);
            }
            if (aliveCounter % 600 == 599)
            {
                WeightedRandom<string> messages = new WeightedRandom<string>();
                messages.Add("Die, I tell you, DIE!");
                messages.Add("How does though still live!?");
                messages.Add("We remember you, incarnate, we remember you.");
                messages.Add("Thine end has come!");
                messages.Add("May your woes be many!");
                CombatText.NewText(npc.getRect(), Color.Gold, messages, true);
            }

            if (AIPhase == 0)
            {
                attackCounter++;

                if (attackCounter % 45 == 44)
                {
                    Main.PlaySound(SoundID.Item68, laserPosition);
                    TerrorbornMod.ScreenShake(10);
                    Projectile proj = Main.projectile[Projectile.NewProjectile(laserPosition + new Vector2(0, 3000), new Vector2(0, -1), ModContent.ProjectileType<Incendiary.AngelBeam>(), 120 / 4, 0f)];
                    proj.velocity.Normalize();
                    laserPosition = player.Center + player.velocity * 45;
                }

                spawningLaser = true;
                attacking = false;
                hasWeapon = true;
                float speed = MathHelper.Lerp(0.5f, 0.3f, (float)npc.life / (float)npc.lifeMax);

                int yDirection = Math.Sign(player.Center.Y - npc.Center.Y);
                npc.velocity.Y += speed * yDirection;

                int xDirection = Math.Sign(player.Center.X - npc.Center.X);
                npc.velocity.X += speed * xDirection;
                npc.spriteDirection = xDirection;

                npc.velocity *= 0.99f;

                if (attackCounter >= 45 * 5)
                {
                    AIPhase++;
                    spawningLaser = false;
                    attackCounter = 0;
                }
            }
            else if (AIPhase == 1)
            {
                npc.velocity *= 0.95f;
                attackCounter++;

                if (attackCounter > 120)
                {
                    AIPhase++;
                    attackCounter = 0;
                    attacking = true;
                    frame = 6;
                    npc.frameCounter = 0;
                }
            }
            else if (AIPhase == 2)
            {
                npc.velocity *= 0.98f;
                npc.frameCounter++;
                if (npc.frameCounter > 4)
                {
                    npc.frameCounter = 0;
                    frame++;

                    if (frame == 8)
                    {
                        Main.PlaySound(SoundID.Item71, npc.Center);
                        npc.velocity = npc.DirectionTo(player.Center) * 15f;
                    }

                    if (frame == 13)
                    {
                        Main.PlaySound(SoundID.Item71, npc.Center);
                        int proj = Projectile.NewProjectile(npc.Center + new Vector2(20 * npc.spriteDirection, -40), npc.velocity * 3, ModContent.ProjectileType<DreadScytheHostile>(), 140 / 4, 0f);
                        Main.projectile[proj].spriteDirection = -npc.spriteDirection;
                    }

                    if (frame == 14)
                    {
                        AIPhase++;
                        attackCounter = 0;
                        hasWeapon = false;
                        attacking = false;
                    }
                }
            }
            else if ( AIPhase == 3)
            {
                float speed = MathHelper.Lerp(0.5f, 0.3f, (float)npc.life / (float)npc.lifeMax);

                int yDirection = Math.Sign(player.Center.Y - npc.Center.Y);
                npc.velocity.Y += speed * yDirection;

                int xDirection = Math.Sign(player.Center.X - npc.Center.X);
                npc.velocity.X += speed * xDirection;
                npc.spriteDirection = xDirection;

                npc.velocity *= 0.99f;

                attackCounter++;

                bool hasScytheOut = false;

                foreach (Projectile projectile in Main.projectile)
                {
                    if (projectile.type == ModContent.ProjectileType<DreadScytheHostile>() && projectile.active)
                    {
                        hasScytheOut = true;
                        break;
                    }
                }

                if (!hasScytheOut)
                {
                    AIPhase = 0;
                    attackCounter = 0;
                }
            }
        }
    }

    class DreadScytheHostile : ModProjectile
    {
        int trueTimeLeft = 60 * 5;

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 74;
            projectile.aiStyle = -1;
            projectile.penetrate = 5;
            projectile.hostile = true;
            projectile.hide = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 3000;
        }

        public override void AI()
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<DreadAngel>()))
            {
                projectile.active = false;
                return;
            }

            projectile.rotation += MathHelper.ToRadians(30f) * -projectile.spriteDirection;

            if (trueTimeLeft > 0)
            {
                projectile.velocity += projectile.DirectionTo(Main.LocalPlayer.Center) * 1.4f;
                projectile.velocity *= 0.97f;
                trueTimeLeft--;
            }
            else
            {
                projectile.velocity = projectile.DirectionTo(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<DreadAngel>())].Center) * 30f;
                projectile.hostile = false;
                if (projectile.Distance(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<DreadAngel>())].Center) <= 30f)
                {
                    projectile.active = false;
                }
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle originalHitbox = hitbox;
            int newSize = 50;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }
    }
}