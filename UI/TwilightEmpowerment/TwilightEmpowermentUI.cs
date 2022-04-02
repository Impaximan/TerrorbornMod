using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace TerrorbornMod.UI.TwilightEmpowerment
{
    class TwilightEmpowermentUI : UIState
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (!TerrorbornSystem.TwilightMode || Main.npcChatText != "" || modPlayer.ShowTerrorAbilityMenu)
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

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/UI/TwilightEmpowerment/TwilightMeterBar");
            spriteBatch.Draw(texture, position - new Vector2(texture.Width / 2, texture.Height / 2) * Main.UIScale, new Rectangle(0, 0, (int)(MathHelper.Lerp(28f, texture.Width - 28f, modPlayer.TwilightPower)), texture.Height), Color.White, 0f, Vector2.Zero, Main.UIScale, SpriteEffects.None, 0f);

            texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/UI/TwilightEmpowerment/TwilightMeterEmpty");
            spriteBatch.Draw(texture, position - new Vector2(texture.Width / 2, texture.Height / 2) * Main.UIScale, texture.Bounds, Color.White, 0f, Vector2.Zero, Main.UIScale, SpriteEffects.None, 0f);
        }
    }
}
