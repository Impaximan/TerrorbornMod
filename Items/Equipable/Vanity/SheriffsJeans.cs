using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Vanity
{
    [AutoloadEquip(EquipType.Legs)]
    public class SheriffsJeans : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sheriff's Jeans");
            Tooltip.SetDefault("Don't worry, he has more");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}
