using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class GraveNeedle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Sticks onto hit enemies" +
                "\nIf 5 in total are stuck in enemies, all of them will explode");
        }

        public override void SetDefaults()
        {
            TerrorbornItem.modItem(Item).countAsThrown = true;
            Item.damage = 14;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 28;
            Item.height = 26;
            Item.consumable = false;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 35;
            Item.shoot = ModContent.ProjectileType<GraveNeedleProjectile>();
        }
    }

    class GraveNeedleProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/GraveNeedle";

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 26;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 3600;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.extraUpdates = 4;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.arrow = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 8;
            height = 8;
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!stuck)
            {
                SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
                stuck = true;
                stuckNPC = target;
                offset = target.position - Projectile.position;
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (stuck)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle originalHitbox = hitbox;
            int newSize = 12;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }

        bool stuck = false;
        NPC stuckNPC;
        Vector2 offset;
        bool start = true;
        int ProjectileCounter = 0;
        public override void AI()
        {
            if (start)
            {
                start = false;
                Projectile.velocity /= Projectile.extraUpdates + 1;
                Projectile.timeLeft *= Projectile.extraUpdates + 1;
            }

            if (stuck)
            {
                Projectile.ai[0] = 1;

                Projectile.tileCollide = false;
                Projectile.position = stuckNPC.position - offset;
                if (!stuckNPC.active)
                {
                    Projectile.active = false;
                }

                List<Projectile> needles = new List<Projectile>();
                foreach (Projectile needle in Main.projectile)
                {
                    if (needle.active && needle.type == Projectile.type && needle.ai[0] == 1)
                    {
                        needles.Add(needle);
                    }
                }

                if (needles.Count >= 5)
                {
                    foreach (Projectile needle in needles)
                    {
                        needle.active = false;

                        float speed = 10f;
                        Vector2 velocity = (needle.rotation - MathHelper.ToRadians(45)).ToRotationVector2() * speed;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), needle.Center, velocity, ModContent.ProjectileType<NeedleExplosion>(), Projectile.damage, Projectile.knockBack * 3, Projectile.owner);
                    }
                    SoundExtensions.PlaySoundOld(SoundID.Item62, Projectile.Center);
                    TerrorbornSystem.ScreenShake(3f);
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
            }
        }
    }

    class NeedleExplosion : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Effects/Textures/Glow_2";

        int timeLeft = 15;
        const int defaultSize = 100;
        int currentSize = defaultSize;
        public override void SetDefaults()
        {
            Projectile.width = defaultSize;
            Projectile.height = defaultSize;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.timeLeft = timeLeft;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            TBUtils.Graphics.DrawGlow_1(Main.spriteBatch, Projectile.Center - Main.screenPosition, currentSize, new Color(234, 79, 9));
            return false;
        }

        public override void AI()
        {
            currentSize -= defaultSize / timeLeft;
        }
    }
}