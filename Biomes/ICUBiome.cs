using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Biomes
{
    class ICUBiome : ModBiome
    {
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/ICU");

        public override bool IsBiomeActive(Player player)
        {
            return player.Distance(TerrorbornSystem.IIShrinePosition * 16 + new Vector2(0f, 140f) * 8) <= 1200 && player.behindBackWall;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override void SpecialVisuals(Player player, bool isActive)
        {
            player.ManageSpecialBiomeVisuals("TerrorbornMod:BlandnessShader", isActive && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.InfectedIncarnate.InfectedIncarnate>()));
            player.ManageSpecialBiomeVisuals("TerrorbornMod:DarknessShader", isActive);
        }

        public override void OnEnter(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ZoneICU = true;
        }

        public override void OnLeave(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ZoneICU = false;
            player.ManageSpecialBiomeVisuals("TerrorbornMod:BlandnessShader", false);
            player.ManageSpecialBiomeVisuals("TerrorbornMod:DarknessShader", false);
        }
    }
}
