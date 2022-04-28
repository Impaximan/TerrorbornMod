using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items
{
    class LunarRitual : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Not consumable" +
                "\nSummons a Mysterious Crab");
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
        }
        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == ModContent.ProjectileType<LunarPortal>() && proj.active)
                {
                    return false;
                }
            }
            return player.ZoneBeach && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.TidalTitan.TidalTitan>()) && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.TidalTitan.MysteriousCrab>());
        }
        public override bool? UseItem(Player player)
        {
            Vector2 position = player.Center + new Vector2(0, -350);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item117, position);
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), position, Vector2.Zero, ModContent.ProjectileType<LunarPortal>(), 0, 0);
            return true;
        }
    }

    class LunarPortal : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Portal";
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.timeLeft = 10000;
            Projectile.damage = 1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
        }

        float backRotation;
        float frontRotation;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Color color = Color.DeepSkyBlue;
            float scaleMultiplier = 4f;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Projectile.GetAlpha(color), backRotation, new Vector2(texture.Width / 2, texture.Height / 2), 1.2f * scaleMultiplier, SpriteEffects.FlipHorizontally, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Projectile.GetAlpha(color), frontRotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f * scaleMultiplier, SpriteEffects.None, 0);
            return false;
        }

        int trueTimeLeft = 120;
        bool spawned = false;
        int spawnCounter = 75;


        public override void AI()
        {
            if (trueTimeLeft > 0)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 255 / 30;
                }
                else
                {
                    Projectile.alpha = 0;
                }
                trueTimeLeft--;
            }
            else
            {
                if (Projectile.alpha < 255)
                {
                    Projectile.alpha += 255 / 30;
                }
                else
                {
                    Projectile.active = false;
                }
            }
            float rotationSpeed = 5f;
            backRotation += MathHelper.ToRadians(rotationSpeed);
            frontRotation -= MathHelper.ToRadians(rotationSpeed);

            if (spawnCounter > 0)
            {
                spawnCounter--;
            }
            else if (!spawned)
            {
                spawned = true;
                NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, ModContent.NPCType<NPCs.Bosses.TidalTitan.MysteriousCrab>());
            }
        }
    }
}
