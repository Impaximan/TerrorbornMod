using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items
{
    class MysteriousCompass : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("You feel emotionally attached to it somehow..." +
                "\n[c/FF1919:Points to something important]");
        }

        public override void SetDefaults()
        {
            Item.rare = -11;
            Item.accessory = true;
        }

        public override void HoldItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.MysteriousCompass = true;
            bool arrowExists = false;
            for (int i = 0; i < 1000; i++)
            {
                Projectile Projectile = Main.projectile[i];
                if (Projectile.type == ModContent.ProjectileType<CompassPointer>() && Projectile.active)
                {
                    arrowExists = true;
                    break;
                }
            }
            if (!arrowExists)
            {
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<CompassPointer>(), 0, 0, player.whoAmI);
            }
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.MysteriousCompass = true;
            bool arrowExists = false;
            for (int i = 0; i < 1000; i++)
            {
                Projectile Projectile = Main.projectile[i];
                if (Projectile.type == ModContent.ProjectileType<CompassPointer>() && Projectile.active)
                {
                    arrowExists = true;
                    break;
                }
            }
            if (!arrowExists)
            {
                Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<CompassPointer>(), 0, 0, player.whoAmI);
            }
        }
    }

    class CompassPointer : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 36;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 500;
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
            return TerrorbornSystem.ShriekOfHorror;
        }


        public override void AI()
        {
            Projectile.timeLeft = 500;

            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (!modPlayer.MysteriousCompass)
            {
                Projectile.active = false;
            }

            Projectile.position = player.Center + new Vector2(0, -65);
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
