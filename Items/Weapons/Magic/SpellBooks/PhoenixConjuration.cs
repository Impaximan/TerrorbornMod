using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Magic.SpellBooks
{
    class PhoenixConjuration : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Conjures a swarm of phoenices that attack at your cursor");
        }

        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.width = 32;
            Item.height = 38;
            Item.DamageType = DamageClass.Magic; ;
            Item.damage = 40;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.mana = 25;
            Item.rare = ModContent.RarityType<Rarities.Golden>();
            Item.shoot = ModContent.ProjectileType<ConjuredPhoenix>();
            Item.shootSpeed = 25f;
            Item.UseSound = SoundID.Item20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0.1f;
            Item.autoReuse = true;
            Item.noMelee = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                Projectile.NewProjectile(source, new Vector2(player.Center.X + Main.rand.Next(-50, 51), player.Center.Y + Main.rand.Next(-50, 51)), new Vector2(velocity.X, velocity.Y), type, damage, knockback, Owner: player.whoAmI);
            }
            return false;
        }
    }

    class ConjuredPhoenix : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
            Main.projFrames[Projectile.type] = 6;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                SpriteEffects effects = SpriteEffects.None;
                if (Projectile.spriteDirection == 1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), color, Projectile.rotation, drawOrigin, Projectile.scale * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length), effects, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 276 / 6;
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;
            Projectile.timeLeft = 180;
        }

        int attackCounter = 0;
        int frameCounter = 0;
        public override void AI()
        {
            Projectile.spriteDirection = Math.Sign(Main.MouseWorld.X - Projectile.Center.X);

            Projectile.velocity = Projectile.velocity.ToRotation().AngleLerp(Projectile.DirectionTo(Main.MouseWorld).ToRotation(), 0.15f).ToRotationVector2() * Projectile.velocity.Length();

            if (Projectile.Distance(Main.MouseWorld) < 150)
            {
                attackCounter++;
                if (attackCounter > 30)
                {
                    attackCounter = 0;
                    Projectile.velocity = Projectile.DirectionTo(Main.MouseWorld) * (Projectile.velocity.Length() + 3f);
                }
            }

            frameCounter++;
            if (frameCounter > 3)
            {
                frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= 6)
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 0, 15, 10, 6, DustScale: 1f, NoGravity: true);
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

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(360 / Streams * i + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth)), DustType, direction, 0, default, DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }
    }
}