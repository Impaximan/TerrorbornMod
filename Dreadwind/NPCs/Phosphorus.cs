using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;

namespace TerrorbornMod.Dreadwind.NPCs
{
    class Phosphorus : ModNPC
    {
        public override bool CheckActive()
        {
            return false;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 12;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override void SetDefaults()
        {
            NPC.width = 110;
            NPC.height = 110;
            NPC.damage = 10;
            NPC.lifeMax = 7500;
            NPC.defense = 35;
            NPC.HitSound = SoundID.DD2_WitherBeastCrystalImpact;
            NPC.DeathSound = SoundID.NPCDeath59;
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
        }

        bool firing = false;
        int bowFrame = 0;
        float bowRotation = 0f;
        int frame;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 5)
            {
                NPC.frameCounter = 0;
                frame++;
                if (frame >= 6)
                {
                    frame = 0;
                }
            }
            NPC.frame.Y = frame * NPC.frame.Height;
            if (firing) NPC.frame.Y += 660;
        }


        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (firing)
            {
                SpriteEffects effects = SpriteEffects.None;
                float extraRotation = MathHelper.ToRadians(180);
                if (NPC.spriteDirection == 1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                    extraRotation = 0f;
                }
                spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_Bow").Value, NPC.Center - Main.screenPosition + new Vector2(10 * NPC.spriteDirection, 0f) + bowRotation.ToRotationVector2() * 20, new Rectangle(0, bowFrame * 44, 46, 44), Color.White, bowRotation + extraRotation, new Vector2(22, 22), 1f, effects, 0f);
            }
        }

        int currentAttack = 0;

        Vector2 targetPosition;
        bool start = true;
        float currentDistance = 1000;
        int attackCounter1 = 0;
        int attackCounter2 = 0;
        public override void AI()
        {
            if (start)
            {
                start = false;
                currentDistance = 500;
                currentAttack = 0;
                attackCounter1 = 0;
                attackCounter2 = 0;
            }

            NPC.TargetClosest(true);
            NPC.spriteDirection = NPC.direction;
            Player player = Main.player[NPC.target];
            targetPosition = new Vector2(currentDistance * NPC.ai[0], currentDistance * 0.6f * NPC.ai[1]);

            if (player.dead || !DreadwindSystem.DreadwindActive)
            {
                NPC.velocity.Y = 30;
                NPC.velocity.X = 0;
                NPC.rotation = 0f;
                if (NPC.Center.Y > player.Center.Y + Main.screenHeight)
                {
                    NPC.active = false;
                }
                return;
            }

            switch (currentAttack)
            {
                case 0:
                    NPC.velocity = NPC.Distance(targetPosition + player.Center) * 0.2f * NPC.DirectionTo(targetPosition + player.Center);
                    firing = true;
                    bowRotation = NPC.DirectionTo(player.Center).ToRotation();

                    attackCounter1++;
                    if (attackCounter1 > 10)
                    {
                        attackCounter1 = 0;
                        bowFrame++;
                        if (bowFrame == 6)
                        {
                            //Fire projectile
                            SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot, NPC.Center);
                            Vector2 arrowPos = NPC.Center + new Vector2(10 * NPC.spriteDirection, 0f) + bowRotation.ToRotationVector2() * 20;
                            int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), arrowPos, bowRotation.ToRotationVector2() * 12f, ModContent.ProjectileType<PhosphorusArrow>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                            Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation() + MathHelper.ToRadians(90);
                            Main.projectile[proj].velocity += NPC.velocity;
                            TerrorbornSystem.ScreenShake(3f);

                            attackCounter2++;
                            if (attackCounter2 > 4)
                            {
                                currentAttack = 1;
                                bowFrame = 0;
                                attackCounter2 = 0;
                                attackCounter1 = 0;
                            }
                        }
                        if (bowFrame >= 8)
                        {
                            bowFrame = 0;
                        }
                    }
                    break;
                case 1:

                    firing = false;

                    attackCounter1++;
                    if (attackCounter1 > 30)
                    {
                        attackCounter1 = 0;
                        int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), player.Center, player.velocity * Main.rand.NextFloat(1.5f, 3f), ModContent.ProjectileType<HeartTelegraph>(), DreadwindSystem.DreadwindLargeDamage / 4, 0f);
                    }

                    attackCounter2++;
                    if (attackCounter2 > 180)
                    {
                        attackCounter2 = 0;
                        currentAttack = 2;
                    }

                    NPC.velocity *= 0.98f;
                    break;
                case 2:

                    firing = false;
                    attackCounter2++;
                    if (attackCounter2 > 60)
                    {
                        attackCounter2 = 0;
                        attackCounter1 = 0;
                        currentAttack = 0;
                    }

                    currentDistance -= 100f / 60f;
                    if (currentDistance < 100f)
                    {
                        currentDistance = 100f;
                    }
                    NPC.velocity = NPC.Distance(targetPosition + player.Center) * 0.2f * NPC.DirectionTo(targetPosition + player.Center);
                    break;
                default:
                    break;
            }
        }

        public override void OnKill()
        {
            int hesp = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Hesperus>());
            Main.npc[hesp].ai[0] = NPC.ai[0];
            Main.npc[hesp].ai[1] = NPC.ai[1];
            Main.npc[hesp].ai[2] = currentDistance;
        }
    }

    class PhosphorusArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 28;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle originalHitbox = hitbox;
            hitbox.Width = 20;
            hitbox.Height = 20;
            hitbox.X = originalHitbox.Center.X - 10;
            hitbox.Y = originalHitbox.Center.Y - 10;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        int heartCounter = 0;
        public override void AI()
        {
            int PhosphorusCount = 0;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.type == ModContent.NPCType<Phosphorus>())
                {
                    PhosphorusCount++;
                    if (PhosphorusCount >= 4)
                    {
                        break;
                    }
                }
            }

            if (PhosphorusCount != 0 && TerrorbornSystem.TwilightMode)
            {
                heartCounter++;
                if (heartCounter > 30 * PhosphorusCount)
                {
                    heartCounter = -1;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.LocalPlayer.velocity, ModContent.ProjectileType<HeartTelegraph>(), DreadwindSystem.DreadwindLowDamage / 4, 0f);
                }
            }
        }
    }

    class HeartTelegraph : ModProjectile
    {
        public override string Texture => base.Texture + "Pink";

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.scale = 0f;
        }

        float whiteAlpha = 0f;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White * 0.3f, 0f, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/HeartTelegraphWhite").Value, Projectile.Center - Main.screenPosition, null, Color.White * whiteAlpha * 0.5f, 0f, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        int whiteDirection = 1;
        public override void AI()
        {
            Projectile.scale = MathHelper.Lerp(Projectile.scale, 1f, 0.075f);

            whiteAlpha += 1f / 120f;

            if (Projectile.Center != Main.LocalPlayer.Center)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Main.LocalPlayer.Center), 0.05f);
            }
        }

        int soulAmount = 12;
        public override void Kill(int timeLeft)
        {
            Vector2 velocity = new Vector2(0, -20);

            for (int i = 0; i < soulAmount; i++)
            {
                velocity = velocity.RotatedBy(MathHelper.ToRadians(i * (360 / soulAmount)));
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<SpiralSoul_Light>(), Projectile.damage, 0f);
                Main.projectile[proj].ai[0] = 1f;
            }
        }
    }

    class SpiralSoul_Light : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
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
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.MediumPurple, Color.HotPink, mult)) * mult;
                    TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(35f * mult), color);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.penetrate = 25;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.hide = false;
            Projectile.timeLeft = (int)(360 / rotationSpeed);
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        bool start = true;
        float rotationSpeed = 10;
        public override void AI()
        {
            if (start)
            {
                start = false;
            }

            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(rotationSpeed) * Projectile.ai[0]);

            Dust dust = Dust.NewDustPerfect(Projectile.Center, 21);
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
        }
    }
}
