using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.Items.Weapons.Ranged.Thrown
{
    class CursedJavelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Creates a horizontal deathray upon hitting an enemy");
        }
        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 56;
            Item.height = 56;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 30f;
            Item.shoot = ModContent.ProjectileType<CursedJavelinProjectile>();
        }
    }

    class CursedJavelinProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/Thrown/CursedJavelin";
        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 56;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hide = false;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 3000;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle originalHitbox = hitbox;
            int newSize = 25;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, new Vector2(1, 0), ModContent.ProjectileType<JavelinDeathray>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, new Vector2(-1, 0), ModContent.ProjectileType<JavelinDeathray>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
        }
    }

    class JavelinDeathray : Deathray
    {
        int timeLeft = 20;
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/LightBlast";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.hide = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.timeLeft = timeLeft;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
            MoveDistance = 0f;
            RealMaxDistance = 2000f;
            bodyRect = new Rectangle(0, 0, 10, 10);
            headRect = new Rectangle(0, 0, 10, 10);
            tailRect = new Rectangle(0, 0, 10, 10);
            FollowPosition = false;
            drawColor = new Color(255, 228, 200);
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / timeLeft;
            Projectile.velocity.Normalize();
        }
    }
}