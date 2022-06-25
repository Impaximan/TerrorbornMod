using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;

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
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 100;
            Item.width = 80;
            Item.height = 80;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.DD2_SkyDragonsFuryShot;
            Item.shoot = ModContent.ProjectileType<ProtonSplitterProjectile>();
            Item.shootSpeed = 40;
            Item.crit = 7;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            modItem.restlessTerrorDrain = 6f;
            modItem.restlessChargeUpUses = 6;
        }

        public override bool RestlessShoot(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            if (modItem.RestlessChargedUp())
            {
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                Main.projectile[proj].ai[1] = 1;
            }
            else
            {
                int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 20)
                .AddIngredient(ItemID.FragmentVortex, 10)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    class ProtonSplitterProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/ProtonSplitter";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 3600;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.extraUpdates = 8;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.arrow = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 15;
            height = 15;
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!stuck)
            {
                SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
                stuck = true;
                stuckNPC = target;
                offset = target.position - Projectile.position;
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
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

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[1] == 1)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

                BezierCurve bezier = new BezierCurve();
                bezier.Controls.Clear();
                foreach (Vector2 pos in Projectile.oldPos)
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
                        Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                        Color color = Projectile.GetAlpha(Color.Lerp(Color.MediumPurple, Color.LightPink, mult)) * mult;
                        TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(50f * mult), color);
                    }
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            }
            return true;
        }

        bool stuck = false;
        NPC stuckNPC;
        Vector2 offset;
        bool start = true;
        int ProjectileCounter = 0;
        public override void AI()
        {
            if (start)
            {
                start = false;
                Projectile.velocity /= Projectile.extraUpdates + 1;
                Projectile.timeLeft *= Projectile.extraUpdates + 1;
            }

            if (stuck)
            {
                Projectile.ai[0] = 1;

                Projectile.tileCollide = false;
                Projectile.position = stuckNPC.position - offset;
                if (!stuckNPC.active)
                {
                    Projectile.active = false;
                }

                List<Projectile> splitters = new List<Projectile>();
                foreach (Projectile splitter in Main.projectile)
                {
                    if (splitter.active && splitter.type == Projectile.type && splitter.ai[0] == 1)
                    {
                        splitters.Add(splitter);
                    }
                }

                if (Projectile.ai[1] == 1)
                {
                    foreach (Projectile splitter in splitters)
                    {
                        splitter.active = false;

                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), splitter.Center, Vector2.Zero, ModContent.ProjectileType<ProtonOrb>(), Projectile.damage, Projectile.knockBack * 3, Projectile.owner);
                    }
                    SoundExtensions.PlaySoundOld(SoundID.Item62, Projectile.Center);
                    TerrorbornSystem.ScreenShake(3f);
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
            }
        }
    }

    class ProtonOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }


        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            BezierCurve bezier = new BezierCurve();
            bezier.Controls.Clear();
            foreach (Vector2 pos in Projectile.oldPos)
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
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.MediumPurple, Color.LightPink, mult)) * mult;
                    TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(50f * mult), color);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/AtomReaper";
        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 56;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.timeLeft = 600;
        }

        float speed = 0f;
        public override void AI()
        {
            speed += 0.3f;
            Player player = Main.player[Projectile.owner];

            Projectile.rotation += 0.5f * player.direction;

            Vector2 direction = Projectile.DirectionTo(player.Center);
            Projectile.velocity = direction * speed;

            if (Main.player[Projectile.owner].Distance(Projectile.Center) <= speed)
            {
                Projectile.active = false;
            }
        }
    }
}