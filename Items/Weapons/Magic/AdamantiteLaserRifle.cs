using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class AdamantiteLaserRifle : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.AdamantiteBar, 13);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires an adamantite laser that ricochets to nearby enemies on critical hits and bounces off of walls");
        }

        public override void SetDefaults()
        {
            item.damage = 33;
            item.noMelee = true;
            item.width = 66;
            item.height = 34;
            item.useTime = 5;
            item.useAnimation = 5;
            item.useStyle = 5;
            item.crit = 14;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item33;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("AdamantiteLaser");
            item.shootSpeed = 10f;
            item.mana = 4;
            item.magic = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 66);
            return true;
        }
    }

    class AdamantiteLaser : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 12;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.hide = true;
            projectile.extraUpdates = 100;
            projectile.timeLeft = 350;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustPerfect(projectile.Center, 60);
            dust.noGravity = true;
            dust.noLight = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < projectile.localNPCImmunity.Length; i++)
            {
                if (projectile.localNPCImmunity[i] < 0 || projectile.localNPCImmunity[i] > 5)
                {
                    projectile.localNPCImmunity[i] = 5;
                }
            }
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        List<NPC> alreadyHit = new List<NPC>();
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!crit)
            {
                return;
            }
            alreadyHit.Add(target);

            bool targeted = false;
            NPC launchTo = Main.npc[0];
            float distance = 2000;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.Distance(projectile.Center) < distance && !alreadyHit.Contains(npc) && projectile.CanHit(npc) && !npc.friendly && npc.CanBeChasedBy())
                {
                    distance = npc.Distance(projectile.Center);
                    launchTo = npc;
                    targeted = true;
                }
            }

            if (targeted)
            {
                float speed = projectile.velocity.Length();
                Vector2 direction = projectile.DirectionTo(launchTo.Center);
                projectile.velocity = speed * direction;
            }
        }
    }
}

