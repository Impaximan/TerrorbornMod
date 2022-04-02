using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class TidalClaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Throws a claw that will remain on the ground after landing\nClaws will stick into enemies they hit for repeated damage\nOnly the first hit will do full damage, repeated damage does only\n1/4 of the base damage");
        }
        public override void SetDefaults()
        {
            Item.shootSpeed = 20f;
            Item.damage = 28;
            Item.knockBack = 5f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.width = 16;
            Item.height = 18;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Green;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Melee;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 0, 0, 5);
            Item.shoot = ModContent.ProjectileType<TidalClawProjectile>();
        }
    }
    class TidalClawProjectile : ModProjectile
    {
        bool HasStuck = false;
        int TrueTimeleft = 420;
        NPC stuckTo;
        Vector2 RelativePosition;
        int StrikeWait = 120;
        public override string Texture => "TerrorbornMod/Items/Weapons/Melee/TidalClaw";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tidal Claw");
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 10000;
            Projectile.hide = false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= 0f;
            if (HasStuck)
            {
                Projectile.active = false;
            }
            return HasStuck;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!HasStuck)
            {
                Projectile.damage /= 4;
                HasStuck = true;
                stuckTo = target;
                RelativePosition = new Vector2(target.position.X - Projectile.position.X, target.position.Y - Projectile.position.Y);
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            return !HasStuck;
        }
        public override void AI()
        {
            TrueTimeleft--;
            if (TrueTimeleft <= 0)
            {
                Projectile.alpha += 10;
                if (Projectile.alpha >= 255)
                {
                    Projectile.active = false;
                }
            }
            if (!HasStuck)
            {
                Projectile.velocity.X *= 0.95f;
                Projectile.velocity.Y *= 0.98f;
                Projectile.velocity.Y += 0.25f;
                Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X * 2);
            }
            else
            {
                StrikeWait--;
                if (StrikeWait <= 0)
                {
                    StrikeWait = 120;
                    stuckTo.StrikeNPC(Projectile.damage, 0, 0);
                }
                Projectile.position.X = stuckTo.position.X - RelativePosition.X;
                Projectile.position.Y = stuckTo.position.Y - RelativePosition.Y;
                if (stuckTo.life <= 0 || !stuckTo.active)
                {
                    Projectile.active = false;
                }
            }
        }
    }
}

