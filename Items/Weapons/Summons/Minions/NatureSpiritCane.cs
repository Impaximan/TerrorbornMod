using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Weapons.Summons.Minions
{
    class NatureSpiritCane : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a nature spirit that spins around you and attacks foes, inflicting poison");
        }
        public override void SetDefaults()
        {
            Item.mana = 5;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 8;
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<NatureSpirit>();
            Item.shootSpeed = 10f;
            Item.value = Item.sellPrice(0, 0, 50, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.Wood, 25)
                .AddIngredient(ItemID.Emerald, 2)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim(false);
            }
            return base.UseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                int proj = Projectile.NewProjectile(source, new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition, Vector2.Zero, type, damage, knockback, player.whoAmI);
                int minionNumber = 1;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].minion && player.slotsMinions > player.maxMinions && Main.projectile[i].active)
                    {
                        Main.projectile[i].active = false;
                    }
                    if (Main.projectile[i].type == type && Main.projectile[i].active)
                    {
                        minionNumber++;
                        if (minionNumber > player.maxMinions + 1) minionNumber = player.maxMinions + 1;
                    }
                }
                Main.projectile[proj].ai[0] = minionNumber;
                foreach (Projectile Projectile in Main.projectile)
                {
                    if (Projectile.type == type && Projectile.active)
                    {
                        Projectile.ai[1] = minionNumber - 1;
                    }
                }
                if (player.slotsMinions <= player.maxMinions)
                {
                    player.AddBuff(ModContent.BuffType<NatureSpiritBuff>(), 60);
                }
            }
            return false;
        }
    }

    class NatureSpirit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = 26;
            Projectile.height = 36;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 360;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.minion = true;
Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle originalHitbox = hitbox;
            int newSize = 40;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
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

        public override void Kill(int timeLeft)
        {
            DustExplosion(Projectile.Center, 0, 25, 7, DustID.t_LivingWood, DustScale: 1f, NoGravity: true);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 originPoint = Main.player[Projectile.owner].Center;
            Vector2 center = Projectile.Center;
            Vector2 distToProj = originPoint - Projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TerrorbornMod/Items/Weapons/Summons/Minions/NatureSpiritVine");

            while (distance > texture.Height && !float.IsNaN(distance))
            {
                distToProj.Normalize();
                distToProj *= texture.Height;
                center += distToProj;
                distToProj = originPoint - center;
                distance = distToProj.Length();

                //Draw chain
                Main.spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, texture.Width, texture.Height), lightColor, projRotation,
                    new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return true;
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

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, Color.Red, DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Poisoned, 60 * 10);
        }

        int mode = 0;
        float distance = 0f;
        float rotation = 0f;
        public override void AI()
        {
            Projectile.timeLeft = 500;
            if (Projectile.velocity.X > 0)
            {
                Projectile.spriteDirection = -1;
            }
            else
            {
                Projectile.spriteDirection = 1;
            }

            FindFrame(Projectile.height);

            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!player.HasBuff(ModContent.BuffType<NatureSpiritBuff>()))
            {
                Projectile.active = false;
            }

            bool Targeted = false;
            NPC target = Main.npc[0];

            float Distance = 1000;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(Projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    target = Main.npc[i];
                    Distance = Main.npc[i].Distance(Projectile.Center);
                    Targeted = true;
                }
            }

            if (player.HasMinionAttackTargetNPC)
            {
                target = Main.npc[player.MinionAttackTargetNPC];
            }

            if (!Projectile.CanHitWithOwnBody(player) || !Targeted || !Projectile.CanHitWithOwnBody(target))
            {
                mode = 0;
            }
            else
            {
                mode = 1;
            }

            rotation = modPlayer.NatureSpiritRotation + MathHelper.ToRadians(360f * Projectile.ai[0] / Projectile.ai[1]);

            if (mode == 0)
            {
                distance = MathHelper.Lerp(distance, 100f, 0.1f);
                Projectile.friendly = false;
            }

            if (mode == 1)
            {
                distance = MathHelper.Lerp(distance, player.Distance(target.Center), 0.1f);
                Projectile.friendly = true;
            }

            Projectile.position = player.Center + rotation.ToRotationVector2() * distance - Projectile.Size / 2;
            Projectile.rotation = rotation - MathHelper.ToRadians(90f);
        }
    }

    class NatureSpiritBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nature Spirit");
            Description.SetDefault("A nature spirit is fighting for you!");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.NatureSpiritRotation += MathHelper.ToRadians(10f);
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<NatureSpirit>() && Main.projectile[i].active)
                {
                    player.buffTime[buffIndex] = 60;
                }
            }
        }
    }
}

