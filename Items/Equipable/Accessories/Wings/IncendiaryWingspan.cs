using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Equipable.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class IncendiaryWingspan : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(20 * TerrorbornMod.IncendiaryAlloyMultiplier));
            recipe.AddRecipeGroup("cobalt", 10);
            recipe.AddIngredient(ItemID.SoulofFlight, 15);
            recipe.AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows flight and slow fall" +
                "\nTemporary immunity to lava and immunity to fire");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = (int)(60 * 2.67f);
            player.lavaMax += 60 * 3;
            player.buffImmune[BuffID.OnFire] = true;
            player.fireWalk = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.08f;
            maxCanAscendMultiplier = 1.2f;
            maxAscentMultiplier = 1.5f;
            constantAscend = 0.1f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            acceleration *= 1.2f;
        }
    }
}
