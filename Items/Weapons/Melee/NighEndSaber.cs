using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Melee
{
    public class NighEndSaber : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hitting enemies on left click grants you terror" +
                "\nRight click to consume 25% terror and throw the saber, swapping places with the enemy it hits");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.1f;
            Item.damage = 16;
            Item.DamageType = DamageClass.Melee;
            Item.width = 34;
            Item.height = 36;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 15f;
            Item.shoot = ModContent.ProjectileType<NighEndThrown>();
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            TerrorbornPlayer.modPlayer(player).GainTerror(3f, false, false, true);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noUseGraphic = true;
                Item.autoReuse = false;
                Item.noMelee = true;
                if (TerrorbornPlayer.modPlayer(player).TerrorPercent < 25f)
                {
                    return false;
                }
                TerrorbornPlayer.modPlayer(player).LoseTerror(25f);
            }
            else
            {
                Item.noUseGraphic = false;
                Item.autoReuse = true;
                Item.noMelee = false;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                return false;
            }
            return true;
        }
    }

    public class NighEndThrown : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Melee/NighEndSaber";
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 36;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.hide = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.timeLeft = 3000;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 originalPosition = player.Center;
            player.position = target.Center - player.Size / 2;
            target.position = originalPosition - target.Size / 2;
            SoundExtensions.PlaySoundOld(SoundID.Item6, player.Center);
        }

        public override void AI()
        {
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

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundExtensions.PlaySoundOld(SoundID.Tink, Projectile.position, 0);
        }
    }
}
