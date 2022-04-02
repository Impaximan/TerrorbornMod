using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class TheLiesOfNourishment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Lies of Nourishment");
            Tooltip.SetDefault("The first hit on an enemy will always be critical" +
                "\nKilling enemies has an increased chance to drop hearts");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 36;
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 3, 75, 0);
            Item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.LiesOfNourishment = true;
        }
    }
}
