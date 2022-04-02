using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Ammo
{
    class IncendiaryArrow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.shootSpeed = 4.5f;
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ModContent.ProjectileType<IncendiaryArrowProjectile>();
            Item.ammo = AmmoID.Arrow;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Waits for a moment before firing" +
                "\nUpon firing it has incredibly high velocity" +
                "\nIgnores half of enemy defense" +
                "\nInflicts hit enemies with a random type of fire");
        }

        public override void AddRecipes()
        {
            CreateRecipe(222)
                .AddRecipeGroup("cobalt")
                .AddTile<Tiles.Incendiary.IncendiaryAltar>()
                .AddIngredient<Materials.IncendiusAlloy>()
                .Register();
        }
    }
    class IncendiaryArrowProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 32;
            Projectile.timeLeft = 360;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.arrow = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 2;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 14;
            height = 14;
            return true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return Countdown <= 0;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 4;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int choice = Main.rand.Next(4);
            if (choice == 0)
            {
                target.AddBuff(BuffID.OnFire, 60 * 5);
            }
            if (choice == 1)
            {
                target.AddBuff(BuffID.Frostburn, 60 * 5);
            }
            if (choice == 2)
            {
                target.AddBuff(BuffID.CursedInferno, 60 * 5);
            }
            if (choice == 3)
            {
                target.AddBuff(BuffID.ShadowFlame, 60 * 5);
            }
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
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
                Color color = Projectile.GetAlpha(Color.White) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
            }
            if (Countdown > 0)
            {
                Color lineColor = Color.FromNonPremultiplied(247, 201, 155, 255 / 2) * 0.25f;
                Utils.DrawLine(Main.spriteBatch, Projectile.Center, Projectile.Center + (Projectile.rotation + MathHelper.ToRadians(90)).ToRotationVector2() * 3000, lineColor, lineColor, 3);
            }
            return false;
        }

        int Countdown = 240;
        Vector2 originalVelocity = Vector2.Zero;
        float rotationOffset;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Countdown == 240)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(90);
                originalVelocity = Projectile.velocity;
                //Projectile.position += originalVelocity * 3;
                Projectile.velocity = Vector2.Zero;

                rotationOffset = Projectile.DirectionTo(new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition).ToRotation() - Projectile.rotation;
            }
            if (Countdown > 0)
            {
                Projectile.rotation = Projectile.DirectionTo(new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition).ToRotation() - rotationOffset;
                Countdown--;
                if (Countdown <= 0)
                {
                    Projectile.velocity = (Projectile.rotation + MathHelper.ToRadians(90)).ToRotationVector2() * originalVelocity.Length() * 2;
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(90);
            }
        }

        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            }
        }

    }
}
