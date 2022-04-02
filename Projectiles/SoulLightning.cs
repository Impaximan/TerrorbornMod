using Terraria.ModLoader;
using System.Collections.Generic;
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
            return target.whoAmI == (int)Projectile.ai[0];
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.extraUpdates = 4;
            Projectile.timeLeft = 1000;
            Projectile.penetrate = 1;
            Projectile.hide = false;
            Projectile.ignoreWater = true;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
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
                List<Vector2> positions = bezier.GetPoints(75);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.MediumPurple, Color.LightPink, mult)) * mult;
                    TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(25f * mult), color);
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
            //    int dust = Dust.NewDust(Projectile.position - (Projectile.velocity * i / 4), 1, 1, 62, 0, 0, Scale: 2, newColor: Color.White);
            //    Main.dust[dust].noGravity = true;

            //    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
            //    float speed = Main.rand.NextFloat(1.5f, 3f);

            //    Main.dust[dust].velocity = direction * speed;
            //}

            NPC target = Main.npc[(int)Projectile.ai[0]];
            if (target.active)
            {
                Vector2 direction = Projectile.DirectionTo(target.Center);
                float speed = 2f;
                Projectile.velocity += speed * direction;

                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(target.Center).ToRotation(), MathHelper.ToRadians(0.75f * (Projectile.velocity.Length() / 20))).ToRotationVector2() * Projectile.velocity.Length();
                Projectile.velocity *= 0.96f;
            }
            else
            {
                Projectile.active = false;
            }
        }
    }
}
