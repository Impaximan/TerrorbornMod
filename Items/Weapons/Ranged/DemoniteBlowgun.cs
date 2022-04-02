using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class DemoniteBlowgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vile Blowgun");
            Tooltip.SetDefault("Uses darts as ammo" +
                "\nEvery third shot consumes 1% terror to fire 2 darts at once");
        }
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 44;
            Item.height = 16;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = 101;
            Item.knockBack = 0.6f;
            Item.value = 0;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item64;
            Item.autoReuse = true;
            Item.shootSpeed = 11.25f;
            Item.useAmmo = AmmoID.Dart;
        }

        Vector2 offset = new Vector2(0, 1);
        public override void UseItemFrame(Player player)
        {
            player.bodyFrame.Y = 2 * player.bodyFrame.Height;
            player.itemLocation = player.Center + offset;
        }

        int twoCounter = 3;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = player.Center + offset;
            position.Y -= 3;
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
            if (player.direction == -1)
            {
                player.itemRotation += MathHelper.ToRadians(180);
            }
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            twoCounter--;
            int maxRotation = 10;
            if (twoCounter <= 0)
            {
                twoCounter = 3;
                float cost = 1f;
                if (modPlayer.TerrorPercent >= cost)
                {
                    Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(maxRotation)), type, damage, knockback, player.whoAmI);
                    modPlayer.LoseTerror(cost);
                }
            }
            Vector2 newSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(maxRotation));
            velocity.X = newSpeed.X;
            velocity.Y = newSpeed.Y;
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override bool? UseItem(Player player)
        {
            player.bodyFrame.Y = 56 * 2;
            player.itemAnimation = 2;
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}


