using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged.Thrown
{
    class Jawvelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Latches onto hit enemies, continuing to knaw on them over time" +
                "\nExplodes into azure shards if an enemy it's latched onto dies"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 50;
            Item.height = 48;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 35;
            Item.shoot = ModContent.ProjectileType<JawvelinProjectile>();
        }

        public override void HoldItem(Player player)
        {
            player.GetArmorPenetration(DamageClass.Ranged) += 10;
        }
    }

    class JawvelinProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 52;
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
            width = 14;
            height = 14;
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
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
            int newSize = 14;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }

        bool stuck = false;
        NPC stuckNPC;
        int stuckTimeLeft = 180;
        Vector2 offset;
        bool start = true;
        int hurtCounter = 0;
        public override void AI()
        {
            if (start)
            {
                start = false;
                Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
                Projectile.velocity /= Projectile.extraUpdates + 1;
                stuckTimeLeft *= Projectile.extraUpdates + 1;
                Projectile.timeLeft *= Projectile.extraUpdates + 1;
            }

            if (stuck)
            {
                Projectile.frame = 1;
                Projectile.tileCollide = false;
                Projectile.position = stuckNPC.position - offset;
                if (!stuckNPC.active)
                {
                    Projectile.active = false;
                    SoundExtensions.PlaySoundOld(SoundID.Item110, Projectile.Center);
                    for (int i = 0; i < Main.rand.Next(3, 5); i++)
                    {
                        float speed = 35f;
                        Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * speed;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<Projectiles.AzuriteShard>(), Projectile.damage / 3, 1, Projectile.owner);
                        Main.projectile[proj].DamageType = DamageClass.Ranged;
                    }
                }

                stuckTimeLeft--;
                if (stuckTimeLeft <= 0)
                {
                    Projectile.timeLeft = 1;
                }

                hurtCounter++;
                if (hurtCounter >= 4 * (Projectile.extraUpdates + 1) / Main.player[Projectile.owner].GetAttackSpeed(DamageClass.Ranged))
                {
                    hurtCounter = 0;
                    stuckNPC.StrikeNPC(stuckNPC.CalculateHitInfo(1, 0, false, 0));
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135);
                if (Projectile.spriteDirection == 1) Projectile.rotation -= MathHelper.ToRadians(90);
            }
        }
    }
}

