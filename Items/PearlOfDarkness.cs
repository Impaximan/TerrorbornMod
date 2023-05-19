using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items
{
    class PearlOfDarkness : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Use to point to where the next terror ability is for 10 seconds");
        }

        public override void SetDefaults()
        {
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<PearlOfDarknessProjectile>();
            Item.maxStack = 999;
            Item.noUseGraphic = true;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            bool canSpawn = true;
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == Item.shoot && proj.active)
                {
                    canSpawn = false;
                    break;
                }
            }
            return canSpawn;
        }
    }

    class PearlOfDarknessProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 60 * 10;
            Projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            position.Y += 4;
            Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, Projectile.width, Projectile.height), new Rectangle(0, 0, Projectile.width, Projectile.height), Color.White, Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), SpriteEffects.None, 0);
            return false;
        }

        Vector2 targetPosition()
        {
            Vector2 positionOfProjectile = Vector2.Zero;
            bool foundProjectile = false;

            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == ModContent.ProjectileType<Abilities.NecromanticCurse>() && proj.active)
                {
                    positionOfProjectile = proj.Center;
                    foundProjectile = true;
                    break;
                }
            }
            if (!foundProjectile)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.type == ModContent.ProjectileType<Abilities.HorrificAdaptation>() && proj.active)
                    {
                        positionOfProjectile = proj.Center;
                        foundProjectile = true;
                        break;
                    }
                }
            }
            if (!foundProjectile)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.type == ModContent.ProjectileType<Abilities.VoidBlink>() && proj.active)
                    {
                        positionOfProjectile = proj.Center;
                        foundProjectile = true;
                        break;
                    }
                }
            }
            if (!foundProjectile)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.type == ModContent.ProjectileType<Abilities.TerrorWarp>() && proj.active)
                    {
                        positionOfProjectile = proj.Center;
                        foundProjectile = true;
                        break;
                    }
                }
            }

            return positionOfProjectile;
        }


        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            Projectile.position = player.Center + new Vector2(0, -50);
            Projectile.position -= new Vector2(Projectile.width / 2, Projectile.height / 2);

            if (targetPosition() == Vector2.Zero)
            {
                return;
            }
            else
            {
                Projectile.rotation = Projectile.DirectionTo(targetPosition()).ToRotation() + MathHelper.ToRadians(90);
            }
        }
    }
}

