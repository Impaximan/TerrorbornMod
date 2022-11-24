using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Incendius
{
    class IncendiusChakram : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(25 * TerrorbornMod.IncendiaryAlloyMultiplier))
                .AddRecipeGroup("cobalt", 15)
                .AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>())
                .Register();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purgatory Chakram");
            Tooltip.SetDefault("Throws a chakram that returns to you" +
                "\nIf a chakram hits the same enemy twice, it will home into that enemy for a few seconds, hitting it repeatedly");
        }
        public override void SetDefaults()
        {
            Item.damage = 25;
            Item.width = 54;
            Item.height = 54;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 0, 60, 0);
            Item.shootSpeed = 35;
            Item.shoot = ModContent.ProjectileType<IncendiusChakram_Projectile>();
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
        }
    }
    class IncendiusChakram_Projectile : ModProjectile
    {
        public override string Texture { get { return "TerrorbornMod/Items/Incendius/IncendiusChakram"; } }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 8;
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
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                if (homing)
                {
                    Utils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, 60, Color.OrangeRed * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length) * 0.5f);
                }
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
            }
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 30;
            height = 30;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 54;
            Projectile.height = 54;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        NPC firstEnemy;
        bool foundFirst = false;
        bool homing = false;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (foundFirst)
            {
                if (target == firstEnemy && !homing)
                {
                    homing = true;
                    Projectile.tileCollide = false;
                    Projectile.penetrate = 3;
                }
            }
            else
            {
                foundFirst = true;
                firstEnemy = target;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position); //Sound for when it hits a block

            // B O U N C E
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        int TimeUntilReturn = 15;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.spriteDirection = player.direction * -1;
            Projectile.rotation += 0.5f * player.direction;
            if (homing)
            {
                Vector2 targetPosition = firstEnemy.Center;
                float speed = 1f;
                Projectile.velocity += Projectile.DirectionTo(targetPosition) * speed;
                Projectile.velocity *= 0.98f;
                if (firstEnemy.active == false)
                {
                    Projectile.active = false;
                }
            }
            else if (TimeUntilReturn <= 0)
            {
                Projectile.tileCollide = false;
                Vector2 targetPosition = Main.player[Projectile.owner].Center;
                Projectile.velocity = (Projectile.velocity.Length() + 1) * Projectile.DirectionTo(targetPosition);
                if (Main.player[Projectile.owner].Distance(Projectile.Center) <= Projectile.velocity.Length())
                {
                    Projectile.active = false;
                }
            }
            else
            {
                TimeUntilReturn--;
                if (TimeUntilReturn <= 0)
                {
                    Projectile.velocity = Vector2.Zero;
                }
            }
        }
    }
}

