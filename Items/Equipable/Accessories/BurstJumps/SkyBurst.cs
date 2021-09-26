using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;

namespace TerrorbornMod.Items.Equipable.Accessories.BurstJumps
{
    class SkyBurst : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(TBUtils.Accessories.GetBurstJumpString((int)(60 * 1.5f)) +
                "\nGrants you controllable slow fall for 3 seconds upon activation" +
                "\nCannot be charged up again while the slow fall effect is active");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Feather, 3);
            recipe.AddIngredient(ItemID.SunplateBlock, 10);
            recipe.AddIngredient(ItemID.Cloud, 15);
            recipe.AddIngredient(ItemID.RainCloud, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = 2;
            item.defense = 5;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.useAnimation = 5;
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.burstJump = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TBUtils.Accessories.UpdateBurstJump((int)(60 * 1.5f), 60 * 3, item, player, new Vector2(15, -10), Color.Azure, SoundID.Item14);
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
