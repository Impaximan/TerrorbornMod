using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Vanity.BossMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class PrototypeIMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prototype I Mask");
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}

