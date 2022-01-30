using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class ThunderJavelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Sticks onto hit enemies, zapping them or other nearby foes" +
                "\nWhile holding this item you gain 10 armor penetration");
        }

        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.critDamageMult = 1.15f;
            item.damage = 42;
            item.ranged = true;
            item.width = 58;
            item.height = 58;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 15;
            item.useAnimation = 15;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 0, 5, 0);
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = true;
            item.shootSpeed = 35;
            item.shoot = mod.ProjectileType("ThunderJavelinProjectile");
        }

        public override void HoldItem(Player player)
        {
            player.armorPenetration += 10;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 3);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 225);
            recipe.AddRecipe();
        }
    }

    class ThunderJavelinProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 74;
            projectile.height = 74;
            projectile.ranged = true;
            projectile.timeLeft = 3600;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.extraUpdates = 4;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.arrow = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 14;
            height = 14;
            return true;
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
            int newSize = 14;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }

        bool stuck = false;
        NPC stuckNPC;
        int stuckTimeLeft = 180;
        Vector2 offset;
        bool start = true;
        int projectileCounter = 0;
        public override void AI()
        {
            if (start)
            {
                start = false;
                projectile.velocity /= projectile.extraUpdates + 1;
                stuckTimeLeft *= projectile.extraUpdates + 1;
                projectile.timeLeft *= projectile.extraUpdates + 1;
            }

            if (stuck)
            {
                projectile.tileCollide = false;
                projectile.position = stuckNPC.position - offset;
                if (!stuckNPC.active)
                {
                    projectile.active = false;
                }

                stuckTimeLeft--;
                if (stuckTimeLeft <= 0)
                {
                    projectile.timeLeft = 1;
                }

                projectileCounter--;
                if (projectileCounter <= 0)
                {
                    projectileCounter = 50 * (projectile.extraUpdates + 1);

                    NPC target = Main.npc[0];
                    bool foundTarget = false;
                    float distance = 2000;

                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.whoAmI != stuckNPC.whoAmI && npc.Distance(stuckNPC.Center) <= distance && npc.CanBeChasedBy() && projectile.CanHit(npc) && !npc.friendly)
                        {
                            distance = npc.Distance(stuckNPC.Center);
                            target = npc;
                            foundTarget = true;
                        }
                    }

                    if (!foundTarget)
                    {
                        target = stuckNPC;
                    }


                    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                    float speed = Main.rand.NextFloat(10f, 25f);

                    int proj = Projectile.NewProjectile(projectile.Center, direction * speed, ModContent.ProjectileType<Projectiles.SoulLightning>(), projectile.damage / 3, 0.5f, projectile.owner);
                    Main.projectile[proj].ranged = true;
                    Main.projectile[proj].ai[0] = target.whoAmI;
                }
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135);
            }
        }
    }
}
