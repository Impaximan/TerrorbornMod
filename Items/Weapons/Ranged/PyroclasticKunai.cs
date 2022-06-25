using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TerrorbornMod.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class PyroclasticKunai : ModItem
    {
        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            modItem.critDamageMult = 1.2f;
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 21;
            Item.useAnimation = 21;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item39;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shootSpeed = 25;
            Item.shoot = ModContent.ProjectileType<PyroclasticKunaiProjectile>();
        }

        float maxRotation = MathHelper.ToRadians(10f);
        int kunaiCount = 5;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = -2; i <= 2; i++)
            {
                velocity = velocity.RotatedBy((maxRotation / kunaiCount) * i);
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<PyroclasticGemstone>(), 15)
                .AddIngredient(ModContent.ItemType<IncendiusAlloy>(), 10)
                .AddIngredient(ItemID.ThrowingKnife, 50)
                .AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>())
                .Register();
        }
    }

    class PyroclasticKunaiProjectile : ModProjectile
    {
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture + "Glow");
            Vector2 position = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, position, new Rectangle(0, 0, Projectile.width, Projectile.height), Color.White * (1f - (Projectile.alpha / 255f)), Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.scale, SpriteEffects.None, 0);
        }

        int FallWait = 40;
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 38;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hide = false;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 3000;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (crit)
            {
                bool spawnPortal = true;
                foreach (Projectile Projectile in Main.projectile)
                {
                    if (Projectile.type == ModContent.ProjectileType<PyroclasticPortal>() && Projectile.active)
                    {
                        spawnPortal = false;
                        break;
                    }
                }

                if (spawnPortal)
                {
                    Player player = Main.player[Projectile.owner];
                    Projectile.NewProjectile(Projectile.GetSource_OnHit(target), player.Center, Vector2.Zero, ModContent.ProjectileType<PyroclasticPortal>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (FallWait <= 0)
            {
                damage = (int)(damage * 0.75f);
            }
        }

        public override void AI()
        {
            if (FallWait > 0)
            {
                FallWait--;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            }
            else
            {
                Projectile.velocity *= 0.9f;
                Projectile.alpha += 255 / 20;
                if (Projectile.alpha >= 255)
                {
                    Projectile.active = false;
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundExtensions.PlaySoundOld(SoundID.Dig, Projectile.position);
        }
    }

    class PyroclasticPortal : ModProjectile
    {
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture + "Glow");
            Vector2 position = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, position, new Rectangle(0, 0, Projectile.width, Projectile.height), Color.White * (1f - (Projectile.alpha / 255f)), Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), Projectile.scale, SpriteEffects.None, 0);
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hide = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = trueTimeLeft + 300;
        }

        float maxRotation = MathHelper.ToRadians(10f);
        int kunaiCount = 3;
        int trueTimeLeft = 120;
        int ProjectileWait = 30;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.rotation -= MathHelper.ToRadians(5f);

            if (trueTimeLeft > 0)
            {
                trueTimeLeft--;
                if (player.itemTime > 0)
                {
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        ProjectileWait = 30;
                        for (int i = -1; i <= 1; i++)
                        {
                            Vector2 velocity = (Projectile.DirectionTo(Main.MouseWorld) * 25).RotatedBy((maxRotation / kunaiCount) * i);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<PyroclasticKunaiProjectile>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                        }
                    }
                }
            }
            else
            {
                Projectile.alpha += 15;
                if (Projectile.alpha >= 255)
                {
                    Projectile.active = false;
                }
                Projectile.scale -= 0.02f;
            }
        }
    }
}
