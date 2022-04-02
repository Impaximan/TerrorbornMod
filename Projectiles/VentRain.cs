using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Projectiles
{
    class VentRain : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 30;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 500;
            Projectile.tileCollide = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            //position.Y += 4;
            Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, Projectile.width, Projectile.height), new Rectangle(0, 0, Projectile.width, Projectile.height), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), SpriteEffects.None, 0);
            return false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }
    }
}
