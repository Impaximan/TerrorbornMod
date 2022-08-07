using Microsoft.Xna.Framework;
using Terraria.ID;
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

        }
    }
}