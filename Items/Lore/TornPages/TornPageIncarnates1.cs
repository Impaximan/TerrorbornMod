using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Lore.TornPages
{
    class TornPageIncarnates1 : ModItem
    {
        public override string Texture => "TerrorbornMod/TornPage";

        public override void SetStaticDefaults()
        {
            TerrorbornMod.GoldenChestLore.Add(item.type);
            DisplayName.SetDefault("Torn Page - Incarnates 1");
            Tooltip.SetDefault(TerrorbornUtils.AutoSortTooltip("Terror studies textbook for students || Day 3: Incarnates" +
                "\nUp until here in this class, you've heard only about the basics of terror and its purpose within our souls. Now that you understand that, it's time to describe something that takes this to a new level. When any soul is created, as you know, it has varying attunement with its terror. Some of us are hardly able to interact with our terror, and thus struggle to express emotion, whereas others are more attuned. An incarnate is what happens when a soul's attunement with its terror is exponentially greater than everybody else. At such a level of attunement, an incarnate can even manipulate and control the terror of themselves and others. This means that they can cast terror spells, which have a greater capability of altering the world than normal mana spells, which are used primarily for combat. They also have heightened instincts, and, possibly, a form of futuresight, which allows them to learn their oponent's tactics before even fighting them. That, combined with their spells, makes them incredibly valuable in combat, though they rarely show off such prowess due to our current state of peace with Orume. It is also important to note that one cannot artificially become an incarnate, though many have tried, resulting in insanity and large pockets of deimostone. Next class in chapter 6, we will further study upon the nature of incarnates by exploring the concept of a pure incarnate. Now that you have finished reading this, talk with professor Rath and he'll give you your asignment for the day."));
        }

        public override void SetDefaults()
        {
            item.value = 0;
            item.rare = -11;
        }
    }
}