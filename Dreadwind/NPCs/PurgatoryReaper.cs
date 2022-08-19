using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System;

namespace TerrorbornMod.Dreadwind.NPCs
{
    class PurgatoryReaper : ModNPC
    {
        public override bool CheckActive()
        {
            return false;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        enum AnimationState : int
        {
            Idle = 0,
            RaisingSword = 1,
            FreeSwinging = 2,
            PoweringUp = 3
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 14;
        }

        bool canDoContactDamage = false;
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return canDoContactDamage;
        }

        public override void SetDefaults()
        {
            NPC.width = 98;
            NPC.height = 176 - 50;
            NPC.damage = DreadwindSystem.DreadwindLargeDamage / 2;
            NPC.lifeMax = 60000;
            NPC.defense = 35;
            NPC.HitSound = SoundID.NPCHit49;
            NPC.DeathSound = SoundID.NPCDeath51;
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
        }

        int freeArmFrame = 0;
        float freeArmRotation = 0f;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (currentAnimation == AnimationState.FreeSwinging)
            {
                Texture2D texture = ModContent.Request<Texture2D>(Texture + "_Arm").Value;
                SpriteEffects effects = SpriteEffects.None;
                if (NPC.spriteDirection == 1) effects = SpriteEffects.FlipHorizontally;
                Vector2 origin = new Vector2(24, 120 - 2);
                if (NPC.spriteDirection == 1) origin = new Vector2(64 - 24, 120 - 2);
                spriteBatch.Draw(texture, NPC.Center + new Vector2(20 * NPC.spriteDirection, -16) - screenPos, new Rectangle(0, 120 * freeArmFrame, 64, 120), Color.White, freeArmRotation, origin, 1f, effects, 0f);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        AnimationState currentAnimation = AnimationState.Idle;
        int raisingSwordFrame = 4;
        int idleFrame = 0;
        int freeSwingFrame = 8;
        int frameCounter = 0;
        public override void FindFrame(int frameHeight)
        {
            if (currentAnimation != AnimationState.RaisingSword)
            {
                raisingSwordFrame = 4;
            }

            if (currentAnimation == AnimationState.RaisingSword)
            {
                frameCounter++;
                if (frameCounter > 10)
                {
                    frameCounter = 0;
                    raisingSwordFrame++;
                    if (raisingSwordFrame > 7)
                    {
                        raisingSwordFrame = 6;
                    }
                }
                NPC.frame.Y = raisingSwordFrame * frameHeight;
            }

            if (currentAnimation == AnimationState.FreeSwinging)
            {
                frameCounter++;
                if (frameCounter > 10)
                {
                    frameCounter = 0;
                    freeSwingFrame++;
                    if (freeSwingFrame > 9)
                    {
                        freeSwingFrame = 8;
                    }
                }
                NPC.frame.Y = freeSwingFrame * frameHeight;
            }

            if (currentAnimation == AnimationState.Idle || currentAnimation == AnimationState.PoweringUp)
            {
                frameCounter++;
                if (frameCounter > 5)
                {
                    frameCounter = 0;
                    idleFrame++;
                    if (idleFrame > 3)
                    {
                        idleFrame = 0;
                    }
                }

                NPC.frame.Y = idleFrame * frameHeight;
                if (currentAnimation == AnimationState.PoweringUp) NPC.frame.Y += frameHeight * 10;
            }
        }

        int lastAIPhase = -1;
        int AIPhase = -1;
        int SubAIPhase = -1;
        float attackCounter1 = 0;
        float attackCounter2 = 0;
        float attackCounter3 = 0;
        float attackCounter4 = 0;
        Vector2 attackTarget;
        public override void AI()
        {
            canDoContactDamage = false;
            AIPhase = DreadwindSystem.reaper_AIPhase;
            if (AIPhase != lastAIPhase)
            {
                SubAIPhase = -1;
                attackCounter1 = 0;
                attackCounter2 = 0;
                attackCounter3 = 0;
                attackCounter4 = 0;
                lastAIPhase = AIPhase;
            }
            bool handlesSystem = NPC.ai[0] == 1;
            int side = (int)NPC.ai[0] * DreadwindSystem.reaper_SideMult;

            NPC.TargetClosest();
            Player player = Main.player[NPC.target];

            if (player.dead || !DreadwindSystem.DreadwindActive || DreadwindSystem.reaper_ShouldDespawn)
            {
                currentAnimation = AnimationState.RaisingSword;
                NPC.velocity.X = 0;
                NPC.velocity.Y -= 1f;
                if (NPC.Center.Y < -Main.screenHeight + player.Center.Y)
                {
                    NPC.active = false;
                }
                return;
            }

            if (AIPhase == -1)
            {
                if (handlesSystem)
                {
                    DreadwindSystem.reaper_AIPhase = 0;
                }
            }

            if (AIPhase == 0) //Idle/following player
            {
                if (SubAIPhase == -1)
                {
                    SubAIPhase = 0;
                    currentAnimation = AnimationState.Idle;
                }
                if (SubAIPhase == 0)
                {
                    Vector2 targetPosition = player.Center + new Vector2(side * 500, -100);

                    NPC.spriteDirection = NPC.direction;

                    float speed = 0.75f;
                    NPC.velocity.X += Math.Sign(targetPosition.X - NPC.Center.X) * speed;
                    NPC.velocity.Y += Math.Sign(targetPosition.Y - NPC.Center.Y) * speed;
                    NPC.velocity *= 0.965f;

                    attackCounter1++;
                    if (attackCounter1 > 60 * 5 * DreadwindSystem.reaper_HealthLeft)
                    {
                        if (handlesSystem)
                        {
                            DreadwindSystem.Reaper_DecideNextAttack(AIPhase);
                        }
                    }
                }
            }

            if (AIPhase == 1) //Predictive attack
            {
                if (SubAIPhase == -1)
                {
                    SubAIPhase = 0;
                    currentAnimation = AnimationState.PoweringUp;
                    SoundEngine.PlaySound(SoundID.DD2_BetsySummon, NPC.Center);
                    NPC.spriteDirection = NPC.direction;
                }
                if (SubAIPhase == 0)
                {
                    NPC.spriteDirection = NPC.direction;
                    Vector2 targetPosition = player.Center + new Vector2(side * 500, -100);

                    float speed = 0.5f;
                    NPC.velocity.X += Math.Sign(targetPosition.X - NPC.Center.X) * speed;
                    NPC.velocity.Y += Math.Sign(targetPosition.Y - NPC.Center.Y) * speed;
                    NPC.velocity *= 0.975f;

                    freeArmRotation = 0f;

                    attackCounter1++;
                    if (attackCounter1 > MathHelper.Lerp(90f, 120f, DreadwindSystem.reaper_HealthLeft))
                    {
                        SubAIPhase = 1;
                        SoundEngine.PlaySound(SoundID.Item113, NPC.Center);
                        attackCounter2 = 35f;
                        attackTarget = player.Center + player.velocity * attackCounter2;
                        attackCounter1 = 0f;
                        NPC.velocity.Y = (attackTarget.Y - NPC.Center.Y) / attackCounter2;
                        attackCounter3 = 30f * side;
                        NPC.velocity.X = attackCounter3;
                        currentAnimation = AnimationState.FreeSwinging;
                    }
                }
                if (SubAIPhase == 1)
                {
                    canDoContactDamage = true;
                    if (attackCounter1 <= 0.5f)
                    {
                        attackCounter4 = NPC.Center.X;
                        if (side == -1) NPC.velocity.X += side * (attackCounter3 / attackCounter2) * 2f;
                        else NPC.velocity.X -= side * (attackCounter3 / attackCounter2) * 2f;
                    }
                    else
                    {
                        NPC.position.X = MathHelper.SmoothStep(attackCounter4, attackCounter4 - Math.Abs(attackTarget.X - attackCounter4) * 2 * side, attackCounter1 - 0.5f) - NPC.Size.X / 2;
                    }

                    if (attackCounter1 > 1f)
                    {
                        attackCounter1 = 0f;
                        SubAIPhase = 2;
                        attackCounter2 = 30f;
                        NPC.velocity.X = 30f * -side;
                        SoundExtensions.PlaySoundOld(SoundID.Item71, (int)NPC.Center.X, (int)NPC.Center.Y, 71, 3f, -0.25f);
                    }

                    if (SubAIPhase == 1) attackCounter1 += 1f / attackCounter2;
                }
                if (SubAIPhase == 2)
                {
                    canDoContactDamage = true;
                    attackCounter2 *= 0.85f;
                    freeArmRotation += MathHelper.ToRadians(attackCounter2) * NPC.spriteDirection;
                    NPC.velocity *= 0.9f;
                    attackCounter1++;
                    if (attackCounter1 > 60)
                    {
                        if (handlesSystem)
                        {
                            DreadwindSystem.Reaper_DecideNextAttack(AIPhase);
                            DreadwindSystem.reaper_SideMult *= -1;
                        }
                    }
                }
            }

            if (AIPhase == 2) //Soul command
            {
                NPC.spriteDirection = NPC.direction;
                if (SubAIPhase == -1)
                {
                    currentAnimation = AnimationState.RaisingSword;

                    Vector2 targetPosition = player.Center + new Vector2(side * 500, -100);

                    float speed = 0.5f;
                    NPC.velocity.X += Math.Sign(targetPosition.X - NPC.Center.X) * speed;
                    NPC.velocity.Y += Math.Sign(targetPosition.Y - NPC.Center.Y) * speed;

                    attackCounter1++;
                    if (attackCounter1 > 60)
                    {
                        SubAIPhase = 0;
                        attackCounter1 = 0;
                    }
                }
                if (SubAIPhase == 0)
                {
                    attackCounter1++;
                    if (attackCounter1 > 20f + 15f * DreadwindSystem.reaper_HealthLeft)
                    {
                        attackCounter1 = 0f;
                        attackCounter2++;
                        if (attackCounter2 > 5f + (int)(3 * (1f - DreadwindSystem.reaper_HealthLeft)))
                        {
                            SubAIPhase = 1;
                        }

                        float rotation = MathHelper.ToRadians(Main.rand.Next(360));
                        int amount = 5;
                        if (TerrorbornSystem.TwilightMode) amount += 2;
                        for (float i = 0f; i <= 1f; i += 1f / amount)
                        {
                            Vector2 projRotation = new Vector2(1, 0).RotatedBy(rotation + MathHelper.ToDegrees(360 * i));
                            int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + projRotation * (NPC.Distance(player.Center) + Main.screenWidth / 2f * 1.3f), -projRotation * 5f, ModContent.ProjectileType<ReapedSoul>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                            Main.projectile[proj].ai[0] = NPC.whoAmI;
                            Main.projectile[proj].ai[1] = 0.03f;
                        }
                    }
                }
                if (SubAIPhase == 1)
                {
                    attackCounter1++;
                    if (attackCounter1 > 180)
                    {
                        if (handlesSystem)
                        {
                            DreadwindSystem.Reaper_DecideNextAttack(AIPhase);
                        }
                    }
                }
                NPC.velocity *= 0.975f;
            }
        }

    }

    class ReapedSoul : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.penetrate = 25;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.hide = false;
            Projectile.timeLeft = 600;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * (1f - (Projectile.alpha / 255f));
        }

        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            Projectile.rotation = Projectile.DirectionTo(npc.Center).ToRotation() + 3.14f * 0.5f;
            Projectile.velocity = Projectile.DirectionTo(npc.Center) * (Projectile.velocity.Length() + Projectile.ai[1]);
            if (Projectile.Distance(npc.Center) <= Projectile.velocity.Length())
            {
                Projectile.active = false;
            }
        }
    }
}
