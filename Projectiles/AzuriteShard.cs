using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Projectiles
{
    class AzuriteShard : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/MagicGuns/TarSwarm";
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            Projectile.timeLeft = 180;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 88, Scale: 1.35f, newColor: Color.SkyBlue);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity / 3;

            Projectile.velocity.Y += 1.5f;
            Projectile.velocity.X *= 0.98f;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.ArmorPenetration += target.defense / 4;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y * 0.9f;
            }
            return false;
        }
    }
}
