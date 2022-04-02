using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class MandibleNecklace : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SharkToothNecklace)
                .AddIngredient(ModContent.ItemType<AntsMandible>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mandible Necklace");
            Tooltip.SetDefault("Grants 8 armor penetration and increased movement speed");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.armorPenetration += 8;
            player.runAcceleration += 0.065f;
        }
    }
}
