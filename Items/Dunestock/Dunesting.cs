using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Dunestock
{
    class Dunesting : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires a claw that splits into 3 arrows after travelling for a moment");
        }
        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 26;
            Item.height = 56;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.DD2_BallistaTowerShot;
            Item.shoot = ProjectileID.GreenLaser;
            Item.autoReuse = true;
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Arrow;
        }
        public override bool CanConsumeAmmo(Player player)
        {
            return Main.rand.Next(101) <= 25f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Claw>(), damage, knockback, player.whoAmI);
            Main.projectile[proj].ai[0] = type;
            return false;
        }
    }

    class Claw : ModProjectile
    {
        public override string Texture => "TerrorbornMod/NPCs/Bosses/DuneClaw";

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 45;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        int CollideCounter = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            //Terraria.Audio.SoundEngine.PlaySound(SoundID.Run, Projectile.Center);
            CollideCounter += 1;
            if (CollideCounter >= 5)
            {
                Projectile.timeLeft = 0;
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, Projectile.Center);
            float speed = 15f;
            Vector2 velocity = Projectile.DirectionTo(Main.MouseWorld) * speed;
            int proj = Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, velocity, (int)Projectile.ai[0], Projectile.damage, Projectile.knockBack / 2, Projectile.owner);
            Main.projectile[proj].noDropItem = true;
            proj = Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, velocity.RotatedBy(MathHelper.ToRadians(30)), (int)Projectile.ai[0], Projectile.damage, Projectile.knockBack / 2, Projectile.owner);
            Main.projectile[proj].noDropItem = true;
            proj = Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, velocity.RotatedBy(MathHelper.ToRadians(-30)), (int)Projectile.ai[0], Projectile.damage, Projectile.knockBack / 2, Projectile.owner);
            Main.projectile[proj].noDropItem = true;
            Projectile.active = false;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
        }
    }
}
