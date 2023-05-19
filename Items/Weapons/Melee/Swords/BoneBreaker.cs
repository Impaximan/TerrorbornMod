using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Melee.Swords
{
    public class BoneBreaker : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Knocks enemies' bones out on hit" +
                "\nTheir bones will shatter into smaller pieces upon hitting the ground"); */
        }
        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.5f;
            Item.damage = 25;
            Item.DamageType = DamageClass.Melee;
            Item.width = 60;
            Item.height = 56;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7.5f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.DD2_MonkStaffSwing;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<KnockedBone>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundExtensions.PlaySoundOld(SoundID.DD2_SkeletonHurt, target.Center);
            if (hit.Crit)
            {
                SoundExtensions.PlaySoundOld(SoundID.DD2_SkeletonDeath);
            }

            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.Next(10, 23);
                Vector2 velocity = direction * speed;
                Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, velocity, ModContent.ProjectileType<KnockedBone>(), Item.damage / 2, 1f, player.whoAmI);
            }
        }
    }

    public class KnockedBone : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X * 2);
            Projectile.velocity.Y += 0.2f;
        }
        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.NPCHit2, Projectile.Center);

            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.Next(7, 16);
                Vector2 velocity = direction * speed;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<KnockedBonePiece>(), Projectile.damage, 1f, Projectile.owner);
            }
        }
    }

    public class KnockedBonePiece : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X * 2);
            Projectile.velocity.Y += 0.1f;
        }
    }
}
