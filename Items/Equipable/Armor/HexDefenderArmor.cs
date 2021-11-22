using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
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
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 4);
            recipe.AddIngredient(ItemID.CrimtaneBar, evilBars);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 4);
            recipe2.AddIngredient(ItemID.DemoniteBar, evilBars);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("5% increased melee damage" +
                "\n4% increased melee critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.defense = 18;
            item.rare = 5;
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
                Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<HexedArms>(), 0, 0, player.whoAmI);
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.05f;
            player.meleeCrit += 4;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class HexDefenderBreastplate : ModItem
    {
        public override void AddRecipes()
        {
            int evilBars = 12;
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 6);
            recipe.AddIngredient(ItemID.CrimtaneBar, evilBars);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 6);
            recipe2.AddIngredient(ItemID.DemoniteBar, evilBars);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("7% increased melee damage" +
                "\n5% increased melee speed");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 20;
            item.defense = 24;
            item.rare = 5;
            item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override bool DrawBody()
        {
            return false;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.07f;
            player.meleeSpeed += 0.05f;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class HexDefenderGreaves : ModItem
    {
        public override void AddRecipes()
        {
            int evilBars = 8;
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 4);
            recipe.AddIngredient(ItemID.CrimtaneBar, evilBars);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Materials.HexingEssence>(), 4);
            recipe2.AddIngredient(ItemID.DemoniteBar, evilBars);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("6% increased melee damage" +
                "\n2% increased melee critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 12;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.defense = 14;
            item.rare = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.06f;
            player.meleeCrit += 2;
        }
    }

    public class HexedMeleeSpeed : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Hexed Melee Speed");
            Description.SetDefault("Increased melee speed");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
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
            projectile.width = 1;
            projectile.height = 1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.timeLeft = 5;
            projectile.tileCollide = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //26 up from bottom for long
            //8 up from bottom for short

            if (rightTime)
            {
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/Items/Equipable/Armor/HexDefenderArmLong"), projectile.Center - Main.screenPosition, null, Color.White * 1f, longHandRotation, new Vector2(7, 50), 1.2f, SpriteEffects.None, 0f);
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/Items/Equipable/Armor/HexDefenderArmShort"), projectile.Center - Main.screenPosition, null, Color.White * 1f, shortHandRotation, new Vector2(7, 34), 1.2f, SpriteEffects.None, 0f);
                return false;
            }
            else
            {
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/Items/Equipable/Armor/HexDefenderArmLong"), projectile.Center - Main.screenPosition, null, Color.White * 0.25f, longHandRotation, new Vector2(7, 50), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/Items/Equipable/Armor/HexDefenderArmShort"), projectile.Center - Main.screenPosition, null, Color.White * 0.25f, shortHandRotation, new Vector2(7, 34), 1f, SpriteEffects.None, 0f);
                return false;
            }
        }

        float shortHandRotation = 0f;
        float longHandRotation = 0f;
        bool rightTime = false;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            projectile.timeLeft = 5;

            if (!modPlayer.HexDefender)
            {
                projectile.active = false;
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
                TerrorbornMod.ScreenShake(5f);
                Main.PlaySound(SoundID.Item67, player.Center);
            }

            projectile.position = Vector2.Lerp(projectile.position, player.Center + new Vector2(0, -100), 0.2f);
        }
    }
}