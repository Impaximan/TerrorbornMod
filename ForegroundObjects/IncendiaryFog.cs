using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;


namespace TerrorbornMod.ForegroundObjects
{
    class IncendiaryFog : ForegroundObject
    {
        public override void SetDefaults()
        {
            textures = new List<string>()
            {
                { "TerrorbornMod/ForegroundObjects/Fog/Fog0" },
                { "TerrorbornMod/ForegroundObjects/Fog/Fog1" },
                { "TerrorbornMod/ForegroundObjects/Fog/Fog2" },
                { "TerrorbornMod/ForegroundObjects/Fog/Fog3" },
            };

            distance = Main.rand.NextFloat(1.25f, 1.75f);
            alpha = 255;
            timeLeft = Main.rand.Next(180, 360) * 2;
            drawColor = Color.White;
        }

        public int timeLeft;
        public override void AI()
        {
            position.X += Main.windSpeedCurrent * 10;
            spriteDirection = -1;
            if (Main.windSpeedCurrent > 1)
            {
                spriteDirection = 1;
            }

            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            timeLeft--;
            if (timeLeft <= 0 || !modPlayer.ZoneIncendiary)
            {
                alpha += 2;
                if (alpha > 255)
                {
                    active = false;
                }
            }
            else if (alpha > (int)(190))
            {
                alpha -= 2;
            }
        }
    }
}
