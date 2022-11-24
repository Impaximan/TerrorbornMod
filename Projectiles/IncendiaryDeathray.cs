using Microsoft.Xna.Framework;
using Terraria;

namespace TerrorbornMod.Projectiles
{
    class IncendiaryDeathray : Deathray
    {
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 22;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.hide = false;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 2;
			MoveDistance = 20f;
			RealMaxDistance = 1000f;
			bodyRect = new Rectangle(0, 24, 18, 22);
			headRect = new Rectangle(0, 0, 18, 22);
			tailRect = new Rectangle(0, 46, 18, 22);
		}

        public override Vector2 Position()
        {
            return Main.npc[(int)Projectile.ai[0]].Center + new Vector2(0, Projectile.ai[1]);
        }
	}
}
