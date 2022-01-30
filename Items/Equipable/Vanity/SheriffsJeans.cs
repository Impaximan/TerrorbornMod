using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
            item.width = 22;
            item.height = 18;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.Blue;
            item.vanity = true;
        }
    }
}
