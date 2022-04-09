using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ModLoader;

namespace TerrorbornMod.Biomes
{
    class IncendiaryBiome : ModBiome
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/IncendiaryIslands");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sisyphean Islands");
        }

        public override string BestiaryIcon => base.BestiaryIcon;
        public override string BackgroundPath => base.BackgroundPath;
        public override Color? BackgroundColor => base.BackgroundColor;

        public override bool IsBiomeActive(Player player)
        {
            Rectangle incendiaryBiomeRect = new Rectangle(0, 0, (int)(Main.maxTilesX / 4f * 16) + 120 * 16, (int)(Main.maxTilesY / 17f * 16) + 120 * 16);
            if (TerrorbornSystem.incendiaryIslandsSide == 1)
            {
                incendiaryBiomeRect = new Rectangle((Main.maxTilesX * 16) - (int)(Main.maxTilesX / 4f * 16) - 120 * 16, 0, (int)(Main.maxTilesX / 4f * 16) + 120 * 16, (int)(Main.maxTilesY / 17f * 16) + 120 * 16);
            }
            return incendiaryBiomeRect.Intersects(player.getRect()) && Main.hardMode;
        }
    }
}
