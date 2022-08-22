using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace TerrorbornMod.Utils
{
    public static class Graphics
    {

        public static void DrawGlow_1(SpriteBatch spriteBatch, Rectangle originalRect, int extraSize, Color color)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Effects/Textures/Glow_1");

            Rectangle rect = originalRect;
            rect.X -= extraSize;
            rect.Y -= extraSize;
            rect.Width += extraSize * 2;
            rect.Height += extraSize * 2;

            spriteBatch.Draw(texture, rect, new Rectangle(0, 0, texture.Width, texture.Height), color, 0f, texture.Size() / 2, SpriteEffects.None, 0f);
        }

        public static void DrawGlow_1(SpriteBatch spriteBatch, Vector2 position, float scale, Color color)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Effects/Textures/Glow_1");
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color, 0f, texture.Size() / 2, scale, SpriteEffects.None, 0f);
        }

        public static void DrawGlow_1(SpriteBatch spriteBatch, Vector2 position, int size, Color color)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Effects/Textures/Glow_1");
            float scale = (float)size / texture.Width;
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color, 0f, texture.Size() / 2, scale, SpriteEffects.None, 0f);
        }

        public static void DrawGlow(SpriteBatch spriteBatch, Texture2D texture, Rectangle originalRect, int extraSize, Color color)
        {
            Rectangle rect = originalRect;
            rect.X -= extraSize;
            rect.Y -= extraSize;
            rect.Width += extraSize * 2;
            rect.Height += extraSize * 2;

            spriteBatch.Draw(texture, rect, new Rectangle(0, 0, texture.Width, texture.Height), color, 0f, texture.Size() / 2, SpriteEffects.None, 0f);
        }

        public static void DrawGlow(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, float scale, Color color)
        {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color, 0f, texture.Size() / 2, scale, SpriteEffects.None, 0f);
        }

        public static void DrawGlow(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, int size, Color color)
        {
            float scale = (float)size / texture.Width;
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color, 0f, texture.Size() / 2, scale, SpriteEffects.None, 0f);
        }
    }
}
