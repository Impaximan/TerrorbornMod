using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;

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
            Item.damage = 80;
            Item.DamageType = DamageClass.Melee;
            Item.width = 56;
            Item.height = 56;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item71;
            Item.shoot = ModContent.ProjectileType<EnergyOrbProjectile>();
            Item.shootSpeed = 20;
            Item.crit = 7;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.noUseGraphic = false;
            modItem.restlessTerrorDrain = 8f;
            modItem.restlessChargeUpUses = 6;
        }

        public override bool RestlessCanUseItem(Player player)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            if (modItem.RestlessChargedUp())
            {
                Item.noUseGraphic = true;
                Item.noMelee = true;
            }
            else
            {
                Item.noUseGraphic = false;
                Item.noMelee = false;
            }
            return base.RestlessCanUseItem(player);
        }

        public override bool RestlessShoot(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            if (modItem.RestlessChargedUp())
            {
                Item.noUseGraphic = true;
                Item.noMelee = true;
                type = ModContent.ProjectileType<AtomReaperThrown>();
                velocity.X *= 2;
                velocity.Y *= 2;
            }
            else
            {
                Item.noUseGraphic = false;
                Item.noMelee = false;
                type = ModContent.ProjectileType<EnergyOrbProjectile>();
            }
            return base.RestlessShoot(player, source, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.FusionFragment>(), 20)
                .AddIngredient(ItemID.FragmentSolar, 10)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    class AtomReaperThrown : ModProjectile
    {
        int timeUntilReturn = 60;
        int penetrateUntilReturn = 3;

        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/AtomReaper";
        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 62;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 600;
        }

        float speed;
        int ProjectileWait = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            ProjectileWait--;
            if (ProjectileWait <= 0)
            {
                ProjectileWait = 10;
                Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, Projectile.velocity / 2, ModContent.ProjectileType<EnergyOrbProjectile>(), Projectile.damage, 1, Projectile.owner);
            }

            if (timeUntilReturn <= 0)
            {
                Projectile.rotation += 0.5f * player.direction;
                Projectile.tileCollide = false;
                Vector2 direction = Projectile.DirectionTo(player.Center);
                Projectile.velocity = direction * speed;

                if (Main.player[Projectile.owner].Distance(Projectile.Center) <= speed)
                {
                    Projectile.active = false;
                }
            }
            else
            {
                Projectile.direction = player.direction;
                Projectile.spriteDirection = player.direction;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135);
                if (Projectile.spriteDirection == 1)
                {
                    Projectile.rotation -= MathHelper.ToRadians(90f);
                }
                timeUntilReturn--;
                if (timeUntilReturn <= 0)
                {
                    speed = Projectile.velocity.Length();
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
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 600;
        }

        float speed;
        int ProjectileWait = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.rotation += 0.5f * player.direction;

            if (timeUntilReturn <= 0)
            {
                Vector2 direction = Projectile.DirectionTo(player.Center);
                Projectile.velocity = direction * speed;

                if (Main.player[Projectile.owner].Distance(Projectile.Center) <= speed)
                {
                    Projectile.active = false;
                }

                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 21);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = Projectile.velocity;
                Main.dust[d].noLight = true;
            }
            else
            {
                Projectile.spriteDirection = player.direction;
                timeUntilReturn--;
                if (timeUntilReturn <= 0)
                {
                    speed = Projectile.velocity.Length();
                }
            }
        }
    }
}