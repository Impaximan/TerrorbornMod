using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.TBUtils;


namespace TerrorbornMod.TBUtils
{
    public static class Graphics
    {

        public static void DrawGlow_1(SpriteBatch spriteBatch, Rectangle originalRect, int extraSize, Color color)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Texture2D texture = ModContent.GetTexture("TerrorbornMod/Effects/Textures/Glow_1");

            Rectangle rect = originalRect;
            rect.X -= extraSize;
            rect.Y -= extraSize;
            rect.Width += extraSize * 2;
            rect.Height += extraSize * 2;

            spriteBatch.Draw(texture, rect, new Rectangle(0, 0, texture.Width, texture.Height), color, 0f, texture.Size() / 2, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
        }

        public static void DrawGlow_1(SpriteBatch spriteBatch, Vector2 position, float scale, Color color)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Texture2D texture = ModContent.GetTexture("TerrorbornMod/Effects/Textures/Glow_1");
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color, 0f, texture.Size() / 2, scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
        }

        public static void DrawGlow_1(SpriteBatch spriteBatch, Vector2 position, int size, Color color)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Texture2D texture = ModContent.GetTexture("TerrorbornMod/Effects/Textures/Glow_1");
            float scale = (float)size / texture.Width;
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color, 0f, texture.Size() / 2, scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
        }

        public static void DrawGlow(SpriteBatch spriteBatch, Texture2D texture, Rectangle originalRect, int extraSize, Color color)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            Rectangle rect = originalRect;
            rect.X -= extraSize;
            rect.Y -= extraSize;
            rect.Width += extraSize * 2;
            rect.Height += extraSize * 2;

            spriteBatch.Draw(texture, rect, new Rectangle(0, 0, texture.Width, texture.Height), color, 0f, texture.Size() / 2, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
        }

        public static void DrawGlow(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, float scale, Color color)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color, 0f, texture.Size() / 2, scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
        }

        public static void DrawGlow(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, int size, Color color)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            float scale = (float)size / texture.Width;
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color, 0f, texture.Size() / 2, scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
        }
    }
}
