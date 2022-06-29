using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class DeimosteelHelmet : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 8)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();

        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases shriek of horror's use speed");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ShriekSpeed *= 0.65f;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 5;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<DeimosteelChainMail>() && legs.type == ModContent.ItemType<DeimosteelGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Drains terror over time to increase your defense by 8 and item use speed by 10%" +
                "\nIf you have no terror left, your shriek of horror use speed will be increased instead";
            float drainSpeed = 0.65f;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.TerrorPercent >= drainSpeed / 60)
            {
                modPlayer.LoseTerror(drainSpeed, true, true);
                player.GetAttackSpeed(DamageClass.Generic) *= 1.10f;
                player.statDefense += 8;
            }
            else
            {
                modPlayer.ShriekSpeed *= 0.5f;
            }
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class DeimosteelChainMail : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases critical strike chance by 6%");
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 6;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 10)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 6;
        }

        //public override bool DrawBody()
        //{
        //    return false;
        //}
    }
    [AutoloadEquip(EquipType.Legs)]
    public class DeimosteelGreaves : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 7)
                .AddTile(ModContent.TileType<Tiles.MeldingStation>())
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases all item use speeds by 4%");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetAttackSpeed(DamageClass.Generic) *= 1.04f;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 0, 50, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 5;
        }
    }
}
