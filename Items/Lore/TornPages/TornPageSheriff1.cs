using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Lore.TornPages
{
    class TornPageSheriff1 : ModItem
    {
        public override string Texture => "TerrorbornMod/TornPage";

        public override void SetStaticDefaults()
        {
            TerrorbornMod.GoldenChestLore.Add(Item.type);
            // DisplayName.SetDefault("Torn Page - Skeleton Sheriff 1");
            // Tooltip.SetDefault(Utils.General.AutoSortTooltip("Undead Culture Study by Alexander Morbus || Day 18, the Skeleton Sheriff\nMost undead are either brainless, lawless, or incredibly edgy, wandering aimlessly, violently attacking everybody else, or creating a strange orange healing substance at bonfires and selling it to the living. However, there is one I have gotten to meet myself, whose name has slipped my mind (unfortunately), that actively seeks to subdue the chaotic forces of the other undead. This individual, titling himself 'The Skeleton Peacekeeper (or Skeleton Sheriff)', is quite the skilled fighter, miraculously showing off a 1st place medal from an Anekronian tournament, even with his own title etched onto it. Supposedly, he not only fights back the violent undead, but also does environmental control in a variety of biomes. Quite the honorable person, I must say. Makes me wonder how I forgot his name."));
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.rare = -11;
        }
    }
}