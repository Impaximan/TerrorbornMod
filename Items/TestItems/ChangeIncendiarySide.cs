using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TestItems
{
    class ChangeIncendiarySide : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
        }
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nChanges the side of the Sisyphean Islands biome" +
                "\nSide it changes to depends on mouse button"); */
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
                TerrorbornSystem.incendiaryIslandsSide = -1;
            }
            else
            {
                TerrorbornSystem.incendiaryIslandsSide = 1;
            }
            return base.CanUseItem(player);
        }
    }
}

