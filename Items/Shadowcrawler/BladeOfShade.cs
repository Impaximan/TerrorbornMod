using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace TerrorbornMod.Items.Shadowcrawler
{
    public class BladeOfShade : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 18)
                .AddIngredient(ItemID.HallowedBar, 6)
                .AddTile(TileID.MythrilAnvil)
                .Register();

        }

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Hitting enemies causes Nightmare flames to erupt from them towards your cursor");
        }

        public override void SetDefaults()
        {
            Item.damage = 61;
            Item.DamageType = DamageClass.Melee;
            Item.width = 52;
            Item.height = 52;
            Item.useTime = 8;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(gold: 2, silver: 70);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 mousePosition = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            for (int i = 0; i < 2; i++)
            {
                Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center, target.DirectionTo(mousePosition).RotatedByRandom(MathHelper.ToRadians(5)), ModContent.ProjectileType<NightmareBoilMelee>(), Item.damage / 2, hit.Knockback, Owner: player.whoAmI);
            }
        }
    }

    class NightmareBoilMelee : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            //Projectile.extraUpdates = 100;
            Projectile.timeLeft = 60;
            Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.hide = true;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 74, 0f, 0f, 100, Scale: 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity;
            float rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(180);
            float Speed = 1f;
            Projectile.velocity += new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
        }
    }
}

