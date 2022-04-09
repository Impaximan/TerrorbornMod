using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Abilities
{
    class TerrorAbility : ModProjectile
    {
        public override string Texture => "TerrorbornMod/" + TexturePath;
        public virtual string TexturePath => "Abilities/ShriekOfHorror_Icon";
        public override void SetDefaults()
        {
            Projectile.damage = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
            Projectile.penetrate = -1;
            Projectile.light = 0.2f;
            Projectile.width = (int)dimensions().X;
            Projectile.height = (int)dimensions().Y;
            Projectile.scale = getScale();
        }

        public virtual float getScale()
        {
            return 2f;
        }

        public virtual Vector2 dimensions()
        {
            return new Vector2(20, 24);
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }

        public virtual bool HasLockedPosition()
        {
            return true;
        }

        public virtual Vector2 lockedPosition()
        {
            return Vector2.Zero;
        }

        public virtual Vector2 baseOffsets()
        {
            return new Vector2(12, 0);
        }

        public virtual void ActualAI()
        {

        }

        Vector2 positionOffset = Vector2.Zero;
        int offsetDirection = 1;
        float offsetSpeed = 0;

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(TexturePath);
            Main.spriteBatch.Draw(texture, Projectile.Center + positionOffset + baseOffsets() - Main.screenPosition, new Rectangle(0, 0, (int)(texture.Width), (int)(texture.Height)), Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public virtual void UpdateObtainablity(float maxDistance = 30)
        {
            if (Main.player[Player.FindClosest(Projectile.Center + positionOffset + baseOffsets(), 0, 0)].Distance(Projectile.Center + positionOffset + baseOffsets()) <= maxDistance)
            {
                ObtainAbility();
            }
        }

        public virtual void ObtainAbility()
        {

        }

        public virtual void Float(float maxSpeed = 3, float accelerationSpeed = 0.25f)
        {
            offsetSpeed += offsetDirection * accelerationSpeed;
            if ((offsetSpeed > maxSpeed && offsetDirection == 1) || (offsetSpeed < -maxSpeed && offsetDirection == -1))
            {
                offsetDirection *= -1;
            }
            positionOffset.Y += offsetSpeed;
        }

        public override void AI()
        {
            Projectile.timeLeft = 500;
            ActualAI();
            if (HasLockedPosition())
            {
                Projectile.position = (lockedPosition()) - new Vector2(Projectile.width, Projectile.height);
            }
            //Projectile.position += positionOffset;

            int dust = Dust.NewDust(Projectile.position + positionOffset + baseOffsets(), (int)(Projectile.width), (int)(Projectile.height), 91, 0, 0);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Vector2.Zero;
        }
    }
}
