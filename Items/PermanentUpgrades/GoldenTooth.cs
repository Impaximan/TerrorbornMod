using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.PermanentUpgrades
{
    class GoldenTooth : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Permanently doubles Shriek of Horror's range" +
                "\nCan only be used once");
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.maxStack = 9999;
            Item.UseSound = SoundID.Item29;
            Item.value = Item.sellPrice(0, 4, 0, 0);
        }

        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!modPlayer.GoldenTooth)
            {
                CombatText.NewText(new Rectangle((int)(player.Center.X - 50), (int)(player.Center.Y - 50), 100, 10), Color.Red, "Shriek of Horror movement speed increased by 20%", true, false);
                modPlayer.GoldenTooth = true;
                Item.stack--;
                return true;
            }
            return false;
        }
    }
}
