using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TerrorbornMod.Dreadwind.Waves;
using Microsoft.Xna.Framework.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.Dreadwind.NPCs
{
    class DamnationStrider : ModNPC
    {
        public override bool CheckActive()
        {
            return false;
        }

        public float GetLegAngle(float armLength, Vector2 armStart, Vector2 armTarget)
        {
            if (Vector2.Distance(armStart, armTarget) >= armLength * 2)
            {
                return 0f;
            }

            float distance = Vector2.Distance(armStart, armTarget);

            return (float)Math.Acos((Math.Pow(armLength, 2) + Math.Pow(distance, 2f) - Math.Pow(armLength, 2f)) / (2f * armLength * distance));
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return true;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }

        public static Texture2D legTexture1;
        public static Texture2D legTexture2;
        public override void Load()
        {
            legTexture1 = ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/DamnationStriderLeg1", AssetRequestMode.ImmediateLoad).Value;
            legTexture2 =  ModContent.Request<Texture2D>("TerrorbornMod/Dreadwind/NPCs/DamnationStriderLeg2", AssetRequestMode.ImmediateLoad).Value;
        }

        public override void SetDefaults()
        {
            NPC.width = 102;
            NPC.height = 106;
            NPC.damage = DreadwindSystem.DreadwindLargeDamage / 2;
            NPC.lifeMax = 65000;
            NPC.defense = 35;
            NPC.HitSound = SoundID.NPCHit35;
            NPC.DeathSound = SoundID.NPCDeath46;
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = false;
        }

        //Leg origin compared to head, leg position, leg target position, leg original position
        List<Tuple<Vector2, Vector2, Vector2, Vector2>> rightLegs = new List<Tuple<Vector2, Vector2, Vector2, Vector2>>();
        List<Tuple<Vector2, Vector2, Vector2, Vector2>> leftLegs = new List<Tuple<Vector2, Vector2, Vector2, Vector2>>();
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            foreach (Tuple<Vector2 ,Vector2, Vector2, Vector2> leg in rightLegs)
            {
                DrawLeg(leg.Item1 + NPC.Center, leg.Item2, 1, spriteBatch);
            }
            foreach (Tuple<Vector2, Vector2, Vector2, Vector2> leg in leftLegs)
            {
                DrawLeg(leg.Item1 + NPC.Center, leg.Item2, -1, spriteBatch);
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            //Spawn right legs
            for (float i = 0f; i <= 1f; i += 1f / 4f)
            {
                rightLegs.Add(new Tuple<Vector2, Vector2, Vector2, Vector2>(
                    new Vector2(10, -20).RotatedBy(MathHelper.ToRadians(45f) * i), //Leg origin compared to head
                    new Vector2(300, -40).RotatedBy(MathHelper.ToRadians(75f) * i) + NPC.Center, //Leg position
                    new Vector2(300, -40).RotatedBy(MathHelper.ToRadians(75f) * i) + NPC.Center,
                    new Vector2(300, -40).RotatedBy(MathHelper.ToRadians(75f) * i))); //Leg target position
            }

            //Spawn right legs
            for (float i = 0f; i <= 1f; i += 1f / 4f)
            {
                leftLegs.Add(new Tuple<Vector2, Vector2, Vector2, Vector2>(
                    new Vector2(-10, -20).RotatedBy(MathHelper.ToRadians(-45f) * i), //Leg origin compared to head
                    new Vector2(-300, -40).RotatedBy(MathHelper.ToRadians(-75f) * i) + NPC.Center, //Leg position
                    new Vector2(-300, -40).RotatedBy(MathHelper.ToRadians(-75f) * i) + NPC.Center,
                    new Vector2(-300, -40).RotatedBy(MathHelper.ToRadians(-75f) * i))); //Leg target position
            }
        }

        public void ResetLegPositions()
        {
            for (int i = 0; i < rightLegs.Count; i++)
            {
                Tuple<Vector2, Vector2, Vector2, Vector2> leg = rightLegs[i];
                rightLegs[i] = new Tuple<Vector2, Vector2, Vector2, Vector2>(leg.Item1, leg.Item2, leg.Item4 + NPC.Center, leg.Item4);
            }

            for (int i = 0; i < leftLegs.Count; i++)
            {
                Tuple<Vector2, Vector2, Vector2, Vector2> leg = leftLegs[i];
                leftLegs[i] = new Tuple<Vector2, Vector2, Vector2, Vector2>(leg.Item1, leg.Item2, leg.Item4 + NPC.Center, leg.Item4);
            }
        }

        public void UpdateLegMovement()
        {
            int sideLeeway = 50;

            for (int i = 0; i < rightLegs.Count; i++)
            {
                Tuple<Vector2, Vector2, Vector2, Vector2> leg = rightLegs[i];
                Vector2 newTarget = leg.Item3;
                if (leg.Item3.Distance(NPC.Center) >= legTexture1.Height * 2f || leg.Item3.X < NPC.Center.X - sideLeeway)
                {
                    newTarget = NPC.Center + new Vector2(Main.rand.Next(100, 322), Main.rand.Next(-200, 300)) + NPC.velocity * 30f;
                    float velocityMult = 1f;
                    while (newTarget.Distance(NPC.Center) >= legTexture1.Height * 2f || newTarget.X < NPC.Center.X - sideLeeway)
                    {
                        newTarget = NPC.Center + new Vector2(Main.rand.Next(100, 322), Main.rand.Next(-200, 300)) + NPC.velocity * 30f * velocityMult;
                        velocityMult -= 1f / 30f;
                    }
                }
                rightLegs[i] = new Tuple<Vector2, Vector2, Vector2, Vector2>(leg.Item1, Vector2.Lerp(leg.Item2, leg.Item3, 0.35f), newTarget, leg.Item4);
            }

            for (int i = 0; i < leftLegs.Count; i++)
            {
                Tuple<Vector2, Vector2, Vector2, Vector2> leg = leftLegs[i];
                Vector2 newTarget = leg.Item3;
                if (leg.Item3.Distance(NPC.Center) > legTexture1.Height * 1.9f || leg.Item3.X > NPC.Center.X + sideLeeway)
                {
                    newTarget = NPC.Center + new Vector2(-Main.rand.Next(100, 322), Main.rand.Next(-200, 300)) + NPC.velocity * 30f;
                    float velocityMult = 1f;
                    while (newTarget.Distance(NPC.Center) >= legTexture1.Height * 2f || newTarget.X > NPC.Center.X + sideLeeway)
                    {
                        newTarget = NPC.Center + new Vector2(-Main.rand.Next(100, 322), Main.rand.Next(-200, 300)) + NPC.velocity * 30f * velocityMult;
                        velocityMult -= 1f / 30f;
                    }

                }
                leftLegs[i] = new Tuple<Vector2, Vector2, Vector2, Vector2>(leg.Item1, Vector2.Lerp(leg.Item2, leg.Item3, 0.35f), newTarget, leg.Item4);
            }
        }

        List<int> nextAttacksList = new List<int>();
        public void GoDoNextAttack(int previousAttack)
        {
            if (nextAttacksList.Count == 0)
            {
                int count = 2;
                if (NPC.life <= NPC.lifeMax * 0.9f)
                {
                    count++;
                }
                if (NPC.life <= NPC.lifeMax * 0.65f)
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
            attackTarget = Main.player[NPC.target].Center;
            nextAttacksList.RemoveAt(0);
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            if (NPC.frameCounter > 4)
            {
                NPC.frameCounter = 0;
                frame++;
                if (frame >= 4)
                {
                    frame = 0;
                }
            }

            NPC.frame.Y = frameHeight * frame;
        }

        int AIPhase = -1;
        int SubAIPhase = -1;
        float attackCounter1 = 0;
        float attackCounter2 = 0;
        float attackCounter3 = 0;
        Vector2 attackTarget;
        int attackDirection = 0;

        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];

            if (player.dead || !DreadwindSystem.DreadwindActive)
            {
                NPC.velocity.Y += 0.4f;
                UpdateLegMovement();
                if (NPC.position.Y > player.Center.Y + Main.screenHeight)
                {
                    NPC.active = false;
                }
                return;
            }

            if (AIPhase == -1)
            {
                GoDoNextAttack(AIPhase);
            }
            if (AIPhase == 0) //Simply move towards player
            {
                if (SubAIPhase == -1)
                {
                    attackCounter1 = 0;
                    SubAIPhase++;
                }

                NPC.velocity += NPC.DirectionTo(player.Center) * 0.3f;
                NPC.velocity *= 0.98f;
                UpdateLegMovement();

                attackCounter1++;
                if (attackCounter1 >= 360)
                {
                    GoDoNextAttack(AIPhase);
                }
            }
            if (AIPhase == 1) //Burst of souls
            {
                if (SubAIPhase == -1)
                {
                    if (NPC.Distance(player.Center) > 300)
                    {
                        attackCounter2 += 1f / 60f;
                        NPC.velocity += NPC.DirectionTo(player.Center) * attackCounter2;
                        NPC.velocity *= 0.98f;
                        UpdateLegMovement();
                    }
                    else
                    {
                        SubAIPhase++;
                        ResetLegPositions();
                        attackCounter1 = 0;
                        NPC.velocity = Vector2.Zero;
                    }
                }
                else if (SubAIPhase == 0)
                {
                    UpdateLegMovement();
                    attackCounter1++;
                    if (attackCounter1 > 90)
                    {
                        for (float i = 0f; i < 1f; i += 1f / 16f)
                        {
                            Vector2 velocity = new Vector2(15, 0).RotatedBy(MathHelper.ToRadians(360) * i);
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, ModContent.ProjectileType<SoulOfMightProjectile>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                        }
                        SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, NPC.Center);
                        TerrorbornSystem.ScreenShake(10f);
                        GoDoNextAttack(AIPhase);
                    }
                }
            }
            if (AIPhase == 2) //Damnation minefield
            {
                if (SubAIPhase == -1)
                {
                    SubAIPhase++;
                }
                if (SubAIPhase == 0)
                {
                    UpdateLegMovement();
                    NPC.velocity *= 0.95f;
                    attackCounter1++;
                    if (NPC.velocity.Length() <= 1f && attackCounter1 > 60)
                    {
                        SubAIPhase++;
                        attackCounter1 = 0f;
                        SoundStyle style = SoundID.NPCDeath10;
                        style.Pitch = -0.5f;
                        SoundEngine.PlaySound(style);
                        attackTarget = player.Center;
                        attackCounter2 = -5f;
                    }
                }
                if (SubAIPhase == 1)
                {
                    TerrorbornSystem.ScreenShake(10);
                    attackCounter1 += 6000f / 180f;

                    attackCounter2++;
                    if (attackCounter2 > 5f)
                    {
                        attackCounter2 = 0f;
                        attackCounter3 += 6f;
                        float rotationAddend = Main.rand.NextFloat(1f / attackCounter3);
                        for (float i = 0f; i < 1f; i += 1f / attackCounter3)
                        {
                            Vector2 position = new Vector2(attackCounter1, 0).RotatedBy(MathHelper.ToRadians(360) * (i + rotationAddend)) + attackTarget;
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), position, Vector2.Zero, ModContent.ProjectileType<DamnationMine>(), DreadwindSystem.DreadwindMidDamage / 4, 0f);
                        }
                    }

                    if (attackCounter1 > 3000f)
                    {
                        GoDoNextAttack(AIPhase);
                    }
                }
            }
            if (AIPhase == 3) //Damnation Deathray
            {
                if (SubAIPhase == -1)
                {
                    if (NPC.Distance(player.Center + new Vector2(0, -400)) > 300 || attackCounter2 < 0.65f)
                    {
                        attackCounter2 += 1f / 60f;
                        NPC.velocity += NPC.DirectionTo(player.Center + new Vector2(0, -350)) * attackCounter2;
                        NPC.velocity *= 0.98f;
                        UpdateLegMovement();

                        attackDirection = 1;
                        if (NPC.velocity.X < 0) attackDirection = -1;

                        attackCounter1++;
                        if (attackCounter1 > 5)
                        {
                            attackCounter1 = 0;
                            ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/undertalewarning").Value.Play(Main.soundVolume, 0f, 0f);
                        }
                    }
                    else
                    {
                        SubAIPhase++;
                        attackCounter1 = 0;
                        attackTarget = player.Center + new Vector2(300 * attackDirection, 0);
                        TerrorbornSystem.ScreenShake(10f);
                        SoundExtensions.PlaySoundOld(SoundID.Zombie104, (int)NPC.Center.X, (int)NPC.Center.Y, 104, 1, 2f);
                    }
                }

                if (SubAIPhase == 0)
                {
                    UpdateLegMovement();

                    TerrorbornSystem.ScreenShake(2f);
                    NPC.velocity *= 0.98f;

                    attackTarget.X -= 300f / 60f * attackDirection;

                    int proj = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(attackTarget), ModContent.ProjectileType<DamnationDeathray>(), DreadwindSystem.DreadwindLargeDamage / 4, 0f);
                    Main.projectile[proj].ai[0] = NPC.whoAmI;

                    attackCounter1++;
                    if (attackCounter1 > 60)
                    {
                        GoDoNextAttack(AIPhase);
                    }
                }
            }
        }

        public void DrawLeg(Vector2 legStart, Vector2 legEnd, int side, SpriteBatch spriteBatch)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (side == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            spriteBatch.Draw(legTexture1, legStart - Main.screenPosition, null, Color.White, (legEnd - legStart).RotatedBy(MathHelper.ToRadians(-90)).ToRotation() + GetLegAngle(legTexture1.Height, legStart, legEnd) * -side, new Vector2(legTexture1.Width / 2, 0), 1f, effects, 0f);
            spriteBatch.Draw(legTexture2, legEnd - Main.screenPosition, null, Color.White, (legStart - legEnd).RotatedBy(MathHelper.ToRadians(90)).ToRotation() + GetLegAngle(legTexture1.Height, legStart, legEnd) * side, new Vector2(legTexture2.Width / 2, legTexture2.Height), 1f, effects, 0f);
        }
    }

    class DamnationDeathray : Deathray
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 100000;
        }

        public override string Texture => "TerrorbornMod/Dreadwind/NPCs/DamnationDeathray";
        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.hide = false;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.timeLeft = 2;
            MoveDistance = 50f;
            RealMaxDistance = 3000f;
            bodyRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
            headRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
            tailRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
        }

        public override Vector2 Position()
        {
            return Main.npc[(int)Projectile.ai[0]].Center;
        }
    }

    class DamnationMine : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 1000;
            Projectile.tileCollide = false;
            Projectile.scale = 0f;
        }

        int timeAlive = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * 32, 30, 32), Color.White, 0f, Projectile.Size, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return timeAlive > 60 && Projectile.scale > 0.25f;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= 8)
                {
                    Projectile.frame = 0;
                }
            }

            timeAlive++;
            if (timeAlive > 240)
            {
                Projectile.scale -= 1f / 60f;
                if (Projectile.scale <= 0f)
                {
                    Projectile.active = false;
                }
            }
            else
            {
                Projectile.scale = MathHelper.Lerp(Projectile.scale, 1f, 0.075f);
            }

            if (Projectile.Center != Main.LocalPlayer.Center)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Main.LocalPlayer.Center), 0.05f);
            }
        }
    }

    class SoulOfMightProjectile : ModProjectile
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
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.RoyalBlue, Color.Azure, mult)) * mult;
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

            Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.BlueTorch);
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;

            if (Projectile.Distance(Main.LocalPlayer.Center) > 1000)
            {
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.LocalPlayer.Center).ToRotation(), MathHelper.ToRadians(0.75f)).ToRotationVector2() * (Projectile.velocity.Length() + 0.15f);
            }
            else
            {
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.LocalPlayer.Center).ToRotation(), MathHelper.ToRadians(0.5f)).ToRotationVector2() * (Projectile.velocity.Length() + 0.1f);
            }
        }
    }
}
