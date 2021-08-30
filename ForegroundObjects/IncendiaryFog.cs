using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TerrorbornMod.Abilities;
using TerrorbornMod.ForegroundObjects;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Input;
using Extensions;


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
            drawColor = Color.Red;
        }

        public int timeLeft;
        public override void AI()
        {
            position.X += Main.windSpeed * 10;
            spriteDirection = -1;
            if (Main.windSpeed > 1)
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
