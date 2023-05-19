using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class BanditGlove : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bandit's Glove");
            Tooltip.SetDefault("Hitting enemies causes them to be inflicted with stacking damage over time" +
                "\nThe damage over time will be reset after 5 seconds" +
                "\n12% increased critical strike damage");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Green;
            Item.defense = 1;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.BanditGlove = true;
            modPlayer.critDamage += 0.12f;
        }
    }
}
