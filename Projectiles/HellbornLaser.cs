using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using TerrorbornMod.TBUtils;

namespace TerrorbornMod.Projectiles
{
    class HellbornLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

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
                    Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * mult), color);
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
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}