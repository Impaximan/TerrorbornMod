using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class GaussStriker : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires eratic, but infinitely piercing, bolts of lightning");
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 18)
                .AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.noMelee = true;
            Item.width = 48;
            Item.height = 20;
            Item.useTime = 8;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAnimation = 8;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item12;
            Item.autoReuse = true;
            Item.shootSpeed = 25f;
            Item.shoot = ModContent.ProjectileType<GaussBolt>();
            Item.mana = 6;
            Item.DamageType = DamageClass.Magic;;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
    }

    class GaussBolt : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly; } }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 25;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 400;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.extraUpdates = 100;
            Projectile.hide = true;
        }

        void FindFrame(int FrameHeight)
        {
            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frame++;
                Projectile.frameCounter = 5;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }

        //public override void PostDraw(Color lightColor)
        //{
        //    base.PostDraw(spriteBatch, lightColor);
        //    Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
        //    Vector2 position = Projectile.position - Main.screenPosition;
        //    position += new Vector2(Projectile.width / 2, Projectile.height / 2);
        //    //position.Y += 4;
        //    Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, Projectile.width, Projectile.height), new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), SpriteEffects.None, 0);
        //}

        bool start = true;
        float rotationCounter = 15;
        public override void AI()
        {
            if (start)
            {
                start = false;
                Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(25));
            }

            //FindFrame(Projectile.height);
            //Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

            if (Main.player[Projectile.owner].Distance(Projectile.Center) > 30)
            {
                for (int i = 0; i < 4; i++)
                {
                    int dust = Dust.NewDust(Projectile.Center - (Projectile.velocity * i / 4), 1, 1, 62, 0, 0, Scale: 2, newColor: Color.White);
                    Main.dust[dust].noGravity = true;

                    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                    float speed = Main.rand.NextFloat(1.5f, 3f);

                    Main.dust[dust].velocity = direction * speed;
                }
            }

            rotationCounter--;
            if (rotationCounter <= 0)
            {
                rotationCounter = Main.rand.Next(10, 20);
                Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(25));
            }
        }
    }
}


