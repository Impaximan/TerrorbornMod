using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;

namespace TerrorbornMod.NPCs
{
    class DreadSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement("With dreadful essence filling the world, terror slimes seem to have grown in size and power. In classic slime nature, they're still not much of a threat.")
            });
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BlueSlime);
            NPC.width = 64;
            NPC.height = 52;
            NPC.damage = 75;
            NPC.defense = 22;
            NPC.lifeMax = 3500;
            NPC.value *= 50;
            NPC.knockBackResist = 0f;
            NPC.lavaImmune = true;
            NPC.color = Color.White;
            AnimationType = NPCID.BlueSlime;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, minimumDropped: 7, maximumDropped: 12));
            npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.ShriekOfHorrorUnlockedCondition(), ModContent.ItemType<Items.DarkEnergy>()));
            npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.ShriekOfHorrorUnlockedCondition(), ModContent.ItemType<Items.Materials.TerrorSample>(), 3, chanceNumerator: 2));
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
            if (!NPC.downedMoonlord)
            {
                return 0f;
            }
            return SpawnCondition.OverworldDaySlime.Chance * 0.5f;
        }
    }
}