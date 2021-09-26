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
            Tooltip.SetDefault(TBUtils.Accessories.GetBurstJumpString(60 * 2));
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.noMelee = true;
            item.rare = 1;
            item.defense = 3;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.useAnimation = 5;
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.burstJump = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TBUtils.Accessories.UpdateBurstJump(60 * 2, 60 * 1, item, player, new Vector2(15, -15), Color.LightBlue, SoundID.Item14);
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
