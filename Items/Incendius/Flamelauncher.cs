using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Incendius
{
    class Flamelauncher : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 25);
            recipe.AddIngredient(ItemID.CobaltBar, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 25);
            recipe2.AddIngredient(ItemID.PalladiumBar, 15);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Has a 20% chance to let out a cloud of flames" +
                "\n20% chance to not consume ammo");
        }
        public override void SetDefaults()
        {
            item.damage = 34;
            item.ranged = true;
            item.noMelee = true;
            item.scale = 0.8f;
            item.width = 56;
            item.height = 40;
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item41;
            item.shoot = 10;
            item.autoReuse = true;
            item.shootSpeed = 10f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .20f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.NextFloat() <= .20f)
            {
                float Multiplier = Main.rand.NextFloat(.4f, .6f);
                Projectile.NewProjectile(position, new Vector2(speedX * Multiplier, speedY * Multiplier).RotatedByRandom(MathHelper.ToRadians(10)), mod.ProjectileType("FlameCloud"), damage / 2, 0, item.owner);
            }
            if (speedX > 0)
            {
                position += Vector2.Normalize(new Vector2(speedX, speedY)).RotatedBy(60) * 15f;
            }
            else
            {
                position += Vector2.Normalize(new Vector2(speedX, speedY)).RotatedBy(-60) * 15f;
            }
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }
    }
    class FlameCloud : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly; } }
        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.ranged = true;
            projectile.hide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
        public override void AI()
        {
            int dust = Dust.NewDust(projectile.position, 25, 25, DustID.Fire, Scale: 1.5f);
            Main.dust[dust].noGravity = true;
            projectile.velocity *= 0.96f;
        }
    }
}
