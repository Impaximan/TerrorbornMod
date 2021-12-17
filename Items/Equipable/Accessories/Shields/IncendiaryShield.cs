using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.Shields
{
    class IncendiaryShield : ModItem
    {
        int cooldown = 10 * 60;
        float knockback = 7.5f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(TBUtils.Accessories.GetParryShieldString(cooldown, knockback) + "\nParrying attacks will also launch you towards the cursor");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = ItemRarityID.LightRed;
            item.defense = 8;
            item.knockBack = knockback;
            item.value = Item.sellPrice(0, 5, 0, 0);
            TerrorbornItem.modItem(item).parryShield = true;
        }

        int dashTime = 0;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.parryColor = Color.Red;
            if (modPlayer.JustParried)
            {
                dashTime = 30;
            }
            if (dashTime > 0)
            {
                dashTime--;
                float speed = 20f;
                player.velocity = player.DirectionTo(Main.MouseWorld) * speed;
                if (player.maxFallSpeed < speed)
                {
                    player.maxFallSpeed = speed;
                }
                Dust dust = Main.dust[Dust.NewDust(player.position, player.width, player.height, 235)];
                dust.noGravity = true;
            }
            TBUtils.Accessories.UpdateParryShield(cooldown, item, player);
        }
    }
}


