using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class ChlorophyteBlowpipe : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses darts as ammo" +
                "\nFires two darts in a row");
        }
        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 42;
            Item.height = 14;
            Item.useTime = 5;
            Item.useAnimation = Item.useTime * 2;
            Item.reuseDelay = 20;
            Item.useStyle = 101;
            Item.knockBack = 0.6f;
            Item.value = 0;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.rare = ItemRarityID.Lime;
            Item.autoReuse = true;
            Item.shootSpeed = 16;
            Item.useAmmo = AmmoID.Dart;
        }

        Vector2 offset = new Vector2(0, 0);
        public override void UseItemFrame(Player player)
        {
            player.bodyFrame.Y = 2 * player.bodyFrame.Height;
            player.itemLocation = player.Center + offset;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item63, player.Center);
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
            if (player.direction == -1)
            {
                player.itemRotation += MathHelper.ToRadians(180);
            }
            position = player.Center + offset;
            position.Y -= 3;
            int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = 10;
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
                .AddIngredient(ItemID.ChlorophyteBar, 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}



