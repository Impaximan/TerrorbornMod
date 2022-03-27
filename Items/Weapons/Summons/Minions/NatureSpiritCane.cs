using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

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
            item.mana = 5;
            item.summon = true;
            item.damage = 8;
            item.width = 34;
            item.height = 34;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 0;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("NatureSpirit");
            item.shootSpeed = 10f;
            item.value = Item.sellPrice(0, 0, 50, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup(RecipeGroupID.Wood, 25);
            recipe.AddIngredient(ItemID.Emerald, 2);
            recipe.SetResult(this);
            recipe.AddTile(TileID.Anvils);
            recipe.AddRecipe();
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse != 2)
            {
                int proj = Projectile.NewProjectile(new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition, Vector2.Zero, type, damage, knockBack, item.owner);
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
                foreach (Projectile projectile in Main.projectile)
                {
                    if (projectile.type == type && projectile.active)
                    {
                        projectile.ai[1] = minionNumber - 1;
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
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.penetrate = -1;
            projectile.width = 26;
            projectile.height = 36;
            projectile.tileCollide = false;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 360;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.minion = true;
            projectile.minionSlots = 1;
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
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 5;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 25, 7, DustID.t_LivingWood, DustScale: 1f, NoGravity: true);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 originPoint = Main.player[projectile.owner].Center;
            Vector2 center = projectile.Center;
            Vector2 distToProj = originPoint - projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            Texture2D texture = ModContent.GetTexture("TerrorbornMod/Items/Weapons/Summons/Minions/NatureSpiritVine");

            while (distance > texture.Height && !float.IsNaN(distance))
            {
                distToProj.Normalize();
                distToProj *= texture.Height;
                center += distToProj;
                distToProj = originPoint - center;
                distance = distToProj.Length();

                //Draw chain
                spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, texture.Width, texture.Height), lightColor, projRotation,
                    new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }
            return base.PreDraw(spriteBatch, lightColor);
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
            projectile.timeLeft = 500;
            if (projectile.velocity.X > 0)
            {
                projectile.spriteDirection = -1;
            }
            else
            {
                projectile.spriteDirection = 1;
            }

            FindFrame(projectile.height);

            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!player.HasBuff(ModContent.BuffType<NatureSpiritBuff>()))
            {
                projectile.active = false;
            }

            bool Targeted = false;
            NPC target = Main.npc[0];

            float Distance = 1000;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    target = Main.npc[i];
                    Distance = Main.npc[i].Distance(projectile.Center);
                    Targeted = true;
                }
            }

            if (player.HasMinionAttackTargetNPC)
            {
                target = Main.npc[player.MinionAttackTargetNPC];
            }

            if (!projectile.CanHit(player) || !Targeted || !projectile.CanHit(target))
            {
                mode = 0;
            }
            else
            {
                mode = 1;
            }

            rotation = modPlayer.NatureSpiritRotation + MathHelper.ToRadians(360f * projectile.ai[0] / projectile.ai[1]);

            if (mode == 0)
            {
                distance = MathHelper.Lerp(distance, 100f, 0.1f);
                projectile.friendly = false;
            }

            if (mode == 1)
            {
                distance = MathHelper.Lerp(distance, player.Distance(target.Center), 0.1f);
                projectile.friendly = true;
            }

            projectile.position = player.Center + rotation.ToRotationVector2() * distance - projectile.Size / 2;
            projectile.rotation = rotation - MathHelper.ToRadians(90f);
        }
    }

    class NatureSpiritBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Nature Spirit");
            Description.SetDefault("A nature spirit is fighting for you!");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            longerExpertDebuff = false;

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

