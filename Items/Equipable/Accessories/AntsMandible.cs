using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class AntsMandible : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Royal Mandible");
            Tooltip.SetDefault("Grants 3 armor penetration and increased movement speed");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetArmorPenetration(DamageClass.Generic) += 3;
            player.runAcceleration += 0.065f;
        }
    }
}
