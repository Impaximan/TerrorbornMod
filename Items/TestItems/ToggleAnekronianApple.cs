using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TestItems
{
    class ToggleAnekronianApple : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Toggle AA");
            /* Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nToggles [i/s1:" + ModContent.ItemType<PermanentUpgrades.AnekronianApple>() + "]"); */
        }
        public override void SetDefaults()
        {
            Item.rare = -12;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!modPlayer.AnekronianApple)
            {
                modPlayer.AnekronianApple = true;
                Main.NewText("Anekronian Apple toggled on");

            }
            else
            {
                modPlayer.AnekronianApple = false;
                Main.NewText("Anekronian Apple toggled off");
            }
            return base.CanUseItem(player);
        }
    }
}