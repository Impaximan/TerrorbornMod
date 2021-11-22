using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class MirageBow : ModItem
    {
        public override void AddRecipes()
        {
            int evilBars = 10;
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 8);
            recipe.AddIngredient(ItemID.CrimtaneBar, evilBars);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 8);
            recipe2.AddIngredient(ItemID.DemoniteBar, evilBars);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates numerous spectral versions of itself to fire at your cursor");
        }
        public override void SetDefaults()
        {
            item.damage = 16;
            item.ranged = true;
            item.width = 22;
            item.height = 52;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 4, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item117;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 15f;
            item.useAmmo = AmmoID.Arrow;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 2; i++)
            {
                Projectile projectile = Main.projectile[Projectile.NewProjectile(position.X - 50 + Main.rand.Next(100), position.Y - 50 + Main.rand.Next(100), 0, 0, ModContent.ProjectileType<SpectralBow>(), damage, knockBack, player.whoAmI)];
                projectile.ai[0] = type;
                projectile.ai[1] = new Vector2(speedX, speedY).Length();
            }
            return false;
        }
    }

    class SpectralBow : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 52;
            projectile.ranged = true;
            projectile.timeLeft = 60;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
        }

        int projectileWait = 20;
        public override void AI()
        {
            projectile.rotation = projectile.DirectionTo(Main.MouseWorld).ToRotation();

            projectileWait--;
            if (projectileWait <= 0)
            {
                projectileWait = Main.rand.Next(15, 25);
                Main.PlaySound(SoundID.Item5, projectile.Center);
                Vector2 velocity = projectile.ai[1] * projectile.DirectionTo(Main.MouseWorld);
                Projectile proj = Main.projectile[Projectile.NewProjectile(projectile.Center, velocity, (int)projectile.ai[0], projectile.damage, projectile.knockBack, projectile.owner)];
                proj.noDropItem = true;
            }

            if (projectile.alpha > 255 / 2)
            {
                projectile.alpha -= 15;
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 10, 15, DustID.Fire, DustScale: 1.5f, NoGravity: true);
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }
    }
}
