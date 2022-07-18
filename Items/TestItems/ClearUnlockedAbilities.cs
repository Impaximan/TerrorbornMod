using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TestItems
{
    class ClearUnlockedAbilities : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clear Unlocked Abilities");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nCleras all of the your unlocked abilities" +
                "\nRight click to get a list of unlocked abilities");
        }
        public override void SetDefaults()
        {
            Item.rare = -12;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer tPlayer = TerrorbornPlayer.modPlayer(player);
            if (player.altFunctionUse != 2)
            {
                tPlayer.unlockedAbilities.Clear();
                Main.NewText("Cleared Unlocked Abilities!");
            }
            else
            {
                if (tPlayer.unlockedAbilities.Count < 1)
                {
                    Main.NewText("No abilities currently unlocked!");
                }
                else
                {
                    for (int i = 0; i < tPlayer.unlockedAbilities.Count; i++)
                    {
                        Main.NewText(TerrorbornUtils.intToAbility(tPlayer.unlockedAbilities[i]).Name());
                    }
                }
            }
            return base.CanUseItem(player);
        }
    }
}

