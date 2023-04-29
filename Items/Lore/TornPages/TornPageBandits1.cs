using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Lore.TornPages
{
    class TornPageBandits1 : ModItem
    {
        public override string Texture => "TerrorbornMod/TornPage";

        public override void SetStaticDefaults()
        {
            TerrorbornMod.GoldenChestLore.Add(Item.type);
            // DisplayName.SetDefault("Torn Page - Skeleton Bandits 1");
            // Tooltip.SetDefault(Utils.General.AutoSortTooltip("Screw all these bandits! They keep stealing all of our supplies, including food, which they don't even eat. Heck, they just do it for fun. Whenever they show up, I always want to fight back or say something to them, but of course, they carry guns, and granted their attitude towards stealing, I doubt they'd feel any remorse shooting me. That said, I've gathered some materials and made a longsword and buckler. Hopefully things go well. If they don't... then, I suppose you'll be finding this somewhere else. Writing this solely in spite of them and because I feel like writing."));
        }

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.rare = -11;
        }
    }
}
