using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.BurstJumps
{
    class CompressedGelatin : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault(Utils.Accessories.GetBurstJumpString(60 * 2));
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 3;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.useAnimation = 5;
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.burstJump = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Utils.Accessories.UpdateBurstJump(60 * 2, 60 * 1, Item, player, new Vector2(20, -15), Color.LightBlue, SoundID.Item14);
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.BurstJumpTime > 0)
            {
                Dust dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, 59)];
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity = Vector2.Zero;
            }
        }
    }
}
