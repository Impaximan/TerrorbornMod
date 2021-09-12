using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class Armorang : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.ShellFragments>(), 4);
            recipe.AddIngredient(ItemID.IronBar, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Throws a boomerang that bounces between enemies" +
                "\nMaximum stack of three");
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.width = 20;
            item.height = 34;
            item.useTime = 16;
            item.useAnimation = 16;
            item.rare = 1;
            item.useStyle = 1;
            item.knockBack = 3f;
            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.shootSpeed = 20;
            item.shoot = ModContent.ProjectileType<Armorang_projectile>();
            item.noUseGraphic = true;
            item.autoReuse = false;
            item.maxStack = 3;
            item.melee = true;
            item.noMelee = true;
        }

        public override bool CanUseItem(Player player)
        {
            int CrescentCount = 1;
            for (int i = 0; i < 300; i++)
            {
                if (Main.projectile[i].type == item.shoot && Main.projectile[i].active)
                {
                    CrescentCount++;
                }
            }
            return CrescentCount <= item.stack;
        }
    }

    class Armorang_projectile : ModProjectile
    {
        public override string Texture { get { return "TerrorbornMod/Items/Weapons/Melee/Armorang"; } }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                SpriteEffects effects = SpriteEffects.None;
                if (projectile.spriteDirection == -1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/Items/Weapons/Melee/Armorang_Glow"), drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
            }
        }

        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.width = 18;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 8;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 18;
            height = 18;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(0, projectile.position);

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

            TimeUntilReturn = 0;
            return false;
        }



        int TimeUntilReturn = 25;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            projectile.spriteDirection = player.direction * -1;
            projectile.rotation += 0.5f * player.direction;
            if (TimeUntilReturn <= 0)
            {
                projectile.tileCollide = false;
                Vector2 targetPosition = Main.player[projectile.owner].Center;
                float speed = 3.4f;
                Vector2 move = targetPosition - projectile.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                move *= speed / magnitude;
                projectile.velocity += move;
                projectile.velocity *= 0.90f;
                if (Main.player[projectile.owner].Distance(projectile.Center) <= 30)
                {
                    projectile.active = false;
                }
            }
            else
            {
                TimeUntilReturn--;
            }
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (TimeUntilReturn <= 0)
            {
                return;
            }

            bool targeted = false;
            NPC launchTo = Main.npc[0];
            float distance = 2000;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.Distance(projectile.Center) < distance && projectile.CanHit(npc) && !npc.friendly && npc.CanBeChasedBy() && !(npc == target))
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
