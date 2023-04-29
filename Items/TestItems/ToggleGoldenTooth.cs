using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TestItems
{
    class ToggleGoldenTooth : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Toggle Golden Tooth");
            /* Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nToggles [i/s1:" + ModContent.ItemType<PermanentUpgrades.GoldenTooth>() + "]"); */
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
            if (!modPlayer.GoldenTooth)
            {
                modPlayer.GoldenTooth = true;
                Main.NewText("Golden Tooth toggled on");

            }
            else
            {
                modPlayer.GoldenTooth = false;
                Main.NewText("Golden Tooth toggled off");
            }
            return base.CanUseItem(player);
        }
    }
}


