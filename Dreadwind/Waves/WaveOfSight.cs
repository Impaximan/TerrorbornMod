using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TerrorbornMod.Dreadwind.NPCs;

namespace TerrorbornMod.Dreadwind.Waves
{
    internal class WaveOfSight : DreadwindWave
    {
        public override Color WaveColor => Color.LightGreen;

        public override string WaveName => "Oculus";

        public override void InitializeWave(Player player)
        {
            SpawnEnemy(ModContent.NPCType<Oculus>(), player.Center + new Vector2(0, -1000));
            SpawnEnemy(ModContent.NPCType<Oculus>(), player.Center + new Vector2(500, -1000));
            SpawnEnemy(ModContent.NPCType<Oculus>(), player.Center + new Vector2(500, -1000));
            SpawnEnemy(ModContent.NPCType<Oculus>(), player.Center + new Vector2(500, 1000));
            SpawnEnemy(ModContent.NPCType<Oculus>(), player.Center + new Vector2(500, 1000));
        }
    }
}