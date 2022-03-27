using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Lore.JournalEntries.Rath
{
    class Rath_TidalTitan : ModItem
    {
        public override string Texture => "TerrorbornMod/TornPage";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Torn Page");
            Tooltip.SetDefault(TerrorbornUtils.AutoSortTooltip("Magnificent are the incarnates of the sea! A newly discovered species- the azuredire- is comprised entirely of incarnates. And yet, they don't seem to have the intelligence to properly utilize their exclusively abundant terror abilities. If we were to capture and train them, they could be used as mounts for military purposes, each with powerful terror spells. I proposed this idea to the king earlier today, but he didn't seem fond of it. Can't say I entirely blame him, after all, we are currently at peace with Orume. I feel that if we were to start militarizing in this way, Orume would attack us out of fear. Nonetheless, it is an interesting idea to explore."));
        }
        public override void SetDefaults()
        {
            item.value = 0;
            item.rare = -11;
        }
    }
}