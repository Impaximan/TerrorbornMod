using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
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
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            item.damage = 135;
            item.width = 60;
            item.height = 58;
            item.melee = true;
            item.channel = true;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 100;
            item.knockBack = 6f;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.shoot = ModContent.ProjectileType<PurgatoryBatonProjectile>();
            item.noUseGraphic = true;
            item.noMelee = true;
        }
    }
    public class PurgatoryBatonProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Melee/PurgatoryBaton";

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = projectile.spriteDirection;
        }

        public override void SetDefaults()
        {
            projectile.idStaticNPCHitCooldown = 6;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.width = 60;
            projectile.height = 58;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.melee = true;
        }

        bool Start = true;
        int DeflectCounter = 120;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 vector2000 = Main.MouseWorld - vector;
            vector2000.Normalize();
            projectile.soundDelay--;
            if (projectile.soundDelay <= 0)
            {
                Main.PlaySound(SoundID.DD2_SkyDragonsFurySwing, (int)projectile.Center.X, (int)projectile.Center.Y);
                projectile.soundDelay = 50;

            }

            if (TerrorbornItem.modItem(player.HeldItem).TerrorCost > 0f)
            {
                if (modPlayer.TerrorPercent < TerrorbornItem.modItem(player.HeldItem).TerrorCost / 60f)
                {
                    projectile.active = false;
                    projectile.timeLeft = 0;
                    return;
                }
                modPlayer.LoseTerror(TerrorbornItem.modItem(player.HeldItem).TerrorCost, true, true);
            }

            if (Main.myPlayer == projectile.owner)
            {
                if (!player.channel || player.noItems || player.CCed)
                {
                    projectile.Kill();
                }
            }


            projectile.Center = player.MountedCenter;
            projectile.position.X += player.width / 2 * player.direction;
            projectile.spriteDirection = player.direction;
            if (projectile.spriteDirection == 0)
            {
                projectile.spriteDirection = 1;
            }
            player.ChangeDir((int)(vector2000.X / (float)Math.Abs(vector2000.X)));
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2((double)(projectile.velocity.Y * (float)projectile.direction), (double)(projectile.velocity.X * (float)projectile.direction));
            player.bodyFrame.Y = 3 * player.bodyFrame.Height;
            projectile.rotation += MathHelper.ToRadians(10f) * projectile.spriteDirection;

            int rotationMult = projectile.spriteDirection;
            Vector2 position = projectile.Center + projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(-45 * rotationMult)) * 30;
            Vector2 velocity = projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(-45 * rotationMult));
            Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<PurgatoryLaser>(), projectile.damage, 0f, projectile.owner);
            position = projectile.Center + projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(135 * rotationMult)) * 30;
            velocity = projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(135 * rotationMult));
            Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<PurgatoryLaser>(), projectile.damage, 0f, projectile.owner);
        }
    }

    class PurgatoryLaser : Deathray
    {
        int timeLeft = 5;
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/LightBlast";
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.hide = false;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.timeLeft = timeLeft;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
            projectile.arrow = true;
            MoveDistance = 20f;
            RealMaxDistance = 2000f;
            bodyRect = new Rectangle(0, 0, 10, 10);
            headRect = new Rectangle(0, 0, 10, 10);
            tailRect = new Rectangle(0, 0, 10, 10);
            FollowPosition = false;
            drawColor = new Color(255, 228, 200);
        }

        public override void PostAI()
        {
            deathrayWidth -= 1f / (float)timeLeft;
            projectile.velocity.Normalize();
        }
    }
}
