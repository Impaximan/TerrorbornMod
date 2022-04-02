using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class HexDefenderMask : ModItem
    {
        public override void AddRecipes()
        {
            int evilBars = 8;
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 4)
                .AddIngredient(ItemID.CrimtaneBar, evilBars)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 4)
                .AddIngredient(ItemID.DemoniteBar, evilBars)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("5% increased melee damage" +
                "\n4% increased melee critical strike chance");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.defense = 18;
            Item.rare = ItemRarityID.Pink;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<HexDefenderBreastplate>() && legs.type == ModContent.ItemType<HexDefenderGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Creates two spinning clock hands above you." +
                "\nPress the <ArmorAbility> mod hotkey while the arms are facing the same direction" +
                "\nto gain a temporary weapon speed bonus.";

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.HexDefender = true;

            bool spawnArms = true;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.type == ModContent.ProjectileType<HexedArms>())
                {
                    spawnArms = false;
                }
            }

            if (spawnArms)
            {
                Projectile.NewProjectile(player.GetProjectileSource_SetBonus(player.whoAmI), player.Center, Vector2.Zero, ModContent.ProjectileType<HexedArms>(), 0, 0, player.whoAmI);
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) *= 1.05f;
            player.GetCritChance(DamageClass.Melee) += 4;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class HexDefenderBreastplate : ModItem
    {
        public override void AddRecipes()
        {
            int evilBars = 12;
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 6)
                .AddIngredient(ItemID.CrimtaneBar, evilBars)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 6)
                .AddIngredient(ItemID.DemoniteBar, evilBars)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("7% increased melee damage" +
                "\n5% increased melee speed");
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 20;
            Item.defense = 24;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) *= 1.07f;
            player.meleeSpeed += 0.05f;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class HexDefenderGreaves : ModItem
    {
        public override void AddRecipes()
        {
            int evilBars = 8;
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 4)
                .AddIngredient(ItemID.CrimtaneBar, evilBars)
                .AddTile(TileID.MythrilAnvil)
                .Register();
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 4)
                .AddIngredient(ItemID.DemoniteBar, evilBars)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("6% increased melee damage" +
                "\n2% increased melee critical strike chance");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 12;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.defense = 14;
            Item.rare = ItemRarityID.Pink;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) *= 1.06f;
            player.GetCritChance(DamageClass.Melee) += 2;
        }
    }

    public class HexedMeleeSpeed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hexed Melee Speed");
            Description.SetDefault("Increased melee speed");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.allUseSpeed *= 1.25f;
        }
    }

    public class HexedArms : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Equipable/Armor/HexDefenderArmLong";

        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.timeLeft = 5;
            Projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //26 up from bottom for long
            //8 up from bottom for short

            if (rightTime)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("TerrorbornMod/Items/Equipable/Armor/HexDefenderArmLong").Value, Projectile.Center - Main.screenPosition, null, Color.White * 1f, longHandRotation, new Vector2(7, 50), 1.2f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("TerrorbornMod/Items/Equipable/Armor/HexDefenderArmShort").Value, Projectile.Center - Main.screenPosition, null, Color.White * 1f, shortHandRotation, new Vector2(7, 34), 1.2f, SpriteEffects.None, 0f);
                return false;
            }
            else
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("TerrorbornMod/Items/Equipable/Armor/HexDefenderArmLong").Value, Projectile.Center - Main.screenPosition, null, Color.White * 0.25f, longHandRotation, new Vector2(7, 50), 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("TerrorbornMod/Items/Equipable/Armor/HexDefenderArmShort").Value, Projectile.Center - Main.screenPosition, null, Color.White * 0.25f, shortHandRotation, new Vector2(7, 34), 1f, SpriteEffects.None, 0f);
                return false;
            }
        }

        float shortHandRotation = 0f;
        float longHandRotation = 0f;
        bool rightTime = false;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            Projectile.timeLeft = 5;

            if (!modPlayer.HexDefender)
            {
                Projectile.active = false;
            }

            float timeMult = 10f;
            shortHandRotation += MathHelper.ToRadians(1f / timeMult);
            longHandRotation += MathHelper.ToRadians(12f / timeMult);

            longHandRotation = MathHelper.WrapAngle(longHandRotation);
            shortHandRotation = MathHelper.WrapAngle(shortHandRotation);

            rightTime = false;
            if (Math.Abs(longHandRotation - shortHandRotation) <= MathHelper.ToRadians(12f))
            {
                rightTime = true;
            }

            if (rightTime && TerrorbornMod.ArmorAbility.JustPressed)
            {
                player.AddBuff(ModContent.BuffType<HexedMeleeSpeed>(), 5 * 60);
                TerrorbornSystem.ScreenShake(5f);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item67, player.Center);
            }

            Projectile.position = Vector2.Lerp(Projectile.position, player.Center + new Vector2(0, -100), 0.2f);
        }
    }
}