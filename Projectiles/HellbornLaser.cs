using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Projectiles
{
    class HellbornLaser : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 38;
            projectile.height = 6;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
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
            Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Fire, Vector2.Zero);
            dust.noGravity = true;
        }
    }
}
