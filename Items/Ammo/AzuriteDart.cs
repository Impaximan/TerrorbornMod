using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Ammo
{
    class AzuriteDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates a weaker but piercing clone of itself upon hitting an enemy");
        }

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 22;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1;
            Item.shootSpeed = 2;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<AzuriteDartProjectile>();
            Item.ammo = AmmoID.Dart;
        }

        public override void AddRecipes()
        {
            CreateRecipe(111)
                .AddIngredient<Materials.AzuriteBar>()
                .AddIngredient(ModContent.ItemType<WoodDart>(), 111)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    class AzuriteDartProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Ammo/AzuriteDart";

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
            Projectile.height = 22;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 1000;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        int DustCooldown = 69;
        bool start = true;
        Vector2 originalVelocity;
        Vector2 originalPosition;
        public override void AI()
        {
            if (start)
            {
                start = false;
                originalVelocity = Projectile.velocity;
                originalPosition = Projectile.Center;
            }
            Projectile.velocity.Y += 0.03f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            if (Projectile.ai[0] > 0)
            {
                Projectile.alpha = 255 / 2;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.ai[0] > 0)
            {
                return;
            }
            int proj = Projectile.NewProjectile(Projectile.GetProjectileSource_OnHit(target, Projectile.whoAmI), originalPosition, originalVelocity, Projectile.type, Projectile.damage / 4, Projectile.knockBack / 4, Projectile.owner);
            Main.projectile[proj].ai[0] = 1;
            Main.projectile[proj].penetrate = 3;
        }
    }
}