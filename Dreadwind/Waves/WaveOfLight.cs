using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using TerrorbornMod.Dreadwind.NPCs;

namespace TerrorbornMod.Dreadwind.Waves
{
    internal class WaveOfLight : DreadwindWave
    {
        public override Color WaveColor => Color.HotPink;

        public override string WaveName => "Phosphorus";

        public override void InitializeWave(Player player)
        {
            int currentEnemy = SpawnEnemy(ModContent.NPCType<Phosphorus>(), player.Center - new Vector2(0, 2000));
            Main.npc[currentEnemy].ai[0] = -1;
            Main.npc[currentEnemy].ai[1] = -1;

            currentEnemy = SpawnEnemy(ModContent.NPCType<Phosphorus>(), player.Center - new Vector2(0, 2000));
            Main.npc[currentEnemy].ai[0] = 1;
            Main.npc[currentEnemy].ai[1] = -1;

            currentEnemy = SpawnEnemy(ModContent.NPCType<Phosphorus>(), player.Center - new Vector2(0, 2000));
            Main.npc[currentEnemy].ai[0] = -1;
            Main.npc[currentEnemy].ai[1] = 1;

            currentEnemy = SpawnEnemy(ModContent.NPCType<Phosphorus>(), player.Center - new Vector2(0, 2000));
            Main.npc[currentEnemy].ai[0] = 1;
            Main.npc[currentEnemy].ai[1] = 1;
        }
    }
}
