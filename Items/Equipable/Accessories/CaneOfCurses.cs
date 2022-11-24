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
            Item.width = 32;
            Item.height = 34;
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.CaneOfCurses = true;
        }
    }
}