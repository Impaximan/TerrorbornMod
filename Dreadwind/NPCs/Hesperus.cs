using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;
using System;

namespace TerrorbornMod.Dreadwind.NPCs
{
    class Hesperus : ModNPC
    {
        public override bool CheckActive()
        {
            return false;
        }

        public override void Load()
        {
            ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/PhosphorusSpirit", ReLogic.Content.AssetRequestMode.ImmediateLoad);
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override void SetDefaults()
        {
            NPC.width = 342;
            NPC.height = 214;
            NPC.damage = 10;
            NPC.lifeMax = 12000;
            NPC.defense = 35;
            NPC.HitSound = new SoundStyle("TerrorbornMod/Sounds/Effects/DreadAngelHurt");
            NPC.DeathSound = SoundID.NPCDeath59;
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
        }

        bool crystalCurrently = true;
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
        }

        float crystalRotation = 0f;
        float segmentRotation = 0f;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 drawCenter = NPC.Center + new Vector2(0, NPC.height / 2 - 84);

            for (float i = 0f; i < 0.99f; i += 1f / 4f)
            {
                Terraria.Utils.DrawLine(spriteBatch, drawCenter, drawCenter + telegraphRotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(360f * i)) * 150f, Color.MediumPurple, Color.Transparent, 3f);
            }

            if (crystalCurrently)
            {
                Texture2D crystal = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/PhosphorusSpirit").Value;
                crystalRotation -= MathHelper.ToRadians(7f);
                crystalRotation = MathHelper.WrapAngle(crystalRotation);
                spriteBatch.Draw(crystal, drawCenter - Main.screenPosition, null, Color.White, crystalRotation, crystal.Size() / 2, 1f, SpriteEffects.None, 0f);
            }
            else
            {

                //Middle chain
                Texture2D segmentTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/HesperusChainSegment2").Value;

                float currentRotation = 0f;
                Vector2 currentPosition = drawCenter + new Vector2(0, 30);
                int amount = 6;
                for (int i = 0; i < amount; i++)
                {
                    if (i == amount - 1)
                    {
                        segmentTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/HesperusChainTip2").Value;
                    }
                    spriteBatch.Draw(segmentTexture, currentPosition - Main.screenPosition, null, Color.White, currentRotation, new Vector2(segmentTexture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
                    currentPosition += new Vector2(0, segmentTexture.Height).RotatedBy(currentRotation);
                    currentRotation += segmentRotation;
                }

                //Left chain
                segmentTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/HesperusChainSegment1").Value;

                currentRotation = 0f;
                currentPosition = drawCenter + new Vector2(-20, 30);
                amount = 4;
                for (int i = 0; i < amount; i++)
                {
                    if (i == amount - 1)
                    {
                        segmentTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/HesperusChainTip1").Value;
                    }
                    spriteBatch.Draw(segmentTexture, currentPosition - Main.screenPosition, null, Color.White, currentRotation, new Vector2(segmentTexture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
                    currentPosition += new Vector2(0, segmentTexture.Height).RotatedBy(currentRotation);
                    currentRotation += segmentRotation;
                }

                segmentTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/HesperusChainSegment1").Value;

                //Right chain
                currentRotation = 0f;
                currentPosition = drawCenter + new Vector2(20, 30);
                amount = 4;
                for (int i = 0; i < amount; i++)
                {
                    if (i == amount - 1)
                    {
                        segmentTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/HesperusChainTip1").Value;
                    }
                    spriteBatch.Draw(segmentTexture, currentPosition - Main.screenPosition, null, Color.White, currentRotation, new Vector2(segmentTexture.Width / 2, 0), 1f, SpriteEffects.None, 0f);
                    currentPosition += new Vector2(0, segmentTexture.Height).RotatedBy(currentRotation);
                    currentRotation += segmentRotation;
                }
            }
            return !crystalCurrently;
        }

        int currentAttack = 0;
        float telegraphRotation = 0f;
        Vector2 targetPosition;
        bool start = true;
        float currentDistance = 1000;
        int attackCounter1 = 0;
        int attackCounter2 = 0;
        int projectileCounter = 0;
        public override void AI()
        {
            if (start)
            {
                start = false;
                crystalCurrently = true;
                currentDistance = NPC.ai[2];

            }

            segmentRotation = MathHelper.Lerp(segmentRotation, MathHelper.ToRadians(NPC.velocity.X) / (Math.Abs(NPC.velocity.Y / 3) + 1), 0.1f);

            NPC.TargetClosest(true);
            NPC.spriteDirection = NPC.direction;
            Player player = Main.player[NPC.target];
            targetPosition = new Vector2(currentDistance * NPC.ai[0], currentDistance * 0.6f * NPC.ai[1]) + player.Center;

            telegraphRotation = DreadwindSystem.HesperusTelegraphRotation;

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

            if (crystalCurrently)
            {
                NPC.velocity = (targetPosition - NPC.Center) * 0.1f;
                NPC.dontTakeDamage = true;
                if (DreadwindSystem.waveNumber == 2)
                {
                    crystalCurrently = false;
                    NPC.velocity = Vector2.Zero;
                }
            }
            else
            {
                int HesperusCount = 0;
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && npc.type == ModContent.NPCType<Hesperus>())
                    {
                        HesperusCount++;
                        if (HesperusCount >= 4)
                        {
                            break;
                        }
                    }
                }

                NPC.dontTakeDamage = false;

                float speed = 0.6f;
                NPC.velocity.X += Math.Sign(targetPosition.X - NPC.Center.X) * speed;
                NPC.velocity.Y += Math.Sign(targetPosition.Y - NPC.Center.Y) * speed;
                NPC.velocity *= 0.96f;

                projectileCounter++;
                if (projectileCounter > 40 * HesperusCount)
                {
                    projectileCounter = 0;
                    for (float i = 0f; i < 0.99f; i += 1f / 4f)
                    {
                        Vector2 velocity = telegraphRotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(360f * i)) * 12f / HesperusCount;
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, ModContent.ProjectileType<SoulOfNightProjectile>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                    }
                }
            }
        }
    }

    class SoulOfNightProjectile : ModProjectile
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
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.MediumPurple, Color.Purple, mult)) * mult;
                    Utils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(35f * mult), color);
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

            Dust dust = Dust.NewDustPerfect(Projectile.Center, 21);
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;

            if (Projectile.Distance(Main.LocalPlayer.Center) > 1000)
            {
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.LocalPlayer.Center).ToRotation(), MathHelper.ToRadians(0.5f)).ToRotationVector2() * (Projectile.velocity.Length() + 0.1f);
            }
            else
            {
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.LocalPlayer.Center).ToRotation(), MathHelper.ToRadians(0.35f)).ToRotationVector2() * (Projectile.velocity.Length() + 0.05f);
            }
        }
    }
}
