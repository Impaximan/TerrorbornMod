using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class SpiderNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("-10 defense" +
                "\nIncreases max minions by 1" +
                "\n10% increased minion damage"); */
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.noMelee = true;
            Item.lifeRegen = 5;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.useAnimation = 5;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpiderFang, 8)
                .AddIngredient(ItemID.SoulofFright, 10)
                .AddIngredient(ItemID.Cobweb, 100)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense -= 10;
            player.maxMinions++;
            player.GetDamage(DamageClass.Summon) *= 1.1f;
        }
    }
}
