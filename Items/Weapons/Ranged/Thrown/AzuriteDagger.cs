using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged.Thrown
{
    class AzuriteDagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Throws a dagger that isn't slowed by water" +
                "\nThis dagger will ricochet off of enemies and momentarily linger instead of falling"); */
        }
        public override void SetDefaults()
        {
            Item.damage = 19;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 26;
            Item.height = 32;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.useTime = 15;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 1, 0, 0) / 100;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 15;
            Item.shoot = ModContent.ProjectileType<AzuriteDaggerProjectile>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(111)
                .AddIngredient<Materials.AzuriteBar>()
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    class AzuriteDaggerProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/Thrown/AzuriteDagger";

        int FallWait = 60;
        int leaveWait = 120;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Azurite Dagger");
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hide = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.timeLeft = 3000;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (FallWait <= 0)
            {
                return;
            }
            float speed = Projectile.velocity.Length() / 3;
            Vector2 direction = (Projectile.DirectionTo(target.Center).ToRotation() + MathHelper.ToRadians(180)).ToRotationVector2();
            Projectile.velocity = speed * direction;
            FallWait = 0;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (FallWait <= 0)
            {
                damage = (int)(damage * 0.75f);
            }
        }

        public override void AI()
        {
            if (FallWait > 0)
            {
                FallWait--;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);

                if (Projectile.velocity.X > 0)
                {
                    Projectile.spriteDirection = 1;
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
                }
                else
                {
                    Projectile.spriteDirection = -1;
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
                }
            }
            else
            {
                Projectile.velocity *= 0.98f;
                if (Projectile.velocity.X > 0)
                {
                    Projectile.rotation += MathHelper.ToRadians(20);
                }
                else
                {
                    Projectile.rotation -= MathHelper.ToRadians(20);
                }
                if (leaveWait > 0)
                {
                    leaveWait--;
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
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
        }
    }
}
