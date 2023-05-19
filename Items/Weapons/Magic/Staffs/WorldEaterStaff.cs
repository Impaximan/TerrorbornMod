using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Magic.Staffs
{
    class WorldEaterStaff : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            // DisplayName.SetDefault("Toothache staff");
            /* Tooltip.SetDefault("Fires multiple eater's tooth Projectiles" +
                "\nIgnites hit enemies with cursed flames on critical hits"); */
        }
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.noMelee = true;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item17;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<WorldEaterTooth>();
            Item.shootSpeed = 10f;
            Item.mana = 6;
            Item.DamageType = DamageClass.Magic; ;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 3; i++)
            {
                position = player.Center + player.DirectionTo(Main.MouseWorld) * 40;
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }

    class WorldEaterTooth : ModProjectile
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
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.hide = false;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                target.AddBuff(BuffID.CursedInferno, 60 * 3);
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 14;
            height = 14;
            return true;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle originalHitbox = hitbox;
            int newSize = 14;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }
    }
}
