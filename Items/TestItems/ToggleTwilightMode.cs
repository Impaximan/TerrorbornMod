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
            Item.rare = -12;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornSystem.TwilightMode = !TerrorbornSystem.TwilightMode;
            Main.NewText(TerrorbornSystem.TwilightMode);
            return base.CanUseItem(player);
        }
    }
}