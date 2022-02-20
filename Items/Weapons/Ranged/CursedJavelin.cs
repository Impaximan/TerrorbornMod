using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class CursedJavelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates a horizontal deathray upon hitting an enemy");
        }
        public override void SetDefaults()
        {
            item.damage = 35;
            item.ranged = true;
            item.width = 56;
            item.height = 56;
            item.useTime = 20;
            item.useAnimation = 20;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 0, 2, 0);
            item.rare = ItemRarityID.LightRed;
            item.UseSound = SoundID.Item1;
            item.consumable = true;
            item.maxStack = 9999;
            item.autoReuse = true;
            item.noMelee = true;
            item.shootSpeed = 30f;
            item.shoot = ModContent.ProjectileType<CursedJavelinProjectile>();
        }
    }

    class CursedJavelinProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/CursedJavelin";
        int FallWait = 40;
        public override void SetDefaults()
        {
            projectile.width = 56;
            projectile.height = 56;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 4;
            projectile.ranged = true;
            projectile.hide = false;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.timeLeft = 3000;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectile(target.Center, new Vector2(1, 0), ModContent.ProjectileType<JavelinDeathray>(), projectile.damage / 3, projectile.knockBack, projectile.owner);
            Projectile.NewProjectile(target.Center, new Vector2(-1, 0), ModContent.ProjectileType<JavelinDeathray>(), projectile.damage / 3, projectile.knockBack, projectile.owner);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (FallWait <= 0)
            {
                damage = (int)(damage * 0.75f);
            }
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Main.PlaySound(SoundID.Dig, projectile.position);
        }
    }

    class JavelinDeathray : Deathray
    {
        int timeLeft = 20;
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/LightBlast";
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.hide = false;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.friendly = true;
            projectile.timeLeft = timeLeft;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 5;
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
            deathrayWidth -= 1f / (float)timeLeft;
            projectile.velocity.Normalize();
        }
    }
}