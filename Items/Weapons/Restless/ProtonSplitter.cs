using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class ProtonSplitter : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/ProtonSplitter";
        int UntilBlast;
        public override void restlessSetStaticDefaults()
        {

        }

        public override string defaultTooltip()
        {
            return "Throws a javelin that sticks into enemies";
        }

        public override string altTooltip()
        {
            return "Throws a more powerful javelin that causes all stuck javelins to turn into returning proton orbs on hit";
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            item.ranged = true;
            item.damage = 100;
            item.width = 80;
            item.height = 80;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.DD2_SkyDragonsFuryShot;
            item.shoot = ModContent.ProjectileType<ProtonSplitterProjectile>();
            item.shootSpeed = 40;
            item.crit = 7;
            item.autoReuse = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            modItem.restlessTerrorDrain = 6f;
            modItem.restlessChargeUpUses = 6;
        }

        public override bool RestlessShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            if (modItem.RestlessChargedUp())
            {
                Vector2 velocity = new Vector2(speedX, speedY);
                int proj = Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
                Main.projectile[proj].ai[1] = 1;
            }
            else
            {
                Vector2 velocity = new Vector2(speedX, speedY);
                int proj = Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 20);
            recipe.AddIngredient(ItemID.FragmentVortex, 10);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class ProtonSplitterProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/ProtonSplitter";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 80;
            projectile.height = 80;
            projectile.ranged = true;
            projectile.timeLeft = 3600;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.extraUpdates = 8;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.arrow = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 15;
            height = 15;
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!stuck)
            {
                Main.PlaySound(SoundID.Dig, projectile.position);
                stuck = true;
                stuckNPC = target;
                offset = target.position - projectile.position;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (stuck)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle originalHitbox = hitbox;
            int newSize = 15;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.ai[1] == 1)
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
            }
            return true;
        }

        bool stuck = false;
        NPC stuckNPC;
        Vector2 offset;
        bool start = true;
        int projectileCounter = 0;
        public override void AI()
        {
            if (start)
            {
                start = false;
                projectile.velocity /= projectile.extraUpdates + 1;
                projectile.timeLeft *= projectile.extraUpdates + 1;
            }

            if (stuck)
            {
                projectile.ai[0] = 1;

                projectile.tileCollide = false;
                projectile.position = stuckNPC.position - offset;
                if (!stuckNPC.active)
                {
                    projectile.active = false;
                }

                List<Projectile> splitters = new List<Projectile>();
                foreach (Projectile splitter in Main.projectile)
                {
                    if (splitter.active && splitter.type == projectile.type && splitter.ai[0] == 1)
                    {
                        splitters.Add(splitter);
                    }
                }

                if (projectile.ai[1] == 1)
                {
                    foreach (Projectile splitter in splitters)
                    {
                        splitter.active = false;

                        Projectile.NewProjectile(splitter.Center, Vector2.Zero, ModContent.ProjectileType<ProtonOrb>(), projectile.damage, projectile.knockBack * 3, projectile.owner);
                    }
                    Main.PlaySound(SoundID.Item62, projectile.Center);
                    TerrorbornMod.ScreenShake(3f);
                }
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
            }
        }
    }

    class ProtonOrb : ModProjectile
    {
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
            projectile.localNPCHitCooldown = 5;
            projectile.timeLeft = 600;
        }

        float speed = 0f;
        public override void AI()
        {
            speed += 0.3f;
            Player player = Main.player[projectile.owner];

            projectile.rotation += 0.5f * player.direction;

            Vector2 direction = projectile.DirectionTo(player.Center);
            projectile.velocity = direction * speed;

            if (Main.player[projectile.owner].Distance(projectile.Center) <= speed)
            {
                projectile.active = false;
            }
        }
    }
}