using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.TBUtils;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class SongOfTime : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a spectral clock that accellerates your cursor" +
                "\nThe clock will deal 5x damage if it is moving at a high velocity");
        }

        public override void SetDefaults()
        {
            item.damage = 50;
            item.noMelee = true;
            item.width = 52;
            item.height = 52;
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = 5;
            item.knockBack = 2.5f;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item28;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<SpectralClock>();
            item.shootSpeed = 5f;
            item.mana = 3;
            item.magic = true;
            item.channel = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld;
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.active && projectile.type == type)
                {
                    return false;
                }
            }
            return true;
        }
    }

    class SpectralClock : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 20;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }

        float requiredSpeed = 15f;
        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(10);
            projectile.timeLeft = 20;

            if (projectile.alpha > (int)(255 * 0.25f))
            {
                projectile.alpha -= 15;
            }

            if (!Main.player[projectile.owner].channel || Main.player[projectile.owner].statMana < Main.player[projectile.owner].HeldItem.mana * Main.player[projectile.owner].manaCost)
            {
                DustExplosion(projectile.Center, 10, 25f, 46f);
                projectile.active = false;
            }

            if (projectile.Center != Main.MouseWorld)
            {
                projectile.velocity += projectile.DirectionTo(Main.MouseWorld) * 0.5f;
            }
            projectile.velocity *= 0.98f;

            if (projectile.velocity.Length() > requiredSpeed)
            {
                Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 127)];
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            TBUtils.Graphics.DrawGlow_1(spriteBatch, projectile.Center - Main.screenPosition, 75, Color.OrangeRed * 0.5f);
            return base.PreDraw(spriteBatch, lightColor);
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 10, 25f, 46f);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.velocity.Length() > requiredSpeed)
            {
                damage *= 5;
            }
        }

        public void DustExplosion(Vector2 position, int dustAmount, float minDistance, float maxDistance)
        {
            Vector2 direction = new Vector2(0, 1);
            for (int i = 0; i < dustAmount; i++)
            {
                Vector2 newPos = position + direction.RotatedBy(MathHelper.ToRadians((360f / dustAmount) * i)) * Main.rand.NextFloat(minDistance, maxDistance);
                Dust dust = Dust.NewDustPerfect(newPos, 127);
                dust.scale = 1f;
                dust.velocity = (newPos - position) / 5;
                dust.noGravity = true;
            }
        }
    }
}
