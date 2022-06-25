using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class SoulCyclone : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/SoulCyclone";
        int UntilBlast;
        public override void restlessSetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Cyclone");
            Item.staff[Item.type] = true;
        }

        public override string defaultTooltip()
        {
            return "Fires a random type of soul" +
                "\nEach soul type has a different behavior";
        }

        public override string altTooltip()
        {
            return "Fires all soul types at once";
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            Item.damage = 66;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;;
            Item.width = 74;
            Item.mana = 15;
            Item.height = 78;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item8;
            Item.shoot = ModContent.ProjectileType<SoulProjectile>();
            Item.shootSpeed = 20;
            Item.crit = 7;
            Item.autoReuse = true;
            modItem.restlessTerrorDrain = 8f;
            modItem.restlessChargeUpUses = 4;
        }

        public override bool RestlessCanUseItem(Player player)
        {
            return base.RestlessCanUseItem(player);
        }

        public override bool RestlessShoot(Player player, EntitySource_ItemUse_WithAmmo source, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(Item);
            if (modItem.RestlessChargedUp())
            {
                SoundExtensions.PlaySoundOld(SoundID.Item117, player.Center);
                for (int i = 0; i < 7; i++)
                {
                    int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                    Main.projectile[proj].ai[0] = i;
                }
            }
            else
            {
                int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, knockback, player.whoAmI);
                Main.projectile[proj].ai[0] = Main.rand.Next(7);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.ThunderShard>(), 20)
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ModContent.ItemType<Materials.TerrorSample>(), 5)
                .AddIngredient(ModContent.ItemType<Materials.SoulOfPlight>(), 5)
                .AddIngredient(ItemID.SoulofFright, 5)
                .AddIngredient(ItemID.SoulofSight, 5)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.SoulofFlight, 5)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }
    }

    class SoulProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        
        //SOUL NUMBERS:
        //0 = Soul of Flight
        //1 = Soul of Light
        //2 = Soul of Night
        //3 = Soul of Might
        //4 = Soul of Sight
        //5 = Soul of Fright
        //6 = Soul of Plight

        public override bool PreDraw(ref Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Color soulColor = Color.White;
            switch (Projectile.ai[0])
            {
                case 0:
                    soulColor = Color.Cyan;
                    break;
                case 1:
                    soulColor = Color.HotPink;
                    break;
                case 2:
                    soulColor = Color.Purple;
                    break;
                case 3:
                    soulColor = Color.CornflowerBlue;
                    break;
                case 4:
                    soulColor = Color.FromNonPremultiplied(144, 255, 144, 255);
                    break;
                case 5:
                    soulColor = Color.OrangeRed;
                    break;
                case 6:
                    soulColor = Color.Green;
                    break;
            }

            Vector2 drawOrigin = new Vector2(ModContent.Request<Texture2D>(Texture).Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(soulColor) * ((float)(Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, drawPos, new Rectangle?(), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 30;
            Projectile.aiStyle = 0;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 14;
            height = 14;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

            switch (Projectile.ai[0])
            {
                case 0:
                    FlightAI();
                    break;
                case 1:
                    LightAI();
                    break;
                case 2:
                    NightAI();
                    break;
                case 3:
                    MightAI();
                    break;
                case 4:
                    SightAI();
                    break;
                case 5:
                    FrightAI();
                    break;
                case 6:
                    PlightAI();
                    break;
            }
        }

        public void FlightAI()
        {
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(5f * (Projectile.velocity.Length() / 20))).ToRotationVector2() * Projectile.velocity.Length();
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public void LightAI()
        {
            int RotatationSpeed = 5;
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(RotatationSpeed * Direction));
            DirectionCounter--;
            if (DirectionCounter <= 0)
            {
                DirectionCounter = 10;
                Direction *= -1;
            }
        }

        public void NightAI()
        {
            int RotatationSpeed = 5;
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(RotatationSpeed * -Direction));
            DirectionCounter--;
            if (DirectionCounter <= 0)
            {
                DirectionCounter = 10;
                Direction *= -1;
            }
        }

        public void MightAI()
        {
            float speed = 3f;
            float currentVelocityLength = Projectile.velocity.Length();
            Projectile.velocity.Normalize();
            Projectile.velocity *= currentVelocityLength + speed;
        }

        public void SightAI()
        {
            NPC targetNPC = Main.npc[0];
            float Distance = 700; //max distance away
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
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(2f * (Projectile.velocity.Length() / 20))).ToRotationVector2() * Projectile.velocity.Length();
            }
        }

        public void FrightAI()
        {
            Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15));
        }

        public void PlightAI()
        {
            Projectile.extraUpdates = 2;
        }
    }
}
