using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class SkullPiercer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hitting an enemy causes you to throw an extra dagger" +
                "\nThis can only occur up to 3 times per throw");
        }

        public override void SetDefaults()
        {
            Item.damage = 52;
            Item.width = 42;
            Item.height = 34;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item39;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.shootSpeed = 30;
            Item.shoot = ModContent.ProjectileType<SkullPiercerDagger>();
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 12)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    class SkullPiercerDagger : ModProjectile
    {
        int timeUntilReturn = 30;
        float speed = -1;

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 30;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 20;
            height = 20;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            timeUntilReturn = 0;
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] < 3 && timeUntilReturn > 0)
            {
                Vector2 velocity = Projectile.DirectionFrom(player.Center) * speed;
                Projectile proj = Main.projectile[Projectile.NewProjectile(Projectile.GetProjectileSource_OnHit(target, ProjectileSourceID.None), player.Center, velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner)];
                proj.ai[0] = Projectile.ai[0] + 1;
            }
            timeUntilReturn = 0;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (speed == -1)
            {
                speed = Projectile.velocity.Length();
            }
            Projectile.rotation = Projectile.DirectionFrom(player.Center).ToRotation() + MathHelper.ToRadians(180);

            if (timeUntilReturn > 0)
            {
                timeUntilReturn--;
            }
            else
            {
                Projectile.tileCollide = false;
                Projectile.velocity = Projectile.DirectionTo(player.Center) * speed;
                if (Projectile.Distance(player.Center) <= speed)
                {
                    Projectile.active = false;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 originPoint = Main.player[Projectile.owner].Center;
            Vector2 center = Projectile.Center;
            Vector2 distToProj = originPoint - Projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Items/Weapons/Melee/SkullPiercerChain");

            while (distance > texture.Height && !float.IsNaN(distance))
            {
                distToProj.Normalize();
                distToProj *= texture.Height;
                center += distToProj;
                distToProj = originPoint - center;
                distance = distToProj.Length();


                //Draw chain
                Main.spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, texture.Width, texture.Height), Color.White, projRotation,
                    new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }

            Texture2D texture2 = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture2, new Rectangle((int)position.X, (int)position.Y, texture2.Width, texture2.Height), new Rectangle(0, 0, texture2.Width, texture2.Height), Projectile.GetAlpha(Color.White), Projectile.rotation - MathHelper.ToRadians(90), new Vector2(texture2.Width / 2, texture2.Height / 2), SpriteEffects.None, 0);
            return false;
        }
    }
}