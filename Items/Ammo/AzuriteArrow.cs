using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Ammo
{
    class AzuriteArrow : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 11;
            item.ranged = true;
            item.width = 14;
            item.height = 32;
            item.maxStack = 9999;
            item.consumable = true;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 0, 0, 20);
            item.shootSpeed = 3;
            item.rare = ItemRarityID.Green;
            item.shoot = ModContent.ProjectileType<AzuriteArrowProjectile>();
            item.ammo = AmmoID.Arrow;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates an azure orb that returns to you after a moment");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.AzuriteBar>());
            recipe.AddIngredient(ItemID.WoodenArrow, 111);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
        }
    }

    class AzuriteArrowProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Ammo/AzuriteArrow";
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 32;
            projectile.ranged = true;
            projectile.timeLeft = 3600;
            projectile.penetrate = 3;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.arrow = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 14;
            height = 14;
            return true;
        }

        public override void Kill(int timeLeft)
        {
            if (timeUntilOrb > 0)
            {
                DustExplosion(projectile.Center, 10, 5f, 10f);
                Main.PlaySound(SoundID.Item92, projectile.Center);
                projectile.velocity /= 2;
                Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<AzureOrb>(), projectile.damage / 4, projectile.knockBack / 10f, projectile.owner);
            }
            Main.PlaySound(SoundID.Dig, projectile.position);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.Width = 14;
            hitbox.Height = 14;
            hitbox.Y += (int)(32f * (14f / 32f) / 2f);
            base.ModifyDamageHitbox(ref hitbox);
        }

        public void DustExplosion(Vector2 position, int dustAmount, float minDistance, float maxDistance)
        {
            Vector2 direction = new Vector2(0, 1);
            for (int i = 0; i < dustAmount; i++)
            {
                Vector2 newPos = position + direction.RotatedBy(MathHelper.ToRadians((360f / dustAmount) * i)) * Main.rand.NextFloat(minDistance, maxDistance);
                Dust dust = Dust.NewDustPerfect(newPos, 88);
                dust.scale = 1f;
                dust.velocity = (newPos - position) / 10;
                dust.noGravity = true;
            }
        }

        int timeUntilOrb = 45;
        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
                projectile.velocity *= 1.5f;
            }

            if (timeUntilOrb > 0)
            {
                timeUntilOrb--;
                if (timeUntilOrb <= 0)
                {
                    DustExplosion(projectile.Center, 10, 5f, 10f);
                    Main.PlaySound(SoundID.Item92, projectile.Center);
                    projectile.velocity /= 2;
                    Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<AzureOrb>(), projectile.damage / 4, projectile.knockBack / 10f, projectile.owner);
                }
            }

            projectile.rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(90);
        }
    }

    class AzureOrb : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            BezierCurve bezier = new BezierCurve();
            bezier.Controls.Clear();
            foreach (Vector2 pos in projectile.oldPos)
            {
                if (pos != Vector2.Zero && pos != null)
                {
                    bezier.Controls.Add(pos);
                }
            }

            if (bezier.Controls.Count > 1)
            {
                List<Vector2> positions = bezier.GetPoints(50);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
                    Color color = projectile.GetAlpha(Color.Lerp(Color.Azure, Color.Azure, mult)) * mult;
                    TBUtils.Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(15f * mult), color);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            int speed = 12;
            projectile.velocity = projectile.DirectionTo(player.Center) * speed;
            if (projectile.Distance(player.Center) <= speed)
            {
                projectile.active = false;
            }
        }
    }
}