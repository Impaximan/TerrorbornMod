using Microsoft.Xna.Framework;
using Terraria;
using TerrorbornMod.ForegroundObjects;
using Terraria.ModLoader;

namespace TerrorbornMod.Biomes
{
    class IncendiaryBiome : ModBiome
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/IncendiaryIslands");

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Sisyphean Islands");
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

        int effectCounter = 0;
        public override void OnInBiome(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ZoneIncendiary = true;
        }

        public override void SpecialVisuals(Player player, bool isActive)
        {
            if (!isActive)
            {
                return;
            }
            effectCounter--;
            if (effectCounter <= 0)
            {
                effectCounter = Main.rand.Next(3, 6);
                int maxDistance = 1500;
                ForegroundObject.NewForegroundObject(new Vector2(Main.rand.Next(-maxDistance, maxDistance), Main.rand.Next(-maxDistance, maxDistance)), new IncendiaryFog());
            }
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

        public override void OnEnter(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ZoneIncendiary = true;
        }

        public override void OnLeave(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ZoneIncendiary = false;
        }
    }
}
