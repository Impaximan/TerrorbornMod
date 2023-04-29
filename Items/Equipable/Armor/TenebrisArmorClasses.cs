using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class TenebrisMask : ModItem
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
            // DisplayName.SetDefault("Tenebris Helmet");
            /* Tooltip.SetDefault("6% increased melee damage" +
                "\n16% increased melee critical strike chance" +
                "\n+15 max life"); */
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TenebrisChestplate>() && legs.type == ModContent.ItemType<TenebrisLeggings>();
        }

        int blackoutTime = 0;
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Press the <ArmorAbility> mod hotkey during a voidslip to teleport to the cursor" +
                "\nThis teleport has no cost";

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.VoidBlinkTime > 0 && TerrorbornMod.ArmorAbility.JustPressed)
            {
                if (!Collision.SolidCollision(Main.MouseWorld - player.Size / 2, player.width, player.height))
                {
                    blackoutTime = 7;
                    player.position = Main.MouseWorld - player.Size / 2;
                    ModContent.Request<SoundEffect>("TerrorbornMod/Sounds/Effects/undertalewarning").Value.Play(Main.soundVolume, 0.5f, 0f);
                }
            }

            if (blackoutTime > 0)
            {
                blackoutTime--;
                TerrorbornMod.ScreenDarknessAlpha = 1f;
                if (blackoutTime <= 0)
                {
                    TerrorbornMod.ScreenDarknessAlpha = 0f;
                }
            }
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetDamage(DamageClass.Melee) *= 1.06f;
            player.GetCritChance(DamageClass.Melee) += 16;
            player.statLifeMax2 += 15;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class TenebrisChestplate : ModItem
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
            /* Tooltip.SetDefault("6% increased melee damage" +
                "\n35% terror from Shriek of Horror" +
                "\n+20 max life"); */
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetDamage(DamageClass.Melee) *= 1.06f;
            modPlayer.ShriekTerrorMultiplier *= 1.35f;
            player.statLifeMax2 += 20;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class TenebrisLeggings : ModItem
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
            // DisplayName.SetDefault("Tenebris Greaves");
            /* Tooltip.SetDefault("6% increased melee damage" +
                "\nIncreased movement speed" +
                "\n+15 max life"); */
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 12;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetDamage(DamageClass.Melee) *= 1.06f;
            player.runAcceleration *= 1.3f;
            player.statLifeMax2 += 15;
        }
    }
}


