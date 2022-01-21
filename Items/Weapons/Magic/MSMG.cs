using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using System.Collections.Generic;
using TerrorbornMod.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class MSMG : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 20);
            recipe.AddIngredient(ModContent.ItemType<Materials.NovagoldBar>(), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("M.S.M");
            Tooltip.SetDefault("Rapidly fires space lasers at your cursor" +
                "\nStands for 'Meteor Shower Minigun'");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.critDamageMult = 1.45f;
            item.damage = 8;
            item.noMelee = true;
            item.width = 48;
            item.height = 28;
            item.useTime = 6;
            item.useAnimation = 6;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.crit = 14;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Blue;
            item.shootSpeed = 1f;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<SpaceLaser>();
            item.mana = 3;
            item.magic = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            TerrorbornMod.ScreenShake(1f);
            Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 125, 1, 1);
            Vector2 shootSpeed = new Vector2(speedX, speedY);
            shootSpeed.Normalize();
            position += shootSpeed * 22;
            shootSpeed = shootSpeed.RotatedByRandom(MathHelper.ToRadians(3));
            speedX = shootSpeed.X;
            speedY = shootSpeed.Y;
            return true;
        }
    }

    class SpaceLaser : Deathray
    {
        int timeLeft = 10;
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/LightBlast";
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.hide = false;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = timeLeft;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            MoveDistance = 20f;
            RealMaxDistance = 2000f;
            bodyRect = new Rectangle(0, 0, 10, 10);
            headRect = new Rectangle(0, 0, 10, 10);
            tailRect = new Rectangle(0, 0, 10, 10);
            FollowPosition = false;
            drawColor = Color.LightGreen;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
        }
    }
}


