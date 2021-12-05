using System;
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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(25 * TerrorbornMod.IncendiaryAlloyMultiplier));
            recipe.AddIngredient(ItemID.CobaltBar, 15);
            recipe.AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>());
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(25 * TerrorbornMod.IncendiaryAlloyMultiplier));
            recipe2.AddIngredient(ItemID.PalladiumBar, 15);
            recipe.AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>());
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purgatory Chakram");
            Tooltip.SetDefault("Throws a chakram that returns to you" +
                "\nIf a chakram hits the same enemy twice, it will home into that enemy for a few seconds, hitting it repeatedly");
        }
        public override void SetDefaults()
        {
            item.damage = 33;
            item.width = 54;
            item.height = 54;
            item.useTime = 20;
            item.useAnimation = 20;
            item.rare = 4;
            item.useStyle = 1;
            item.knockBack = 3f;
            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(0, 0, 60, 0);
            item.shootSpeed = 35;
            item.shoot = ModContent.ProjectileType<IncendiusChakram_projectile>();
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.melee = true;
            item.noMelee = true;
        }
    }
    class IncendiusChakram_projectile : ModProjectile
    {
        public override string Texture { get { return "TerrorbornMod/Items/Incendius/IncendiusChakram"; } }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 8;
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
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                if (homing)
                {
                    TBUtils.Graphics.DrawGlow_1(spriteBatch, drawPos, 60, Color.OrangeRed * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length) * 0.5f);
                }
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
            }
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 30;
            height = 30;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.width = 54;
            projectile.height = 54;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 8;
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
                    projectile.tileCollide = false;
                    projectile.penetrate = 3;
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
            Main.PlaySound(0, projectile.position); //Sound for when it hits a block

            // B O U N C E
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        int TimeUntilReturn = 15;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            projectile.spriteDirection = player.direction * -1;
            projectile.rotation += 0.5f * player.direction;
            if (homing)
            {
                Vector2 targetPosition = firstEnemy.Center;
                float speed = 1f;
                projectile.velocity += projectile.DirectionTo(targetPosition) * speed;
                projectile.velocity *= 0.98f;
                if (firstEnemy.active == false)
                {
                    projectile.active = false;
                }
            }
            else if (TimeUntilReturn <= 0)
            {
                projectile.tileCollide = false;
                Vector2 targetPosition = Main.player[projectile.owner].Center;
                projectile.velocity = (projectile.velocity.Length() + 1) * projectile.DirectionTo(targetPosition);
                if (Main.player[projectile.owner].Distance(projectile.Center) <= projectile.velocity.Length())
                {
                    projectile.active = false;
                }
            }
            else
            {
                TimeUntilReturn--;
                if (TimeUntilReturn <= 0)
                {
                    projectile.velocity = Vector2.Zero;
                }
            }
        }
    }
}

