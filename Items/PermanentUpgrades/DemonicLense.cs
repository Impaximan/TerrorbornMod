using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.PermanentUpgrades
{
    class DemonicLense : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Permanently allows you to use Shriek of Horror twice as fast" +
                "\nCan only be used once"); */
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.maxStack = 9999;
            Item.UseSound = SoundID.Item29;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }

        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!modPlayer.DemonicLense)
            {
                CombatText.NewText(new Rectangle((int)(player.Center.X - 50), (int)(player.Center.Y - 50), 100, 10), Color.Red, "Shriek of Horror movement speed increased by 20%", true, false);
                modPlayer.DemonicLense = true;
                Item.stack--;
                return true;
            }
            return false;
        }
    }
}
