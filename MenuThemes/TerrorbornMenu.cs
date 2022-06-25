using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using System;

namespace TerrorbornMod.MenuThemes
{
    class TerrorbornMenu : ModMenu
    {
        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("TerrorbornMod/MenuLogo");

        public override int Music => MusicLoader.GetMusicSlot("TerrorbornMod/Sounds/Music/FallOfTheArchangel1");

        public override string DisplayName => "Terrorborn : Fall of the Archangel";

        public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<TerrorbornMenuSurfaceBGStyle>();
    }

    class TerrorbornMenuSurfaceBGStyle : ModSurfaceBackgroundStyle
    {
        public override void ModifyFarFades(float[] fades, float transitionSpeed)
        {

        }


        List<MenuSpark> sparks = new List<MenuSpark>();
        float timeUp = 0f;
        public override bool PreDrawCloseBackground(SpriteBatch spriteBatch)
        {
            timeUp++;
            Texture2D texture = ModContent.Request<Texture2D>("TerrorbornMod/MainMenuForeground1").Value;
            spriteBatch.Draw(texture, new Vector2(Main.screenWidth * 0.5f, Main.screenHeight), null, Color.White, 0f, new Vector2(1000, 2000), new Vector2((float)Main.screenWidth / (float)texture.Width, (float)Main.screenHeight / (float)texture.Height * (1.4f + (float)Math.Sin(timeUp / 45f) / 4f)), SpriteEffects.None, 0f);
            Main.dayTime = true;
            Main.time = Main.dayLength * 0.5f;
            float sparkScale = 10f;
            if (sparks.Count == 0)
            {
                for (float i = 0; i < 1f; i += 1f / ((Main.screenWidth + Main.screenHeight) / 160f))
                {
                    sparks.Add(new MenuSpark(new Vector2(Main.rand.Next(Main.screenWidth), Main.rand.Next(Main.screenHeight)), new Vector2(Main.screenWidth / 60, Main.rand.Next(Main.screenHeight / -200, Main.screenHeight / 200)), i));
                }
            }
            for (int i = sparks.Count; i >= 0; i--)
            {
                if (i < sparks.Count && sparks[i] != null)
                {
                    MenuSpark spark = sparks[i];
                    spark.position += spark.velocity;
                    if (spark.position.X > Main.screenWidth + sparkScale)
                    {
                        spark.position.X = 0;
                        spark.position.Y = Main.rand.NextFloat(Main.screenHeight * 1f);
                    }
                    sparks[i] = spark;
                }
            }
            foreach (MenuSpark spark in sparks)
            {
                spriteBatch.Draw(ModContent.Request<Texture2D>("TerrorbornMod/WhitePixel").Value, spark.position, null, spark.color, 0f, new Vector2(0.5f, 0.5f), sparkScale * spark.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
    }

    public class MenuSpark
    {
        public Vector2 position = Vector2.Zero;
        public Vector2 velocity = Vector2.Zero;
        public float scale = 1f;
        public Color color = Color.White;

        public MenuSpark(Vector2 position, Vector2 velocity, float iProgress)
        {
            float closeness = MathHelper.Lerp(0.6f, 1.4f, iProgress);
            this.position = position;
            this.velocity = velocity * closeness;
            scale = closeness;

            color = Color.White;
            switch (Main.rand.Next(3))
            {
                default:
                    color = Color.White;
                    break;
                case 0:
                    color = Color.LightGoldenrodYellow;
                    break;
                case 1:
                    color = Color.Yellow;
                    break;
                case 2:
                    color = Color.SlateGray;
                    break;
            }
            color *= Main.rand.NextFloat(0.5f, 1f);
        }
    }
}
