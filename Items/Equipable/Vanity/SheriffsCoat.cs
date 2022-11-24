using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    class SheriffsCoat : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Sheriff's Coat");
            Tooltip.SetDefault("Don't worry, he has another");
            ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 26;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}
