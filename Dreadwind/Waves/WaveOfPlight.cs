using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TerrorbornMod.Dreadwind.NPCs;

namespace TerrorbornMod.Dreadwind.Waves
{
    internal class WaveOfPlight : DreadwindWave
    {
        public override Color WaveColor => Color.LimeGreen;

        public override string WaveName => "Purgatory Reaper";

        public override void InitializeWave(Player player)
        {
            int npc = SpawnEnemy(ModContent.NPCType<PurgatoryReaper>(), player.Center + new Vector2(Main.screenWidth / 2 + 200, -200));
            Main.npc[npc].ai[0] = 1;
            npc = SpawnEnemy(ModContent.NPCType<PurgatoryReaper>(), player.Center + new Vector2(-(Main.screenWidth / 2 + 200), -200));
            Main.npc[npc].ai[0] = -1;
        }
    }
}