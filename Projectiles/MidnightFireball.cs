using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Utils;

namespace TerrorbornMod.Projectiles
{
    public class MidnightFireball : ModProjectile
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 3);
            Projectile.timeLeft = 1;
        }
        //private bool HasGravity = true;
        private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 69;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, 65, Color.LimeGreen * 0.5f);
            return true;
        }

        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
                Projectile.timeLeft = (int)Projectile.ai[0];
            }

            Projectile.rotation += MathHelper.ToRadians(5);
            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * 0.80f * Main.essScale);

            //int dust = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 74);
            //Main.dust[dust].velocity = Projectile.velocity;

            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 74);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].noLight = true;
            Main.dust[dust].velocity = Projectile.velocity;


            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)].Center).ToRotation(), MathHelper.ToRadians(Projectile.ai[1] * (Projectile.velocity.Length() / 20))).ToRotationVector2() * Projectile.velocity.Length();
            }

            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frameCounter = 5;
                Projectile.frame++;
                if (Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 0, 12, 7, 74, 2f, true);
            SoundExtensions.PlaySoundOld(SoundID.Item14, Projectile.Center);
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

