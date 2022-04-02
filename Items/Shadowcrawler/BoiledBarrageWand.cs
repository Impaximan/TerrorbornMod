using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Shadowcrawler
{
    class BoiledBarrageWand : ModItem
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
            DisplayName.SetDefault("Midnight Barrage Staff");
            Tooltip.SetDefault("Rains midnight fireballs from above");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Pink;
            Item.width = 44;
            Item.height = 54;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Magic;;
            Item.UseSound = SoundID.Item20;
            Item.mana = 12;
            Item.damage = 76;
            Item.shootSpeed = 25;
            Item.knockBack = 1;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<MidnightFireballMagic>();
            Item.value = Item.sellPrice(0, 2, 70, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                Vector2 newPosition = new Vector2(Main.MouseWorld.X, player.Center.Y - 1000);
                newPosition.X += Main.rand.Next(-200, 201);
                newPosition.Y += Main.rand.Next(-100, 50);
                Vector2 direction = Main.MouseWorld - newPosition;
                direction.Normalize();
                Projectile.NewProjectile(source, newPosition, direction * Item.shootSpeed, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
    public class MidnightFireballMagic : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Projectiles/MidnightFireball";
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 60 * 2);
            Projectile.timeLeft = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }

        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
            }

            Projectile.tileCollide = Projectile.Center.Y > Main.player[Projectile.owner].Center.Y;

            Projectile.rotation += MathHelper.ToRadians(5);
            Lighting.AddLight(Projectile.Center, Color.Green.ToVector3() * 0.80f * Main.essScale);

            //int dust = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, 74);
            //Main.dust[dust].velocity = Projectile.velocity;

            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 74);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].noLight = true;
            Main.dust[dust].velocity = Projectile.velocity;

            NPC targetNPC = Main.npc[0];
            float Distance = 375; //max distance away
            bool Targeted = false;

            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(Projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    targetNPC = Main.npc[i];
                    Distance = Main.npc[i].Distance(Projectile.Center);
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                //HOME IN
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(1f * (Projectile.velocity.Length() / 20))).ToRotationVector2() * Projectile.velocity.Length();
            }

            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frameCounter = 5;
                Projectile.frame++;
                if (Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 0, 12, 7, 74, 2f, true);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }
    }
}

