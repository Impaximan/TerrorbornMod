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
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.hostile = false;
            projectile.hide = false;
            projectile.timeLeft = 110;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (moveCounter <= 0)
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
                    List<Vector2> positions = bezier.GetPoints(50);
                    for (int i = 0; i < positions.Count; i++)
                    {
                        float mult = (float)(positions.Count - i) / (float)positions.Count;
                        Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
                        Color color = projectile.GetAlpha(Color.Lerp(Color.Crimson, Color.Red, mult)) * mult;
                        TBUtils.Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(15f * mult), color);
                    }
                }

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            }
            return false;
        }

        int moveCounter = 10;
        public override void AI()
        {
            if (projectile.ai[0] == 1)
            {
                moveCounter = 0;
            }
            if (moveCounter > 0)
            {
                moveCounter--;
                projectile.position -= projectile.velocity;
            }
            else
            {
                //int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
                //Main.dust[dust].noGravity = true;
                //Main.dust[dust].velocity = projectile.velocity;
                projectile.velocity.Y += 0.2f;
                Lighting.AddLight(projectile.Center, 0.5f, 0, 0);
            }
        }
    }
}
