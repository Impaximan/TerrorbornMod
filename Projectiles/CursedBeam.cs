using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Projectiles
{
    public class CursedBeam : ModProjectile
    {
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 3);
        }
        //private bool HasGravity = true;
        private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 120 / 3;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }

        bool start = true;
        Vector2 targetPosition;
        public override void AI()
        {
            if (start)
            {
                start = false;
                targetPosition = Main.player[Main.myPlayer].Center;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * 0.80f * Main.essScale);

            //int dust = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 74);
            //Main.dust[dust].velocity = Projectile.velocity;

            Dust.NewDustPerfect(Projectile.Center, 74, Vector2.Zero);

            if (Projectile.ai[0] == 1)
            {
                float speed = Projectile.velocity.Length() + 0.15f;
                Projectile.velocity.Normalize();
                Projectile.velocity *= speed;
                if (Projectile.timeLeft > 90)
                {
                    Projectile.timeLeft = 90;
                }
            }
            if (Projectile.ai[0] == 2)
            {
                if (Projectile.Distance(targetPosition) <= Projectile.velocity.Length() + 2)
                {
                    Projectile.timeLeft = 1;
                }
            }

            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frameCounter = 5;
                Projectile.frame++;
                if (Projectile.frame >= 3)
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 0, 12, 7, 74, 2f, true);
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }
    }
}
