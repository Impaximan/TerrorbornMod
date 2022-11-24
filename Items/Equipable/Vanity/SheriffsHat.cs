using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class SheriffsHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sheriff's Hat");
            Tooltip.SetDefault("Don't worry, he has another");
            //ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}

