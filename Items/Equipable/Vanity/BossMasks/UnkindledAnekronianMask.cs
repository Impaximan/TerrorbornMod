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

        }

        public override bool DrawHead()
        {
            return true;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}