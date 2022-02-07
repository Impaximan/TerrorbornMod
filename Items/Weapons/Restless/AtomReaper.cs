using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class AtomReaper : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/AtomReaper";
        int UntilBlast;
        public override void restlessSetStaticDefaults()
        {

        }

        public override string defaultTooltip()
        {
            return "Creates an energy orb that returns to you";
        }

        public override string altTooltip()
        {
            return "Throws a scythe that leaves a trail of energy orbs that return to you";
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            item.damage = 80;
            item.melee = true;
            item.width = 56;
            item.height = 56;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item71;
            item.shoot = ModContent.ProjectileType<EnergyOrbProjectile>();
            item.shootSpeed = 20;
            item.crit = 7;
            item.autoReuse = true;
            item.noMelee = false;
            item.noUseGraphic = false;
            modItem.restlessTerrorDrain = 8f;
            modItem.restlessChargeUpUses = 6;
        }

        public override bool RestlessCanUseItem(Player player)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            if (modItem.RestlessChargedUp())
            {
                item.noUseGraphic = true;
                item.noMelee = true;
            }
            else
            {
                item.noUseGraphic = false;
                item.noMelee = false;
            }
            return base.RestlessCanUseItem(player);
        }

        public override bool RestlessShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            if (modItem.RestlessChargedUp())
            {
                item.noUseGraphic = true;
                item.noMelee = true;
                type = ModContent.ProjectileType<AtomReaperThrown>();
                speedX *= 2;
                speedY *= 2;
            }
            else
            {
                item.noUseGraphic = false;
                item.noMelee = false;
                type = ModContent.ProjectileType<EnergyOrbProjectile>();
            }
            return base.RestlessShoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 20);
            recipe.AddIngredient(ItemID.FragmentSolar, 10);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class AtomReaperThrown : ModProjectile
    {
        int timeUntilReturn = 60;
        int penetrateUntilReturn = 3;

        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/AtomReaper";
        public override void SetDefaults()
        {
            projectile.width = 58;
            projectile.height = 62;
            projectile.melee = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
            projectile.timeLeft = 600;
        }

        float speed;
        int projectileWait = 0;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            projectileWait--;
            if (projectileWait <= 0)
            {
                projectileWait = 10;
                Projectile.NewProjectile(projectile.Center, projectile.velocity / 2, ModContent.ProjectileType<EnergyOrbProjectile>(), projectile.damage, 1, projectile.owner);
            }

            if (timeUntilReturn <= 0)
            {
                projectile.rotation += 0.5f * player.direction;
                projectile.tileCollide = false;
                Vector2 direction = projectile.DirectionTo(player.Center);
                projectile.velocity = direction * speed;

                if (Main.player[projectile.owner].Distance(projectile.Center) <= speed)
                {
                    projectile.active = false;
                }
            }
            else
            {
                projectile.direction = player.direction;
                projectile.spriteDirection = player.direction;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135);
                if (projectile.spriteDirection == 1)
                {
                    projectile.rotation -= MathHelper.ToRadians(90f);
                }
                timeUntilReturn--;
                if (timeUntilReturn <= 0)
                {
                    speed = projectile.velocity.Length();
                }
            }
        }
    }

    class EnergyOrbProjectile : ModProjectile
    {
        int timeUntilReturn = 30;
        int penetrateUntilReturn = 3;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 10;
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
                    Color color = projectile.GetAlpha(Color.Lerp(Color.MediumPurple, Color.LightPink, mult)) * mult;
                    TBUtils.Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(50f * mult), color);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/AtomReaper";
        public override void SetDefaults()
        {
            projectile.width = 56;
            projectile.height = 56;
            projectile.melee = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
            projectile.timeLeft = 600;
        }

        float speed;
        int projectileWait = 0;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            projectile.rotation += 0.5f * player.direction;

            if (timeUntilReturn <= 0)
            {
                Vector2 direction = projectile.DirectionTo(player.Center);
                projectile.velocity = direction * speed;

                if (Main.player[projectile.owner].Distance(projectile.Center) <= speed)
                {
                    projectile.active = false;
                }

                int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 21);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = projectile.velocity;
                Main.dust[d].noLight = true;
            }
            else
            {
                projectile.spriteDirection = player.direction;
                timeUntilReturn--;
                if (timeUntilReturn <= 0)
                {
                    speed = projectile.velocity.Length();
                }
            }
        }
    }
}