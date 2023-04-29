using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Shadowcrawler
{
    class Nightbrood : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 18)
                .AddIngredient(ItemID.HallowedBar, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Fires eight arrows at once" +
                "\nThe accuracy of the arrows increases as you fire but resets when you stop firing"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 26;
            Item.height = 56;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.channel = true;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.DD2_BallistaTowerShot;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Arrow;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, 0);
        }

        int accuracy = 45;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 8; i++)
            {
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(accuracy));
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }

            if (accuracy > 10)
            {
                accuracy -= 5;
            }

            return false;
        }

        public override void UpdateInventory(Player player)
        {
            if (!player.channel)
            {
                accuracy = 45;
            }
        }

    }
}
