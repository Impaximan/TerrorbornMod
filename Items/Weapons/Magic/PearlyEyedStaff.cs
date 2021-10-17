using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class PearlyEyedStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Casts a light orb that follows the cursor" +
                "\nThe orb will explode into powerful beams of light after 2 seconds");
        }
        public override void SetDefaults()
        {
            item.damage = 15;
            item.noMelee = true;
            item.width = 46;
            item.height = 46;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<LightSingularity>();
            item.shootSpeed = 1f;
            item.mana = 8;
            item.magic = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 50);
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, item.owner, 1);
            return false;
        }
    }

    class LightSingularity : ModProjectile
    {
        int trueTimeLeft = 120;
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
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
            TBUtils.Graphics.DrawGlow_1(spriteBatch, projectile.Center - Main.screenPosition, (int)(30 * projectile.scale), Color.White);
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
            projectile.timeLeft = 2;
            if (trueTimeLeft > 10)
            {
                trueTimeLeft--;
                projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(3)).ToRotationVector2() * projectile.velocity.Length();
                if (trueTimeLeft == 10)
                {
                    for (int i = 0; i < Main.rand.Next(2, 4); i++)
                    {
                        Vector2 velocity = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15));
                        velocity.Normalize();
                        Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<LightBlast>(), projectile.damage, projectile.knockBack, projectile.owner);
                    }
                    projectile.scale = 3f;
                    Main.PlaySound(SoundID.Item68, projectile.Center);
                }
            }
            else if (trueTimeLeft > 0)
            {
                trueTimeLeft--;
                projectile.velocity = Vector2.Zero;
                projectile.scale -= 3f / 10f;
            }
            else
            {
                projectile.active = false;
            }
        }
    }


    class LightBlast : Deathray
    {
        int timeLeft = 10;
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/LightBlast";
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
            projectile.localNPCHitCooldown = -1;
            MoveDistance = 20f;
            RealMaxDistance = 2000f;
            bodyRect = new Rectangle(0, 0, 10, 10);
            headRect = new Rectangle(0, 0, 10, 10);
            tailRect = new Rectangle(0, 0, 10, 10);
            FollowPosition = false;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
        }
    }
}

