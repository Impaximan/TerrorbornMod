using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Shadowcrawler
{
    class ContaminatedMarinePistol : ModItem
    {
        int UntilBlast = 4;
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 18);
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires multiple bullets at once.\nFires a nightmare flame every few shots.");
        }
        public override void SetDefaults()
        {
            item.damage = 16;
            item.ranged = true;
            item.noMelee = true;
            item.width = 50;
            item.height = 24;
            item.useTime = 6;
            item.useAnimation = 6;
            item.shoot = 10;
            item.useStyle = 5;
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 2, 70, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            UntilBlast--;
            if (UntilBlast <= 0)
            {
                UntilBlast = 4;
                Main.PlaySound(SoundID.Item41, position);
                Projectile.NewProjectile(position.X, position.Y, speedX / 5, speedY / 5, mod.ProjectileType("NightmareBoilRanged"), damage * 2, knockBack, player.whoAmI);
            }
            for (int i = 0; i < 2; i++)
            {
                Vector2 Velocity = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
                Projectile.NewProjectile(position, Velocity, type, damage - 6, knockBack, player.whoAmI);
            }
            return false;
        }
    }
    class NightmareBoilRanged : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.ranged = true;
            //projectile.extraUpdates = 100;
            projectile.timeLeft = 60;
            projectile.penetrate = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.hide = true;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 74, 0f, 0f, 100, Scale: 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;
            float rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(180);
            float Speed = 1f;
            projectile.velocity += new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
        }
    }
}


