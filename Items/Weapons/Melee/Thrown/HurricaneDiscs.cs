using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Melee.Thrown
{
    public class HurricaneDiscs : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates two discs that rapidly spin around you, shredding through foes" +
                "\nThe distance at which they spin around you is controlled by the mouse cursor");
        }

        public override void SetDefaults()
        {
            Item.damage = 67;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Melee;
            Item.width = 25;
            Item.height = 25;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.DD2_MonkStaffSwing;
            Item.autoReuse = true;
            Item.shootSpeed = 0f;
            Item.shoot = ModContent.ProjectileType<HurricaneDisc>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool spawnProjectile = true;
            foreach (Projectile Projectile in Main.projectile)
            {
                if (Projectile.type == type && Projectile.active)
                {
                    spawnProjectile = false;
                    break;
                }
            }

            if (spawnProjectile)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
                int proj = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
                Main.projectile[proj].ai[0] = 1;
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.ThunderShard>(), 18)
                .AddIngredient(ModContent.ItemType<Materials.NoxiousScale>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class HurricaneDisc : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.extraUpdates = 2;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        float rotationFromPlayer;
        float distance;
        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
                rotationFromPlayer = 0f;
                if (Projectile.ai[0] == 1)
                {
                    rotationFromPlayer += MathHelper.ToRadians(180);
                }
                distance = 0f;
            }

            Player player = Main.player[Projectile.owner];
            Projectile.rotation += MathHelper.ToRadians(45) * player.direction / (Projectile.extraUpdates + 1);
            rotationFromPlayer += MathHelper.ToRadians(15) * player.direction / (Projectile.extraUpdates + 1) * player.GetAttackSpeed(DamageClass.Melee);
            Projectile.spriteDirection = player.direction;
            Projectile.active = player.channel;
            Projectile.timeLeft = 300;

            float targetDistance = player.Distance(Main.MouseWorld);
            if (targetDistance < 125)
            {
                targetDistance = 125;
            }
            else if (targetDistance > 600)
            {
                targetDistance = 600;
            }
            distance = MathHelper.Lerp(distance, targetDistance, 0.02f);

            Projectile.position = player.Center + distance * rotationFromPlayer.ToRotationVector2();
            Projectile.position -= new Vector2(Projectile.width / 2, Projectile.height / 2);
        }
    }
}


