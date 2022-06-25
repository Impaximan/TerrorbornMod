using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Biomes
{
    class ScreenShaderBiome : ModSceneEffect
    {
        public override bool IsSceneEffectActive(Player player)
        {
            return true;
        }

        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

        public override void SpecialVisuals(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.ManageSpecialBiomeVisuals("TerrorbornMod:PrototypeIShader", NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.PrototypeI.PrototypeI>()));
            player.ManageSpecialBiomeVisuals("TerrorbornMod:BlandnessShader", modPlayer.ZoneICU && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.InfectedIncarnate.InfectedIncarnate>()));
            player.ManageSpecialBiomeVisuals("TerrorbornMod:IncarnateBoss", NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.InfectedIncarnate.InfectedIncarnate>()));
            player.ManageSpecialBiomeVisuals("TerrorbornMod:ColorlessShader", modPlayer.TimeFreezeTime > 0);
            player.ManageSpecialBiomeVisuals("TerrorbornMod:TwilightShaderNight", modPlayer.InTwilightOverload);

            player.ManageSpecialBiomeVisuals("TerrorbornMod:HexedMirage", modPlayer.HexedMirage);
        }
    }
}

