using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class EctoGale : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/EctoGale";
        int UntilBlast;
        public override void restlessSetStaticDefaults()
        {
            DisplayName.SetDefault("EctoBurst");
        }

        public override string defaultTooltip()
        {
            return "Converts arrows into ecto orbs";
        }

        public override string altTooltip()
        {
            return "Ecto orbs will bounce off walls and eventually explode into arrows";
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            Item.damage = 120;
            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;
            Item.width = 38;
            Item.height = 18;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = SoundID.DD2_BallistaTowerShot;
            Item.autoReuse = true;
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Arrow;
            modItem.restlessChargeUpUses = 4;
            modItem.restlessTerrorDrain = 6;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool RestlessShoot(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            if (modItem.RestlessChargedUp())
            {
                int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<EctoOrb>(), damage, knockback, player.whoAmI);
                Main.projectile[proj].ai[0] = 1;
                Main.projectile[proj].ai[1] = type;
                Main.projectile[proj].penetrate = 1;
            }
            else
            {
                int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<EctoOrb>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Shadowcrawler.Nightbrood>())
                .AddIngredient(ItemID.Ectoplasm, 10)
                .AddIngredient(ItemID.Obsidian, 25)
                .AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 5)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }
    }

    class EctoOrb : ModProjectile
    {
        int timeUntilReturn = 30;
        int penetrateUntilReturn = 3;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.Projectile.type] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);

            BezierCurve bezier = new BezierCurve();
            bezier.Controls.Clear();
            foreach (Vector2 pos in Projectile.oldPos)
            {
                if (pos != Vector2.Zero && pos != null)
                {
                    bezier.Controls.Add(pos);
                }
            }

            if (bezier.Controls.Count > 1)
            {
                List<Vector2> positions = bezier.GetPoints(50);
                for (int i = 0; i < positions.Count; i++)
                {
                    float mult = (float)(positions.Count - i) / (float)positions.Count;
                    Vector2 drawPos = positions[i] - Main.screenPosition + Projectile.Size / 2;
                    Color color = Projectile.GetAlpha(Color.Lerp(Color.DarkCyan, Color.LightCyan, mult)) * mult;
                    Utils.Graphics.DrawGlow_1(Main.spriteBatch, drawPos, (int)(50f * mult), color);
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[0] == 1)
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                    Projectile.velocity.X = -oldVelocity.X * 0.5f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                    Projectile.velocity.Y = -oldVelocity.Y * 0.5f;
                }
                return false;
            }
            return base.OnTileCollide(oldVelocity);
        }

        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/AtomReaper"; 

        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 56;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 180;
        }

        float speed;
        int ProjectileWait = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[0] == 1)
            {
                for (int i = 0; i < Main.rand.Next(15, 22); i++)
                {
                    float speed = Main.rand.Next(15, 25);
                    Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * speed;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, (int)Projectile.ai[1], Projectile.damage / 2, 0f, Projectile.owner);
                    Main.projectile[proj].noDropItem = true;
                }
            }
        }
    }
}
