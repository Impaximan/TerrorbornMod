using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Projectiles
{
    class SoulLightning : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly; } }

        public override bool? CanHitNPC(NPC target)
        {
            return target.whoAmI == (int)projectile.ai[0];
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.extraUpdates = 4;
            projectile.timeLeft = 1000;
            projectile.penetrate = 1;
            projectile.hide = false;
            projectile.ignoreWater = true;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
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
                List<Vector2> positions = bezier.GetPoints(75);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
                    Color color = projectile.GetAlpha(Color.Lerp(Color.MediumPurple, Color.LightPink, mult)) * mult;
                    TBUtils.Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(25f * mult), color);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void AI()
        {
            //for (int i = 0; i < 4; i++)
            //{
            //    int dust = Dust.NewDust(projectile.position - (projectile.velocity * i / 4), 1, 1, 62, 0, 0, Scale: 2, newColor: Color.White);
            //    Main.dust[dust].noGravity = true;

            //    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
            //    float speed = Main.rand.NextFloat(1.5f, 3f);

            //    Main.dust[dust].velocity = direction * speed;
            //}

            NPC target = Main.npc[(int)projectile.ai[0]];
            if (target.active)
            {
                Vector2 direction = projectile.DirectionTo(target.Center);
                float speed = 2f;
                projectile.velocity += speed * direction;

                projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(target.Center).ToRotation(), MathHelper.ToRadians(0.75f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
                projectile.velocity *= 0.96f;
            }
            else
            {
                projectile.active = false;
            }
        }
    }
}
