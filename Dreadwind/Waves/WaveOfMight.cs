using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TerrorbornMod.Dreadwind.NPCs;

namespace TerrorbornMod.Dreadwind.Waves
{
    internal class WaveOfMight : DreadwindWave
    {
        public override Color WaveColor => Color.RoyalBlue;

        public override string WaveName => "Damnation Strider";

        public override void InitializeWave(Player player)
        {
            SpawnEnemy(ModContent.NPCType<DamnationStrider>(), player.Center + new Vector2(0, 1000));
        }
    }
}