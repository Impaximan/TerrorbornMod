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

        }

        public override bool DrawHead()
        {
            return false;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}

