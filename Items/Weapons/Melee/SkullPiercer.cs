using System;
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
            item.damage = 52;
            item.width = 42;
            item.height = 34;
            item.useTime = 15;
            item.useAnimation = 15;
            item.rare = 8;
            item.useStyle = 1;
            item.knockBack = 3f;
            item.UseSound = SoundID.Item39;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.shootSpeed = 30;
            item.shoot = ModContent.ProjectileType<SkullPiercerDagger>();
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.melee = true;
            item.noMelee = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SkullmoundBar>(), 12);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class SkullPiercerDagger : ModProjectile
    {
        int timeUntilReturn = 30;
        float speed = -1;

        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.width = 30;
            projectile.height = 42;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 20;
            height = 20;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            timeUntilReturn = 0;
            Main.PlaySound(0, projectile.position);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.ai[0] < 3 && timeUntilReturn > 0)
            {
                Vector2 velocity = projectile.DirectionFrom(player.Center) * speed;
                Projectile proj = Main.projectile[Projectile.NewProjectile(player.Center, velocity, projectile.type, projectile.damage, projectile.knockBack, projectile.owner)];
                proj.ai[0] = projectile.ai[0] + 1;
            }
            timeUntilReturn = 0;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (speed == -1)
            {
                speed = projectile.velocity.Length();
            }
            projectile.rotation = projectile.DirectionFrom(player.Center).ToRotation() + MathHelper.ToRadians(180);

            if (timeUntilReturn > 0)
            {
                timeUntilReturn--;
            }
            else
            {
                projectile.tileCollide = false;
                projectile.velocity = projectile.DirectionTo(player.Center) * speed;
                if (projectile.Distance(player.Center) <= speed)
                {
                    projectile.active = false;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 originPoint = Main.player[projectile.owner].Center;
            Vector2 center = projectile.Center;
            Vector2 distToProj = originPoint - projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            Texture2D texture = ModContent.GetTexture("TerrorbornMod/Items/Weapons/Melee/SkullPiercerChain");

            while (distance > texture.Height && !float.IsNaN(distance))
            {
                distToProj.Normalize();
                distToProj *= texture.Height;
                center += distToProj;
                distToProj = originPoint - center;
                distance = distToProj.Length();


                //Draw chain
                spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, texture.Width, texture.Height), Color.White, projRotation,
                    new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }

            Texture2D texture2 = Main.projectileTexture[projectile.type];
            Vector2 position = projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture2, new Rectangle((int)position.X, (int)position.Y, texture2.Width, texture2.Height), new Rectangle(0, 0, texture2.Width, texture2.Height), projectile.GetAlpha(Color.White), projectile.rotation - MathHelper.ToRadians(90), new Vector2(texture2.Width / 2, texture2.Height / 2), SpriteEffects.None, 0);
            return false;
        }
    }
}