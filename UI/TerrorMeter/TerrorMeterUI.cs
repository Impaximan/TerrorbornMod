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

namespace TerrorbornMod.UI.TerrorMeter
{
    class TerrorMeterUI : UIState
    {
        public override void OnInitialize()
        {
            shownTerror = 0f;
        }

        public float shownTerror = 0f;
        int frame = 0;
        int frameCounter = 0;
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!TerrorbornWorld.obtainedShriekOfHorror)
            {
                return;
            }

            Player player = Main.player[Main.myPlayer];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            Color meterColor = modPlayer.terrorMeterColor;

            if (TerrorbornMod.TerrorMeterStyle == "Legacy")
            {
                Vector2 position;
                position.X = (int)(Main.screenWidth * Main.UIScale * 0.5f);
                position.Y = (int)(60.05882352941f * Main.UIScale);

                int frameCount = 7;
                int meterFrame = (int)((modPlayer.TerrorPercent / 100) * frameCount);

                if (meterFrame > frameCount)
                {
                    meterFrame = frameCount;
                }

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend);

                Texture2D texture = ModContent.GetTexture("TerrorbornMod/UI/TerrorMeter/Frames/meterpiece_" + meterFrame);
                float scale = Main.UIScale * 1.75f;
                spriteBatch.Draw(texture, position, texture.Bounds, meterColor, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);


                scale = Main.UIScale * 0.65f;
                string meterText = (int)modPlayer.TerrorPercent + "%";
                position.X -= (meterText.Length) * scale * 8;
                position.Y -= scale * 14;

                if (TerrorbornMod.TerrorMeterText) spriteBatch.DrawString(Main.fontDeathText, meterText, position, Color.FromNonPremultiplied(209, 233, 246, 255), 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            else
            {
                Vector2 position;
                position.X = (int)(Main.screenWidth * Main.UIScale * 0.5f);
                position.Y = (int)(60.05882352941f * Main.UIScale);

                frameCounter++;
                if (frameCounter >= 10)
                {
                    frame++;
                    if (frame >= 4)
                    {
                        frame = 0;
                    }
                    frameCounter = 0;
                }

                shownTerror = MathHelper.Lerp(shownTerror, modPlayer.TerrorPercent, 0.1f);
                if (MathHelper.Distance(shownTerror, modPlayer.TerrorPercent) <= 0.1f)
                {
                    shownTerror = modPlayer.TerrorPercent;
                }

                if (TerrorbornMod.TerrorMeterStyle == "Text Only (Instant)")
                {
                    shownTerror = modPlayer.TerrorPercent;
                }

                spriteBatch.End();
                spriteBatch.Begin(default, BlendState.AlphaBlend);

                Texture2D texture = ModContent.GetTexture("TerrorbornMod/UI/TerrorMeter/Frames/MeterEmpty");
                if (TerrorbornMod.TerrorMeterStyle == "Filled In")
                {
                    texture = ModContent.GetTexture("TerrorbornMod/UI/TerrorMeter/Frames/MeterEmptyFilled");
                }

                int width = texture.Width;
                int height = texture.Height;
                float scale = Main.UIScale * 1f;
                if (!TerrorbornMod.TerrorMeterStyle.Contains("Text Only"))
                {
                    spriteBatch.Draw(texture, position - new Vector2(texture.Width / 2, texture.Height / 2) * Main.UIScale, texture.Bounds, meterColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

                    int meterTop = (int)MathHelper.Lerp(0f, (float)texture.Height, 1f - (modPlayer.TerrorPercent / 100f));
                    if (shownTerror > modPlayer.TerrorPercent)
                    {
                        meterTop = (int)MathHelper.Lerp(0f, (float)texture.Height, 1f - (shownTerror / 100f));
                    }
                    texture = ModContent.GetTexture("TerrorbornMod/UI/TerrorMeter/Frames/MeterInside");
                    spriteBatch.Draw(texture, position + new Vector2(0, (int)(meterTop * scale)) - new Vector2(texture.Width / 2, texture.Height / 2) * Main.UIScale, new Rectangle(0, meterTop, texture.Width, texture.Height - meterTop), meterColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

                    meterTop = (int)MathHelper.Lerp(0f, (float)texture.Height, 1f - (shownTerror / 100f));
                    if (shownTerror > modPlayer.TerrorPercent)
                    {
                        meterTop = (int)MathHelper.Lerp(0f, (float)texture.Height, 1f - (modPlayer.TerrorPercent / 100f));
                    }
                    texture = ModContent.GetTexture("TerrorbornMod/UI/TerrorMeter/Frames/MeterFull");
                    spriteBatch.Draw(texture, position + new Vector2(0, (int)(meterTop * scale)) - new Vector2(width / 2, height / 2) * Main.UIScale, new Rectangle(0, meterTop + (height * frame), width, height - meterTop), meterColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }

                scale = Main.UIScale * 0.65f;
                string meterText = (int)shownTerror + "%";
                position.X -= Main.fontDeathText.MeasureString(meterText).X * scale / 2;
                position.Y -= scale * 26f;

                if (TerrorbornMod.TerrorMeterText) spriteBatch.DrawString(Main.fontDeathText, meterText, position, Color.FromNonPremultiplied(209, 233, 246, 255), 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }
    }
}
