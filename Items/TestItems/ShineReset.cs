using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TestItems
{
    class ShineReset : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override bool IsLoadingEnabled(Mod mod)
        {
            return TerrorbornMod.IsInTestingMode;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reset 'SH!NE'");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nAllows 'SH!NE' to play again.");
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
            Interludes.SHINE.PlayedSong = false;
            return base.CanUseItem(player);
        }
    }
}


