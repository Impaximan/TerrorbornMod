using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TestItems
{
    class ResetMidnightFruit : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Resets your Midnight Fruit count");
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
            TerrorbornPlayer.modPlayer(player).MidnightFruit = 0;
            Main.NewText("Midnight fruit count reset!");
            return base.CanUseItem(player);
        }
    }
}

