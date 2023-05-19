using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TestItems
{
    class ToggleEyeOfTheMenace : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Toggle EotM");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nToggles [i/s1:" + ModContent.ItemType<PermanentUpgrades.EyeOfTheMenace>() +"]");
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
            if (!modPlayer.EyeOfTheMenace)
            {
                modPlayer.EyeOfTheMenace = true;
                Main.NewText("Eye of the Menace toggled on");

            }
            else
            {
                modPlayer.EyeOfTheMenace = false;
                Main.NewText("Eye of the Menace toggled off");
            }
            return base.CanUseItem(player);
        }
    }
}

