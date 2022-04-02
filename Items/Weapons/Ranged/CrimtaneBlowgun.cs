using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class CrimtaneBlowgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloodied Shotpipe");
            Tooltip.SetDefault("Costs 0.5% terror to use" +
                "\nUses darts as ammo and fires 3-4 darts at once");
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 44;
            Item.height = 16;
            Item.useTime = 45;
            Item.useAnimation = 45;
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

        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return modPlayer.TerrorPercent >= 0.5f;
        }

        Vector2 offset = new Vector2(0, 1);
        public override void UseItemFrame(Player player)
        {
            player.bodyFrame.Y = 2 * player.bodyFrame.Height;
            player.itemLocation = player.Center + offset;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.LoseTerror(0.5f);
            position = player.Center + offset;
            position.Y -= 3;
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
            if (player.direction == -1)
            {
                player.itemRotation += MathHelper.ToRadians(180);
            }
            int maxRotation = 15;
            for (int i = 0; i < Main.rand.Next(3, 5); i++)
            {
                Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(maxRotation)), type, damage, knockback, player.whoAmI);
            }
            return false;
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
                .AddIngredient(ItemID.CrimtaneBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}