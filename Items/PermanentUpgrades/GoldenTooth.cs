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
            Item.UseSound = SoundID.Item29;
            Item.value = Item.sellPrice(0, 4, 0, 0);
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return !modPlayer.GoldenTooth;
        }
        public override void OnConsumeItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!modPlayer.GoldenTooth)
            {
                Item.stack--;
                CombatText.NewText(new Rectangle((int)(player.Center.X - 50), (int)(player.Center.Y - 50), 100, 10), Color.Red, "Shriek of Horror range increased by 100%", true, false);
                modPlayer.GoldenTooth = true;
            }
            base.OnConsumeItem(player);
        }
    }
}
