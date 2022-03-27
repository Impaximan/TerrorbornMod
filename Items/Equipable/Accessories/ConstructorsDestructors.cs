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
            item.width = 32;
            item.height = 34;
            item.accessory = true;
            item.noMelee = true;
            item.rare = ItemRarityID.Pink;
            item.defense = 8;
            item.value = Item.sellPrice(0, 15, 0, 0);
            item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ConstructorsDestructors = true;

            bool destructorActive = true;
            for (int i = 0; i < 1000; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.type == ModContent.ProjectileType<ConstructorsDestructorsClaw>() && projectile.active)
                {
                    destructorActive = false;
                }
            }
            if (destructorActive)
            {
                int proj = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<ConstructorsDestructorsClaw>(), 50, 0, player.whoAmI);
                Main.projectile[proj].ai[0] = 1;
                proj = Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<ConstructorsDestructorsClaw>(), 50, 0, player.whoAmI);
                Main.projectile[proj].ai[0] = -1;
            }
        }
    }

    class ConstructorsDestructorsClaw : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.width = 78;
            projectile.height = 76;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (projectile.frame == 2)
            {
                spriteBatch.Draw(ModContent.GetTexture(Texture), projectile.Center - Main.screenPosition, new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), Color.White, projectile.rotation, new Vector2(projectile.width / 2, 42), projectile.scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(ModContent.GetTexture(Texture), projectile.Center - Main.screenPosition, new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), Color.White, projectile.rotation, new Vector2(projectile.width / 2, 45), projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
        }

        public override bool? CanHitNPC(NPC target)
        {
            return target.type != NPCID.TruffleWorm && !target.friendly && AIPhase == 1;
        }

        public void HoverAroundPlayer(Player player)
        {
            projectile.frame = 2;
            projectile.rotation += MathHelper.ToRadians(15f) * -projectile.ai[0];
            Vector2 targetPosition = player.Center + new Vector2(250 * projectile.ai[0], 0).RotatedBy(Math.Sin(projectile.ai[1] / 25f) * MathHelper.ToRadians(35f));
            projectile.velocity = (targetPosition - projectile.Center) / 5f;
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
            projectile.ai[1]++;
            Player player = Main.player[projectile.owner];
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
                        projectile.friendly = false;
                        if (item.melee)
                        {
                            projectile.friendly = true;
                            AIPhase = 1;
                            attackCounter1 = 0;
                            attackCounter2 = 0;
                            projectile.damage = (int)(40 * player.meleeDamage * player.allDamageMult);
                            projectile.melee = true;
                            projectile.magic = false;
                            projectile.ranged = false;
                            projectile.localNPCHitCooldown = 25;
                        }
                        else if (item.magic)
                        {
                            AIPhase = 2;
                            attackCounter1 = 0;
                            attackCounter2 = 0;
                            projectile.melee = false;
                            projectile.magic = true;
                            projectile.ranged = false;
                            attackDirection = projectile.DirectionTo(Main.MouseWorld);
                            Main.PlaySound(SoundID.Zombie, (int)projectile.Center.X, (int)projectile.Center.Y, 104, 0.5f, 2f);
                        }
                        else if (item.ranged)
                        {
                            AIPhase = 3;
                            projectile.melee = false;
                            projectile.magic = false;
                            projectile.ranged = true;
                            attackCounter1 = 0;
                            currentOffset = projectile.Center - player.Center;
                            attackCounter2 = 0;
                            lerpAmount = 0f;
                        }
                    }
                }
                else if (AIPhase == 1)
                {
                    if (attackCounter2 == 0)
                    {
                        projectile.velocity = projectile.DirectionTo(Main.MouseWorld) * projectile.Distance(Main.MouseWorld) / 10f;
                        projectile.rotation = projectile.DirectionTo(Main.MouseWorld).RotatedBy(MathHelper.ToRadians(90)).ToRotation();
                        projectile.frame = 0;
                        attackCounter2++;
                    }
                    else if (attackCounter2 == 1)
                    {
                        projectile.rotation = projectile.rotation.AngleLerp(projectile.DirectionTo(Main.MouseWorld).RotatedBy(MathHelper.ToRadians(90)).ToRotation(), 0.1f);
                        attackCounter1++;
                        if (attackCounter1 > 25)
                        {
                            AIPhase = 0;
                            cooldownTime = (int)(Main.rand.Next(25, 35) / player.meleeSpeed / superiorPlayer.allUseSpeed);
                        }
                        projectile.velocity *= 0.95f;
                    }
                }
                else if (AIPhase == 2)
                {
                    projectile.rotation += MathHelper.ToRadians(25f) * -projectile.ai[0];
                    projectile.velocity *= 0.98f;
                    attackCounter1++;
                    if (attackCounter1 > 60)
                    {
                        AIPhase = 0;
                        cooldownTime = (int)(120 / superiorPlayer.magicUseSpeed / superiorPlayer.allUseSpeed);
                    }
                    attackDirection = attackDirection.ToRotation().AngleLerp(projectile.DirectionTo(Main.MouseWorld).ToRotation(), 0.1f).ToRotationVector2();
                    int proj = Projectile.NewProjectile(projectile.Center, attackDirection, ModContent.ProjectileType<ClockworkDeathrayFriendly>(), (int)(50 * player.magicDamage * player.allDamageMult), 0.1f, projectile.owner);
                    Main.projectile[proj].ai[0] = projectile.whoAmI;
                    TerrorbornMod.ScreenShake(1.5f);
                }
                else if (AIPhase == 3)
                {
                    rotationAmount = (float)Math.Sin(projectile.ai[1] / 20f) * 50f;
                    attackDirection = player.DirectionTo(Main.MouseWorld);
                    Vector2 targetOffset = attackDirection.RotatedBy(MathHelper.ToRadians(rotationAmount * projectile.ai[0])) * 150f;
                    if (lerpAmount < 1f)
                    {
                        lerpAmount += 1f / 60f;
                    }
                    currentOffset = Vector2.Lerp(currentOffset, targetOffset, lerpAmount);
                    projectile.position = player.Center + currentOffset - projectile.Size / 2;
                    projectile.rotation = attackDirection.RotatedBy(MathHelper.ToRadians(rotationAmount * projectile.ai[0] + 90f)).ToRotation();
                    projectile.frame = 1;
                    if (player.itemTime <= 0)
                    {
                        AIPhase = 0;
                    }
                    attackCounter1++;
                    if (attackCounter1 >= 15 / superiorPlayer.rangedUseSpeed / superiorPlayer.allUseSpeed)
                    {
                        attackCounter1 = 0;
                        Main.PlaySound(SoundID.Item125, projectile.Center);
                        Projectile.NewProjectile(projectile.Center, attackDirection.RotatedBy(MathHelper.ToRadians(rotationAmount * projectile.ai[0])) * 25f, ModContent.ProjectileType<HellbornLaserFriendly>(), (int)(35 * player.rangedDamage * player.allDamageMult), 0.1f, projectile.owner);
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
                projectile.active = false;
            }
            else
            {
                projectile.timeLeft = 60;
            }
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 5;
            }

            direction += MathHelper.ToRadians(1f);
        }
    }
    class ClockworkDeathrayFriendly : Deathray
    {
        public override string Texture => "TerrorbornMod/Projectiles/IncendiaryDeathray";
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 22;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.hide = false;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 2;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 13;
            MoveDistance = 20f;
            RealMaxDistance = 2500f;
            bodyRect = new Rectangle(0, 24, 18, 22);
            headRect = new Rectangle(0, 0, 18, 22);
            tailRect = new Rectangle(0, 46, 18, 22);
        }

        public override Vector2 Position()
        {
            return Main.projectile[(int)projectile.ai[0]].Center;
        }
    }

    class HellbornLaserFriendly : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Projectiles/HellbornLaser";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.penetrate = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.ranged = true;
            projectile.timeLeft = 240;
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
                List<Vector2> positions = bezier.GetPoints(30);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
                    Color color = projectile.GetAlpha(Color.Lerp(Color.Crimson, new Color(255, 194, 177), mult)) * mult;
                    TBUtils.Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(25f * mult), color);
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
}