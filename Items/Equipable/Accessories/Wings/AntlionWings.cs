using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Items.Equipable.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class AntlionWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swarmer Wings");
            Tooltip.SetDefault("Allows flight and slow fall" +
                "\nWhile airborn you are immune to high wind speeds");
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new Terraria.DataStructures.WingStats(55, 1f, 1.5f, true, 1f);
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = 55;
            if (player.velocity.Y != 0)
            {
                player.buffImmune[BuffID.WindPushed] = true;
            }
        }

        public override void UpdateVanity(Player player)
        {
            if (player.velocity.Y != 0)
            {
                player.wingFrameCounter += 2;
            }
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.08f;
            maxCanAscendMultiplier = 0.8f;
            maxAscentMultiplier = 1.5f;
            constantAscend = 0.1f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            base.HorizontalWingSpeeds(player, ref speed, ref acceleration);
            speed *= 1f;
            acceleration *= 1.5f;
        }
    }
}