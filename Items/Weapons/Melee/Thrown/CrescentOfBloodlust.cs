using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Melee.Thrown
{
    class CrescentOfBloodlust : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 2)
                .AddIngredient(ItemID.TissueSample, 1)
                .AddIngredient<Materials.SanguineFang>(4)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 2)
                .AddIngredient(ItemID.ShadowScale, 1)
                .AddIngredient<Materials.SanguineFang>(4)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Throws a boomerang that returns to you and leaves a trail of homing blood" +
                "\nThe higher the stack, the more boomerangs you can throw at once" +
                "\nMaximum stack of three"); */
        }
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.width = 20;
            Item.height = 34;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(0, 0, 35, 0);
            Item.shootSpeed = 35;
            Item.shoot = ModContent.ProjectileType<CrescentOfBloodlust_Projectile>();
            Item.noUseGraphic = true;
            Item.autoReuse = true;
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
    class CrescentOfBloodlust_Projectile : ModProjectile
    {
        public override string Texture { get { return "TerrorbornMod/Items/Weapons/Melee/Thrown/CrescentOfBloodlust"; } }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
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
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0f);
            }
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 20;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position); //Sound for when it hits a block

            // B O U N C E
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
            return false;
        }

        int TimeUntilReturn = 9;
        int BloodWait = 10;
        bool Start = true;
        public override void AI()
        {
            if (Start)
            {
                Start = false;
                BloodWait = Main.rand.Next(8, 15);
            }
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

            BloodWait--;
            if (BloodWait <= 0)
            {
                BloodWait = Main.rand.Next(8, 15);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position + new Vector2(Main.rand.Next(Projectile.width), Main.rand.Next(Projectile.height)), Projectile.velocity / 9, ModContent.ProjectileType<Sanguine>(), Projectile.damage / 2, 0, Projectile.owner);
            }
        }
    }
    public class Sanguine : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.hide = true;
            Projectile.timeLeft = 100;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity;
            NPC targetNPC = Main.npc[0];
            float Distance = 375; //max distance away
            bool Targeted = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(Projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    targetNPC = Main.npc[i];
                    Distance = Main.npc[i].Distance(Projectile.Center);
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                //HOME IN
                float speed = .6f;
                Vector2 move = targetNPC.Center - Projectile.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                move *= speed / magnitude;
                Projectile.velocity += move;
            }
        }
    }
}
