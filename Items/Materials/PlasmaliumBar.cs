using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Materials
{
    class PlasmaliumBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Contaminated Alloy");
            Tooltip.SetDefault("Highly radioactive, drains terror from you over time while in your inventory");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Cyan;
        }

        public override void UpdateInventory(Player player)
        {
            TerrorbornPlayer.modPlayer(player).LoseTerror(0.5f, true, true);
        }
    }
}

