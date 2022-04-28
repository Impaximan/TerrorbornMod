using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Biomes
{
    class DeimostoneBiome : ModBiome
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CreepyCaverns");

        public override bool IsBiomeActive(Player player)
        {
            int checkBoxSize = 50;
            int deimostones = 0;
            int deimostonesRequired = 50;
            for (int i = -checkBoxSize + player.Center.ToTileCoordinates16().X; i <= checkBoxSize + player.Center.ToTileCoordinates16().X; i++)
            {
                if (i > 0 && i < Main.maxTilesX)
                {
                    for (int j = -checkBoxSize + player.Center.ToTileCoordinates16().Y; j <= checkBoxSize + player.Center.ToTileCoordinates16().Y; j++)
                    {
                        if (j > 0 && j < Main.maxTilesY)
                        {
                            if (Main.tile[i, j].TileType == ModContent.TileType<Tiles.Deimostone>())
                            {
                                deimostones++;
                                if (deimostones >= deimostonesRequired)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return deimostones >= deimostonesRequired;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override void SpecialVisuals(Player player)
        {
            player.ManageSpecialBiomeVisuals("TerrorbornMod:DarknessShader", true);
        }

        public override void OnEnter(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ZoneDeimostone = true;
        }

        public override void OnLeave(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ZoneDeimostone = false;
            player.ManageSpecialBiomeVisuals("TerrorbornMod:DarknessShader", false);
        }
    }
}
