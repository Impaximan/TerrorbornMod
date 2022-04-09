using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Melee
{
    public class Tarfury : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Causes multiple tar Droplets to rain from the sky.\nTrue melee hits will inflict the enemies with 'Oiled' and 'On Fire!'\nOiled enemies will take 30% increased damage from tar Droplets." +
                "\n'Hunger will always rain'");
        }

        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.DamageType = DamageClass.Melee;
            Item.width = 32;
            Item.height = 46;
            Item.useTime = 27;
            Item.useAnimation = 27;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TarDroplet>();
            Item.shootSpeed = 25f;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 60 * 4);
            target.AddBuff(BuffID.Oiled, 60 * 8);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AntlionMandible, 8)
                .AddIngredient(ItemID.Starfury)
                .AddIngredient(ModContent.ItemType<Materials.TarOfHunger>(), 80)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 target = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            float ceilingLimit = target.Y;
            if (ceilingLimit > player.Center.Y - 200f)
            {
                ceilingLimit = player.Center.Y - 200f;
            }
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                position = player.Center + new Vector2((-(float)Main.rand.Next(0, 401) * player.direction), -600f);
                position.Y -= (100 * i);
                Vector2 heading = target - position;
                if (heading.Y < 0f)
                {
                    heading.Y *= -1f;
                }
                if (heading.Y < 20f)
                {
                    heading.Y = 20f;
                }
                heading.Normalize();
                heading *= new Vector2(velocity.X, velocity.Y).Length();
                velocity.X = heading.X;
                velocity.Y = heading.Y + Main.rand.Next(-40, 41) * 0.02f;
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X / 5, velocity.Y / 5, type, damage, knockback, player.whoAmI, 0f, ceilingLimit);
            }
            return false;
        }
    }
    class TarDroplet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 12;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = false;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 4;
            Projectile.timeLeft = 600;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.HasBuff(BuffID.Oiled))
            {
                damage = (int)(damage * 1.3f);
            }
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }
    }
}
