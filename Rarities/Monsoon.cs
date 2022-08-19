using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrorbornMod.Rarities
{
    class Monsoon : ModRarity
    {
        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset == -1)
            {
                return ModContent.RarityType<Golden>();
            }
            return base.GetPrefixedRarity(offset, valueMult);
        }

        public static float rarityColorProgress = 0f;
        public static int rarityColorDirection = 1;
        int rarityColorTransitionTime = 180;
        public override Color RarityColor
        {
            get
            {
                rarityColorProgress += (1f / rarityColorTransitionTime) * rarityColorDirection;

                if (rarityColorDirection == 1 && rarityColorProgress >= 1f)
                {
                    rarityColorDirection *= -1;
                }

                if (rarityColorDirection == -1 && rarityColorProgress <= 0f)
                {
                    rarityColorDirection *= -1;
                }

                return Color.Lerp(Color.Turquoise * 0.75f, Color.Red, rarityColorProgress);
            }
        }
    }
}