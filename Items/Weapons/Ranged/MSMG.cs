using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class MSMG : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("M.S.M.G");
            Tooltip.SetDefault("Rains bullets from the sky" +
                "\n'Meteor Shower Machine Gun'");
        }

        public override void SetDefaults()
        {
            item.damage = 7;
            item.ranged = true;
            item.noMelee = true;
            item.width = 38;
            item.height = 18;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item11;
            item.shoot = ProjectileID.PurificationPowder;
            item.autoReuse = true;
            item.shootSpeed = 20f;
            item.useAmmo = AmmoID.Bullet;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        // 33% chance not to consume ammo
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .33f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ItemID.MeteoriteBar, 15);
            recipe1.AddTile(TileID.Anvils);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 newPosition = player.Center - new Vector2(0, 750);
            Vector2 velocity = new Vector2(speedX, speedY).Length() * (Main.MouseWorld - newPosition).ToRotation().ToRotationVector2();
            Projectile.NewProjectile(newPosition, velocity, type, damage, knockBack, player.whoAmI);
            return true;
        }
    }
}
