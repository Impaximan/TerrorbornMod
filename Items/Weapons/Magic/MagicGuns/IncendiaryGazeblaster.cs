using Microsoft.Xna.Framework;
using Terraria;
using TerrorbornMod.Projectiles;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Magic.MagicGuns
{
    class IncendiaryGazeblaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gazeblaster");
            Tooltip.SetDefault("Fires mini deathrays at your cursor that can hit enemies multiple times");
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.noMelee = true;
            Item.width = 46;
            Item.height = 26;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.crit = 14;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item33;
            Item.shootSpeed = 1f;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IncendiaryGazeblast>();
            Item.mana = 6;
            Item.DamageType = DamageClass.Magic; ;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 shootSpeed = new Vector2(velocity.X, velocity.Y);
            shootSpeed.Normalize();
            velocity.X = shootSpeed.X;
            velocity.Y = shootSpeed.Y;
            return true;
        }
    }

    class IncendiaryGazeblast : Deathray
    {
        int timeLeft = 30;
        public override string Texture => "TerrorbornMod/Projectiles/IncendiaryDeathray";
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 22;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.hide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.timeLeft = timeLeft;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            MoveDistance = 20f;
            RealMaxDistance = 2000f;
            bodyRect = new Rectangle(0, 24, 18, 22);
            headRect = new Rectangle(0, 0, 18, 22);
            tailRect = new Rectangle(0, 46, 18, 22);
        }

        public override Vector2 Position()
        {
            return Main.player[Projectile.owner].Center;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / timeLeft;
        }
    }
}
