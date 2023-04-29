using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Melee.Nunchucks
{
    class Nunchucks : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Can deflect Projectiles with a 1.5 second cooldown");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.3f;
            Item.damage = 8;
            Item.width = 52;
            Item.height = 94;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = 100;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<NunchucksProjectile>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
    }

    public class NunchucksProjectile : ModProjectile
    {
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            hitDirection = Projectile.spriteDirection * -1;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            Projectile.idStaticNPCHitCooldown = 6;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
        }

        void FindFrame(int FrameHeight)
        {
            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frame++;
                Projectile.frameCounter = 1;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }

        bool Start = true;
        int DeflectCounter = 120;
        public override void AI()
        {
            DeflectCounter--;
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i < 200; i++)
            {
                if (Main.projectile[i].Distance(Projectile.Center) < 40 && Main.projectile[i].hostile && DeflectCounter <= 0)
                {
                    DeflectCounter = 120;
                    Main.projectile[i].velocity = Main.projectile[i].velocity.RotatedByRandom(MathHelper.ToRadians(5)) * -1.5f;
                    Main.projectile[i].friendly = true;
                    Main.projectile[i].hostile = false;
                    Main.projectile[i].damage = 100;
                    Main.projectile[i].DamageType = DamageClass.Melee;
                    Main.projectile[i].owner = Projectile.owner;
                    Main.projectile[i].knockBack = Projectile.knockBack;
                }
            }

            FindFrame(Projectile.height);

            if (Start)
            {
                Start = false;
            }
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 vector2000 = Main.MouseWorld - vector;
            vector2000.Normalize();
            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                SoundExtensions.PlaySoundOld(SoundID.Item1, (int)Projectile.Center.X, (int)Projectile.Center.Y);
                Projectile.soundDelay = 15;
            }
            if (Main.myPlayer == Projectile.owner)
            {
                if (!player.channel || player.noItems || player.CCed)
                {
                    Projectile.Kill();
                }
            }

            //Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.7f);
            Projectile.Center = player.MountedCenter;
            Projectile.position.X += player.width / 2 * player.direction;
            Projectile.spriteDirection = player.direction * -1;
            player.ChangeDir((int)(vector2000.X / (float)Math.Abs(vector2000.X)));
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2((double)(Projectile.velocity.Y * Projectile.direction), (double)(Projectile.velocity.X * Projectile.direction));
            player.bodyFrame.Y = 3 * player.bodyFrame.Height;
        }
    }
}
