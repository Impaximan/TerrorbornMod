using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.TestItems
{
    class ResetTerrorMasterDialogue : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Sets the terror master's dialogue sequence counter to 0");
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
            TerrorbornSystem.TerrorMasterDialogue = 0;
            Main.NewText("Terror Master dialogue reset! You may now go through the sequences once more.");
            return base.CanUseItem(player);
        }
    }
}
