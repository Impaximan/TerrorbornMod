using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Vanity.BossMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class UnkindledAnekronianMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            //ArmorIDs.Head.Sets.DrawFullHair[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}