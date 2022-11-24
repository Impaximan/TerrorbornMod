using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Rarities
{
    class Golden : ModRarity
    {
        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            if (offset == -1)
            {
                return ItemRarityID.Purple;
            }
            if (offset >= 1)
            {
                return ModContent.RarityType<Monsoon>();
            }
            return base.GetPrefixedRarity(offset, valueMult);
        }

        public static float rarityColorProgress = 0f;
        public static int rarityColorDirection = 1;
        int rarityColorTransitionTime = 30;
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

                return Color.Lerp(Color.Goldenrod, Color.LightGoldenrodYellow, rarityColorProgress);
            }
        }
    }
}
