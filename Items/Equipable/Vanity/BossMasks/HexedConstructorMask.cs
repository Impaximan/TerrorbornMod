using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Vanity.BossMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class HexedConstructorMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            ////ArmorIDs.Head.Sets.DrawHead[Item.type] = false;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}

