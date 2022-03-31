using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Terraria.UI;
using TerrorbornMod;
using Terraria.Map;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using System.Runtime.InteropServices;
using ReLogic.Graphics;

namespace TerrorbornMod.UI.TwilightEmpowerment
{
    class TwilightEmpowermentUI : UIState
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (!TerrorbornWorld.TwilightMode || Main.npcChatText != "" || modPlayer.ShowTerrorAbilityMenu)
            {
                return;
            }

            Vector2 position;
            position.X = (int)(Main.screenWidth * TerrorbornMod.TwilightMeterX);
            position.Y = (int)(Main.screenHeight * TerrorbornMod.TwilightMeterY);

            if (modPlayer.InTwilightOverload)
            {
                position += new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)) * Main.UIScale;
            }

            Texture2D texture = ModContent.GetTexture("TerrorbornMod/UI/TwilightEmpowerment/TwilightMeterBar");
            spriteBatch.Draw(texture, position - new Vector2(texture.Width / 2, texture.Height / 2) * Main.UIScale, new Rectangle(0, 0, (int)(MathHelper.Lerp(28f, texture.Width - 28f, modPlayer.TwilightPower)), texture.Height), Color.White, 0f, Vector2.Zero, Main.UIScale, SpriteEffects.None, 0f);

            texture = ModContent.GetTexture("TerrorbornMod/UI/TwilightEmpowerment/TwilightMeterEmpty");
            spriteBatch.Draw(texture, position - new Vector2(texture.Width / 2, texture.Height / 2) * Main.UIScale, texture.Bounds, Color.White, 0f, Vector2.Zero, Main.UIScale, SpriteEffects.None, 0f);
        }
    }
}
