using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class Megawhale : ModItem
    {
        int baseReuseDelay = 25;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires faster the longer you use it" +
                "\nFires three bullets at once in a fluctuating spread");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.critDamageMult = 1.3f;
            item.damage = 25;
            item.ranged = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.width = 112;
            item.height = 46;
            item.useTime = 6;
            item.useAnimation = 6;
            item.knockBack = 5;
            item.UseSound = SoundID.Item11;
            item.shoot = ProjectileID.PurificationPowder;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.value = Item.sellPrice(0, 0, 25, 0);
            item.rare = ItemRarityID.Cyan;
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Bullet;
            item.scale = 0.75f;
            item.channel = true;
            item.reuseDelay = baseReuseDelay;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-15, -2);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Megashark, 1);
            recipe.AddIngredient(ModContent.ItemType<Materials.PlasmaliumBar>(), 12);
            recipe.AddIngredient(ModContent.ItemType<Materials.AzuriteBar>(), 12);
            recipe.AddIngredient(ModContent.ItemType<Materials.HellbornEssence>(), 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateInventory(Player player)
        {
            if (!player.channel)
            {
                item.reuseDelay = baseReuseDelay;
            }
        }

        public override bool CanUseItem(Player player)
        {
            if (item.reuseDelay > 0)
            {
                item.reuseDelay--;
            }
            return base.CanUseItem(player);
        }

        int shots = 0;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            shots++;
            Vector2 velocity = new Vector2(speedX, speedY);
            float rotationAmount = (float)Math.Sin((float)shots / 10f) * 25f;
            Projectile.NewProjectile(position, velocity.RotatedBy(MathHelper.ToRadians(rotationAmount)), type, damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position, velocity.RotatedBy(MathHelper.ToRadians(-rotationAmount)), type, damage, knockBack, player.whoAmI);
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
    }
}