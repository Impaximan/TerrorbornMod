using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace TerrorbornMod.NPCs
{
    class TerrorSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BlueSlime);
            NPC.width = 32;
            NPC.height = 26;
            NPC.damage = 13;
            NPC.defense = 2;
            NPC.lifeMax = 35;
            NPC.value *= 2;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            NPC.color = Color.White;
            AnimationType = NPCID.BlueSlime;
            if (Main.hardMode)
            {
                NPC.defense = 5;
                NPC.lifeMax = 160;
                NPC.damage = 34;
            }
            if (NPC.downedPlantBoss)
            {
                NPC.defense = 8;
                NPC.lifeMax = 190;
                NPC.damage = 38;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement("A pot possessed by the living tar within the desert, which uses javelins made with other pot scraps.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.ShriekOfHorrorUnlockedCondition(), ModContent.ItemType<Items.DarkEnergy>()));
            npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.ShriekOfHorrorUnlockedCondition(), ModContent.ItemType<Items.Materials.TerrorSample>(), 3, chanceNumerator: 1));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!TerrorbornSystem.obtainedShriekOfHorror)
            {
                return 0f;
            }
            if (TerrorbornPlayer.modPlayer(spawnInfo.player).ZoneIncendiary)
            {
                return 0f;
            }
            if (NPC.downedMoonlord)
            {
                return 0f;
            }
            return SpawnCondition.OverworldDaySlime.Chance * 0.5f;
        }
    }
}

