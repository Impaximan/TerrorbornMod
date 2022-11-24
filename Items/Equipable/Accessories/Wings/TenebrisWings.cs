using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Equipable.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class TenebrisWings : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 10)
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddIngredient(ItemID.SoulofFlight, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();

        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows flight and slow fall" +
                "\nHold DOWN to fall faster");
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new Terraria.DataStructures.WingStats((int)(60 * 2.75f), 1f, 1.4f, false);
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = (int)(60 * 2.75f);
            player.armorEffectDrawShadow = true;
            if (player.controlDown)
            {
                player.maxFallSpeed *= 10f;
            }
        }

        public override void UpdateVanity(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 1.3f;
            ascentWhenRising = 0.14f;
            maxCanAscendMultiplier = 1.7f;
            maxAscentMultiplier = 2f;
            constantAscend = 0.1f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            acceleration *= 1.4f;
            speed *= 1.35f;
        }
    }
}