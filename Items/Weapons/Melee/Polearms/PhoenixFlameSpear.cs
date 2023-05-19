using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Melee.Polearms
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
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 60;
            Item.height = 60;
            Item.damage = 14;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.shootSpeed = 5f;
            Item.knockBack = 1;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PhoenixFlameSpearProjectile>();
            Item.crit = 10;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool fireFeathers = true;
            foreach (Projectile Projectile in Main.projectile)
            {
                if (Projectile.type == ModContent.ProjectileType<PhoenixFeather>() && Projectile.active)
                {
                    fireFeathers = false;
                }
            }

            if (fireFeathers)
            {
                SoundExtensions.PlaySoundOld(SoundID.Item102, player.Center);
                TerrorbornSystem.ScreenShake(1.5f);
                for (int i = 0; i < Main.rand.Next(3, 6); i++)
                {
                    velocity = velocity.ToRotation().ToRotationVector2().RotatedByRandom(MathHelper.ToRadians(15f)) * 25f;
                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<PhoenixFeather>(), (int)(damage * 0.65f), 0f, player.whoAmI);
                }
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }

    class PhoenixFlameSpearProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.aiStyle = 19;
            Projectile.penetrate = -1;
            Projectile.scale = 1.3f;
            Projectile.alpha = 0;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            foreach (Projectile Projectile in Main.projectile)
            {
                if (Projectile.type == ModContent.ProjectileType<PhoenixFeather>())
                {
                    Projectile.ai[0] = 1;
                }
            }
        }

        public float movementFactor
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
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
            Player projOwner = Main.player[Projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            Projectile.direction = projOwner.direction;
            projOwner.heldProj = Projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            Projectile.position.X = ownerMountedCenter.X - Projectile.width / 2;
            Projectile.position.Y = ownerMountedCenter.Y - Projectile.height / 2;
            if (!projOwner.frozen)
            {
                if (movementFactor == 0f)
                {
                    movementFactor = 3f;
                    Projectile.netUpdate = true;
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
            Projectile.position += Projectile.velocity * movementFactor;
            if (projOwner.itemAnimation == 0)
            {
                Projectile.Kill();
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            if (projOwner.direction == 1)
            {
                Projectile.spriteDirection = -1;
            }
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.ToRadians(90f);
            }
        }
    }

    class PhoenixFeather : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 38;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 3600;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
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

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
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

            if (Projectile.Distance(Main.player[Projectile.owner].Center) >= 1500f)
            {
                Projectile.ai[0] = 1;
            }

            if (Projectile.Distance(Main.player[Projectile.owner].Center) >= 3000f)
            {
                Projectile.active = false;
            }

            if (stuck && Projectile.ai[0] == 0)
            {
                Projectile.tileCollide = false;
                Projectile.position = stuckNPC.position - offset;
                if (!stuckNPC.active)
                {
                    Projectile.active = false;
                }
            }
            else if (Projectile.ai[0] == 1)
            {
                stuck = false;
                Projectile.tileCollide = false;
                Projectile.velocity = Projectile.DirectionTo(Main.player[Projectile.owner].Center) * 25f;
                Projectile.velocity /= Projectile.extraUpdates + 1;
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(90);
                if (Projectile.Distance(Main.player[Projectile.owner].Center) <= 15f)
                {
                    Projectile.active = false;
                }
            }
            else
            {
                if (Projectile.velocity != Vector2.Zero)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
                }
            }
        }
    }
}