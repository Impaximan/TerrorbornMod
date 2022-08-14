using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;
using System;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.Dreadwind.NPCs
{
    class Oculus : ModNPC
    {
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 178;
            NPC.height = 112;
            NPC.damage = DreadwindSystem.DreadwindLargeDamage / 2;
            NPC.lifeMax = 5000;
            NPC.defense = 35;
            NPC.HitSound = SoundID.NPCHit32;
            NPC.DeathSound = SoundID.NPCDeath34;
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = false;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 2)
            {
                NPC.frameCounter = 0;
                frame++;
                if (frame >= 4)
                {
                    frame = 0;
                }
            }

            NPC.frame.Y = frame * frameHeight;
        }

        bool dying = false;
        float IrisDirection = 0f;
        bool IrisCentered = false;
        float IrisDistanceMult = 0f;
        float segmentRotation = 0f;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (dying)
            {
                return false;
            }

            Texture2D segmentTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/OculusTailSegment").Value;

            float currentRotation = 0f;
            Vector2 currentPosition = NPC.Center + new Vector2(0, 12);
            int amount = 5;
            for (int i = 0; i < amount; i++)
            {
                if (i == amount - 1)
                {
                    segmentTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/OculusTailEnd").Value;
                }
                spriteBatch.Draw(segmentTexture, currentPosition - Main.screenPosition, null, Color.White, currentRotation, new Vector2(segmentTexture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
                currentPosition += new Vector2(0, segmentTexture.Height).RotatedBy(currentRotation);
                currentRotation += segmentRotation;
            }

            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (dying)
            {
                return;
            }

            if (IrisCentered) IrisDistanceMult = MathHelper.Lerp(IrisDistanceMult, 0f, 0.1f);
            else IrisDistanceMult = MathHelper.Lerp(IrisDistanceMult, 1f, 0.1f);

            Texture2D IrisTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/OculusIris").Value;
            spriteBatch.Draw(IrisTexture, NPC.Center + IrisDirection.ToRotationVector2() * 10f * IrisDistanceMult - Main.screenPosition + new Vector2(0, 4), null, Color.White, 0f, IrisTexture.Size() / 2, 1f, SpriteEffects.None, 0f);
        }

        public void GoDoNextAttack(int previousAttack)
        {
            AIPhase = Main.rand.Next(3);
            while (AIPhase == previousAttack)
            {
                AIPhase = Main.rand.Next(3);
            }
            SubAIPhase = -1;
        }

        public override void OnKill()
        {
            int count = 0;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.type == Type && npc.whoAmI != NPC.whoAmI)
                {
                    count++;
                    npc.HealEffect(npc.lifeMax - npc.life);
                    npc.life = NPC.lifeMax;
                }
            }

            if (count % 3 == 2)
            {
                Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, NPC.DirectionTo(Main.LocalPlayer.Center), ModContent.ProjectileType<OculusDeathray>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, NPC.DirectionTo(Main.LocalPlayer.Center) * -1f, ModContent.ProjectileType<OculusDeathray>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
            }
            if (count % 3 == 1)
            {
                Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, new Vector2(1, 0), ModContent.ProjectileType<OculusDeathray>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, new Vector2(-1, 0), ModContent.ProjectileType<OculusDeathray>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
            }
            if (count % 3 == 0)
            {
                Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, new Vector2(0, 1), ModContent.ProjectileType<OculusDeathray>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center, new Vector2(0, -1), ModContent.ProjectileType<OculusDeathray>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
            }
        }

        int AIPhase = -1;
        int SubAIPhase = -1;
        float attackCounter1 = 0;
        float attackCounter2 = 0;
        float attackCounter3 = 0;
        Vector2 movementTarget;
        public override void AI()
        {
            Player player = Main.LocalPlayer;
            int OculusCount = 0;

            segmentRotation = MathHelper.Lerp(segmentRotation, MathHelper.ToRadians(NPC.velocity.X) / (Math.Abs(NPC.velocity.Y / 3) + 1), 0.1f);

            if (player.dead || !DreadwindSystem.DreadwindActive)
            {
                NPC.velocity.X = 0;
                NPC.velocity.Y = 15;
                IrisCentered = false;
                IrisDirection = MathHelper.ToRadians(90);
                if (NPC.Center.Y > Main.screenHeight + player.Center.Y)
                {
                    NPC.active = false;
                }
                return;
            }

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.type == Type)
                {
                    OculusCount++;
                }
            }
            if (AIPhase == -1)
            {
                GoDoNextAttack(AIPhase);
            }
            if (AIPhase == 0)
            {
                IrisDirection = NPC.DirectionTo(player.Center).ToRotation();
                IrisCentered = false;
                if (SubAIPhase == -1)
                {
                    movementTarget = player.Center + NPC.DirectionTo(player.Center) * 300f + new Vector2(Main.rand.Next(-300, 300), Main.rand.Next(-300, 300));
                    SubAIPhase = 0;
                    attackCounter1 = 0;
                }
                if (SubAIPhase == 0)
                {
                    if (NPC.Distance(movementTarget) <= 20f)
                    {
                        SubAIPhase = 1;
                        NPC.velocity = Vector2.Zero;
                    }
                    else
                    {
                        NPC.velocity = NPC.DirectionTo(movementTarget) * 20f;
                    }
                }
                if (SubAIPhase == 1)
                {
                    attackCounter1++;
                    if (attackCounter1 > 30 * OculusCount)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(player.Center) * 15f, ModContent.ProjectileType<SoulOfSightProjectile>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                        SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                        GoDoNextAttack(AIPhase);
                    }
                }
            }
            if (AIPhase == 1)
            {
                IrisCentered = true;

                if (SubAIPhase == -1)
                {
                    movementTarget = player.Center + NPC.DirectionFrom(player.Center) * 300f + new Vector2(Main.rand.Next(-300, 300), Main.rand.Next(-300, 300));
                    SubAIPhase = 0;
                    attackCounter1 = 0;
                }
                if (SubAIPhase == 0)
                {
                    if (NPC.Distance(movementTarget) <= 20f)
                    {
                        SubAIPhase = 1;
                        NPC.velocity = Vector2.Zero;
                    }
                    else
                    {
                        NPC.velocity = NPC.DirectionTo(movementTarget) * 20f;
                    }
                }
                if (SubAIPhase == 1)
                {
                    attackCounter1++;
                    if (attackCounter1 > 40 * OculusCount)
                    {
                        for (float i = 0f; i < 1f; i += 1f / 8f)
                        {
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(10f, 0f).RotatedBy(MathHelper.ToRadians(360) * i), ModContent.ProjectileType<SoulOfSightProjectile>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                        }

                        SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                        GoDoNextAttack(AIPhase);
                    }
                }
            }
            if (AIPhase == 2)
            {
                IrisCentered = false;
                IrisDirection = MathHelper.ToRadians(-90);

                if (SubAIPhase == -1)
                {
                    movementTarget = player.Center + new Vector2(Main.rand.Next(-300, 300), Main.rand.Next(-300, 300));
                    SubAIPhase = 0;
                    attackCounter1 = 0;
                }
                if (SubAIPhase == 0)
                {
                    if (NPC.Distance(movementTarget) <= 20f)
                    {
                        SubAIPhase = 1;
                        NPC.velocity = Vector2.Zero;
                    }
                    else
                    {
                        NPC.velocity = NPC.DirectionTo(movementTarget) * 20f;
                    }
                }
                if (SubAIPhase == 1)
                {
                    attackCounter1++;
                    if (attackCounter1 > 40 * OculusCount)
                    {
                        for (float i = 0f; i <= 1f; i += 1f / 2f)
                        {
                            int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(MathHelper.Lerp(-5f, 5f, i), -15f), ModContent.ProjectileType<SoulOfSightProjectile>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                            Main.projectile[proj].ai[0] = 0.25f;
                        }

                        SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                        GoDoNextAttack(AIPhase);
                    }
                }
            }
        }
    }

    class OculusDeathray : Deathray
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 100000;
        }

        public override string Texture => "TerrorbornMod/Dreadwind/NPCs/OculusDeathray";
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = false;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 500;
            MoveDistance = 0f;
            RealMaxDistance = 20000f;
            bodyRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
            headRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
            tailRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
            deathrayWidth = 0.3f;
            Projectile.alpha = 255 / 4;
            FollowPosition = false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return deathrayWidth >= 1f;
        }

        int waitTime = 0;
        public override void PostAI()
        {
            Projectile.timeLeft = 500;

            if (DreadwindSystem.currentWave.WaveName == "Oculus" && DreadwindSystem.DreadwindActive)
            {
                if (waitTime < 60)
                {
                    waitTime++;
                }
                else
                {
                    if (deathrayWidth < 1f) deathrayWidth += 1f / 10f;
                    if (Projectile.alpha < 255) Projectile.alpha += 15;
                }
            }
            else
            {
                if (deathrayWidth > 0f) deathrayWidth -= 1f / 10f;
                else Projectile.active = false;
            }
        }
    }

    class SoulOfSightProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 20;
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
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.Green, Color.LightGreen, mult)) * mult;
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
            Projectile.timeLeft = 600;
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

            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.GreenTorch);
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;

            Projectile.velocity.Y += Projectile.ai[0];
        }
    }
}
