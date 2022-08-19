using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;
using System;
using Terraria.DataStructures;

namespace TerrorbornMod.Dreadwind.NPCs
{
    class Locust : ModNPC
    {
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 8;
            NPCID.Sets.MustAlwaysDraw[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 126;
            NPC.height = 110;
            NPC.damage = DreadwindSystem.DreadwindMidDamage / 2;
            NPC.lifeMax = 14000;
            NPC.defense = 35;
            NPC.HitSound = SoundID.NPCHit44;
            NPC.DeathSound = SoundID.NPCDeath56;
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = false;
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 2)
            {
                NPC.frameCounter = 0;
                frame++;
                if (frame >= 8)
                {
                    frame = 0;
                }
            }

            NPC.frame.Y = frame * frameHeight;
        }

        public override void OnKill()
        {
            int count = 0;
            NPC targetLocust = null;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.type == Type && npc.whoAmI != NPC.whoAmI)
                {
                    count++;
                    targetLocust = npc;
                }
            }

            if (count == 1)
            {
                targetLocust.ai[0] = 1;
                targetLocust.lifeMax *= 2;
                targetLocust.life = targetLocust.lifeMax;
                CombatText.NewText(targetLocust.getRect(), Color.Red, "ENRAGED", true);
                TerrorbornSystem.ScreenShake(10f);

                SoundStyle style = SoundID.Item119;
                style.Volume *= 2f;
                style.Pitch += 0.3f;
                SoundEngine.PlaySound(style, targetLocust.Center);
            }
        }

        float segmentRotation = 0f;
        float wholeTailRotation = 0f;
        float lineStrength = 0f;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D segmentTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/LocustTailSegment").Value;

            float currentRotation = 0f;
            Vector2 tailOrigin = new Vector2(10, -36);
            if (frame == 1) tailOrigin.X += 2;
            if (frame == 2) tailOrigin.X += 4;
            if (frame == 3) tailOrigin.X += 2;
            if (frame == 4) tailOrigin.X += 0;
            if (frame == 5) tailOrigin.X += 2;
            if (frame == 6) tailOrigin.X += 4;
            if (frame == 7) tailOrigin.X += 2;
            tailOrigin.X *= -NPC.spriteDirection;
            Vector2 currentPosition = Vector2.Zero;
            int amount = 15;
            for (int i = 0; i < amount; i++)
            {
                spriteBatch.Draw(segmentTexture, NPC.Center + tailOrigin + currentPosition.RotatedBy(wholeTailRotation) * new Vector2(-NPC.spriteDirection, 1f) - Main.screenPosition, null, Color.White, 0f, new Vector2(segmentTexture.Width / 2, segmentTexture.Height / 2), 1f, SpriteEffects.None, 0f);
                currentPosition += new Vector2(segmentTexture.Width, 0).RotatedBy(currentRotation);
                currentRotation += segmentRotation;
            }

            segmentTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/LocustChainsaw").Value;
            spriteBatch.Draw(segmentTexture, GetChainsawPosition() - Main.screenPosition, null, Color.White, (currentRotation + wholeTailRotation + (float)Math.PI / 2f) * -NPC.spriteDirection, new Vector2(segmentTexture.Width / 2, segmentTexture.Height - 7), 1f, SpriteEffects.None, 0f);
            if (lineStrength != 0f)
            {
                Vector2 position = GetChainsawPosition();
                Utils.DrawLine(spriteBatch, position, Main.LocalPlayer.DirectionFrom(position) * 3000f + position, Color.OrangeRed * lineStrength, Color.Transparent, 9f * lineStrength + 1f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        Vector2 tailOrigin;
        public Vector2 GetChainsawPosition(bool forHitbox = false)
        {
            Texture2D segmentTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/LocustTailSegment").Value;

            float currentRotation = 0f;
            Vector2 currentPosition = Vector2.Zero;
            int amount = 15;
            for (int i = 0; i < amount; i++)
            {
                currentPosition += new Vector2(segmentTexture.Width, 0).RotatedBy(currentRotation);
                currentRotation += segmentRotation;
            }
            if (forHitbox)
            {
                segmentTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/LocustChainsaw").Value;
                currentPosition += new Vector2(segmentTexture.Height / 2, 0).RotatedBy(currentRotation);
                return NPC.Center + tailOrigin + currentPosition.RotatedBy(wholeTailRotation) * new Vector2(-NPC.spriteDirection, 1f);
            }
            return NPC.Center + tailOrigin + currentPosition.RotatedBy(wholeTailRotation) * new Vector2(-NPC.spriteDirection, 1f);
        }

        public void DamageAtChainsaw(Player player, int damage)
        {
            Texture2D chainsawTexture = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/LocustChainsaw").Value;
            Point position = GetChainsawPosition(true).ToPoint();

            int hitboxWidth = 40;
            int hitboxHeight = 40;
            Rectangle hitbox = new Rectangle(position.X - hitboxWidth / 2, position.Y - hitboxHeight / 2, hitboxWidth, hitboxHeight);

            if (player.getRect().Intersects(hitbox) && player.immuneTime <= 0)
            {
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was chainsawed"), damage, NPC.direction);
            }
        }

        List<int> nextAttacksList = new List<int>();
        public void GoDoNextAttack(int previousAttack)
        {
            if (nextAttacksList.Count == 0)
            {
                int count = 3;
                if (NPC.ai[0] == 1)
                {
                    count++;
                }
                while (nextAttacksList.Count < count)
                {
                    int attack = Main.rand.Next(count);
                    while (nextAttacksList.Contains(attack) || (nextAttacksList.Count == 0 && attack == previousAttack))
                    {
                        attack = Main.rand.Next(count);
                    }
                    nextAttacksList.Add(attack);
                }
                while (AIPhase == previousAttack)
                {
                    AIPhase = Main.rand.Next(count);
                }
            }
            AIPhase = nextAttacksList[0];
            SubAIPhase = -1;
            attackCounter1 = 0;
            attackCounter2 = 0;
            attackCounter3 = 0;
            nextAttacksList.RemoveAt(0);
        }

        int AIPhase = -1;
        int SubAIPhase = -1;
        float attackCounter1 = 0;
        float attackCounter2 = 0;
        float attackCounter3 = 0;
        float attackRotation;
        int attackDirection = 0;
        public override void AI()
        {
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];

            if (player.dead || !DreadwindSystem.DreadwindActive)
            {
                NPC.velocity.X = 0;
                NPC.velocity.Y = -15;
                if (NPC.Center.Y < -Main.screenHeight + player.Center.Y)
                {
                    NPC.active = false;
                }
                return;
            }

            bool enraged = NPC.ai[0] == 1;

            tailOrigin = new Vector2(10, -36);
            if (frame == 1) tailOrigin.X += 2;
            if (frame == 2) tailOrigin.X += 4;
            if (frame == 3) tailOrigin.X += 2;
            if (frame == 4) tailOrigin.X += 0;
            if (frame == 5) tailOrigin.X += 2;
            if (frame == 6) tailOrigin.X += 4;
            if (frame == 7) tailOrigin.X += 2;
            tailOrigin.X *= -NPC.spriteDirection;

            if (AIPhase == -1)
            {
                AIPhase = 0;
                SubAIPhase = -1;
                attackCounter1 = 0;
                attackCounter2 = 0;
                attackCounter3 = 0;
            }
            if (AIPhase == 0) //Hover
            {
                if (SubAIPhase == -1)
                {
                    SubAIPhase = 0;
                    attackDirection = NPC.direction;
                }
                if (SubAIPhase == 0)
                {
                    DamageAtChainsaw(player, NPC.damage);
                    NPC.spriteDirection = NPC.direction;
                    segmentRotation = MathHelper.Lerp(segmentRotation, MathHelper.ToRadians(-10), 0.1f);
                    wholeTailRotation = MathHelper.Lerp(wholeTailRotation, -(float)Math.PI / 2f, 0.1f);

                    attackCounter2 += 1f / 180f;
                    Vector2 targetPosition = new Vector2(MathHelper.Lerp(player.Center.X - 1000f * attackDirection, player.Center.X + 500f * attackDirection, attackCounter2), player.Center.Y - 300f);

                    float speed = 1f;
                    NPC.velocity.X += Math.Sign(targetPosition.X - NPC.Center.X) * speed;
                    NPC.velocity.Y += Math.Sign(targetPosition.Y - NPC.Center.Y) * speed;
                    NPC.velocity *= 0.94f;

                    attackCounter1++;
                    if (attackCounter1 > 180)
                    {
                        GoDoNextAttack(AIPhase);
                    }
                }
            }
            if (AIPhase == 1) //Throw
            {
                if (SubAIPhase == -1)
                {
                    NPC.spriteDirection = NPC.direction;
                    NPC.velocity *= 0.95f;
                    segmentRotation = MathHelper.Lerp(segmentRotation, MathHelper.ToRadians(5), 0.1f);
                    wholeTailRotation = MathHelper.Lerp(wholeTailRotation, -(float)Math.PI / 2f, 0.1f);

                    attackCounter1++;
                    if (attackCounter1 == 1)
                    {
                        SoundEngine.PlaySound(SoundID.DD2_BetsySummon, NPC.Center);
                    }
                    if (attackCounter1 > 90)
                    {
                        SoundEngine.PlaySound(SoundID.DD2_MonkStaffSwing, GetChainsawPosition());
                        SubAIPhase = 0;
                        attackCounter1 = 0;

                        Vector2 position = GetChainsawPosition(true);
                        float speed = 20f;
                        if (enraged)
                        {
                            speed *= 1.5f;
                        }
                        Vector2 velocity = ((player.Distance(position) / speed * player.velocity + player.Center) - position).ToRotation().ToRotationVector2() * speed;

                        int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<SoulOfFrightProjectile>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                        Main.projectile[proj].ai[0] = -0.2f;

                        proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<SoulOfFrightProjectile>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                        Main.projectile[proj].ai[0] = 0.2f;

                        proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<SoulOfFrightProjectile>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                    }
                }
                if (SubAIPhase == 0)
                {
                    attackCounter1++;
                    segmentRotation = MathHelper.Lerp(segmentRotation, MathHelper.ToRadians(-15), 0.2f);
                    wholeTailRotation = MathHelper.Lerp(wholeTailRotation, -(float)Math.PI / 2f, 0.1f);

                    if (attackCounter1 > 60)
                    {
                        GoDoNextAttack(AIPhase);
                    }
                }
            }
            if (AIPhase == 2) //Dash
            {
                if (SubAIPhase == -1)
                {
                    NPC.spriteDirection = NPC.direction;
                    NPC.velocity *= 0.95f;
                    segmentRotation = MathHelper.Lerp(segmentRotation, MathHelper.ToRadians(12.5f), 0.1f);
                    wholeTailRotation = MathHelper.Lerp(wholeTailRotation, -(float)Math.PI / 4f, 0.1f);

                    attackCounter1++;
                    if (attackCounter1 == 1)
                    {
                        lineStrength = 1f;
                    }
                    if (attackCounter1 > 80 || (enraged && attackCounter1 > 50))
                    {
                        SubAIPhase = 0;
                        NPC.velocity = Vector2.Zero;
                        attackCounter1 = 0;
                    }
                }
                if (SubAIPhase == 0)
                {
                    float speed = 2f;
                    if (enraged) speed *= 1.5f;
                    NPC.velocity += NPC.DirectionTo(player.Center) * speed;

                    attackCounter1++;
                    if (attackCounter1 > 20)
                    {
                        SubAIPhase++;
                        attackCounter1 = 0;
                    }
                }
                if (SubAIPhase == 1)
                {
                    NPC.spriteDirection = NPC.direction;
                    NPC.velocity *= 0.95f;

                    attackCounter1++;
                    if (attackCounter1 > 30)
                    {
                        GoDoNextAttack(AIPhase);
                    }
                }
            }
            if (AIPhase == 3) //Spin
            {
                if (SubAIPhase == -1)
                {
                    SubAIPhase = 0;
                }
                if (SubAIPhase == 0)
                {
                    DamageAtChainsaw(player, (int)(NPC.damage * 1.5f));

                    NPC.spriteDirection = NPC.direction;

                    float maxSpinSpeed = 25f;

                    if (attackCounter1 < maxSpinSpeed) attackCounter1 += maxSpinSpeed / 60f;
                    wholeTailRotation += MathHelper.ToRadians(attackCounter1);

                    segmentRotation = MathHelper.Lerp(segmentRotation, MathHelper.ToRadians(-1), 0.1f);

                    float chainsawDistanceFromOrigin = Vector2.Distance(GetChainsawPosition(true), tailOrigin + NPC.Center);
                    Vector2 targetPosition = player.Center + new Vector2(chainsawDistanceFromOrigin * -NPC.direction, 0f) - tailOrigin;

                    float speed = 0.5f;
                    if (attackCounter3 > 60)
                    {
                        NPC.velocity.X += Math.Sign(targetPosition.X - NPC.Center.X) * speed;
                        NPC.velocity.Y += Math.Sign(targetPosition.Y - NPC.Center.Y) * speed;
                    }
                    NPC.velocity *= 0.98f;

                    attackCounter2++;
                    if (attackCounter2 > MathHelper.Lerp(90f, 10f, attackCounter1 / maxSpinSpeed))
                    {
                        attackCounter2 = 0;
                        SoundEngine.PlaySound(SoundID.Item152, GetChainsawPosition());
                    }

                    attackCounter3++;
                    if ((attackCounter3 > 240 && !enraged) || attackCounter3 > 360)
                    {
                        GoDoNextAttack(AIPhase);
                    }
                }
            }

            if (lineStrength > 0f)
            {
                lineStrength -= 1f / 60f;
            }

            wholeTailRotation = MathHelper.WrapAngle(wholeTailRotation);
        }
    }

    class SoulOfFrightProjectile : ModProjectile
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
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.Orange, Color.Red, mult)) * mult;
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

        public override void AI()
        {
            Projectile.velocity.Y += Projectile.ai[0];
        }
    }
}
