using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Projectiles;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class NewPerception : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Tooltip.SetDefault("Casts strange eyes that eventually fire rays of light at your cursor");
        }
        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.noMelee = true;
            Item.width = 62;
            Item.height = 62;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 25, 0, 0);
            Item.rare = ModContent.RarityType<Rarities.Golden>();
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<NewPerceptionEye>();
            Item.shootSpeed = 7.5f;
            Item.mana = 30;
            Item.DamageType = DamageClass.Magic;;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 50);
            for (int i = 0; i < Main.rand.Next(3, 5); i++)
            {
                Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.8f, 1.2f), type, damage, knockback, player.whoAmI, 1);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.DreadfulEssence>(), 3)
                .AddIngredient(ItemID.LunarBar, 10)
                .AddIngredient(ModContent.ItemType<PearlyEyedStaff>())
                .AddIngredient(ItemID.FragmentNebula, 12)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }

    class NewPerceptionEye : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        int trueTimeLeft = 120;
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.timeLeft = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Utils.Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, (int)(25 * Projectile.scale), Color.LightGoldenrodYellow);
            return true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        int frameWait = 0;
        public override void AI()
        {
            Projectile.timeLeft = 2;
            if (trueTimeLeft > 30)
            {
                frameWait++;
                if (frameWait > 15)
                {
                    frameWait = 0;
                    Projectile.frame++;
                    if (Projectile.frame >= 3)
                    {
                        Projectile.frame = 2;
                    }
                }
                trueTimeLeft--;
                Projectile.velocity *= 0.98f;
                Projectile.rotation = Projectile.DirectionTo(Main.MouseWorld).ToRotation();
                if (trueTimeLeft == 30)
                {
                    Vector2 velocity = Projectile.DirectionTo(Main.MouseWorld);
                    velocity.Normalize();
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<PerceptiveRay>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Projectile.scale = 1.5f;
                    SoundExtensions.PlaySoundOld(SoundID.Item68, Projectile.Center);
                }
            }
            else if (trueTimeLeft > 0)
            {
                Projectile.rotation = Projectile.rotation.AngleTowards(Projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(1f));
                trueTimeLeft--;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale -= 1.5f / 30f;
            }
            else
            {
                Projectile.active = false;
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
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.hide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.timeLeft = timeLeft;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
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
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(1)).ToRotationVector2() * Projectile.velocity.Length();
        }
    }
}