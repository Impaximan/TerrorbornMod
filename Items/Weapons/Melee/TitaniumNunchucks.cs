using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class TitaniumNunchucks : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Can deflect Projectiles with a 1.5 second cooldown");
        }
        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.3f;
            Item.damage = 62;
            Item.width = 52;
            Item.height = 94;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = 100;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.shoot = ModContent.ProjectileType<TitaniumNunchucksProjectile>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TitaniumBar, 13)
                .AddIngredient(ItemID.WhiteString, 3)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    public class TitaniumNunchucksProjectile : ModProjectile
    {
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
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
            Projectile.width = 52;
            Projectile.height = 60;
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
                if (Main.projectile[i].Distance(Projectile.Center) < 50 + (Main.projectile[i].width + Main.projectile[i].height) / 2 && Main.projectile[i].hostile && DeflectCounter <= 0)
                {
                    DeflectCounter = 90;
                    Main.projectile[i].velocity = Main.projectile[i].velocity.RotatedByRandom(MathHelper.ToRadians(5)) * -1.5f;
                    Main.projectile[i].friendly = true;
                    Main.projectile[i].hostile = false;
                    Main.projectile[i].damage = 850;
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
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item1, (int)Projectile.Center.X, (int)Projectile.Center.Y);
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
            player.itemRotation = (float)Math.Atan2((double)(Projectile.velocity.Y * (float)Projectile.direction), (double)(Projectile.velocity.X * (float)Projectile.direction));
            player.bodyFrame.Y = 3 * player.bodyFrame.Height;
        }
    }
}

