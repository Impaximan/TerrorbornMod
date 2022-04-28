using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Projectiles
{
    class VeinBurst : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.hostile = false;
            Projectile.hide = false;
            Projectile.timeLeft = 110;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (moveCounter <= 0)
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
                        Color color = Projectile.GetAlpha(Color.Lerp(Color.Crimson, Color.Red, mult)) * mult;
                        TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(15f * mult), color);
                    }
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            }
            return false;
        }

        int moveCounter = 10;
        public override void AI()
        {
            if (Projectile.ai[0] == 1)
            {
                moveCounter = 0;
            }
            if (moveCounter > 0)
            {
                moveCounter--;
                Projectile.position -= Projectile.velocity;
            }
            else
            {
                //int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
                //Main.dust[dust].noGravity = true;
                //Main.dust[dust].velocity = Projectile.velocity;
                Projectile.velocity.Y += 0.2f;
                Lighting.AddLight(Projectile.Center, 0.5f, 0, 0);
            }
        }
    }
}
