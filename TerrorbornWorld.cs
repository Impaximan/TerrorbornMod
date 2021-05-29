using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using StructureHelper;

namespace TerrorbornMod
{
    public class TerrorbornWorld : ModWorld
    {
        public static bool downedNightcrawler;
        public static bool downedPrototypeI;
        public static bool downedTidalTitan;
        public static bool downedDunestock;
        public static bool downedSangrune;
        public static bool downedSangrune2;
        public static bool downedUndyingSpirit;
        public static bool obtainedShriekOfHorror;
        public static int ShadowTiles = 0;
        public static int CurrentBountyBiome = 69; //You can't stop me from keeping it like this
        public static bool UnaliveInvasionUp;
        public static string SkeletonSheriffName;
        public static int TerrorMasterDialogue;

        public static Vector2 ShriekOfHorror;
        public static Vector2 HorrificAdaptation;
        public static Vector2 VoidBlink;
        public static Vector2 TerrorWarp;

        int BountyBiomeCount = 7;

        bool WasNight = false;

        public override void Initialize()
        {
            downedNightcrawler = false;
            downedPrototypeI = false;
            downedTidalTitan = false;
            downedDunestock = false;
            downedSangrune = false;
            downedUndyingSpirit = false;
            obtainedShriekOfHorror = false;
            TerrorMasterDialogue = 0;
            SkeletonSheriffName = getSkeletonSheriffName();
            VoidBlink = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.95f);
            TerrorWarp = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.66f);
        }

        public string getSkeletonSheriffName()
        {
            switch (WorldGen.genRand.Next(7))
            {
                case 0:
                    return "Femurandus";
                case 1:
                    return "Cartage";
                case 2:
                    return "Pelven";
                case 3:
                    return "Chronius";
                case 4:
                    return "Scapulan";
                case 5:
                    return "Chad";
                case 6:
                    return "Raster";
                default:
                    return "Liven't";
            }
        }

        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (downedNightcrawler) downed.Add("Shadowcrawler");
            if (downedPrototypeI) downed.Add("PrototypeI");
            if (downedTidalTitan) downed.Add("TidalTitan");
            if (downedDunestock) downed.Add("Dunestock");
            if (downedSangrune) downed.Add("Sangrune");
            if (downedSangrune2) downed.Add("Sangrune2");
            if (downedUndyingSpirit) downed.Add("UndyingSpirit");
            if (obtainedShriekOfHorror) downed.Add("ShriekOfHorror");

            return new TagCompound {
                {"downed", downed},
                {"CurrentBountyBiome", CurrentBountyBiome},
                {"SkeletonSheriffName", SkeletonSheriffName},
                {"TerrorMasterDialogue", TerrorMasterDialogue},
                {"VoidBlink", VoidBlink},
                {"TerrorWarp", TerrorWarp}
            };
            
        }
        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            downedNightcrawler = downed.Contains("Shadowcrawler");
            downedPrototypeI = downed.Contains("PrototypeI");
            downedTidalTitan = downed.Contains("TidalTitan");
            downedDunestock = downed.Contains("Dunestock");
            downedSangrune = downed.Contains("Sangrune");
            downedSangrune2 = downed.Contains("Sangrune2");
            downedUndyingSpirit = downed.Contains("UndyingSpirit");
            obtainedShriekOfHorror = downed.Contains("ShriekOfHorror");
            CurrentBountyBiome = tag.GetInt("CurrentBountyBiome");
            SkeletonSheriffName = tag.GetString("SkeletonSheriffName");
            TerrorMasterDialogue = tag.GetInt("TerrorMasterDialogue");
            VoidBlink = tag.Get<Vector2>("VoidBlink");
            TerrorWarp = tag.Get<Vector2>("TerrorWarp");
        }
        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();
            if (loadVersion == 0)
            {
                BitsByte flags = reader.ReadByte();
                downedNightcrawler = flags[0];
            }
            else
            {
                ErrorLogger.Log("Terrorborn Mod: Unknown loadVersion: " + loadVersion);
            }
        }
        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedNightcrawler;
            flags[1] = downedPrototypeI;
            flags[2] = downedTidalTitan;
            flags[3] = downedDunestock;
            flags[4] = downedSangrune;
            flags[5] = obtainedShriekOfHorror;
            writer.Write(flags);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedNightcrawler = flags[0];
            downedPrototypeI = flags[1];
            downedTidalTitan = flags[2];
            downedDunestock = flags[3];
            downedSangrune = flags[4];
            obtainedShriekOfHorror = flags[5];
        }
        //public override void Kill()
        //{
        //    OreComet();
        //}
        public void CreateLineOfBlocksHorizontal(int LineX, int LineY, int Length, int Type)
        {
            for (int i = 0; i < Length; i++)
            {
                WorldGen.PlaceTile(LineX + i, LineY, Type);
            }
        }
        public void CreateLineOfBlocksVerticle(int LineX, int LineY, int Length, int Type)
        {
            for (int i = 0; i < Length; i++)
            {
                WorldGen.PlaceTile(LineX, LineY + i, Type);
            }
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            tasks.Insert(genIndex + 1, new PassLegacy("Terror Shrines", delegate (GenerationProgress progress)
            {
                GenerateShrineStructures();
                progress.Message = "Terror Shrines";
            }));

        }

        public override void PostWorldGen()
        {
            for (int i = 0; i < 1000; i++)
            {
                Chest chest = Main.chest[i];
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 11 * 36)
                {
                    if (Main.rand.Next(101) <= 25)
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Summons.Minions.FrigidStaff>());
                                break;
                            }
                        }
                    }
                }
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 2 * 36)
                {
                    if (Main.rand.Next(101) <= 20)
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Ranged.DualpipeDartgun>());
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void GenerateShrineStructures()
        {
            Vector2 ShriekOfHorrorPosition = new Vector2(Main.spawnTileX, Main.spawnTileY);
            float sizeMultiplier = 1;
            if (Main.maxTilesX == 6400)
            {
                sizeMultiplier = 1.5f;
            }
            if (Main.maxTilesX == 8400)
            {
                sizeMultiplier = 2f;
            }
            ShriekOfHorrorPosition.Y += 285 * sizeMultiplier;
            ShriekOfHorrorPosition.X -= 15;

            StructureHelper.Generator.GenerateStructure("Structures/ShriekOfHorrorShrine", new Point16((int)ShriekOfHorrorPosition.X, (int)ShriekOfHorrorPosition.Y), mod);

            int DungeonDirection = 1;
            if (Main.dungeonX < Main.spawnTileX)
            {
                DungeonDirection = -1;
            }
            Vector2 HorrificAdaptationPosition = new Vector2(Main.spawnTileX + (Main.maxTilesX / 4) * -DungeonDirection, Main.maxTilesY / 2);
            StructureHelper.Generator.GenerateStructure("Structures/HorrificAdaptationShrine", new Point16((int)HorrificAdaptationPosition.X, (int)HorrificAdaptationPosition.Y), mod);

            VoidBlink = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.95f);
            StructureHelper.Generator.GenerateStructure("Structures/VoidBlinkShrine", new Point16((int)VoidBlink.X, (int)VoidBlink.Y), mod);

            TerrorWarp = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.66f);
            StructureHelper.Generator.GenerateStructure("Structures/TerrorWarpShrine", new Point16((int)TerrorWarp.X, (int)TerrorWarp.Y), mod);
        }

        public void SelectBountyBiome()
        {
            CurrentBountyBiome = Main.rand.Next(BountyBiomeCount);
            //0 = Underground desert
            //1 = Underground snow
            //2 = Underworld
            //3 = Jungle
            //4 = Underground Jungle;
            //5 = World Evil
            //6 = Snow

            //HARDMODE ONLY:
            //7 = Hallowed
            //8 = Underground Hallowed
            //9 = Underground Evil
        }

        public void NewDayActions()
        {
            SelectBountyBiome();
        }

        void SetTerrorAbilityPositions()
        {
            ShriekOfHorror = new Vector2(Main.spawnTileX, Main.spawnTileY);
            float sizeMultiplier = 1;
            if (Main.maxTilesX == 6400)
            {
                sizeMultiplier = 1.5f;
            }
            if (Main.maxTilesX == 8400)
            {
                sizeMultiplier = 2f;
            }
            ShriekOfHorror.Y += 285 * sizeMultiplier;
            ShriekOfHorror.X -= 15;
            ShriekOfHorror.X += 35;
            ShriekOfHorror.Y += 11.5f;
            ShriekOfHorror *= 16;

            int DungeonDirection = 1;
            if (Main.dungeonX < Main.spawnTileX)
            {
                DungeonDirection = -1;
            }
            HorrificAdaptation = new Vector2(Main.spawnTileX + (Main.maxTilesX / 4) * -DungeonDirection, Main.maxTilesY / 2);
            HorrificAdaptation *= 16;
        }

        public override void PostUpdate()
        {
            //Vector2 textVector = VoidBlink * 16 - Main.player[Main.myPlayer].position;
            //Main.NewText(textVector.X + ", " + textVector.Y);
            Player player = Main.player[Main.myPlayer];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            SetTerrorAbilityPositions();

            bool SpawnShriek = !obtainedShriekOfHorror;
            bool SpawnHA = !modPlayer.unlockedAbilities.Contains(1);
            bool SpawnNC = !modPlayer.unlockedAbilities.Contains(4);
            bool SpawnVB = !modPlayer.unlockedAbilities.Contains(3);
            bool SpawnTW = !modPlayer.unlockedAbilities.Contains(2);
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<Abilities.ShriekOfHorror>())
                    {
                        SpawnShriek = false;
                    }
                    if (Main.projectile[i].type == ModContent.ProjectileType<Abilities.HorrificAdaptation>())
                    {
                        SpawnHA = false;
                    }
                    if (Main.projectile[i].type == ModContent.ProjectileType<Abilities.NecromanticCurse>())
                    {
                        SpawnNC = false;
                    }
                    if (Main.projectile[i].type == ModContent.ProjectileType<Abilities.VoidBlink>())
                    {
                        SpawnVB = false;
                    }
                    if (Main.projectile[i].type == ModContent.ProjectileType<Abilities.TerrorWarp>())
                    {
                        SpawnTW = false;
                    }
                }
            }
            if (!obtainedShriekOfHorror)
            {
                SpawnHA = false;
                SpawnNC = false;
                SpawnVB = false;
                SpawnTW = false;
            }
            if (SpawnShriek)
            {
                Projectile.NewProjectile(ShriekOfHorror, Vector2.Zero, ModContent.ProjectileType<Abilities.ShriekOfHorror>(), 0, 0);
            }
            if (SpawnHA)
            {
                Projectile.NewProjectile(HorrificAdaptation, Vector2.Zero, ModContent.ProjectileType<Abilities.HorrificAdaptation>(), 0, 0);
            }
            if (SpawnNC)
            {
                Projectile.NewProjectile(HorrificAdaptation, Vector2.Zero, ModContent.ProjectileType<Abilities.NecromanticCurse>(), 0, 0);
            }
            if (SpawnVB)
            {
                Projectile.NewProjectile(VoidBlink, Vector2.Zero, ModContent.ProjectileType<Abilities.VoidBlink>(), 0, 0);
            }
            if (SpawnTW)
            {
                Projectile.NewProjectile(TerrorWarp, Vector2.Zero, ModContent.ProjectileType<Abilities.TerrorWarp>(), 0, 0);
            }

            if (CurrentBountyBiome == 69)
            {
                SelectBountyBiome();
            }
            if (Main.hardMode)
            {
                BountyBiomeCount = 10;
            }
            if (Main.dayTime && WasNight)
            {
                WasNight = false;
                NewDayActions();
            }
            if (!Main.dayTime)
            {
                WasNight = true;
            }

            if (SkeletonSheriffName == "")
            {
                SkeletonSheriffName = getSkeletonSheriffName();
            }
        }

        public void GenerateAzuriteOres()
        {
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY);
                WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(4, 7), WorldGen.genRand.Next(3, 6), mod.TileType("Azurite"), false, 0f, 0f, false, true);
            }
        }
    }
}
