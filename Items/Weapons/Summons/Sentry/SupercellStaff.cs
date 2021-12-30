using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.Weapons.Summons.Sentry
{
    class SupercellStaff : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 18);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a storm cell sentry that rapidly zaps nearby enemies");
        }

        public override void SetDefaults()
        {
            item.mana = 10;
            item.summon = true;
            item.damage = 55;
            item.width = 54;
            item.height = 58;
            item.sentry = true;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 0;
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item44;
            item.shoot = ModContent.ProjectileType<Supercell>();
            item.shootSpeed = 10f;
            item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse != 2)
            {
                int turretAmount = 0;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].sentry && Main.projectile[i].active)
                    {
                        turretAmount++;
                        if (turretAmount >= player.maxTurrets)
                        {
                            Main.projectile[i].active = false;
                        }
                    }
                }
                Projectile.NewProjectile(new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition, Vector2.Zero, type, damage, knockBack, item.owner);
            }
            return false;
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
    }

    class Supercell : ModProjectile
    {
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
        }
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 52;
            projectile.height = 50;
            projectile.friendly = true;
            projectile.sentry = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 10;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        void FindFrame(int FrameHeight)
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 4;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }

        int PinWait = 60;
        int PinRoundsLeft = 2;
        public override void AI()
        {
            FindFrame(projectile.height);
            projectile.timeLeft = 10;
            bool Targeted = false;
            //projectile.velocity.Y = 50;
            //projectile.velocity.X = 0;
            Player player = Main.player[projectile.owner];
            NPC target = Main.npc[0];
            if (player.HasMinionAttackTargetNPC && Main.npc[player.MinionAttackTargetNPC].Distance(projectile.Center) < 1500)
            {
                target = Main.npc[player.MinionAttackTargetNPC];
                Targeted = true;
            }
            else
            {
                float Distance = 750;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                    {
                        target = Main.npc[i];
                        Distance = Main.npc[i].Distance(projectile.Center);
                        Targeted = true;
                    }
                }
            }
            if (Targeted)
            {
                PinWait--;
                if (PinWait <= 0)
                {
                    PinWait = 10;
                    SpawnLightning(target.whoAmI);
                }
            }
        }


        public void SpawnLightning(int target)
        {
            Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
            float speed = Main.rand.NextFloat(10f, 25f);

            int proj = Projectile.NewProjectile(projectile.Center, direction * speed, ModContent.ProjectileType<Projectiles.SoulLightning>(), projectile.damage, 0.5f, projectile.owner);
            Main.projectile[proj].ai[0] = target;
        }
    }
}

