using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.BurstJumps
{
    class SkyBurst : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault(Utils.Accessories.GetBurstJumpString((int)(60 * 1.5f)) +
                "\nGrants you controllable slow fall for 3 seconds upon activation" +
                "\nCannot be charged up again while the slow fall effect is active"); */
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Feather, 3)
                .AddIngredient(ItemID.SunplateBlock, 10)
                .AddIngredient(ItemID.Cloud, 15)
                .AddIngredient(ItemID.RainCloud, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.defense = 5;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.useAnimation = 5;
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.burstJump = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Utils.Accessories.UpdateBurstJump((int)(60 * 1.5f), 60 * 3, Item, player, new Vector2(15, -10), Color.Azure, SoundID.Item14);
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.BurstJumpTime > 0)
            {
                Dust dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, 15)];
                dust.noGravity = true;
                dust.velocity = new Vector2(0, -10);
                player.slowFall = true;
                modPlayer.BurstJumpChargingTime = 0;
            }
        }
    }
}
