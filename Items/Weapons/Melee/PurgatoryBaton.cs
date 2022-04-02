using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrorbornMod.Projectiles;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class PurgatoryBaton : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires deathrays in all directions");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            Item.damage = 135;
            Item.width = 60;
            Item.height = 58;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = 100;
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<PurgatoryBatonProjectile>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
    }
    public class PurgatoryBatonProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Melee/PurgatoryBaton";

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = Projectile.spriteDirection;
        }

        public override void SetDefaults()
        {
            Projectile.idStaticNPCHitCooldown = 6;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.width = 60;
            Projectile.height = 58;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
        }

        bool Start = true;
        int DeflectCounter = 120;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 vector2000 = Main.MouseWorld - vector;
            vector2000.Normalize();
            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_SkyDragonsFurySwing, (int)Projectile.Center.X, (int)Projectile.Center.Y);
                Projectile.soundDelay = 50;

            }

            if (TerrorbornItem.modItem(player.HeldItem).TerrorCost > 0f)
            {
                if (modPlayer.TerrorPercent < TerrorbornItem.modItem(player.HeldItem).TerrorCost / 60f)
                {
                    Projectile.active = false;
                    Projectile.timeLeft = 0;
                    return;
                }
                modPlayer.LoseTerror(TerrorbornItem.modItem(player.HeldItem).TerrorCost, true, true);
            }

            if (Main.myPlayer == Projectile.owner)
            {
                if (!player.channel || player.noItems || player.CCed)
                {
                    Projectile.Kill();
                }
            }


            Projectile.Center = player.MountedCenter;
            Projectile.position.X += player.width / 2 * player.direction;
            Projectile.spriteDirection = player.direction;
            if (Projectile.spriteDirection == 0)
            {
                Projectile.spriteDirection = 1;
            }
            player.ChangeDir((int)(vector2000.X / (float)Math.Abs(vector2000.X)));
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2((double)(Projectile.velocity.Y * (float)Projectile.direction), (double)(Projectile.velocity.X * (float)Projectile.direction));
            player.bodyFrame.Y = 3 * player.bodyFrame.Height;
            Projectile.rotation += MathHelper.ToRadians(10f) * Projectile.spriteDirection;

            int rotationMult = Projectile.spriteDirection;
            Vector2 position = Projectile.Center + Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(-45 * rotationMult)) * 30;
            Vector2 velocity = Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(-45 * rotationMult));
            Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), position, velocity, ModContent.ProjectileType<PurgatoryLaser>(), Projectile.damage, 0f, Projectile.owner);
            position = Projectile.Center + Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(135 * rotationMult)) * 30;
            velocity = Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(135 * rotationMult));
            Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), position, velocity, ModContent.ProjectileType<PurgatoryLaser>(), Projectile.damage, 0f, Projectile.owner);
        }
    }

    class PurgatoryLaser : Deathray
    {
        int timeLeft = 5;
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/LightBlast";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.hide = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = timeLeft;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            MoveDistance = 20f;
            RealMaxDistance = 2000f;
            bodyRect = new Rectangle(0, 0, 10, 10);
            headRect = new Rectangle(0, 0, 10, 10);
            tailRect = new Rectangle(0, 0, 10, 10);
            FollowPosition = false;
            drawColor = new Color(255, 228, 200) * 0.5f;
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
            Projectile.velocity.Normalize();
        }
    }
}
