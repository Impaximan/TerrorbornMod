using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Projectiles
{
    class DreadRain : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 168;
            Projectile.height = 16;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = false;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle ogHitbox = hitbox;
            hitbox.Width = 16;
            hitbox.Height = 80;
            hitbox.X = ogHitbox.Center.X - hitbox.Width / 2;
            hitbox.Y = ogHitbox.Center.Y - hitbox.Height / 2;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        int frame = -1;
        public override void AI()
        {
            if (frame == -1)
            {
                frame = Main.rand.Next(3);
            }
            Projectile.frame = frame;

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.position.Y > Main.screenPosition.Y + Main.screenHeight)
            {
                Projectile.active = false;
            }
        }
    }
}
