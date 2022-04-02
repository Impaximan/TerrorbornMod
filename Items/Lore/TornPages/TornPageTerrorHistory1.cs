using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Lore.TornPages
{
    class TornPageTerrorHistory1 : ModItem
    {
        public override string Texture => "TerrorbornMod/TornPage";

        public override void SetStaticDefaults()
        {
            TerrorbornMod.GoldenChestLore.Add(Item.type);
            DisplayName.SetDefault("Torn Page - Terror History 1");
            Tooltip.SetDefault(TerrorbornUtils.AutoSortTooltip("Terror studies textbook for students || Day 2: A Brief History of Terror" +
                "\nDue to the time between now and the original discovery of terror many centuries ago, not much is known about the process and how they lived before then. However, we do have a general idea of how it went. Note that it was actually early humans who discovered terror, not Anekronians. When they did so, due to its erratic movement and dark nature, they immediately saw it as a threat and named it 'terror', because that's what it reminded them of. The connections terror has to our instincts is purely coincedental with the name. Later, when they discovered an alternate form of terror that forms the souls of angels, it was called dread, and thus for simplicity's sake the angels started calling it dread as well so as to not confuse the humans. Anyways, later after they discovered terror, they of course began to study it and its properties, and when they gained the ability to study souls, they realized that everybody has terror, and that it's not something to be afraid of at all. This also allowed them to figure out how incarnates work and explain them- previously, they were worshipped and referred to as gods amongst men. With the discovery of the properties of terror and incarnates, they began using incarnates as leaders of military conflict, and surely enough, this allowed them to surpass all other forms of society, later coming together in the city of Orume. The origin of Anekronians and our relation to this in history is quite the mystery, due to the opaque nature of our great king. Now that you have finished reading this, talk with professor Rath and he'll give you your asignment for the day."));
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.rare = -11;
        }
    }
}