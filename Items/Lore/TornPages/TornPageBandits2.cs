using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Lore.TornPages
{
    class TornPageBandits2 : ModItem
    {
        public override string Texture => "TerrorbornMod/TornPage";

        public override void SetStaticDefaults()
        {
            TerrorbornMod.GoldenChestLore.Add(Item.type);
            DisplayName.SetDefault("Torn Page - Skeleton Bandits 2");
            Tooltip.SetDefault(TerrorbornUtils.AutoSortTooltip("Undead Culture Study by Alexander Morbus || Day 27, the Skeleton Bandits\nThese guys are like if you gave an immortal crack. Honestly, I'm surprised that's not literally exactly what they are. Essentially, they're undead who retained their intelligence even after revival, who over time banded together and decided to just do whatever they want. This carefree, lawless lifestyle must be incredibly fun for them, but unfortunately, it happens to be at the expense of everyone else. They find immense joy, for some reason, in actively going out of their way to participate in violence and robbery. Since they're not strong enough to really carry anything, they like to hide whatever they rob in golden chests within shacks they build for storage underground. Things like roller skates, boots, boomerangs, even nunchucks- you name it. Their greed and violence knows no limits. Unfortunately for them, the ones who actually manage to get themselves killed get EXTRA punishment in hell from the angels, because of how sinful they were in their second chance at life. If you find this in a golden chest, know that all future editions of Undead Culture Study have been canceled for the following reasons:" +
                "\nReason #1: I'm dead."));
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.rare = -11;
        }
    }
}