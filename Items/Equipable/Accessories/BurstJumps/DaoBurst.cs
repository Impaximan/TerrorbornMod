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
            CreateRecipe()
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.DarkShard)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.LightShard)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Pink;
            Item.defense = 8;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.useAnimation = 5;
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.burstJump = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TBUtils.Accessories.UpdateBurstJump(60, 60 * 1, Item, player, new Vector2(10, 30), Color.MediumPurple, SoundID.Item14);
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