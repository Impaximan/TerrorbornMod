using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Ranged.DartWeapons
{
    class DualpipeDartgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires two darts at once in an even spread");
        }
        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 44;
            Item.height = 16;
            Item.useTime = 13;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0.6f;
            Item.value = 0;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item98;
            Item.autoReuse = false;
            Item.shootSpeed = 20f;
            Item.scale = 0.85f;
            Item.useAmmo = AmmoID.Dart;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int spread = 4;
            Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(-spread)), type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.ToRadians(spread)), type, damage, knockback, player.whoAmI);
            return false;
        }
        public override bool? UseItem(Player player)
        {
            player.bodyFrame.Y = 56 * 2;
            return true;
        }
    }
}




