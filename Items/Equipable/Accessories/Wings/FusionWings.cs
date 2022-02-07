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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 14);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 2);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows flight and slow fall");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.Red;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = (int)(60 * 4f);
        }

        public override void UpdateVanity(Player player, EquipType type)
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