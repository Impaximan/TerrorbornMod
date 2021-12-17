using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.BurstJumps
{
    class FrostFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(TBUtils.Accessories.GetBurstJumpString((int)(60 * 1.5f)));
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.noMelee = true;
            item.rare = ItemRarityID.Blue;
            item.defense = 2;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.useAnimation = 5;
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.burstJump = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TBUtils.Accessories.UpdateBurstJump((int)(60 * 1.5f), 45, item, player, new Vector2(8, -20), Color.White, SoundID.Item14);
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.BurstJumpTime > 0)
            {
                Dust dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, 92)];
                dust.noGravity = true;
                dust.noLight = false;
                dust.velocity = Vector2.Zero;
            }
        }
    }
}
