using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TerrorbornMod.Dreadwind.NPCs;

namespace TerrorbornMod.Dreadwind.Waves
{
    internal class WaveOfNight : DreadwindWave
    {
        public override Color WaveColor => Color.Purple;

        public override string WaveName => "Hesperus";

        public override void InitializeWave(Player player)
        {
            requiredKills.Add(ModContent.NPCType<Hesperus>());
        }
    }
}