using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged.DartWeapons
{
    class WoodenBlowpipe : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses darts as ammo");
        }
        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 36;
            Item.height = 12;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = 101;
            Item.knockBack = 0.6f;
            Item.value = 0;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item63;
            Item.autoReuse = true;
            Item.shootSpeed = 11.25f;
            Item.useAmmo = AmmoID.Dart;
        }
        Vector2 offset = new Vector2(0, 0);
        public override void UseItemFrame(Player player)
        {
            player.bodyFrame.Y = 2 * player.bodyFrame.Height;
            player.itemLocation = player.Center + offset;
        }
        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
            if (player.direction == -1)
            {
                player.itemRotation += MathHelper.ToRadians(180);
            }
            position = player.Center + offset;
            position.Y -= 3;
        }

        public override bool? UseItem(Player player)
        {
            player.bodyFrame.Y = 56 * 2;
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Wood", 15)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
