using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Ammo
{
    class PincushionArrow : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.shootSpeed = 3;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<PincushionArrowProjectile>();
            Item.ammo = AmmoID.Arrow;
        }
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Sticks onto enemies and then breaks shortly after, dealing extra damage" +
                "\nAdditionally ignores half of an enemy's defense" +
                "\nIf an enemy they're stuck into dies they'll launch into another enemy"); */
        }
    }
    class PincushionArrowProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Ammo/PincushionArrow";
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 32;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 3600;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
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
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            damage += target.defense / 4;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!stuck)
            {
                SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
                stuck = true;
                wasCrit = crit;
                stuckNPC = target;
                offset = target.position - Projectile.position;
            }
        }
        public override void Kill(int timeLeft)
        {
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            if (stuck)
            {
                stuckNPC.StrikeNPC(Projectile.damage / 10 + stuckNPC.defense / 2, 0, 0, wasCrit);
            }
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
            hitbox.Width = 14;
            hitbox.Height = 14;
            hitbox.Y += (int)(32f * (14f / 32f) / 2f);
            base.ModifyDamageHitbox(ref hitbox);
        }

        bool stuck = false;
        bool wasCrit;
        NPC stuckNPC;
        int stuckTimeLeft = 300;
        Vector2 offset;
        bool start = true;
        float originalVelocity;
        NPC target;
        public override void AI()
        {
            if (start)
            {
                start = false;
                originalVelocity = Projectile.velocity.Length();
            }
            if (stuck)
            {
                Projectile.position = stuckNPC.position - offset;
                stuckNPC.lifeRegen -= 5;
                if (!stuckNPC.active)
                {
                    bool foundTarget = false;
                    float range = 600;
                    for (int i = 0; i < 200; i++)
                    {
                        NPC NPC = Main.npc[i];
                        if (Projectile.Distance(NPC.Center) < range && Projectile.CanHitWithOwnBody(NPC) && NPC.active && !NPC.friendly && !NPC.dontTakeDamage && NPC.chaseable)
                        {
                            range = Projectile.Distance(NPC.Center);
                            target = NPC;
                            foundTarget = true;
                        }
                    }
                    if (foundTarget)
                    {
                        Vector2 newVelocity = Projectile.DirectionTo(target.Center) * originalVelocity;
                        Projectile.velocity = newVelocity;
                        stuck = false;
                        stuckTimeLeft = 300;
                    }
                    else
                    {
                        Projectile.timeLeft = 1;
                    }
                }
                stuckTimeLeft--;
                if (stuckTimeLeft <= 0)
                {
                    Projectile.timeLeft = 1;
                }
            }
            else
            {
                Projectile.velocity.Y += 0.05f;
                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) - MathHelper.ToRadians(90);
            }
        }
    }
}

