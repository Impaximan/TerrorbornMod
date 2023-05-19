using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Magic.SpellBooks
{
    class SearingDaggers : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HellstoneBar, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Fires many flaming daggers in an even spread" +
                "\nThese daggers will create weaker versions of themselves on critical hits"); */
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.width = 32;
            Item.height = 38;
            Item.DamageType = DamageClass.Magic; ;
            Item.damage = 20;
            Item.useTime = 28;
            Item.useAnimation = 28;
            Item.mana = 10;
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<SearingDagger>();
            Item.shootSpeed = 15;
            Item.UseSound = SoundID.Item20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0.1f;
            Item.autoReuse = true;
            Item.noMelee = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -2; i <= 2; i++)
            {
                Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(10 * i)), type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }

    class SearingDagger : ModProjectile
    {
        int FallWait = 40;
        int penetrateUntilStop = 2;

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
            Projectile.width = 20;
            Projectile.height = 66;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.hide = false;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 60 * 10);

            penetrateUntilStop--;
            if (penetrateUntilStop <= 0)
            {
                FallWait = 0;
            }

            if (hit.Crit && Projectile.ai[0] != 1)
            {
                Projectile proj = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromThis(), originalPosition, originalVelocity, Projectile.type, Projectile.damage / 2, Projectile.knockBack, Projectile.owner)];
                proj.ai[0] = 1;
                proj.alpha = 255 / 2;
            }
        }

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

            if (FallWait > 0)
            {
                FallWait--;
                Projectile.spriteDirection = 1;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            }
            else
            {
                Projectile.velocity *= 0.9f;
                Projectile.alpha += 255 / 20;
                if (Projectile.alpha >= 255)
                {
                    Projectile.active = false;
                }
            }
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
            int newSize = 25;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }
    }
}


