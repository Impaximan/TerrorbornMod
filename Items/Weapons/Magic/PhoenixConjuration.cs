using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class PhoenixConjuration : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Conjures a swarm of phoenices that attack at your cursor");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.width = 32;
            item.height = 38;
            item.magic = true;
            item.damage = 40;
            item.useTime = 18;
            item.useAnimation = 18;
            item.mana = 25;
            item.rare = 12;
            item.shoot = ModContent.ProjectileType<ConjuredPhoenix>();
            item.shootSpeed = 25f;
            item.UseSound = SoundID.Item20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 0.1f;
            item.autoReuse = true;
            item.noMelee = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                Projectile.NewProjectile(new Vector2(player.Center.X + Main.rand.Next(-50, 51), player.Center.Y + Main.rand.Next(-50, 51)), new Vector2(speedX, speedY), type, damage, knockBack, Owner: item.owner);
            }
            return false;
        }
    }

    class ConjuredPhoenix : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
            Main.projFrames[projectile.type] = 6;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                SpriteEffects effects = SpriteEffects.None;
                if (projectile.spriteDirection == 1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), color, projectile.rotation, drawOrigin, projectile.scale * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length), effects, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 276 / 6;
            projectile.magic = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 25;
            projectile.timeLeft = 180;
        }

        int attackCounter = 0;
        int frameCounter = 0;
        public override void AI()
        {
            projectile.spriteDirection = Math.Sign(Main.MouseWorld.X - projectile.Center.X);

            projectile.velocity = projectile.velocity.ToRotation().AngleLerp(projectile.DirectionTo(Main.MouseWorld).ToRotation(), 0.15f).ToRotationVector2() * projectile.velocity.Length();

            if (projectile.Distance(Main.MouseWorld) < 150)
            {
                attackCounter++;
                if (attackCounter > 30)
                {
                    attackCounter = 0;
                    projectile.velocity = projectile.DirectionTo(Main.MouseWorld) * (projectile.velocity.Length() + 3f);
                }
            }

            frameCounter++;
            if (frameCounter > 3)
            {
                frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= 6)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 15, 10, DustID.Fire, DustScale: 1f, NoGravity: true);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 180);
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }
    }
}