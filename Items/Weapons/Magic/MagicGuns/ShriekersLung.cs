using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Magic.MagicGuns
{
    class ShriekersLung : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shrieker's Lung");
            Tooltip.SetDefault("Fires bouncing sound waves that home into confused enemies" +
                "\nHas a chance to confuse enemies for 5 seconds");
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.noMelee = true;
            Item.width = 44;
            Item.height = 26;
            Item.useTime = 24;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 0.0001f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<ShriekWave>();
            Item.mana = 3;
            Item.DamageType = DamageClass.Magic; ;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundExtensions.PlaySoundOld(SoundID.NPCDeath4, (int)player.Center.X, (int)player.Center.Y, 4, 1, -0.6f);
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
    }

    class ShriekWave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i += 2)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic; ;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 14;
            height = 14;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        int collideCounter = 4;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < Projectile.localNPCImmunity.Length; i++)
            {
                if (Projectile.localNPCImmunity[i] < 0 || Projectile.localNPCImmunity[i] > 5)
                {
                    Projectile.localNPCImmunity[i] = 5;
                }
            }
            collideCounter--;
            if (collideCounter <= 0)
            {
                Projectile.timeLeft = 0;
            }
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
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextFloat() <= 0.15f)
            {
                target.AddBuff(BuffID.Confused, 60 * 5);
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

            NPC targetNPC = Main.npc[0];
            float Distance = 700; //max distance away
            bool Targeted = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(Projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy() && Main.npc[i].HasBuff(BuffID.Confused))
                {
                    targetNPC = Main.npc[i];
                    Distance = Main.npc[i].Distance(Projectile.Center);
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                //HOME IN
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(3.5f * (Projectile.velocity.Length() / 20))).ToRotationVector2() * Projectile.velocity.Length();
            }
        }
    }
}