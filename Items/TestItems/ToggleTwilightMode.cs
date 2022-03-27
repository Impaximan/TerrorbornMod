using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TestItems
{
    class ToggleTwilightMode : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Activate/Deactivate Twilight Mode");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nDoes exactly what the name says");
        }
        public override void SetDefaults()
        {
            item.rare = -12;
            item.autoReuse = false;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = 20;
            item.useAnimation = 20;
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornWorld.TwilightMode = !TerrorbornWorld.TwilightMode;
            Main.NewText(TerrorbornWorld.TwilightMode);
            return base.CanUseItem(player);
        }
    }
}