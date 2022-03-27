using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class NewPerception : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Casts strange eyes that eventually fire rays of light at your cursor");
        }
        public override void SetDefaults()
        {
            item.damage = 80;
            item.noMelee = true;
            item.width = 62;
            item.height = 62;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 25, 0, 0);
            item.rare = 12;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<NewPerceptionEye>();
            item.shootSpeed = 7.5f;
            item.mana = 30;
            item.magic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 50);
            for (int i = 0; i < Main.rand.Next(3, 5); i++)
            {
                Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.8f, 1.2f), type, damage, knockBack, item.owner, 1);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.DreadfulEssence>(), 3);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class NewPerceptionEye : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        int trueTimeLeft = 120;
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.timeLeft = 2;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            TBUtils.Graphics.DrawGlow_1(spriteBatch, projectile.Center - Main.screenPosition, (int)(25 * projectile.scale), Color.LightGoldenrodYellow);
            return true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        int frameWait = 0;
        public override void AI()
        {
            projectile.timeLeft = 2;
            if (trueTimeLeft > 30)
            {
                frameWait++;
                if (frameWait > 15)
                {
                    frameWait = 0;
                    projectile.frame++;
                    if (projectile.frame >= 3)
                    {
                        projectile.frame = 2;
                    }
                }
                trueTimeLeft--;
                projectile.velocity *= 0.98f;
                projectile.rotation = projectile.DirectionTo(Main.MouseWorld).ToRotation();
                if (trueTimeLeft == 30)
                {
                    Vector2 velocity = projectile.DirectionTo(Main.MouseWorld);
                    velocity.Normalize();
                    Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<PerceptiveRay>(), projectile.damage, projectile.knockBack, projectile.owner);
                    projectile.scale = 1.5f;
                    Main.PlaySound(SoundID.Item68, projectile.Center);
                }
            }
            else if (trueTimeLeft > 0)
            {
                projectile.rotation = projectile.rotation.AngleTowards(projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(1f));
                trueTimeLeft--;
                projectile.velocity = Vector2.Zero;
                projectile.scale -= 1.5f / 30f;
            }
            else
            {
                projectile.active = false;
            }
        }
    }

    class PerceptiveRay : Deathray
    {
        int timeLeft = 30;
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/LightBlast";

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 60);
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.hide = false;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = timeLeft;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            MoveDistance = 20f;
            RealMaxDistance = 4000f;
            drawColor = Color.LightGoldenrodYellow;
            bodyRect = new Rectangle(0, 0, 10, 10);
            headRect = new Rectangle(0, 0, 10, 10);
            tailRect = new Rectangle(0, 0, 10, 10);
            FollowPosition = false;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
            projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(1)).ToRotationVector2() * projectile.velocity.Length();
        }
    }
}