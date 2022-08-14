using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TerrorbornMod.UI.BossDefeated
{
    class BossDefeatedUI : UIState
    {
        public static float defeatMessageAlpha = 0f;
        public static int defeatMessageLifetimeCounter = 0;
        public static int slainMessageCounter = 0;
        public static float slainScale = 1f;
        public static float slainWhiteness = 0f;
        public static string deathTitle = "";
        public static Color titleColor = Color.White;

        int fadeInTime = 30;
        int fadeOutTime = 90;

        public override void Update(GameTime gameTime)
        {
            if (defeatMessageAlpha == 0f && defeatMessageLifetimeCounter <= 0)
            {

            }
            if (defeatMessageLifetimeCounter > 0 && defeatMessageAlpha < 1f)
            {
                defeatMessageAlpha += 1f / fadeInTime;
            }
            if (defeatMessageLifetimeCounter > 0 && defeatMessageAlpha >= 1f)
            {
                defeatMessageLifetimeCounter--;
            }
            if (defeatMessageLifetimeCounter <= 0 && defeatMessageAlpha > 0f)
            {
                defeatMessageAlpha -= 1f / fadeOutTime;
            }
            if (slainScale > 1f)
            {
                slainScale -= 0.05f;
                if (slainScale <= 1f)
                {
                    TerrorbornSystem.ScreenShake(10f);
                    slainScale = 1f;
                    slainWhiteness = 1f;
                    SoundEffect soundEffect = ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/BossDefeatedSlam").Value;
                    soundEffect.Play(Main.soundVolume, 0f, 0f);
                }
            }
            if (slainWhiteness > 0f)
            {
                slainWhiteness -= 0.05f;
                if (slainWhiteness < 0f)
                {
                    slainWhiteness = 0f;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!TerrorbornMod.defeatMessages || defeatMessageAlpha == 0f)
            {
                return;
            }

            DynamicSpriteFont font = FontAssets.DeathText.Value;

            Vector2 textPosition1;
            textPosition1.X = Main.screenWidth * 0.5f * Main.UIScale;
            textPosition1.Y = Main.screenHeight * 0.25f * Main.UIScale;
            string text1 = deathTitle + "...";
            Color color1 = new Color(209, 233, 246);

            Vector2 textPosition2;
            textPosition2.X = Main.screenWidth * 0.5f * Main.UIScale;
            textPosition2.Y = Main.screenHeight * 0.25f * Main.UIScale;
            string text2 = "SLAIN.";
            Color color2 = titleColor;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend);

            float scale;

            scale = 0.65f * Main.UIScale;
            textPosition1.X -= font.MeasureString(text1).X * scale / 2f;
            textPosition1.Y -= (font.MeasureString(text1).Y * scale) + 10 * scale;
            spriteBatch.DrawString(font, text1, textPosition1, color1 * defeatMessageAlpha, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);


            if (slainMessageCounter > 0)
            {
                slainMessageCounter--;
                if (slainMessageCounter <= 0)
                {
                    slainScale = 1.5f;
                }
            }
            else
            {
                scale = 0.8f * Main.UIScale * slainScale;
                textPosition2.X -= font.MeasureString(text2).X * scale / 2f;
                spriteBatch.DrawString(font, text2, textPosition2, Color.Lerp(color2, Color.White, slainWhiteness) * defeatMessageAlpha, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
            }
        }
    }
}
