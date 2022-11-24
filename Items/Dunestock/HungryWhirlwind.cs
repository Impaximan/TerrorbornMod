using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Dunestock
{
    class HungryWhirlwind : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Pulls nearby enemies closer");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.3f;
            Item.damage = 55;
            Item.width = 66;
            Item.height = 72;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = 100;
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ModContent.ProjectileType<HungryWhirlwindProjectile>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }
    }

    public class HungryWhirlwindProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Dunestock/HungryWhirlwind";

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = Projectile.spriteDirection;
        }

        public override void SetDefaults()
        {
            Projectile.idStaticNPCHitCooldown = 6;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.width = 66;
            Projectile.height = 72;
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

            foreach (NPC NPC in Main.npc)
            {
                if (NPC.active && NPC.Distance(Projectile.Center) <= 1000 && !NPC.friendly && Collision.CanHitLine(Projectile.Center, 1, 1, NPC.Center, 1, 1))
                {
                    NPC.velocity += NPC.DirectionTo(Projectile.Center) * 0.3f * NPC.knockBackResist;
                }
            }

            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 vector2000 = Main.MouseWorld - vector;
            vector2000.Normalize();
            Projectile.soundDelay--;
            if (Projectile.soundDelay <= 0)
            {
                SoundExtensions.PlaySoundOld(SoundID.Item71, (int)Projectile.Center.X, (int)Projectile.Center.Y);
                Projectile.soundDelay = 25;

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

            Dust dust = Dust.NewDustPerfect(Projectile.Center + Projectile.rotation.ToRotationVector2() * 33, DustID.GoldFlame);
            dust.noGravity = true;
            dust.scale = 1.5f;
            dust.velocity = Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(90 * Projectile.spriteDirection)) * 5;
            //Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.7f);
            Projectile.Center = player.MountedCenter;
            Projectile.position.X += player.width / 2 * player.direction;
            Projectile.spriteDirection = player.direction;
            player.ChangeDir((int)(vector2000.X / (float)Math.Abs(vector2000.X)));
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2((double)(Projectile.velocity.Y * (float)Projectile.direction), (double)(Projectile.velocity.X * (float)Projectile.direction));
            player.bodyFrame.Y = 3 * player.bodyFrame.Height;
            Projectile.rotation += MathHelper.ToRadians(25f) * Projectile.spriteDirection;
        }
    }
}

