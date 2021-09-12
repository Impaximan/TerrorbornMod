using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace TerrorbornMod.Projectiles
{
    class IncendiaryDeathray : Deathray
    {
		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 22;
			projectile.penetrate = -1;
			projectile.tileCollide = true;
			projectile.hide = false;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 2;
			MoveDistance = 20f;
			RealMaxDistance = 1000f;
			bodyRect = new Rectangle(0, 24, 18, 22);
			headRect = new Rectangle(0, 0, 18, 22);
			tailRect = new Rectangle(0, 46, 18, 22);
		}

        public override Vector2 Position()
        {
            return Main.npc[(int)projectile.ai[0]].Center + new Vector2(0, projectile.ai[1]);
        }
	}
}
