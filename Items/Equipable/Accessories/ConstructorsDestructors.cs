using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class ConstructorsDestructors : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons two hexed claws to fight with you, attacking when you do" +
                "\nTheir behavior depends on the class you are attacking with");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 34;
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Pink;
            Item.defense = 8;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ConstructorsDestructors = true;

            bool destructorActive = true;
            for (int i = 0; i < 1000; i++)
            {
                Projectile Projectile = Main.projectile[i];
                if (Projectile.type == ModContent.ProjectileType<ConstructorsDestructorsClaw>() && Projectile.active)
                {
                    destructorActive = false;
                }
            }
            if (destructorActive)
            {
                int proj = Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<ConstructorsDestructorsClaw>(), 50, 0, player.whoAmI);
                Main.projectile[proj].ai[0] = 1;
                proj = Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<ConstructorsDestructorsClaw>(), 50, 0, player.whoAmI);
                Main.projectile[proj].ai[0] = -1;
            }
        }
    }

    class ConstructorsDestructorsClaw : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.width = 78;
            Projectile.height = 76;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.frame == 2)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), Color.White, Projectile.rotation, new Vector2(Projectile.width / 2, 42), Projectile.scale, SpriteEffects.None, 0f);
            }
            else
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), Color.White, Projectile.rotation, new Vector2(Projectile.width / 2, 45), Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
        }

        public override bool? CanHitNPC(NPC target)
        {
            return target.type != NPCID.TruffleWorm && !target.friendly && AIPhase == 1;
        }

        public void HoverAroundPlayer(Player player)
        {
            Projectile.frame = 2;
            Projectile.rotation += MathHelper.ToRadians(15f) * -Projectile.ai[0];
            Vector2 targetPosition = player.Center + new Vector2(250 * Projectile.ai[0], 0).RotatedBy(Math.Sin(Projectile.ai[1] / 25f) * MathHelper.ToRadians(35f));
            Projectile.velocity = (targetPosition - Projectile.Center) / 5f;
        }

        float direction = 0f;
        int AIPhase = 0;
        int attackCounter1 = 0;
        int attackCounter2 = 0;
        int cooldownTime = 0;
        Vector2 attackDirection = Vector2.Zero;
        Vector2 currentOffset = Vector2.Zero;
        float lerpAmount = 0f;
        float rotationAmount = 0f;
        public override void AI()
        {
            Projectile.ai[1]++;
            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer superiorPlayer = TerrorbornPlayer.modPlayer(player);
            Item item = player.HeldItem;

            if (item != null && !item.IsAir)
            {
                if (AIPhase == 0)
                {
                    if (player.itemTime <= 0 || cooldownTime > 0)
                    {
                        cooldownTime--;
                        HoverAroundPlayer(player);
                    }
                    else
                    {
                        Projectile.friendly = false;
                        if (item.DamageType == DamageClass.Melee)
                        {
                            Projectile.friendly = true;
                            AIPhase = 1;
                            attackCounter1 = 0;
                            attackCounter2 = 0;
                            Projectile.damage = (int)(40 * player.GetDamage(DamageClass.Melee).Multiplicative * player.GetDamage(DamageClass.Generic).Multiplicative);
                            Projectile.DamageType = DamageClass.Melee;
                            Projectile.localNPCHitCooldown = 25;
                        }
                        else if (item.DamageType == DamageClass.Magic)
                        {
                            AIPhase = 2;
                            attackCounter1 = 0;
                            attackCounter2 = 0;
                            Projectile.DamageType = DamageClass.Magic;
                            attackDirection = Projectile.DirectionTo(Main.MouseWorld);
                            SoundExtensions.PlaySoundOld(SoundID.Zombie104, (int)Projectile.Center.X, (int)Projectile.Center.Y, 104, 0.5f, 2f);
                        }
                        else if (item.DamageType == DamageClass.Ranged)
                        {
                            AIPhase = 3;
                            Projectile.DamageType = DamageClass.Ranged;
                            attackCounter1 = 0;
                            currentOffset = Projectile.Center - player.Center;
                            attackCounter2 = 0;
                            lerpAmount = 0f;
                        }
                    }
                }
                else if (AIPhase == 1)
                {
                    if (attackCounter2 == 0)
                    {
                        Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * Projectile.Distance(Main.MouseWorld) / 10f;
                        Projectile.rotation = Projectile.DirectionTo(Main.MouseWorld).RotatedBy(MathHelper.ToRadians(90)).ToRotation();
                        Projectile.frame = 0;
                        attackCounter2++;
                    }
                    else if (attackCounter2 == 1)
                    {
                        Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.DirectionTo(Main.MouseWorld).RotatedBy(MathHelper.ToRadians(90)).ToRotation(), 0.1f);
                        attackCounter1++;
                        if (attackCounter1 > 25)
                        {
                            AIPhase = 0;
                            cooldownTime = (int)(Main.rand.Next(25, 35) / player.GetAttackSpeed(DamageClass.Melee) / player.GetAttackSpeed(DamageClass.Generic));
                        }
                        Projectile.velocity *= 0.95f;
                    }
                }
                else if (AIPhase == 2)
                {
                    Projectile.rotation += MathHelper.ToRadians(25f) * -Projectile.ai[0];
                    Projectile.velocity *= 0.98f;
                    attackCounter1++;
                    if (attackCounter1 > 60)
                    {
                        AIPhase = 0;
                        cooldownTime = (int)(120 / player.GetAttackSpeed(DamageClass.Magic) / player.GetAttackSpeed(DamageClass.Generic));
                    }
                    attackDirection = attackDirection.ToRotation().AngleLerp(Projectile.DirectionTo(Main.MouseWorld).ToRotation(), 0.1f).ToRotationVector2();
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, attackDirection, ModContent.ProjectileType<ClockworkDeathrayFriendly>(), (int)(50 * player.GetDamage(DamageClass.Magic).Multiplicative * player.GetDamage(DamageClass.Generic).Multiplicative), 0.1f, Projectile.owner);
                    Main.projectile[proj].ai[0] = Projectile.whoAmI;
                    TerrorbornSystem.ScreenShake(1.5f);
                }
                else if (AIPhase == 3)
                {
                    rotationAmount = (float)Math.Sin(Projectile.ai[1] / 20f) * 50f;
                    attackDirection = player.DirectionTo(Main.MouseWorld);
                    Vector2 targetOffset = attackDirection.RotatedBy(MathHelper.ToRadians(rotationAmount * Projectile.ai[0])) * 150f;
                    if (lerpAmount < 1f)
                    {
                        lerpAmount += 1f / 60f;
                    }
                    currentOffset = Vector2.Lerp(currentOffset, targetOffset, lerpAmount);
                    Projectile.position = player.Center + currentOffset - Projectile.Size / 2;
                    Projectile.rotation = attackDirection.RotatedBy(MathHelper.ToRadians(rotationAmount * Projectile.ai[0] + 90f)).ToRotation();
                    Projectile.frame = 1;
                    if (player.itemTime <= 0)
                    {
                        AIPhase = 0;
                    }
                    attackCounter1++;
                    if (attackCounter1 >= 15 / player.GetAttackSpeed(DamageClass.Ranged) / player.GetAttackSpeed(DamageClass.Generic))
                    {
                        attackCounter1 = 0;
                        SoundExtensions.PlaySoundOld(SoundID.Item125, Projectile.Center);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, attackDirection.RotatedBy(MathHelper.ToRadians(rotationAmount * Projectile.ai[0])) * 25f, ModContent.ProjectileType<HellbornLaserFriendly>(), (int)(35 * player.GetDamage(DamageClass.Ranged).Multiplicative * player.GetDamage(DamageClass.Generic).Multiplicative), 0.1f, Projectile.owner);
                    }
                }
            }
            else
            {
                AIPhase = 0;
                HoverAroundPlayer(player);
            }

            if (!superiorPlayer.ConstructorsDestructors)
            {
                Projectile.active = false;
            }
            else
            {
                Projectile.timeLeft = 60;
            }
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 5;
            }

            direction += MathHelper.ToRadians(1f);
        }
    }
    class ClockworkDeathrayFriendly : Deathray
    {
        public override string Texture => "TerrorbornMod/Projectiles/IncendiaryDeathray";
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 22;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.hide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.timeLeft = 2;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 13;
            MoveDistance = 20f;
            RealMaxDistance = 2500f;
            bodyRect = new Rectangle(0, 24, 18, 22);
            headRect = new Rectangle(0, 0, 18, 22);
            tailRect = new Rectangle(0, 46, 18, 22);
        }

        public override Vector2 Position()
        {
            return Main.projectile[(int)Projectile.ai[0]].Center;
        }
    }

    class HellbornLaserFriendly : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Projectiles/HellbornLaser";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 240;
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
                List<Vector2> positions = bezier.GetPoints(30);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.Crimson, new Color(255, 194, 177), mult)) * mult;
                    Utils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * mult), color);
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
}