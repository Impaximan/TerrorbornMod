using Terraria;
using TerrorbornMod.Dreadwind;
using Terraria.ModLoader;

namespace TerrorbornMod.Biomes
{
    class DreadwindEffect : ModSceneEffect
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/FallOfTheArchangel1");

        public override SceneEffectPriority Priority => SceneEffectPriority.BossMedium;

        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("TerrorbornMod:DarknessShader", isActive);
        }

        public override bool IsSceneEffectActive(Player player)
        {
            return DreadwindSystem.DreadwindActive;
        }
    }
}