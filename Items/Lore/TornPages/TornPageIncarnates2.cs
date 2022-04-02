using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Lore.TornPages
{
    class TornPageIncarnates2 : ModItem
    {
        public override string Texture => "TerrorbornMod/TornPage";

        public override void SetStaticDefaults()
        {
            TerrorbornMod.GoldenChestLore.Add(Item.type);
            DisplayName.SetDefault("Torn Page - Incarnates 2");
            Tooltip.SetDefault(TerrorbornUtils.AutoSortTooltip("Terror studies textbook for students || Day 4: Pure Incarnates" +
                "\nJust like us, the attunement to terror of an incarnate varies per instance. But unlike us, they can practice and improve their character to become even more attuned. The 'power' of an incarnate is determined by how virtuous they are and how much stamina they have currently. As far as we're aware, nobody is perfect, and thus no incarnate has reached the peak of its potential. However, in the theoretical situation that an incarnate is perfect, they would have nigh infinite power, being able to alter the world however they please. In other words, they would be a deity. Fortunately, no such event has ever been recorded- the effects of such an event could potentially be disastrous. Nonetheless, it as an interesting theoretical concept that helps explain the power of an incarnate. Now that you have finished reading this, talk with professor Rath and he'll give you your asignment for the day."));
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.rare = -11;
        }
    }
}