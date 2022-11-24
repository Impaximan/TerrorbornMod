using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.ForegroundObjects
{
    public abstract class ForegroundObject
    {
        public ForegroundObject type;
        public int whoAmI;
        public List<string> textures = new List<string>();
        public Texture2D texture;
        public Vector2 position;
        public float distance;
        public float scaleMultiplier = 1f;
        public int alpha = 0;
        public float rotation = 0f;
        public int spriteDirection = -1;
        public bool active = true;
        public Color drawColor = Color.White;

        public void Update()
        {
            AI();
            if (!active)
            {
                Kill();
            }
        }

        public virtual bool PreDraw(SpriteBatch spriteBatch)
        {
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float zoom = Main.GameZoomTarget;
            SpriteEffects effects = SpriteEffects.None;
            if (spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }

            Vector2 screenCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            Vector2 screenCenterWorld = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) + Main.screenPosition;
            spriteBatch.Draw(texture, screenCenter + position * zoom * distance, new Rectangle(0, 0, texture.Width, texture.Height), drawColor * (float)(1f - alpha / 255f), rotation, new Vector2(texture.Width / 2, texture.Height / 2), distance * scaleMultiplier * zoom, effects, 0f);
        }

        public virtual void PostDraw(SpriteBatch spriteBatch)
        {

        }

        public void Kill()
        {
            TerrorbornSystem.foregroundObjects[whoAmI].type = null;
            TerrorbornSystem.foregroundObjects[whoAmI] = null;
        }

        public virtual void OnKill()
        {

        }

        public static int NewForegroundObject(Vector2 position, ForegroundObject type)
        {
            int index = 0;
            for (int i = 0; i < TerrorbornSystem.foregroundObjectsCount; i++)
            {
                if (TerrorbornSystem.foregroundObjects[i] == null)
                {
                    index = i;
                    break;
                }
            }
            TerrorbornSystem.foregroundObjects[index] = type;
            TerrorbornSystem.foregroundObjects[index].type = type;
            TerrorbornSystem.foregroundObjects[index].position = position;
            TerrorbornSystem.foregroundObjects[index].whoAmI = index;
            TerrorbornSystem.foregroundObjects[index].SetDefaults();
            TerrorbornSystem.foregroundObjects[index].texture = (Texture2D)ModContent.Request<Texture2D>(TerrorbornSystem.foregroundObjects[index].textures[Main.rand.Next(TerrorbornSystem.foregroundObjects[index].textures.Count)]);
            return index;
        }

        public virtual void SetDefaults()
        {

        }

        public virtual void AI()
        {

        }

        public static Vector2 ConvertToRegularPosition(Vector2 position, float depthMultiplier)
        {
            float zoom = Main.GameZoomTarget;
            Vector2 screenCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            Vector2 screenCenterWorld = screenCenter + Main.screenPosition;
            
            return screenCenterWorld + (position * zoom * depthMultiplier);
        }
    }
}
