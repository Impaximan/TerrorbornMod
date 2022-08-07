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
    class Synneffo : ModNPC
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
            Main.npcFrameCount[Type] = 10;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return diving;
        }

        public override void SetDefaults()
        {
            NPC.width = 104;
            NPC.height = 126;
            NPC.damage = DreadwindSystem.DreadwindLargeDamage / 2;
            NPC.lifeMax = 10000;
            NPC.defense = 35;
            NPC.HitSound = SoundID.NPCHit13;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = false;
        }

        bool diving = false;
        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 5)
            {
                NPC.frameCounter = 0;
                frame++;
                if (frame >= 5)
                {
                    frame = 0;
                }
            }

            NPC.frame.Y = frame * frameHeight;
            if (diving)
            {
                NPC.frame.Y += 5 * frameHeight;
            }
        }

        public void GoDoNextAttack(int previousAttack)
        {
            AIPhase = Main.rand.Next(2);
            while (AIPhase == previousAttack)
            {
                AIPhase = Main.rand.Next(2);
            }
            SubAIPhase = -1;
        }

        int AIPhase = -1;
        int SubAIPhase = -1;
        int AICounter1 = 0;
        int AICounter2 = 0;
        float AICounter3 = 0;
        public override void AI()
        {
            Player player = Main.LocalPlayer;

            if (player.dead || !DreadwindSystem.DreadwindActive)
            {
                diving = true;
                NPC.velocity.Y -= 0.5f;
                NPC.velocity.X = 0;
                NPC.rotation = 0f;

                NPC.rotation = NPC.velocity.ToRotation();

                if (NPC.spriteDirection == -1)
                {
                    NPC.rotation += MathHelper.ToRadians(180);
                }

                if (NPC.Center.Y < player.Center.Y - Main.screenHeight)
                {
                    NPC.active = false;
                }
                return;
            }

            if (AIPhase == -1)
            {
                GoDoNextAttack(AIPhase);
            }
            if (AIPhase == 0)
            {
                if (SubAIPhase == -1)
                {
                    AICounter1 = Main.rand.Next(2, 4);
                    SubAIPhase = 0;
                    AICounter2 = 0;
                    AICounter3 = 0;
                }
                if (SubAIPhase == 0)
                {
                    diving = false;
                    NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);
                    NPC.rotation = MathHelper.ToRadians(NPC.velocity.X);
                    if (NPC.Distance(player.Center + NPC.DirectionFrom(player.Center) * 500f) >= 100)
                    {
                        NPC.velocity += NPC.DirectionTo(player.Center + NPC.DirectionFrom(player.Center) * 500f) * 2f;
                        NPC.velocity *= 0.93f;
                    }

                    AICounter2++;
                    if (AICounter2 >= 120)
                    {
                        SubAIPhase = 1;
                        AICounter2 = 0;
                        NPC.velocity = NPC.DirectionTo(player.Center) * 5f;
                        SoundEngine.PlaySound(SoundID.Item113, NPC.Center);
                    }
                }
                if (SubAIPhase == 1)
                {
                    AICounter3++;
                    diving = true;
                    NPC.velocity *= 1.05f;
                    NPC.rotation = NPC.velocity.ToRotation();
                    if (NPC.spriteDirection == -1)
                    {
                        NPC.rotation += MathHelper.ToRadians(180);
                    }
                    if (AICounter3 > 55)
                    {
                        NPC.velocity = NPC.velocity.ToRotation().ToRotationVector2() * 15f;
                        AICounter3 = 0;
                        SubAIPhase = 0;
                        AICounter1--;
                        if (AICounter1 <= 0)
                        {
                            GoDoNextAttack(AIPhase);
                        }
                    }
                }
            }
            if (AIPhase == 1)
            {
                if (SubAIPhase == -1)
                {
                    diving = false;
                    NPC.spriteDirection = Math.Sign(player.Center.X - NPC.Center.X);

                    NPC.velocity += NPC.DirectionTo(player.Center) * 1f;
                    NPC.velocity *= 0.96f;
                    if (NPC.Distance(player.Center) <= 500)
                    {
                        SubAIPhase++;
                        AICounter1 = 0;
                        AICounter2 = 0;
                        AICounter3 = 10;
                    }
                    NPC.rotation = MathHelper.ToRadians(NPC.velocity.X);
                }
                if (SubAIPhase == 0)
                {
                    NPC.velocity *= 0.9f;
                    NPC.rotation = MathHelper.ToRadians(NPC.velocity.X);
                    AICounter1++;
                    if (AICounter1 >= 60)
                    {
                        SubAIPhase++;
                        diving = true;
                        NPC.velocity = new Vector2(NPC.spriteDirection * 10, 0);
                        SoundEngine.PlaySound(SoundID.DD2_BetsySummon, NPC.Center);
                    }
                }
                if (SubAIPhase == 1)
                {
                    NPC.velocity = NPC.velocity.RotatedBy(MathHelper.ToRadians(-10) * NPC.spriteDirection);

                    Vector2 attackRotation = NPC.velocity.ToRotation().ToRotationVector2();
                    NPC.velocity += attackRotation / 4;

                    NPC.rotation = NPC.velocity.ToRotation();

                    if (NPC.spriteDirection == -1)
                    {
                        NPC.rotation += MathHelper.ToRadians(180);
                    }

                    AICounter3 += 20f / 240f;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + attackRotation * 50, attackRotation * AICounter3, ModContent.ProjectileType<Synneflame>(), DreadwindSystem.DreadwindLowDamage / 4, 0f);

                    AICounter2++;
                    if (AICounter2 % 10 == 1)
                    {
                        SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath, NPC.Center);
                    }
                    if (AICounter2 >= 240)
                    {
                        GoDoNextAttack(AIPhase);
                    }
                }
            }
        }
    }

    class Synneflame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        int timeLeft = 21;
        public override void SetDefaults()
        {
            Projectile.width = 98;
            Projectile.height = 98;
            Projectile.timeLeft = timeLeft;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle ogHitbox = hitbox;
            hitbox.Width = 40;
            hitbox.Height = 40;
            hitbox.X = ogHitbox.Center.X - 20;
            hitbox.Y = ogHitbox.Center.Y - 20;
        }

        int frameCounter = 0;
        public override void AI()
        {
            frameCounter++;
            if (frameCounter >= timeLeft / 7f)
            {
                frameCounter = 0;
                Projectile.frame++;
            }
        }
    }
}
