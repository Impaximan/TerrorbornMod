using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class GraveNeedle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Sticks onto hit enemies" +
                "\nIf 5 in total are stuck in enemies, all of them will explode");
        }

        public override void SetDefaults()
        {
            TerrorbornItem.modItem(item).countAsThrown = true;
            item.damage = 20;
            item.ranged = true;
            item.width = 28;
            item.height = 26;
            item.consumable = false;
            item.useTime = 20;
            item.useAnimation = 20;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = true;
            item.shootSpeed = 35;
            item.shoot = ModContent.ProjectileType<GraveNeedleProjectile>();
        }
    }

    class GraveNeedleProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/GraveNeedle";

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 26;
            projectile.ranged = true;
            projectile.timeLeft = 3600;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.extraUpdates = 4;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.arrow = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 8;
            height = 8;
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!stuck)
            {
                Main.PlaySound(SoundID.Dig, projectile.position);
                stuck = true;
                stuckNPC = target;
                offset = target.position - projectile.position;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (stuck)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle originalHitbox = hitbox;
            int newSize = 12;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }

        bool stuck = false;
        NPC stuckNPC;
        Vector2 offset;
        bool start = true;
        int projectileCounter = 0;
        public override void AI()
        {
            if (start)
            {
                start = false;
                projectile.velocity /= projectile.extraUpdates + 1;
                projectile.timeLeft *= projectile.extraUpdates + 1;
            }

            if (stuck)
            {
                projectile.ai[0] = 1;

                projectile.tileCollide = false;
                projectile.position = stuckNPC.position - offset;
                if (!stuckNPC.active)
                {
                    projectile.active = false;
                }

                List<Projectile> needles = new List<Projectile>();
                foreach (Projectile needle in Main.projectile)
                {
                    if (needle.active && needle.type == projectile.type && needle.ai[0] == 1)
                    {
                        needles.Add(needle);
                    }
                }

                if (needles.Count >= 5)
                {
                    foreach (Projectile needle in needles)
                    {
                        needle.active = false;

                        float speed = 10f;
                        Vector2 velocity = (needle.rotation - MathHelper.ToRadians(45)).ToRotationVector2() * speed;
                        Projectile.NewProjectile(needle.Center, velocity, ModContent.ProjectileType<NeedleExplosion>(), projectile.damage, projectile.knockBack * 3, projectile.owner);
                    }
                    Main.PlaySound(SoundID.Item62, projectile.Center);
                    TerrorbornMod.ScreenShake(3f);
                }
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
            }
        }
    }

    class NeedleExplosion : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Effects/Textures/Glow_2";

        int timeLeft = 15;
        const int defaultSize = 100;
        int currentSize = defaultSize;
        public override void SetDefaults()
        {
            projectile.width = defaultSize;
            projectile.height = defaultSize;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.localNPCHitCooldown = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.timeLeft = timeLeft;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            TBUtils.Graphics.DrawGlow_1(spriteBatch, projectile.Center - Main.screenPosition, currentSize, new Color(234, 79, 9));
            return false;
        }

        public override void AI()
        {
            currentSize -= defaultSize / timeLeft;
        }
    }
}