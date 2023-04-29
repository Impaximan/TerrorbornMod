using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TestItems
{
    class ToggleCoreOfFear : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
        }
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Toggle CoF");
            /* Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nToggles [i/s1:" + ModContent.ItemType<PermanentUpgrades.CoreOfFear>() + "]"); */
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
            if (!modPlayer.CoreOfFear)
            {
                modPlayer.CoreOfFear = true;
                Main.NewText("Core of Fear toggled on");

            }
            else
            {
                modPlayer.CoreOfFear = false;
                Main.NewText("Core of Fear toggled off");
            }
            return base.CanUseItem(player);
        }
    }
}


