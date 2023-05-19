using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items
{
    class CrackedTimeChime : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Changes the time from day to night, or vice versa" +
                "\nRight click to instead move time forward");
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.width = 20;
            Item.height = 26;
            Item.UseSound = SoundID.Item67;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.autoReuse = true;
                Main.time += 1200;
                Item.UseSound = SoundID.Item28;
            }
            else
            {
                Item.UseSound = SoundID.Item67;
                Item.autoReuse = false;
                Main.dayTime = !Main.dayTime;
                Main.time = 0;
                TerrorbornSystem.ScreenShake(10f);
            }
            return base.CanUseItem(player);
        }
    }
}
