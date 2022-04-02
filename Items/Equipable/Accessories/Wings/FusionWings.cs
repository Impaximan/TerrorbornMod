using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Equipable.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class FusionWings : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 14)
                .AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 2)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows flight and slow fall");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = (int)(60 * 4f);
        }

        public override void UpdateVanity(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 1.4f;
            ascentWhenRising = 0.2f;
            maxCanAscendMultiplier = 1.8f;
            maxAscentMultiplier = 2f;
            constantAscend = 0.12f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            acceleration *= 1.6f;
            speed *= 1.4f;
        }
    }
}