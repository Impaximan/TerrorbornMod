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
    abstract class ForegroundObject
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
            TerrorbornMod.foregroundObjects[whoAmI].type = null;
            TerrorbornMod.foregroundObjects[whoAmI] = null;
        }

        public virtual void OnKill()
        {

        }

        public static int NewForegroundObject(Vector2 position, ForegroundObject type)
        {
            int index = 0;
            for (int i = 0; i < TerrorbornMod.foregroundObjectsCount; i++)
            {
                if (TerrorbornMod.foregroundObjects[i] == null)
                {
                    index = i;
                    break;
                }
            }
            TerrorbornMod.foregroundObjects[index] = type;
            TerrorbornMod.foregroundObjects[index].type = type;
            TerrorbornMod.foregroundObjects[index].position = position;
            TerrorbornMod.foregroundObjects[index].whoAmI = index;
            TerrorbornMod.foregroundObjects[index].SetDefaults();
            TerrorbornMod.foregroundObjects[index].texture = ModContent.GetTexture(TerrorbornMod.foregroundObjects[index].textures[Main.rand.Next(TerrorbornMod.foregroundObjects[index].textures.Count)]);
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
