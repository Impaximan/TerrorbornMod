using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace TerrorbornMod.Rarities
{
    class Twilight : ModRarity
    {
        public static float rarityColorProgress = 0f;
        public static int rarityColorDirection = 1;
        int rarityColorTransitionTime = 60;
        double timeSoFar = 0;
        public override Color RarityColor
        {
            get
            {
                timeSoFar++;
                rarityColorTransitionTime = (int)(60 + Math.Sin(timeSoFar / 180f) * 30f);

                rarityColorProgress += (1f / rarityColorTransitionTime) * rarityColorDirection;

                if (rarityColorDirection == 1 && rarityColorProgress >= 1f)
                {
                    rarityColorDirection *= -1;
                }

                if (rarityColorDirection == -1 && rarityColorProgress <= 0f)
                {
                    rarityColorDirection *= -1;
                }

                return Color.Lerp(Color.Cyan, Color.MediumPurple, rarityColorProgress);
            }
        }
    }
}

