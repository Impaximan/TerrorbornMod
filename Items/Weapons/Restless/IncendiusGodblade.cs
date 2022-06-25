using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class IncendiusGodblade : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/IncendiusGodblade";
        int UntilBlast;
        public override void restlessSetStaticDefaults()
        {
            DisplayName.SetDefault("Sinful Saber");
        }

        public override string defaultTooltip()
        {
            return "Fires an inaccurate slash Projectile by default";
        }

        public override string altTooltip()
        {
            return "Throws the sword at the cursor, causing it to leave a trail of slashes while returning" +
                "\nIf the sword hits an enemy while not returning, it will create multiple slashes";
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            Item.damage = 46;
            Item.DamageType = DamageClass.Melee;
            Item.width = 58;
            Item.height = 62;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<IncendiusSlash>();
            Item.shootSpeed = 20;
            Item.crit = 7;
            Item.autoReuse = true;
            modItem.restlessTerrorDrain = 12f;
            modItem.restlessChargeUpUses = 5;
        }

        public override bool RestlessCanUseItem(Player player)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            if (modItem.RestlessChargedUp())
            {
                Item.noMelee = true;
                Item.noUseGraphic = true;
            }
            else
            {
                Item.noUseGraphic = false;
                Item.noMelee = false;
            }
            return base.RestlessCanUseItem(player);
        }

        public override bool RestlessShoot(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            if (modItem.RestlessChargedUp())
            {
                type = ModContent.ProjectileType<IncendiarySwordThrown>();
                velocity.X *= 2.5f;
                velocity.Y *= 2.5f;
                Item.noMelee = true;
                Item.noUseGraphic = true;
            }
            else
            {
                Vector2 rotatedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(50));
                rotatedSpeed *= Main.rand.NextFloat(0.65f, 1.35f);
                velocity.X = rotatedSpeed.X;
                velocity.Y = rotatedSpeed.Y;
                Item.noUseGraphic = false;
                Item.noMelee = false;
            }
            return base.RestlessShoot(player, source, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(35 * TerrorbornMod.IncendiaryAlloyMultiplier))
                .AddIngredient(ItemID.PalladiumBar, 12)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 10)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), (int)(35 * TerrorbornMod.IncendiaryAlloyMultiplier))
                .AddIngredient(ItemID.CobaltBar, 12)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 10)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }
    }

    class IncendiarySwordThrown : ModProjectile
    {
        int timeUntilReturn = 60;
        int penetrateUntilReturn = 3;

        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/IncendiusGodblade";
        public override void SetDefaults()
        {
            Projectile.width = 58;
            Projectile.height = 62;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 600;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (timeUntilReturn > 0)
            {
                for (int i = 0; i < Main.rand.Next(4, 6); i++)
                {
                    float speed = Main.rand.Next(8, 22);
                    Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * speed;
                    Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, velocity, ModContent.ProjectileType<IncendiusSlash>(), Projectile.damage / 2, 1, Projectile.owner);
                }
            }
            penetrateUntilReturn--;
            if (penetrateUntilReturn <= 0)
            {
                timeUntilReturn = 0;
                speed = Projectile.velocity.Length();
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 10;
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
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
            speed = Projectile.velocity.Length();
            timeUntilReturn = 0;
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
            return false;
        }

        float speed;
        int ProjectileWait = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (timeUntilReturn <= 0)
            {
                Projectile.rotation += 0.5f * player.direction;
                Projectile.tileCollide = false;
                Vector2 direction = Projectile.DirectionTo(player.Center);
                Projectile.velocity = direction * speed;

                if (Main.player[Projectile.owner].Distance(Projectile.Center) <= speed)
                {
                    Projectile.active = false;
                }

                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = Projectile.velocity;
                Main.dust[d].noLight = true;

                ProjectileWait--;
                if (ProjectileWait <= 0)
                {
                    ProjectileWait = 20;
                    float speed = 15f;
                    Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * speed;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<IncendiusSlash>(), Projectile.damage, 1, Projectile.owner);
                }
            }
            else
            {
                Projectile.direction = player.direction;
                Projectile.spriteDirection = player.direction;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135);
                if (Projectile.spriteDirection == 1)
                {
                    Projectile.rotation -= MathHelper.ToRadians(90f);
                }
                timeUntilReturn--;
                if (timeUntilReturn <= 0)
                {
                    speed = Projectile.velocity.Length();
                }
            }
        }
    }

    class IncendiusSlash : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 26;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 300;
        }
        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.ToRadians(90);
            Projectile.alpha += 255 / 60;
            if (Projectile.alpha >= 255)
            {
                Projectile.active = false;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 position = Projectile.Center - Main.screenPosition;
            position.Y += 4;
            Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, Projectile.width, Projectile.height), new Rectangle(0, 0, Projectile.width, Projectile.height), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), SpriteEffects.None, 0);
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 30;
            height = 30;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }
    }
}
