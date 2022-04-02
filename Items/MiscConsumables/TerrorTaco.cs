using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.MiscConsumables
{
    public class TerrorTaco : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Causes you to generate terror over time" +
                "\nWhile this is active, being at 100% terror will cause you to die" +
                "\nYour movement speed will be greatly decreased while over 50% terror");
        }
        public override void SetDefaults()
        {
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.EatingUsing;
            Item.maxStack = 30;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item2;
            Item.useTurn = true;
            Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<HorrificallyStuffed>();
            Item.buffTime = 3600 * 3;
        }
    }

    class HorrificallyStuffed : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Horrifically Stuffed");
            Description.SetDefault("You generate terror over time... but be sure not to get too full");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.GainTerror(1.5f, true, true);
            if (modPlayer.TerrorPercent >= 100)
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " was so full they blew up (literally)"), 10000, 0);
                modPlayer.TerrorPercent = 0f;
            }
            if (modPlayer.TerrorPercent >= 50)
            {
                float maxSpeed = 4.5f;
                if (player.velocity.X > maxSpeed)
                {
                    player.velocity.X = maxSpeed;
                }
                if (player.velocity.X < -maxSpeed)
                {
                    player.velocity.X = -maxSpeed;
                }
            }
        }
    }
}

