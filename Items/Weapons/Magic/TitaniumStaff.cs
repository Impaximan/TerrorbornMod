using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class TitaniumStaff : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 13);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Fires a titanium sawblade that slows down on hit to damage enemies multiple times");
        }
        public override void SetDefaults()
        {
            item.damage = 35;
            item.noMelee = true;
            item.width = 44;
            item.height = 44;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("TitaniumSawblade");
            item.shootSpeed = 10f;
            item.mana = 8;
            item.magic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 44);
            return true;
        }
    }

    class TitaniumSawblade : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                SpriteEffects effects = SpriteEffects.None;
                if (projectile.spriteDirection == -1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 10;
            projectile.magic = true;
            projectile.hide = false;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 5;
        }

        bool hasHit = false;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!hasHit)
            {
                projectile.velocity.Normalize();
                projectile.velocity *= 3f;
                hasHit = true;
            }
        }

        public override void AI()
        {
            projectile.spriteDirection = -1;
            if (projectile.velocity.X <= 0)
            {
                projectile.spriteDirection = 1;
            }
            projectile.rotation += MathHelper.ToRadians(12) * -projectile.spriteDirection;

            projectile.velocity = projectile.velocity.ToRotation().ToRotationVector2() * (projectile.velocity.Length() + 0.5f);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 14;
            height = 14;
            return true;
        }
    }
}

