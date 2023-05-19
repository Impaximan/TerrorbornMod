using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Projectiles;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Magic.Staffs
{
    class PearlyEyedStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            Tooltip.SetDefault("Casts a light orb that follows the cursor" +
                "\nThe orb will explode into powerful beams of light after 2 seconds");
        }
        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.noMelee = true;
            Item.width = 46;
            Item.height = 46;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LightSingularity>();
            Item.shootSpeed = 1f;
            Item.mana = 8;
            Item.DamageType = DamageClass.Magic; ;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = player.Center + player.DirectionTo(Main.MouseWorld) * 50;
            Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI, 1);
            return false;
        }
    }

    class LightSingularity : ModProjectile
    {
        int trueTimeLeft = 120;
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.timeLeft = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Utils.Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, (int)(30 * Projectile.scale), Color.White);
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            Projectile.timeLeft = 2;
            if (trueTimeLeft > 10)
            {
                trueTimeLeft--;
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(6)).ToRotationVector2() * Projectile.velocity.Length();
                if (trueTimeLeft == 10)
                {
                    for (int i = 0; i < Main.rand.Next(2, 4); i++)
                    {
                        Vector2 velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15));
                        velocity.Normalize();
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<LightBlast>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                    Projectile.scale = 3f;
                    SoundExtensions.PlaySoundOld(SoundID.Item68, Projectile.Center);
                }
            }
            else if (trueTimeLeft > 0)
            {
                trueTimeLeft--;
                Projectile.velocity = Vector2.Zero;
                Projectile.scale -= 3f / 10f;
            }
            else
            {
                Projectile.active = false;
            }
        }
    }

    class LightBlast : Deathray
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
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.timeLeft = timeLeft;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            MoveDistance = 20f;
            RealMaxDistance = 2000f;
            bodyRect = new Rectangle(0, 0, 10, 10);
            headRect = new Rectangle(0, 0, 10, 10);
            tailRect = new Rectangle(0, 0, 10, 10);
            FollowPosition = false;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / timeLeft;
        }
    }
}

