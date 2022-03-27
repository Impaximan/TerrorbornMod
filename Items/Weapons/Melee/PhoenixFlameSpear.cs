using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class PhoenixFlameSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires phoenix feathers if there are none currently nearby, which stick into enemies" +
                "\nHitting an enemy with the spear itself causes all feathers to return to you, damaging enemies again");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.width = 60;
            item.height = 60;
            item.damage = 14;
            item.melee = true;
            item.noMelee = true;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.useTime = 20;
            item.useAnimation = 20;
            item.shootSpeed = 5f;
            item.knockBack = 1;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item1;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<PhoenixFlameSpearProjectile>();
            item.crit = 10;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            bool fireFeathers = true;
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.type == ModContent.ProjectileType<PhoenixFeather>() && projectile.active)
                {
                    fireFeathers = false;
                }
            }

            if (fireFeathers)
            {
                Main.PlaySound(SoundID.Item102, player.Center);
                TerrorbornMod.ScreenShake(1.5f);
                for (int i = 0; i < Main.rand.Next(3, 6); i++)
                {
                    Vector2 velocity = new Vector2(speedX, speedY).ToRotation().ToRotationVector2().RotatedByRandom(MathHelper.ToRadians(15f)) * 25f;
                    Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<PhoenixFeather>(), (int)(damage * 0.65f), 0f, player.whoAmI);
                }
            }
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
    }

    class PhoenixFlameSpearProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 90;
            projectile.height = 90;
            projectile.aiStyle = 19;
            projectile.penetrate = -1;
            projectile.scale = 1.3f;
            projectile.alpha = 0;
            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.type == ModContent.ProjectileType<PhoenixFeather>())
                {
                    projectile.ai[0] = 1;
                }
            }
        }

        public float movementFactor
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle originalHitbox = hitbox;
            int newSize = 25;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }

        public override void AI()
        {
            Player projOwner = Main.player[projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            projectile.direction = projOwner.direction;
            projOwner.heldProj = projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
            projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);
            if (!projOwner.frozen)
            {
                if (movementFactor == 0f)
                {
                    movementFactor = 3f;
                    projectile.netUpdate = true;
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3)
                {
                    movementFactor -= 2.4f;
                }
                else
                {
                    movementFactor += 2.1f;
                }
            }
            projectile.position += projectile.velocity * movementFactor;
            if (projOwner.itemAnimation == 0)
            {
                projectile.Kill();
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            if (projOwner.direction == 1)
            {
                projectile.spriteDirection = -1;
            }
            if (projectile.spriteDirection == -1)
            {
                projectile.rotation -= MathHelper.ToRadians(90f);
            }
        }
    }

    class PhoenixFeather : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 38;
            projectile.ranged = true;
            projectile.timeLeft = 3600;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.extraUpdates = 4;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.arrow = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 8;
            height = 8;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!stuck)
            {
                Main.PlaySound(SoundID.Dig, projectile.position);
                stuck = true;
                stuckNPC = target;
                offset = target.position - projectile.position;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Dig, projectile.position);
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
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
        int projectileCounter = 0;
        public override void AI()
        {
            if (start)
            {
                start = false;
                projectile.velocity /= projectile.extraUpdates + 1;
                projectile.timeLeft *= projectile.extraUpdates + 1;
            }

            if (projectile.Distance(Main.player[projectile.owner].Center) >= 1500f)
            {
                projectile.ai[0] = 1;
            }

            if (projectile.Distance(Main.player[projectile.owner].Center) >= 3000f)
            {
                projectile.active = false;
            }

            if (stuck && projectile.ai[0] == 0)
            {
                projectile.tileCollide = false;
                projectile.position = stuckNPC.position - offset;
                if (!stuckNPC.active)
                {
                    projectile.active = false;
                }
            }
            else if (projectile.ai[0] == 1)
            {
                stuck = false;
                projectile.tileCollide = false;
                projectile.velocity = projectile.DirectionTo(Main.player[projectile.owner].Center) * 25f;
                projectile.velocity /= projectile.extraUpdates + 1;
                projectile.rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(90);
                if (projectile.Distance(Main.player[projectile.owner].Center) <= 15f)
                {
                    projectile.active = false;
                }
            }
            else
            {
                if (projectile.velocity != Vector2.Zero)
                {
                    projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
                }
            }
        }
    }
}