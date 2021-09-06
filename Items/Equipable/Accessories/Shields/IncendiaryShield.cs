using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories.Shields
{
    class IncendiaryShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Melee damage has an 8% chance to cause an explosion" +
                "\nThis chance is boosted by half your melee critical strike chance" +
                "\nThis explosion will damage the hit enemy and all nearby hit enemies for half the damage the original hit did" +
                "\nThis explosion additionally inflicts enemies with a random type of fire");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = 4;
            item.defense = 5;
            item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.IncendiaryShield = true;
        }
    }
}

