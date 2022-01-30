using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 26;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.Blue;
            item.vanity = true;
        }

        public override bool DrawBody()
        {
            return true;
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
            drawArms = true;
        }
    }
}
