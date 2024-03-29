﻿using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{

    [AutoloadEquip(EquipType.Head)]
    public class BountyHunterCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("4% increased ranged damage" +
                "\n4% increased ranged critical strike chance");
            //ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = false;
            //ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.defense = 7;
            Item.rare = ItemRarityID.Green;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<BountyHunterLeatherwear>() && legs.type == ModContent.ItemType<BountyHunterPants>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Pressing the <ArmorAbility> hotkey gives you the 'bounty hunter's mark' buff for\n" +
                              "5 seconds. This increases your ranged critical strike chance by 25%, ranged firing speed by" +
                              "\n25% and makes all ranged Projectiles home, but at the cost of 20 defense" +
                              "\n50 second cooldown";
            if (!player.HasBuff(ModContent.BuffType<HuntersRecharge>()) && TerrorbornMod.ArmorAbility.JustPressed)
            {
                player.AddBuff(ModContent.BuffType<HuntersRecharge>(), 60 * 50);
                player.AddBuff(ModContent.BuffType<HuntersMark>(), 60 * 5);
                SoundExtensions.PlaySoundOld(SoundID.Item62, player.Center);

                DustExplosion(player.Center, 0, 80, 25, DustID.AmberBolt, NoGravity: true);
            }
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

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) *= 1.04f;
            player.GetCritChance(DamageClass.Ranged) += 3;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class BountyHunterLeatherwear : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("4% increased ranged damage" +
                "\n3% increased ranged critical strike chance");
            ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesBottomSkin[Item.bodySlot] = true;
            ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 20;
            Item.defense = 7;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) *= 1.04f;
            player.GetCritChance(DamageClass.Ranged) += 3;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class BountyHunterPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("4% increased ranged damage" +
                "\n2% increased ranged critical strike chance");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 12;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.defense = 5;
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) *= 1.04f;
            player.GetCritChance(DamageClass.Ranged) += 3;
        }
    }

    class HuntersRecharge : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hunter's recharge");
            Description.SetDefault("You cannot use 'bounty hunter's mark' again");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
    }

    public class HuntersMark : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bounty Hunter's Mark");
            Description.SetDefault("Increased ranged stats and accuracy... hunt your foes down!");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= 20;
            player.GetCritChance(DamageClass.Ranged) += 25;
            TerrorbornPlayer mPlayer = TerrorbornPlayer.modPlayer(player);
            player.GetAttackSpeed(DamageClass.Ranged) *= 1.25f;
        }
    }
}
