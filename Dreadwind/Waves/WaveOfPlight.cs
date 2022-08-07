using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using TerrorbornMod.Dreadwind.NPCs;

namespace TerrorbornMod.Dreadwind.Waves
{
    internal class WaveOfPlight : DreadwindWave
    {
        public override Color WaveColor => Color.LimeGreen;

        public override string WaveName => "Fallen Ferryman";

        public override void InitializeWave(Player player)
        {

        }
    }
}