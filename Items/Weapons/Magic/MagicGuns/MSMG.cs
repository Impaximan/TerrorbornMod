using Microsoft.Xna.Framework;
using Terraria;
using TerrorbornMod.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Magic.MagicGuns
{
    class MSMG : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 20)
                .AddIngredient(ModContent.ItemType<Materials.NovagoldBar>(), 2)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("M.S.M");
            Tooltip.SetDefault("Rapidly fires space lasers at your cursor" +
                "\nStands for 'Meteor Shower Minigun'");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.45f;
            Item.damage = 8;
            Item.noMelee = true;
            Item.width = 48;
            Item.height = 28;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.crit = 14;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 1f;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SpaceLaser>();
            Item.mana = 3;
            Item.DamageType = DamageClass.Magic; ;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            TerrorbornSystem.ScreenShake(1f);
            SoundExtensions.PlaySoundOld(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 125, 1, 1);
            Vector2 shootSpeed = new Vector2(velocity.X, velocity.Y);
            shootSpeed.Normalize();
            position += shootSpeed * 22;
            shootSpeed = shootSpeed.RotatedByRandom(MathHelper.ToRadians(3));
            velocity.X = shootSpeed.X;
            velocity.Y = shootSpeed.Y;
        }
    }

    class SpaceLaser : Deathray
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
            drawColor = Color.LightGreen;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / timeLeft;
        }
    }
}


