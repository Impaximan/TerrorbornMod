using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Vanity.BossMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class ShadowcrawlerMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadowcrawler Mask");
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Blue;
            Item.vanity = true;
        }
    }
}

