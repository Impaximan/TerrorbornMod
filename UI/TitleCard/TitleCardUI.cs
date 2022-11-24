using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace TerrorbornMod.UI.TitleCard
{
    class TitleCardUI : UIState
    {
        public static float titleCardAlpha = 0f;
        public static int titleCardLifetimeCounter = 0;
        public static string bossName = "";
        public static string bossSubtitle = "";
        public static Color titleColor = Color.White;

        int fadeInTime = 30;
        int fadeOutTime = 90;


        public void UpdateTitleCardUI()
        {
            if (titleCardAlpha == 0f && titleCardLifetimeCounter <= 0)
            {

            }
            if (titleCardLifetimeCounter > 0 && titleCardAlpha < 1f)
            {
                titleCardAlpha += 1f / fadeInTime;
            }
            if (titleCardLifetimeCounter > 0 && titleCardAlpha >= 1f)
            {
                titleCardLifetimeCounter--;
            }
            if (titleCardLifetimeCounter <= 0 && titleCardAlpha > 0f)
            {
                titleCardAlpha -= 1f / fadeOutTime;
            }
        }

        public override void Update(GameTime gameTime)
        {
            UpdateTitleCardUI();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!TerrorbornMod.titleCards || titleCardAlpha == 0f)
            {
                return;
            }

            DynamicSpriteFont font = FontAssets.DeathText.Value;

            Vector2 textPosition1;
            textPosition1.X = Main.screenWidth * 0.5f * Main.UIScale;
            textPosition1.Y = Main.screenHeight * 0.25f * Main.UIScale;
            string text1 = bossSubtitle;
            Color color1 = new Color(209, 233, 246);

            Vector2 textPosition2;
            textPosition2.X = Main.screenWidth * 0.5f * Main.UIScale;
            textPosition2.Y = Main.screenHeight * 0.25f * Main.UIScale;
            string text2 = bossName;
            Color color2 = titleColor;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend);

            float scale;

            scale = 0.65f * Main.UIScale;
            textPosition1.X -= font.MeasureString(text1).X * scale / 2f;
            textPosition1.Y -= (font.MeasureString(text1).Y * scale) + 10 * scale;
            spriteBatch.DrawString(font, text1, textPosition1, color1 * titleCardAlpha, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);

            scale = 0.8f * Main.UIScale;
            textPosition2.X -= font.MeasureString(text2).X * scale / 2f;
            spriteBatch.DrawString(font, text2, textPosition2, color2 * titleCardAlpha, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
        }
    }
}
