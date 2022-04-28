using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Ammo
{
    class GemDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Splits into multiple gem fragments on hit");
        }

        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 28;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1;
            Item.shootSpeed = 2;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<GemDartProjectile>();
            Item.ammo = AmmoID.Dart;
        }

        public override void AddRecipes()
        {
            CreateRecipe(50)
                .AddIngredient(ItemID.Amethyst)
                .AddIngredient(ModContent.ItemType<WoodDart>(), 50)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe(75)
                .AddIngredient(ItemID.Topaz)
                .AddIngredient(ModContent.ItemType<WoodDart>(), 75)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe(100)
                .AddIngredient(ItemID.Sapphire)
                .AddIngredient(ModContent.ItemType<WoodDart>(), 100)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe(125)
                .AddIngredient(ItemID.Emerald)
                .AddIngredient(ModContent.ItemType<WoodDart>(), 125)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe(150)
                .AddIngredient(ItemID.Ruby)
                .AddIngredient(ModContent.ItemType<WoodDart>(), 150)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe(175)
                .AddIngredient(ItemID.Diamond)
                .AddIngredient(ModContent.ItemType<WoodDart>(), 175)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe(175)
                .AddIngredient(ItemID.Amber)
                .AddIngredient(ModContent.ItemType<WoodDart>(), 175)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    class GemDartProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Ammo/GemDart";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 28;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 1000;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        bool start = true;
        public override void AI()
        {
            Projectile.velocity.Y += 0.03f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                float speed = Main.rand.NextFloat(10f, 15f);
                Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * speed;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<GemFragment>(), Projectile.damage / 3, Projectile.knockBack / 3, Projectile.owner);
            }
            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_WitherBeastHurt, Projectile.Center);
        }
    }

    class GemFragment : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.penetrate = 3;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.3f;
            Projectile.velocity *= 0.97f;
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X * 4);
        }
    }
}