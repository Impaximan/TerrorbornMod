﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Magic.Staffs
{
    class TitaniumStaff : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TitaniumBar, 13)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Tooltip.SetDefault("Fires a titanium sawblade that slows down on hit to damage enemies multiple times");
        }
        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.noMelee = true;
            Item.width = 44;
            Item.height = 44;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TitaniumSawblade>();
            Item.shootSpeed = 10f;
            Item.mana = 8;
            Item.DamageType = DamageClass.Magic; ;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = player.Center + player.DirectionTo(Main.MouseWorld) * 44;
        }
    }

    class TitaniumSawblade : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                SpriteEffects effects = SpriteEffects.None;
                if (Projectile.spriteDirection == -1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 10;
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.hide = false;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        bool hasHit = false;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!hasHit)
            {
                Projectile.velocity.Normalize();
                Projectile.velocity *= 3f;
                hasHit = true;
            }
        }

        public override void AI()
        {
            Projectile.spriteDirection = -1;
            if (Projectile.velocity.X <= 0)
            {
                Projectile.spriteDirection = 1;
            }
            Projectile.rotation += MathHelper.ToRadians(12) * -Projectile.spriteDirection;

            Projectile.velocity = Projectile.velocity.ToRotation().ToRotationVector2() * (Projectile.velocity.Length() + 0.5f);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 14;
            height = 14;
            return true;
        }
    }
}

