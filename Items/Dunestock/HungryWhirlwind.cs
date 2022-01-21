using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
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
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.critDamageMult = 1.3f;
            item.damage = 55;
            item.width = 66;
            item.height = 72;
            item.melee = true;
            item.channel = true;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 100;
            item.knockBack = 6f;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.shoot = ModContent.ProjectileType<HungryWhirlwindProjectile>();
            item.noUseGraphic = true;
            item.noMelee = true;
        }
    }

    public class HungryWhirlwindProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Dunestock/HungryWhirlwind";

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = projectile.spriteDirection;
        }

        public override void SetDefaults()
        {
            projectile.idStaticNPCHitCooldown = 6;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.width = 66;
            projectile.height = 72;
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

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.Distance(projectile.Center) <= 1000 && !npc.friendly)
                {
                    npc.velocity += npc.DirectionTo(projectile.Center) * 0.3f * npc.knockBackResist;
                }
            }

            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 vector2000 = Main.MouseWorld - vector;
            vector2000.Normalize();
            projectile.soundDelay--;
            if (projectile.soundDelay <= 0)
            {
                Main.PlaySound(SoundID.Item71, (int)projectile.Center.X, (int)projectile.Center.Y);
                projectile.soundDelay = 25;

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

            Dust dust = Dust.NewDustPerfect(projectile.Center + projectile.rotation.ToRotationVector2() * 33, DustID.GoldFlame);
            dust.noGravity = true;
            dust.scale = 1.5f;
            dust.velocity = projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(90 * projectile.spriteDirection)) * 5;

            //Lighting.AddLight(projectile.Center, 0.5f, 0.5f, 0.7f);
            projectile.Center = player.MountedCenter;
            projectile.position.X += player.width / 2 * player.direction;
            projectile.spriteDirection = player.direction;
            player.ChangeDir((int)(vector2000.X / (float)Math.Abs(vector2000.X)));
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2((double)(projectile.velocity.Y * (float)projectile.direction), (double)(projectile.velocity.X * (float)projectile.direction));
            player.bodyFrame.Y = 3 * player.bodyFrame.Height;
            projectile.rotation += MathHelper.ToRadians(25f) * projectile.spriteDirection;
        }
    }
}

