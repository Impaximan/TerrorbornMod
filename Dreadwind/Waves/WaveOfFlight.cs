using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using TerrorbornMod.Dreadwind.NPCs;

namespace TerrorbornMod.Dreadwind.Waves
{
    internal class WaveOfFlight : DreadwindWave
    {
        public override Color WaveColor => Color.Cyan;

        public override string WaveName => "Synnefo";

        public override void InitializeWave(Player player)
        {
            SpawnEnemy(ModContent.NPCType<Synneffo>(), player.Center + new Vector2(1500, 0));
            SpawnEnemy(ModContent.NPCType<Synneffo>(), player.Center + new Vector2(-1500, 0));
            SpawnEnemy(ModContent.NPCType<Synneffo>(), player.Center + new Vector2(0, -1500));
        }
    }
}