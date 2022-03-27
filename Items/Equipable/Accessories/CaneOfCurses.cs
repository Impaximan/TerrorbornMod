using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class CaneOfCurses : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cane of Cursing");
            Tooltip.SetDefault("Hitting enemies has a chance to inflict them with a random debuff");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 34;
            item.accessory = true;
            item.noMelee = true;
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.CaneOfCurses = true;
        }
    }
}