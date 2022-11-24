using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrorbornMod.Biomes
{
    class AstraphobiaEffect : ModSceneEffect
    {
        public override ModWaterStyle WaterStyle => ModContent.Find<ModWaterStyle>("TerrorbornMod/AstraphobiaWater");
        public override int Music => MusicLoader.GetMusicSlot("TerrorbornMod/Sounds/Music/DarkRain");
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override bool IsSceneEffectActive(Player player)
        {
            return TerrorbornSystem.terrorRain && player.ZoneRain;
        }
    }

    class AstraphobiaWater : ModWaterStyle
    {
        public override Asset<Texture2D> GetRainTexture()
        {
            return ModContent.Request<Texture2D>("TerrorbornMod/TerrorRain", AssetRequestMode.ImmediateLoad);
        }

        public override int ChooseWaterfallStyle()
        {
            return 0;
        }

        public override int GetSplashDust()
        {
            return 66;
        }

        public override int GetDropletGore()
        {
            return GoreID.WaterDrip;
        }

        public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
            r = 0.5f;
            g = 0.85f;
            b = 1f;
        }

        public override Color BiomeHairColor()
        {
            return Color.SlateGray;
        }

        public override byte GetRainVariant()
        {
            return (byte)Main.rand.Next(3);
        }
    }
}
