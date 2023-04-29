using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Melee.Thrown
{
    class Armorang : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.ShellFragments>(), 4)
                .AddRecipeGroup(RecipeGroupID.IronBar, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Throws a boomerang that bounces between enemies" +
                "\nMaximum stack of three"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.width = 20;
            Item.height = 34;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.shootSpeed = 20;
            Item.shoot = ModContent.ProjectileType<Armorang_Projectile>();
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.maxStack = 3;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
        }

        public override bool CanUseItem(Player player)
        {
            int CrescentCount = 1;
            for (int i = 0; i < 300; i++)
            {
                if (Main.projectile[i].type == Item.shoot && Main.projectile[i].active)
                {
                    CrescentCount++;
                }
            }
            return CrescentCount <= Item.stack;
        }
    }

    class Armorang_Projectile : ModProjectile
    {
        public override string Texture { get { return "TerrorbornMod/Items/Weapons/Melee/Thrown/Armorang"; } }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void PostDraw(Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                SpriteEffects effects = SpriteEffects.None;
                if (Projectile.spriteDirection == -1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Items/Weapons/Melee/Thrown/Armorang_Glow"), drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
            }
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 18;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 18;
            height = 18;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);

            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y;
            }

            TimeUntilReturn = 0;
            return false;
        }



        int TimeUntilReturn = 25;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.spriteDirection = player.direction * -1;
            Projectile.rotation += 0.5f * player.direction;
            if (TimeUntilReturn <= 0)
            {
                Projectile.tileCollide = false;
                Vector2 targetPosition = Main.player[Projectile.owner].Center;
                float speed = 3.4f;
                Vector2 move = targetPosition - Projectile.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                move *= speed / magnitude;
                Projectile.velocity += move;
                Projectile.velocity *= 0.90f;
                if (Main.player[Projectile.owner].Distance(Projectile.Center) <= 30)
                {
                    Projectile.active = false;
                }
            }
            else
            {
                TimeUntilReturn--;
            }
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (TimeUntilReturn <= 0)
            {
                return;
            }

            bool targeted = false;
            NPC launchTo = Main.npc[0];
            float distance = 2000;

            foreach (NPC NPC in Main.npc)
            {
                if (NPC.active && NPC.Distance(Projectile.Center) < distance && Projectile.CanHitWithOwnBody(NPC) && !NPC.friendly && NPC.CanBeChasedBy() && !(NPC == target))
                {
                    distance = NPC.Distance(Projectile.Center);
                    launchTo = NPC;
                    targeted = true;
                }
            }

            if (targeted)
            {
                float speed = Projectile.velocity.Length();
                Vector2 direction = Projectile.DirectionTo(launchTo.Center);
                Projectile.velocity = speed * direction;
            }
        }
    }
}
