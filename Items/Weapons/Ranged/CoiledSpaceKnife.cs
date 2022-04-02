using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class CoiledSpaceKnife : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Throws a space knife that fires lasers after hitting enemies");
        }
        public override void SetDefaults()
        {
            Item.damage = 14;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 26;
            Item.height = 26;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(0, 1, 0, 0) / 150;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 15;
            Item.shoot = ModContent.ProjectileType<CoiledSpaceKnifeProjectile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(75)
                .AddIngredient(ItemID.MeteoriteBar, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    class CoiledSpaceKnifeProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/CoiledSpaceKnife";
        int FallWait = 60;
        int leaveWait = 30;
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hide = false;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.timeLeft = 3000;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (FallWait > 0) return base.CanHitNPC(target);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (FallWait <= 0)
            {
                return;
            }

            FallWait = 0;
            Projectile.velocity /= 6;
        }

        public override void AI()
        {
            if (FallWait > 0)
            {
                FallWait--;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            }
            else
            {
                Projectile.velocity *= 0.95f;
                if (leaveWait > 0)
                {
                    leaveWait--;
                    if (leaveWait <= 0)
                    {
                        if (Main.rand.NextBool()) Terraria.Audio.SoundEngine.PlaySound(SoundID.Item114, Projectile.Center);
                        else Terraria.Audio.SoundEngine.PlaySound(SoundID.Item115, Projectile.Center);

                        for (int i = 0; i < Main.rand.Next(2, 4); i++)
                        {
                            Vector2 velocity = Projectile.velocity.ToRotation().ToRotationVector2().RotatedByRandom(MathHelper.ToRadians(25));
                            Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<SpaceDeathray>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack / 2f, Projectile.owner);
                        }
                    }
                }
                else
                {
                    Projectile.alpha += 15;
                    if (Projectile.alpha >= 255)
                    {
                        Projectile.active = false;
                    }
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        }
    }

    class SpaceDeathray : Deathray
    {
        int timeLeft = 10;
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/LightBlast";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.hide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = timeLeft;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.arrow = true;
            MoveDistance = 20f;
            RealMaxDistance = 2000f;
            bodyRect = new Rectangle(0, 0, 10, 10);
            headRect = new Rectangle(0, 0, 10, 10);
            tailRect = new Rectangle(0, 0, 10, 10);
            FollowPosition = false;
            drawColor = Color.LightGreen;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
            Projectile.velocity.Normalize();
        }
    }
}

