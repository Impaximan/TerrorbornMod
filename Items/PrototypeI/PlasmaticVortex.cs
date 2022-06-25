using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.PrototypeI
{
    class PlasmaticVortex : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.PlasmaliumBar>(), 12)
                .AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Slowly returns to you upon hitting an enemy, dealing numerous hits per attack");
        }
        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.width = 102;
            Item.height = 82;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.rare = ItemRarityID.Cyan;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 0f;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 16, 0, 0);
            Item.shootSpeed = 50;
            Item.shoot = ModContent.ProjectileType<PlasmaticVortex_Projectile>();
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
        }
    }
    class PlasmaticVortex_Projectile : ModProjectile
    {
        public override string Texture { get { return "TerrorbornMod/Items/PrototypeI/PlasmaticVortex"; } }

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
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("TerrorbornMod/Items/PrototypeI/PlasmaticVortex").Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
                color = Projectile.GetAlpha(Color.White) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Items/PrototypeI/PlasmaticVortex_Glow"), drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
            }
            return true;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 102;
            Projectile.height = 82;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 30;
            height = 30;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
            TimeUntilReturn = 0;
            speed = 0;
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (TimeUntilReturn > 0)
            {
                TimeUntilReturn = 0;
                speed = 0;
            }
        }

        int TimeUntilReturn = 22;
        float speed = 0;
        bool Start = true;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.spriteDirection = player.direction * -1;
            Projectile.rotation += 0.5f * player.direction;
            if (TimeUntilReturn <= 0)
            {
                Projectile.tileCollide = false;
                Vector2 direction = Projectile.DirectionTo(player.Center);
                speed += 0.3f;
                Projectile.velocity = direction * speed;

                if (Main.player[Projectile.owner].Distance(Projectile.Center) <= speed)
                {
                    Projectile.active = false;
                }
            }
            else
            {
                TimeUntilReturn--;
                if (TimeUntilReturn <= 0)
                {
                    speed = Projectile.velocity.Length();
                }
            }
        }
    }
}

