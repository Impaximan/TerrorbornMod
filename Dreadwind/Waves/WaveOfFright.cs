using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TerrorbornMod.Dreadwind.NPCs;

namespace TerrorbornMod.Dreadwind.Waves
{
    internal class WaveOfFright : DreadwindWave
    {
        public override Color WaveColor => Color.OrangeRed;

        public override string WaveName => "Locusts";

        public override void InitializeWave(Player player)
        {
            SpawnEnemy(ModContent.NPCType<Locust>(), player.Center + new Vector2(1500, 0));
            SpawnEnemy(ModContent.NPCType<Locust>(), player.Center + new Vector2(-1500, 0));
            SpawnEnemy(ModContent.NPCType<Locust>(), player.Center + new Vector2(0, -1500));

            DreadwindSystem.FrightArenaWidth = DreadwindSystem.FrightArenaMaxWidth;
            DreadwindSystem.FrightArenaX = player.Center.X;
            DreadwindSystem.FrightRaining = true;
        }
    }
}