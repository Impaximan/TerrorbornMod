using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Lore.TornPages
{
    class TornPageTerror1 : ModItem
    {
        public override string Texture => "TerrorbornMod/TornPage";

        public override void SetStaticDefaults()
        {
            TerrorbornMod.GoldenChestLore.Add(Item.type);
            DisplayName.SetDefault("Torn Page - Terror 1");
            Tooltip.SetDefault(Utils.General.AutoSortTooltip("Terror studies textbook for students || Day 1: What Is Terror?" +
                "\nIf you've signed up for this course, you, evidently, want to learn about terror. And wise you are in that decision, for terror is highly important in all our lives! All of us have some amount of terror in our souls, which gives us our emotions and instincts. It is the very essence of our personalities. The attunement with terror varies per instance of a person, affecting how easily they can understand and express their emotions. Some of you may be surprised to hear that I, Rath, have a relatively low amount of attunement to terror, resulting in me having less capability of describing my emotions to others. This does not make me inferior to anybody, of course, but rather my mind's focus is on other things. We refer to the measurement of terror atunement as TAQ- Terror Attunement Quotient, which is measured in a percentage, 100% being the average TAQ. I have 76% TAQ, but my friend Stiria, who is an incarnate, has 2586% TAQ. Terror as a substance is capable of altering the world around itself, and, with power, one can manipulate it to cast spells. Note that this is not like the spells casted with mana. There will be no assignment today, as it is the first day of class, however, later today, at 5PM, I hope you will be joining me in my office to do some community building activies. After all, it is best that I get to know my students. See you there!"));
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.rare = -11;
        }
    }
}