using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.BurstJumps
{
    class DaoBurst : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(TBUtils.Accessories.GetBurstJumpString(60) + 
                "\nThe burst from this will launch you downward rather than upward" +
                "\nThe burst additionally gives you some of your wing flight time back");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.DarkShard, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.SoulofLight, 5);
            recipe2.AddIngredient(ItemID.SoulofNight, 5);
            recipe2.AddIngredient(ItemID.LightShard, 1);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.noMelee = true;
            item.rare = ItemRarityID.Pink;
            item.defense = 8;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.useAnimation = 5;
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.burstJump = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TBUtils.Accessories.UpdateBurstJump(60, 60 * 1, item, player, new Vector2(10, 30), Color.MediumPurple, SoundID.Item14);
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (modPlayer.BurstJumpTime > 0)
            {
                Dust dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, 62)];
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity = Vector2.Zero;
            }

            if (modPlayer.JustBurstJumped)
            {
                player.wingTime += player.wingTimeMax / 2;
                if (player.wingTime > player.wingTimeMax)
                {
                    player.wingTime = player.wingTimeMax;
                }
            }
        }
    }
}