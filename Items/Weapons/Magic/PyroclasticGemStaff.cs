using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using TerrorbornMod.TBUtils;
using TerrorbornMod.Items.Materials;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class PyroclasticGemStaff : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HellstoneBar, 12);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ModContent.ItemType<PyroclasticGemstone>(), 12);
            recipe.AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Fires a pyroclastic bolt that splits at the mouse cursor");
        }
        public override void SetDefaults()
        {
            item.damage = 30;
            item.noMelee = true;
            item.width = 56;
            item.height = 52;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item91;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<PyroclasticBolt>();
            item.shootSpeed = 20f;
            item.mana = 8;
            item.magic = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 50);
            return true;
        }
    }

    class PyroclasticBolt : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Effects/Textures/Glow_2";


        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.timeLeft = 300;
            projectile.magic = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.ai[0] != 0 || bursted)
            {
                projectile.active = false;
            }
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
                List<Vector2> positions = bezier.GetPoints(45);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
                    Color color = projectile.GetAlpha(Color.Lerp(new Color(255, 194, 177), new Color(255, 194, 177), mult)) * mult;
                    Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(25f * mult), color);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        Vector2 target;
        bool start = true;
        bool bursted = false;
        public override void AI()
        {
            if (start)
            {
                start = false;
                target = Main.MouseWorld;
                if (Main.player[projectile.owner].Distance(Main.MouseWorld) <= 50 && projectile.ai[0] == 0)
                {
                    bursted = true;
                    Main.PlaySound(SoundID.Item110, projectile.Center);
                    for (int i = 0; i < Main.rand.Next(3, 5); i++)
                    {
                        int proj = Projectile.NewProjectile(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.8f, 1.2f), projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
                        Main.projectile[proj].ai[0] = 1;
                    }
                }
            }

            if (projectile.ai[0] == 0 && projectile.Distance(target) <= projectile.velocity.Length() && !bursted)
            {
                bursted = true;
                Main.PlaySound(SoundID.Item110, projectile.Center);
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    int proj = Projectile.NewProjectile(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.8f, 1.2f), projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
                    Main.projectile[proj].ai[0] = 1;
                }
            }
        }
    }
}
