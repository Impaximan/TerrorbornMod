using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class BFCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BF Cap");
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}
