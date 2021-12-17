using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Events;
using Terraria.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.World.Generation;
using Terraria.UI;
using TerrorbornMod.TBUtils;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class ThrowingStar : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.NovagoldBar>());
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 75);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Implodes on impact with tiles, pulling enemies closer to itself");
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 20;
            item.useAnimation = 20;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 0f;
            item.value = Item.sellPrice(0, 0, 0, 20);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = true;
            item.shootSpeed = 15;
            item.shoot = ModContent.ProjectileType<ThrowingStarProjectile>();
            item.width = 30;
            item.height = 30;
        }
    }

    class ThrowingStarProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/ThrowingStar";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.ranged = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 60 * 10;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            if (timeUntilFall > 0)
            {
                Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
                for (int i = 0; i < projectile.oldPos.Length; i++)
                {
                    Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + projectile.Size / 2;
                    Color color = projectile.GetAlpha(Color.LightSkyBlue) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                    Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(45f * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length)), color * 0.5f);
                }
            }
            return true;
        }

        int timeUntilFall = 60 * 2;
        public override void AI()
        {
            int direction = 1;
            if (projectile.velocity.X <= 0)
            {
                direction = -1;
            }
            projectile.rotation += MathHelper.ToRadians(projectile.velocity.Length() * 2 * direction);

            if (timeUntilFall > 0)
            {
                timeUntilFall--;
            }
            else
            {
                projectile.velocity.Y += 0.2f;
            }
        }

        public override void Kill(int timeLeft)
        {

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (timeUntilFall > 0)
            {
                bool actuallyHappened = false;
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.position.X = projectile.position.X + projectile.velocity.X;
                    projectile.velocity.X = -oldVelocity.X * 0.5f;
                    actuallyHappened = true;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                    projectile.velocity.Y = -oldVelocity.Y * 0.5f;
                    actuallyHappened = true;
                }

                if (actuallyHappened)
                {
                    projectile.penetrate = 1;
                    if (projectile.velocity.Y <= 1f && projectile.velocity.Y >= -5f)
                    {
                        projectile.velocity.Y = -5f;
                    }
                    Main.PlaySound(SoundID.Item14, projectile.Center);
                    TerrorbornMod.ScreenShake(1.5f);
                    timeUntilFall = 0;
                    DustExplosion(projectile.Center, 20, 50f, 100f);
                    foreach (NPC npc in Main.npc)
                    {
                        if (!npc.dontTakeDamage && !npc.friendly && npc.active && npc.knockBackResist != 0f && npc.Distance(projectile.Center) <= 300)
                        {
                            npc.velocity = projectile.DirectionFrom(npc.Center) * 10f * npc.knockBackResist;
                        }
                    }
                    return false;
                }
            }
            return true;
        }

        public void DustExplosion(Vector2 position, int dustAmount, float minDistance, float maxDistance)
        {
            Vector2 direction = new Vector2(0, 1);
            for (int i = 0; i < dustAmount; i++)
            {
                Vector2 newPos = position + direction.RotatedBy(MathHelper.ToRadians((360f / dustAmount) * i)) * Main.rand.NextFloat(minDistance, maxDistance);
                Dust dust = Dust.NewDustPerfect(newPos, 15);
                dust.scale = 1f;
                dust.velocity = (position - newPos) / 10;
                dust.noGravity = true;
            }
        }
    }
}
