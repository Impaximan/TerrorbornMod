using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Ranged.Bows
{
    class StormsWrath : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm's Wrath");
            Tooltip.SetDefault("Fires a storm bolt along side it's arrows. \nStorm bolts will summon smaller bolt upon hitting foes.");
        }

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 26;
            Item.height = 56;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item75;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 18f;
            Item.useAmmo = AmmoID.Arrow;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, 0);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Cloud, 15)
                .AddIngredient(ItemID.RainCloud, 5)
                .AddIngredient(ItemID.Feather, 15)
                .AddIngredient<Materials.AzuriteBar>(12)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X / 5, velocity.Y / 5, ModContent.ProjectileType<StormsBeam>(), damage, knockback, player.whoAmI);
            return true;
        }
    }

    class BoltBallista : ModItem
    {
        int firesTilBolt = 1;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires a storm bolt along side it's arrows, which are converted to Bolt Arrows.\nStorm bolts will summon a smaller bolt upon hitting foes.\n20% chance to not consume ammo");
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(101) <= 20)
            {
                return false;
            }
            return base.CanConsumeAmmo(ammo, player);
        }

        public override void SetDefaults()
        {
            Item.damage = 22;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 36;
            Item.height = 64;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 4, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 35f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<StormsWrath>()
                .AddIngredient(ModContent.ItemType<Materials.ThunderShard>(), 20)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.SoulofFlight, 15)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<ThunderArrow>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            firesTilBolt--;
            if (firesTilBolt <= 0)
            {
                SoundExtensions.PlaySoundOld(SoundID.Item75, position);
                firesTilBolt = 2;
                for (int i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(source, position.X - 50 + Main.rand.Next(100), position.Y - 50 + Main.rand.Next(100), velocity.X / 5, velocity.Y / 5, ModContent.ProjectileType<StormsBeam>(), (int)(damage * 0.75f), knockback, player.whoAmI);
                }
            }
            return true;
        }
    }

    class StormsBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm's Beam");
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 1600;
            Projectile.penetrate = 1;
            Projectile.hide = true;
            Projectile.width = 4;
            Projectile.height = 4;
        }

        public override string Texture => "TerrorbornMod/placeholder";

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Projectile.damage = (int)(Projectile.damage * 0.8);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(target.position.X + Main.rand.Next(0, target.width), target.Center.Y - target.height * 1.5f), new Vector2(0, 10), ModContent.ProjectileType<StormsBolt>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack, Projectile.owner);
        }

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, 1, 1, 88, 0f, 0f, 0, Scale: 0.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].position = Projectile.position;
            Main.dust[dust].scale = 0.75f;
            Main.dust[dust].velocity *= 0.2f;
        }
    }

    class StormsBolt : ModProjectile
    {
        public override string Texture => "TerrorbornMod/placeholder";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm's Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 15;
            Projectile.penetrate = 1;
            Projectile.hide = true;
            Projectile.width = 4;
            Projectile.height = 4;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, 1, 1, 88, 0f, 0f, 0, Scale: 0.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].position = Projectile.position;
            Main.dust[dust].scale = 0.5f;
            Main.dust[dust].velocity *= 0.2f;
        }
    }

    class ThunderArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(Color.White) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 5;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 14;
            height = 14;
            return true;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 88, 0f, 0f, 0, Scale: 1f);
            Main.dust[dust].velocity = Projectile.velocity;
            Main.dust[dust].noGravity = true;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - MathHelper.ToRadians(90);
        }
    }
}
