using System.IO;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Terraria.Utilities;
using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod
{
    public class TerrorbornWorld : ModWorld
    {
        public static bool downedShadowcrawler;
        public static bool downedPrototypeI;
        public static bool downedTidalTitan;
        public static bool downedDunestock;
        public static bool downedUndyingSpirit;
        public static bool downedIncendiaryBoss;
        public static bool obtainedShriekOfHorror;
        public static bool downedTerrorRain;
        public static bool downedFrightcrawler;
        public static bool downedSlateBanshee;
        public static bool terrorRain;
        public static bool talkedToCartographer;
        public static bool talkedToHeretic;
        public static int timeSinceFrightcrawlerSpawn = 0;
        public static int ShadowTiles = 0;
        public static int CurrentBountyBiome = 69; //You can't stop me from keeping it like this
        public static bool UnaliveInvasionUp;
        public static string SkeletonSheriffName;
        public static string CartographerName;
        public static int TerrorMasterDialogue;
        public static int deimostoneTiles;
        public static int incendiaryTiles;
        public static bool incendiaryRitual;
        public static int wormExtraSegmentCount = 0;
        public static int CartographerSpawnCooldown = 0;
        public static int incendiaryIslandsSide = 0;

        public static Vector2 ShriekOfHorror;
        public static Vector2 HorrificAdaptation;
        public static Vector2 VoidBlink;
        public static Vector2 TerrorWarp;

        int BountyBiomeCount = 7;

        bool WasNight = false;

        public override void Initialize()
        {
            CartographerSpawnCooldown = 3600 * 6;
            downedShadowcrawler = false;
            downedPrototypeI = false;
            downedTidalTitan = false;
            downedDunestock = false;
            timeSinceFrightcrawlerSpawn = 0;
            terrorRain = false;
            downedTerrorRain = false;
            downedFrightcrawler = false;
            downedUndyingSpirit = false;
            obtainedShriekOfHorror = false;
            talkedToCartographer = false;
            downedSlateBanshee = false;
            talkedToHeretic = false;
            TerrorMasterDialogue = 0;
            SkeletonSheriffName = getSkeletonSheriffName();
            CartographerName = getCartographerName();
            VoidBlink = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.95f);
            TerrorWarp = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.66f);
        }

        public override void PreUpdate()
        {
            wormExtraSegmentCount = 0;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active)
                {
                    TerrorbornNPC globalNPC = TerrorbornNPC.modNPC(npc);
                    if (globalNPC.extraWormSegment)
                    {
                        wormExtraSegmentCount++;
                    }
                }
            }
        }

        public override void ResetNearbyTileEffects()
        {
            TerrorbornPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<TerrorbornPlayer>();
            deimostoneTiles = 0;
            incendiaryTiles = 0;
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            deimostoneTiles = tileCounts[ModContent.TileType<Tiles.Deimostone>()];

            incendiaryTiles = tileCounts[ModContent.TileType<Tiles.Incendiary.KindlingSoil>()];
            incendiaryTiles += tileCounts[ModContent.TileType<Tiles.Incendiary.KindlingGrass>()];
            incendiaryTiles += tileCounts[ModContent.TileType<Tiles.Incendiary.IncendiaryMachinery>()];

            incendiaryRitual = tileCounts[ModContent.TileType<Tiles.Incendiary.IncendiaryRitual>()] > 0;
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

        public string getCartographerName()
        {
            switch (WorldGen.genRand.Next(7))
            {
                case 0:
                    return "Lupo";
                case 1:
                    return "Albert";
                case 2:
                    return "Cata";
                case 3:
                    return "Cornifer";
                case 4:
                    return "Abraham";
                case 5:
                    return "Gerardus";
                case 6:
                    return "Arthur";
                default:
                    return "David";
            }
        }

        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (downedShadowcrawler) downed.Add("Shadowcrawler");
            if (downedPrototypeI) downed.Add("PrototypeI");
            if (downedTidalTitan) downed.Add("TidalTitan");
            if (downedDunestock) downed.Add("Dunestock");
            if (downedUndyingSpirit) downed.Add("UndyingSpirit");
            if (obtainedShriekOfHorror) downed.Add("ShriekOfHorror");
            if (downedTerrorRain) downed.Add("downedTerrorRain");
            if (downedFrightcrawler) downed.Add("downedFrightcrawler");
            if (downedIncendiaryBoss) downed.Add("downedIncendiaryBoss");
            if (downedSlateBanshee) downed.Add("downedSlateBanshee");

            return new TagCompound {
                {"downed", downed},
                {"CurrentBountyBiome", CurrentBountyBiome},
                {"SkeletonSheriffName", SkeletonSheriffName},
                {"CartographerName", CartographerName},
                {"TerrorMasterDialogue", TerrorMasterDialogue},
                {"VoidBlink", VoidBlink},
                {"TerrorWarp", TerrorWarp},
                {"terrorRain", terrorRain},
                {"talkedToCartographer", talkedToCartographer},
                {"talkedToHeretic", talkedToHeretic},
                {"timeSinceFrightcrawlerSpawn", timeSinceFrightcrawlerSpawn},
                {"CartographerSpawnCooldown", CartographerSpawnCooldown},
                {"incendiaryIslandsSide", incendiaryIslandsSide}
            };
            
        }
        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            downedShadowcrawler = downed.Contains("Shadowcrawler");
            downedPrototypeI = downed.Contains("PrototypeI");
            downedTidalTitan = downed.Contains("TidalTitan");
            downedDunestock = downed.Contains("Dunestock");
            downedUndyingSpirit = downed.Contains("UndyingSpirit");
            downedIncendiaryBoss = downed.Contains("downedIncendiaryBoss");
            obtainedShriekOfHorror = downed.Contains("ShriekOfHorror");
            downedTerrorRain = downed.Contains("downedTerrorRain");
            downedFrightcrawler = downed.Contains("downedFrightcrawler");
            downedSlateBanshee = downed.Contains("downedSlateBanshee");
            CurrentBountyBiome = tag.GetInt("CurrentBountyBiome");
            SkeletonSheriffName = tag.GetString("SkeletonSheriffName");
            CartographerName = tag.GetString("CartographerName");
            TerrorMasterDialogue = tag.GetInt("TerrorMasterDialogue");
            VoidBlink = tag.Get<Vector2>("VoidBlink");
            TerrorWarp = tag.Get<Vector2>("TerrorWarp");
            terrorRain = tag.GetBool("terrorRain");
            talkedToCartographer = tag.GetBool("talkedToCartographer");
            talkedToHeretic = tag.GetBool("talkedToHeretic");
            timeSinceFrightcrawlerSpawn = tag.GetInt("timeSinceFrightcrawlerSpawn");
            CartographerSpawnCooldown = tag.GetInt("CartographerSpawnCooldown");
            incendiaryIslandsSide = tag.GetInt("incendiaryIslandsSide");
        }

        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();
            if (loadVersion == 0)
            {
                BitsByte flags = reader.ReadByte();
                downedShadowcrawler = flags[0];
            }
            else
            {
                ErrorLogger.Log("Terrorborn Mod: Unknown loadVersion: " + loadVersion);
            }
        }

        public static void CreateLineOfBlocksHorizontal(int LineX, int LineY, int Length, int Type, bool Right = true, bool forced = false, bool withWall = false, int wallType = WallID.Dirt)
        {
            for (int i = 0; i < Length; i++)
            {
                if (Right)
                {
                    WorldGen.PlaceTile(LineX + i, LineY, Type, true, forced);
                    if (withWall)
                    {
                        WorldGen.PlaceWall(LineX + i, LineY, wallType, true);
                    }
                }
                else
                {
                    WorldGen.PlaceTile(LineX - i, LineY, Type, true, forced);
                    if (withWall)
                    {
                        WorldGen.PlaceWall(LineX - i, LineY, wallType, true);
                    }
                }
            }
        }

        public static void CreateLineOfBlocksVertical(int LineX, int LineY, int Length, int Type, bool Up = true, bool forced = false, bool withWall = false, int wallType = WallID.Dirt)
        {
            for (int i = 0; i < Length; i++)
            {
                if (Up)
                {
                    WorldGen.PlaceTile(LineX, LineY - i, Type, true, forced);
                    if (withWall)
                    {
                        WorldGen.PlaceWall(LineX, LineY - i, wallType, true);
                    }
                }
                else
                {
                    WorldGen.PlaceTile(LineX, LineY + i, Type, true, forced);
                    if (withWall)
                    {
                        WorldGen.PlaceWall(LineX, LineY + i, wallType, true);
                    }
                }
            }
        }

        public static void CreateLineOfWallsVertical(int LineX, int LineY, int Length, int Type, bool Up = true)
        {
            for (int i = 0; i < Length; i++)
            {
                if (Up)
                {
                    WorldGen.PlaceWall(LineX, LineY - i, Type, true);
                }
                else
                {
                    WorldGen.PlaceWall(LineX, LineY + i, Type, true);
                }
            }
        }

        #region IncendiaryIslands
        public static void GenerateIncendiaryChest(int x, int y)
        {
            WorldGen.KillTile(x, y, noItem: true);
            WorldGen.KillTile(x + 1, y, noItem: true);
            WorldGen.KillTile(x + 1, y + 1, noItem: true);
            WorldGen.KillTile(x, y + 1, noItem: true);
            WorldGen.KillTile(x, y + 2, noItem: true);
            WorldGen.KillTile(x + 1, y + 2, noItem: true);
            WorldGen.PlaceTile(x, y + 2, ModContent.TileType<Tiles.Incendiary.IncendiaryBrick>());
            WorldGen.PlaceTile(x + 1, y + 2, ModContent.TileType<Tiles.Incendiary.IncendiaryBrick>());
            WorldGen.PlaceTile(x, y + 1, (ushort)ModContent.TileType<Tiles.Incendiary.IncendiaryChest>());
            Chest chest = Main.chest[Chest.FindChest(x, y)];
            
            List<int> mainItems = new List<int>();
            mainItems.Add(ModContent.ItemType<Items.Equipable.Accessories.Shields.IncendiaryShield>());
            mainItems.Add(ModContent.ItemType<Items.Equipable.Accessories.SpecterLocket>());
            mainItems.Add(ModContent.ItemType<Items.Weapons.Magic.Asphodel>());
            mainItems.Add(ModContent.ItemType<Items.Equipable.Hooks.HellishHook>());

            List<int> bossSummons = new List<int>();
            bossSummons.Add(ItemID.MechanicalEye);
            bossSummons.Add(ItemID.MechanicalSkull);
            bossSummons.Add(ItemID.MechanicalWorm);

            List<int> souls = new List<int>();
            souls.Add(ItemID.SoulofLight);
            souls.Add(ItemID.SoulofNight);
            souls.Add(ItemID.SoulofFlight);

            List<int> ammosAndThrowables = new List<int>();
            ammosAndThrowables.Add(ItemID.IchorArrow);
            ammosAndThrowables.Add(ItemID.CursedArrow);
            ammosAndThrowables.Add(ItemID.IchorBullet);
            ammosAndThrowables.Add(ItemID.CursedBullet);

            List<int> bars = new List<int>();
            bars.Add(ItemID.CobaltBar);
            bars.Add(ItemID.PalladiumBar);
            bars.Add(ItemID.MythrilBar);
            bars.Add(ItemID.OrichalcumBar);
            bars.Add(ItemID.AdamantiteBar);
            bars.Add(ItemID.TitaniumBar);

            List<int> commonPotions = new List<int>();
            commonPotions.Add(ItemID.SpelunkerPotion);
            commonPotions.Add(ModContent.ItemType<Items.Potions.AerodynamicPotion>());
            commonPotions.Add(ItemID.AmmoReservationPotion);
            commonPotions.Add(ItemID.ObsidianSkinPotion);
            commonPotions.Add(ItemID.ArcheryPotion);
            commonPotions.Add(ItemID.MagicPowerPotion);

            List<int> uncommonPotions = new List<int>();
            commonPotions.Add(ModContent.ItemType<Items.Potions.DarkbloodPotion>());
            uncommonPotions.Add(ItemID.InfernoPotion);
            uncommonPotions.Add(ItemID.LifeforcePotion);
            uncommonPotions.Add(ItemID.WrathPotion);
            uncommonPotions.Add(ItemID.RagePotion);

            int item = 0;
            chest.item[item].SetDefaults(WorldGen.genRand.Next(mainItems));
            item++;

            chest.item[item].SetDefaults(WorldGen.genRand.Next(souls));
            chest.item[item].stack = Main.rand.Next(1, 2);
            item++;

            if (WorldGen.genRand.NextFloat() <= 0.25f)
            {
                chest.item[item].SetDefaults(WorldGen.genRand.Next(bossSummons));
                item++;
            }

            if (WorldGen.genRand.NextFloat() <= 0.5f)
            {
                chest.item[item].SetDefaults(WorldGen.genRand.Next(bars));
                chest.item[item].stack = WorldGen.genRand.Next(5, 15);
                item++;
            }

            if (WorldGen.genRand.NextFloat() <= 0.5f)
            {
                chest.item[item].SetDefaults(WorldGen.genRand.Next(ammosAndThrowables));
                chest.item[item].stack = WorldGen.genRand.Next(225, 565);
                item++;
            }

            if (WorldGen.genRand.NextFloat() <= 0.5f)
            {
                chest.item[item].SetDefaults(ItemID.GreaterHealingPotion);
                chest.item[item].stack = WorldGen.genRand.Next(6, 11);
                item++;
            }

            if (WorldGen.genRand.NextFloat() <= 0.666f)
            {
                chest.item[item].SetDefaults(WorldGen.genRand.Next(commonPotions));
                chest.item[item].stack = WorldGen.genRand.Next(2, 5);
                item++;
            }

            if (WorldGen.genRand.NextFloat() <= 0.333f)
            {
                chest.item[item].SetDefaults(WorldGen.genRand.Next(uncommonPotions));
                chest.item[item].stack = WorldGen.genRand.Next(2, 4);
                item++;
            }

            chest.item[item].SetDefaults(ItemID.GoldCoin);
            chest.item[item].stack = WorldGen.genRand.Next(12, 25);
            item++;
        }

        public static void GenerateIncendiaryShrine(int i, int j)
        {
            Vector2 currentPosition = new Vector2(i, j);
            while (!WorldGen.TileEmpty((int)currentPosition.X, (int)currentPosition.Y))
            {
                currentPosition.Y -= 1;
            }
            currentPosition += new Vector2(0, 2);
            CreateLineOfBlocksHorizontal((int)currentPosition.X - 4, (int)currentPosition.Y, 8, ModContent.TileType<Tiles.Incendiary.IncendiaryBrick>(), forced: true);
            CreateLineOfBlocksHorizontal((int)currentPosition.X - 5, (int)currentPosition.Y - 1, 10, ModContent.TileType<Tiles.Incendiary.IncendiaryBrick>(), forced: true);
            CreateLineOfBlocksHorizontal((int)currentPosition.X - 5, (int)currentPosition.Y - 10, 10, ModContent.TileType<Tiles.Incendiary.IncendiaryBrick>(), forced: true);
            CreateLineOfBlocksHorizontal((int)currentPosition.X - 4, (int)currentPosition.Y - 11, 8, ModContent.TileType<Tiles.Incendiary.IncendiaryBrick>(), forced: true);
            CreateLineOfBlocksHorizontal((int)currentPosition.X - 3, (int)currentPosition.Y - 12, 6, ModContent.TileType<Tiles.Incendiary.IncendiaryBrick>(), forced: true);
            CreateLineOfWallsVertical((int)currentPosition.X - 3, (int)currentPosition.Y - 1, 10, ModContent.WallType<Tiles.Incendiary.IncendiaryBrickWall>());
            CreateLineOfWallsVertical((int)currentPosition.X - 2, (int)currentPosition.Y - 1, 10, ModContent.WallType<Tiles.Incendiary.IncendiaryBrickWall>());
            CreateLineOfWallsVertical((int)currentPosition.X - 1, (int)currentPosition.Y - 1, 10, ModContent.WallType<Tiles.Incendiary.IncendiaryBrickWall>());
            CreateLineOfWallsVertical((int)currentPosition.X, (int)currentPosition.Y - 1, 10, ModContent.WallType<Tiles.Incendiary.IncendiaryBrickWall>());
            CreateLineOfWallsVertical((int)currentPosition.X + 1, (int)currentPosition.Y - 1, 10, ModContent.WallType<Tiles.Incendiary.IncendiaryBrickWall>());
            CreateLineOfWallsVertical((int)currentPosition.X + 2, (int)currentPosition.Y - 1, 10, ModContent.WallType<Tiles.Incendiary.IncendiaryBrickWall>());
            GenerateIncendiaryChest((int)currentPosition.X - 1, (int)currentPosition.Y - 3);
        }

        public static Rectangle GenerateIncendiaryIsland_Skullmound(int i, int j, float sizeMultiplier)
        {
            int islandWidth = (int)(16 * sizeMultiplier);
            int islandHeight = (int)(10 * sizeMultiplier);
            int pipeOffset = WorldGen.genRand.Next((int)(-2 * sizeMultiplier), (int)(2 * sizeMultiplier));
            bool forced = true;

            Rectangle rect = new Rectangle(i, j, islandWidth, islandHeight);
            foreach (Rectangle island in islands)
            {
                if (island != Rectangle.Empty)
                {
                    if (island.Intersects(rect))
                    {
                        return Rectangle.Empty;
                    }
                }
            }

            List<int> verticalPipesUp = new List<int>();

            List<int> verticalPipesDown = new List<int>();

            List<Point16> blockedTiles = new List<Point16>();

            Point16 center = new Point16(i + islandWidth / 2, j + islandHeight / 2);
            CreateLineOfBlocksHorizontal(center.X, center.Y, islandWidth / 2, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), forced: forced);
            CreateLineOfBlocksHorizontal(center.X, center.Y, islandWidth / 2, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), false, forced);

            int extraHeight = 0;
            int blocksFromEdge = 3;
            for (int x = blocksFromEdge; x < islandWidth - blocksFromEdge; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.025f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)(((islandHeight / 2) - (islandHeight / 2 * distanceFromCenter)) * 2f);

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.Skullmound>(), true, forced: true);
            }

            extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.05f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight / 2) - (islandHeight / 4 * distanceFromCenter));

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), forced: forced, withWall: true, wallType: WallID.LavaUnsafe3);
            }

            extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.1f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight / 2) - (islandHeight / 2 * distanceFromCenter)) / 2 + 1;

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), false, forced: forced, withWall: true, wallType: WallID.LavaUnsafe3);
            }


            extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.15f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight) - (islandHeight * distanceFromCenter));

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.PyroclasticCloud>(), false, forced: false);
            }

            int extraDistance = 10;

            for (int x = -extraDistance; x < islandWidth + extraDistance; x++)
            {
                for (int y = -extraDistance; y < islandHeight + extraDistance; y++)
                {
                    Tile tile = Main.tile[i + x, j + y];
                    if (tile.type == ModContent.TileType<Tiles.Incendiary.KindlingSoil>() && TerrorbornUtils.TileShouldBeGrass(i + x, j + y))
                    {
                        tile.type = (ushort)ModContent.TileType<Tiles.Incendiary.KindlingGrass>();
                    }
                }
            }

            int extraBounds = 12;
            return new Rectangle(i - extraBounds, j - extraBounds, islandWidth + extraBounds * 2, islandHeight + extraBounds * 2);
        }

        public static Rectangle GenerateIncendiaryIsland_Chest(int i, int j, float sizeMultiplier)
        {
            int islandWidth = (int)(20 * sizeMultiplier);
            int islandHeight = (int)(10 * sizeMultiplier);
            int pipeOffset = WorldGen.genRand.Next((int)(-2 * sizeMultiplier), (int)(2 * sizeMultiplier));
            bool forced = true;

            List<int> verticalPipesUp = new List<int>();

            List<int> verticalPipesDown = new List<int>();

            List<Point16> blockedTiles = new List<Point16>();

            Point16 center = new Point16(i + islandWidth / 2, j + islandHeight / 2);
            CreateLineOfBlocksHorizontal(center.X, center.Y, islandWidth / 2, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), forced: forced);
            CreateLineOfBlocksHorizontal(center.X, center.Y, islandWidth / 2, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), false, forced);

            int extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.15f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight / 2) - (islandHeight / 2 * distanceFromCenter));

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), forced: forced, withWall: true, wallType: WallID.LavaUnsafe3);
            }

            extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.1f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight / 2) - (islandHeight / 2 * distanceFromCenter)) / 2 + 1;

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), false, forced: forced, withWall: true, wallType: WallID.LavaUnsafe3);
            }

            extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.1f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight) - (islandHeight * distanceFromCenter));

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.PyroclasticCloud>(), false, forced: false);
            }

            GenerateIncendiaryShrine((int)center.X + 1, (int)center.Y);

            int extraDistance = 10;

            for (int x = -extraDistance; x < islandWidth + extraDistance; x++)
            {
                for (int y = -extraDistance; y < islandHeight + extraDistance; y++)
                {
                    Tile tile = Main.tile[i + x, j + y];
                    if (tile.type == ModContent.TileType<Tiles.Incendiary.KindlingSoil>() && TerrorbornUtils.TileShouldBeGrass(i + x, j + y))
                    {
                        tile.type = (ushort)ModContent.TileType<Tiles.Incendiary.KindlingGrass>();
                    }
                }
            }

            int extraBounds = 12;
            return new Rectangle(i - extraBounds, j - extraBounds, islandWidth + extraBounds * 2, islandHeight + extraBounds * 2);
        }

        public static Rectangle GenerateIncendiaryIsland_Main(int i, int j, float sizeMultiplier)
        {
            int islandWidth = (int)(80 * sizeMultiplier);
            int islandHeight = (int)(15 * sizeMultiplier);
            int pipeOffset = WorldGen.genRand.Next((int)(-2 * sizeMultiplier), (int)(2 * sizeMultiplier));
            bool forced = true;

            List<int> verticalPipesUp = new List<int>();

            List<int> verticalPipesDown = new List<int>();

            List<Point16> blockedTiles = new List<Point16>();

            Point16 center = new Point16(i + islandWidth / 2, j + islandHeight / 2);
            CreateLineOfBlocksHorizontal(center.X, center.Y, islandWidth / 2, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), forced: forced);
            CreateLineOfBlocksHorizontal(center.X, center.Y, islandWidth / 2, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), false, forced);

            int extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.15f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                if (extraHeight < -islandHeight / 3)
                {
                    extraHeight = -islandHeight / 3;
                }

                if (extraHeight > islandHeight / 3)
                {
                    extraHeight = islandHeight / 3;
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight / 2) - (islandHeight / 2 * distanceFromCenter));

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), forced: forced, withWall: true, wallType: WallID.LavaUnsafe3);
            }

            extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.1f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                if (extraHeight < -islandHeight / 5)
                {
                    extraHeight = -islandHeight / 5;
                }

                if (extraHeight > islandHeight / 5)
                {
                    extraHeight = islandHeight / 5;
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight / 2) - (islandHeight / 2 * distanceFromCenter)) / 2 + 1;

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), false, forced: forced, withWall: true, wallType: WallID.LavaUnsafe3);
            }

            for (int a = 0; a < WorldGen.genRand.Next(4, 7); a++)
            {
                WorldGen.TileRunner(i + WorldGen.genRand.Next(islandWidth), j + WorldGen.genRand.Next(islandHeight / 2), (double)WorldGen.genRand.Next(8, 10), WorldGen.genRand.Next(3, 6), ModContent.TileType<Tiles.Incendiary.IncendiaryAlloyTile>(), false, 0f, 0f, false, true);
            }

            extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.1f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight) - (islandHeight * distanceFromCenter));

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.PyroclasticCloud>(), false, forced: false);
            }

            GenerateIncendiaryAltar((int)center.X, (int)center.Y);

            int extraDistance = 10;

            for (int x = -extraDistance; x < islandWidth + extraDistance; x++)
            {
                for (int y = -extraDistance; y < islandHeight + extraDistance; y++)
                {
                    Tile tile = Main.tile[i + x, j + y];
                    if (tile.type == ModContent.TileType<Tiles.Incendiary.KindlingSoil>() && TerrorbornUtils.TileShouldBeGrass(i + x, j + y))
                    {
                        tile.type = (ushort)ModContent.TileType<Tiles.Incendiary.KindlingGrass>();
                    }
                }
            }

            int extraBounds = 12;
            return new Rectangle(i - extraBounds, j - extraBounds, islandWidth + extraBounds * 2, islandHeight + extraBounds * 2);
        }

        public static void GenerateIncendiaryAltar(int i, int j)
        {
            Vector2 currentPosition = new Vector2(i, j);
            while (!WorldGen.TileEmpty((int)currentPosition.X, (int)currentPosition.Y))
            {
                currentPosition.Y -= 1;
            }
            currentPosition -= new Vector2(1, 1);
            WorldGen.KillTile((int)currentPosition.X, (int)currentPosition.Y, noItem: true);
            WorldGen.KillTile((int)currentPosition.X + 1, (int)currentPosition.Y, noItem: true);
            WorldGen.KillTile((int)currentPosition.X + 2, (int)currentPosition.Y, noItem: true);
            WorldGen.KillTile((int)currentPosition.X, (int)currentPosition.Y + 1, noItem: true);
            WorldGen.KillTile((int)currentPosition.X + 1, (int)currentPosition.Y + 1, noItem: true);
            WorldGen.KillTile((int)currentPosition.X + 2, (int)currentPosition.Y + 1, noItem: true);
            WorldGen.KillTile((int)currentPosition.X, (int)currentPosition.Y + 2, noItem: true);
            WorldGen.KillTile((int)currentPosition.X + 1, (int)currentPosition.Y + 2, noItem: true);
            WorldGen.KillTile((int)currentPosition.X + 2, (int)currentPosition.Y + 2, noItem: true);
            CreateLineOfBlocksHorizontal((int)currentPosition.X, (int)currentPosition.Y + 2, 3, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), forced: true);
            WorldGen.PlaceTile((int)currentPosition.X + 1, (int)currentPosition.Y + 1, ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>(), forced: true);
        }

        public static void GenerateIncendiaryRitual(int i, int j)
        {
            Vector2 currentPosition = new Vector2(i, j);
            while (!WorldGen.TileEmpty((int)currentPosition.X, (int)currentPosition.Y))
            {
                currentPosition.Y -= 1;
            }
            currentPosition -= new Vector2(1, 1);
            WorldGen.KillTile((int)currentPosition.X, (int)currentPosition.Y, noItem: true);
            WorldGen.KillTile((int)currentPosition.X + 1, (int)currentPosition.Y, noItem: true);
            WorldGen.KillTile((int)currentPosition.X, (int)currentPosition.Y + 1, noItem: true);
            WorldGen.KillTile((int)currentPosition.X + 1, (int)currentPosition.Y + 1, noItem: true);
            WorldGen.KillTile((int)currentPosition.X, (int)currentPosition.Y + 2, noItem: true);
            WorldGen.KillTile((int)currentPosition.X + 1, (int)currentPosition.Y + 2, noItem: true);
            WorldGen.KillTile((int)currentPosition.X, (int)currentPosition.Y + 3, noItem: true);
            WorldGen.KillTile((int)currentPosition.X + 1, (int)currentPosition.Y + 3, noItem: true);
            CreateLineOfBlocksHorizontal((int)currentPosition.X, (int)currentPosition.Y + 3, 3, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), forced: true);
            WorldGen.PlaceTile((int)currentPosition.X + 1, (int)currentPosition.Y + 2, ModContent.TileType<Tiles.Incendiary.IncendiaryRitual>(), forced: true);
        }

        public static Rectangle GenerateIncendiaryIsland_Mechanical(int i, int j, float sizeMultiplier)
        {
            int islandWidth = (int)(20 * sizeMultiplier);
            int islandHeight = (int)(10 * sizeMultiplier);
            int pipeOffset = WorldGen.genRand.Next((int)(-2 * sizeMultiplier), (int)(2 * sizeMultiplier));
            bool forced = true;

            Rectangle rect = new Rectangle(i, j, islandWidth, islandHeight);
            foreach (Rectangle island in islands)
            {
                if (island != Rectangle.Empty)
                {
                    if (island.Intersects(rect))
                    {
                        return Rectangle.Empty;
                    }
                }
            }

            List<int> verticalPipesUp = new List<int>();

            List<int> verticalPipesDown = new List<int>();

            List<Point16> blockedTiles = new List<Point16>();

            Point16 center = new Point16(i + islandWidth / 2, j + islandHeight / 2);
            CreateLineOfBlocksHorizontal(center.X, center.Y, islandWidth / 2, ModContent.TileType<Tiles.Incendiary.IncendiaryMachinery>(), forced: forced);
            CreateLineOfBlocksHorizontal(center.X, center.Y, islandWidth / 2, ModContent.TileType<Tiles.Incendiary.IncendiaryMachinery>(), false, forced);

            CreateLineOfBlocksHorizontal(center.X, center.Y + pipeOffset, islandWidth / 2 + WorldGen.genRand.Next((int)(4 * sizeMultiplier)), ModContent.TileType<Tiles.Incendiary.IncendiaryPiping>(), forced: true);
            CreateLineOfBlocksHorizontal(center.X, center.Y + pipeOffset, islandWidth / 2 + WorldGen.genRand.Next((int)(4 * sizeMultiplier)), ModContent.TileType<Tiles.Incendiary.IncendiaryPiping>(), false, true);

            int extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight / 2) - (islandHeight / 2 * distanceFromCenter));

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.IncendiaryMachinery>(), forced: forced, withWall: true, wallType: ModContent.WallType<Tiles.Incendiary.IncendiaryMachineryWall>());
            }

            extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight / 2) - (islandHeight / 2 * distanceFromCenter)) / 2 + 1;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.IncendiaryMachinery>(), false, forced: forced, withWall: true, wallType: ModContent.WallType<Tiles.Incendiary.IncendiaryMachineryWall>());
            }

            for (int a = 0; a < WorldGen.genRand.Next(2, 4); a++)
            {
                WorldGen.TileRunner(i + WorldGen.genRand.Next(islandWidth), j + WorldGen.genRand.Next(islandHeight / 2), (double)WorldGen.genRand.Next(6, 8), WorldGen.genRand.Next(3, 6), ModContent.TileType<Tiles.Incendiary.IncendiaryAlloyTile>(), false, 0f, 0f, false, true);
            }

            extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.1f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight) - (islandHeight * distanceFromCenter));

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.PyroclasticCloud>(), false, forced: false);
            }

            CreateLineOfBlocksHorizontal(center.X, center.Y + pipeOffset, islandWidth / 2 + WorldGen.genRand.Next((int)(4 * sizeMultiplier)), ModContent.TileType<Tiles.Incendiary.IncendiaryPiping>(), forced: true);
            CreateLineOfBlocksHorizontal(center.X, center.Y + pipeOffset, islandWidth / 2 + WorldGen.genRand.Next((int)(4 * sizeMultiplier)), ModContent.TileType<Tiles.Incendiary.IncendiaryPiping>(), false, true);

            for (int p = 0; p < Main.rand.Next(1, 3); p++)
            {
                int positionX = WorldGen.genRand.Next(-islandWidth / 2 + (int)(4 * sizeMultiplier), islandWidth / 2 - (int)(4 * sizeMultiplier));
                bool canPlace = false;
                while (!canPlace)
                {
                    positionX = WorldGen.genRand.Next(-islandWidth / 2 + (int)(4 * sizeMultiplier), islandWidth / 2 - (int)(4 * sizeMultiplier));
                    canPlace = true;
                    foreach (int pos in verticalPipesUp)
                    {
                        if (MathHelper.Distance(positionX, pos) <= 2)
                        {
                            canPlace = false;
                        }
                    }
                }

                verticalPipesUp.Add(positionX);
                CreateLineOfBlocksVertical(center.X + positionX, center.Y + pipeOffset, Main.rand.Next(islandHeight / 2, islandHeight), ModContent.TileType<Tiles.Incendiary.IncendiaryPiping>(), forced: true);
            }

            for (int p = 0; p < Main.rand.Next(1, 3); p++)
            {
                int positionX = WorldGen.genRand.Next(-islandWidth / 2 + (int)(4 * sizeMultiplier), islandWidth / 2 - (int)(4 * sizeMultiplier));
                bool canPlace = false;
                while (!canPlace)
                {
                    positionX = WorldGen.genRand.Next(-islandWidth / 2 + (int)(4 * sizeMultiplier), islandWidth / 2 - (int)(4 * sizeMultiplier));
                    canPlace = true;
                    foreach (int pos in verticalPipesDown)
                    {
                        if (MathHelper.Distance(positionX, pos) <= 2)
                        {
                            canPlace = false;
                        }
                    }
                }

                verticalPipesDown.Add(positionX);
                CreateLineOfBlocksVertical(center.X + positionX, center.Y + pipeOffset, Main.rand.Next(islandHeight / 2, islandHeight), ModContent.TileType<Tiles.Incendiary.IncendiaryPiping>(), Up: false, forced: true);
            }

            int extraDistance = 10;

            for (int x = -extraDistance; x < islandWidth + extraDistance; x++)
            {
                for (int y = -extraDistance; y < islandHeight + extraDistance; y++)
                {
                    Tile tile = Main.tile[i + x, j + y];
                    if (tile.type == ModContent.TileType<Tiles.Incendiary.KindlingSoil>() && TerrorbornUtils.TileShouldBeGrass(i + x, j + y))
                    {
                        tile.type = (ushort)ModContent.TileType<Tiles.Incendiary.KindlingGrass>();
                    }
                }
            }

            int extraBounds = 12;
            return new Rectangle(i - extraBounds, j - extraBounds, islandWidth + extraBounds * 2, islandHeight + extraBounds * 2);
        }

        public static Rectangle GenerateIncendiaryIsland_Ritual(int i, int j, float sizeMultiplier)
        {
            int islandWidth = (int)(15 * sizeMultiplier);
            int islandHeight = (int)(10 * sizeMultiplier);
            int pipeOffset = WorldGen.genRand.Next((int)(-2 * sizeMultiplier), (int)(2 * sizeMultiplier));
            bool forced = true;

            List<int> verticalPipesUp = new List<int>();

            List<int> verticalPipesDown = new List<int>();

            List<Point16> blockedTiles = new List<Point16>();

            Point16 center = new Point16(i + islandWidth / 2, j + islandHeight / 2);
            CreateLineOfBlocksHorizontal(center.X, center.Y, islandWidth / 2, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), forced: forced);
            CreateLineOfBlocksHorizontal(center.X, center.Y, islandWidth / 2, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), false, forced);

            int extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.15f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                if (extraHeight < -islandHeight / 3)
                {
                    extraHeight = -islandHeight / 3;
                }

                if (extraHeight > islandHeight / 3)
                {
                    extraHeight = islandHeight / 3;
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight / 2) - (islandHeight / 2 * distanceFromCenter));

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), forced: forced, withWall: true, wallType: WallID.LavaUnsafe3);
            }

            extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.1f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                if (extraHeight < -islandHeight / 5)
                {
                    extraHeight = -islandHeight / 5;
                }

                if (extraHeight > islandHeight / 5)
                {
                    extraHeight = islandHeight / 5;
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight / 2) - (islandHeight / 2 * distanceFromCenter)) / 2 + 1;

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), false, forced: forced, withWall: true, wallType: WallID.LavaUnsafe3);
            }

            extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.1f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight) - (islandHeight * distanceFromCenter));

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.PyroclasticCloud>(), false, forced: false);
            }

            GenerateIncendiaryRitual((int)center.X, (int)center.Y);

            int extraDistance = 10;

            for (int x = -extraDistance; x < islandWidth + extraDistance; x++)
            {
                for (int y = -extraDistance; y < islandHeight + extraDistance; y++)
                {
                    Tile tile = Main.tile[i + x, j + y];
                    if (tile.type == ModContent.TileType<Tiles.Incendiary.KindlingSoil>() && TerrorbornUtils.TileShouldBeGrass(i + x, j + y))
                    {
                        tile.type = (ushort)ModContent.TileType<Tiles.Incendiary.KindlingGrass>();
                    }
                }
            }

            int extraBounds = 12;
            return new Rectangle(i - extraBounds, j - extraBounds, islandWidth + extraBounds * 2, islandHeight + extraBounds * 2);
        }

        public static Rectangle GenerateIncendiaryIsland_Normal(int i, int j, float sizeMultiplier)
        {
            int islandWidth = (int)(20 * sizeMultiplier);
            int islandHeight = (int)(10 * sizeMultiplier);
            int pipeOffset = WorldGen.genRand.Next((int)(-2 * sizeMultiplier), (int)(2 * sizeMultiplier));
            bool forced = true;

            Rectangle rect = new Rectangle(i, j, islandWidth, islandHeight);
            foreach (Rectangle island in islands)
            {
                if (island != Rectangle.Empty)
                {
                    if (island.Intersects(rect))
                    {
                        return Rectangle.Empty;
                    }
                }
            }

            List<int> verticalPipesUp = new List<int>();

            List<int> verticalPipesDown = new List<int>();

            List<Point16> blockedTiles = new List<Point16>();

            Point16 center = new Point16(i + islandWidth / 2, j + islandHeight / 2);
            CreateLineOfBlocksHorizontal(center.X, center.Y, islandWidth / 2, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), forced: forced);
            CreateLineOfBlocksHorizontal(center.X, center.Y, islandWidth / 2, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), false, forced);

            CreateLineOfBlocksHorizontal(center.X, center.Y + pipeOffset, islandWidth / 2 + WorldGen.genRand.Next((int)(4 * sizeMultiplier)), ModContent.TileType<Tiles.Incendiary.IncendiaryPiping>(), forced: true);
            CreateLineOfBlocksHorizontal(center.X, center.Y + pipeOffset, islandWidth / 2 + WorldGen.genRand.Next((int)(4 * sizeMultiplier)), ModContent.TileType<Tiles.Incendiary.IncendiaryPiping>(), false, true);

            int extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.15f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                if (extraHeight < -islandHeight / 3)
                {
                    extraHeight = -islandHeight / 3;
                }

                if (extraHeight > islandHeight / 3)
                {
                    extraHeight = islandHeight / 3;
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight / 2) - (islandHeight / 2 * distanceFromCenter));

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), forced: forced, withWall: true, wallType: WallID.LavaUnsafe3);
            }

            extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.1f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                if (extraHeight < -islandHeight / 5)
                {
                    extraHeight = -islandHeight / 5;
                }

                if (extraHeight > islandHeight / 5)
                {
                    extraHeight = islandHeight / 5;
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight / 2) - (islandHeight / 2 * distanceFromCenter)) / 2 + 1;

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.KindlingSoil>(), false, forced: forced, withWall: true, wallType: WallID.LavaUnsafe3);
            }

            if (WorldGen.genRand.NextFloat() <= 0.25f)
            {
                for (int a = 0; a < WorldGen.genRand.Next(2, 4); a++)
                {
                    WorldGen.TileRunner(i + WorldGen.genRand.Next(islandWidth), j + WorldGen.genRand.Next(islandHeight / 2), (double)WorldGen.genRand.Next(4, 6), WorldGen.genRand.Next(3, 6), ModContent.TileType<Tiles.Incendiary.IncendiaryAlloyTile>(), false, 0f, 0f, false, true);
                }
            }

            extraHeight = 0;
            for (int x = 0; x < islandWidth; x++)
            {
                if (WorldGen.genRand.NextFloat() <= 0.15f)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        extraHeight++;
                    }
                    else
                    {
                        extraHeight--;
                    }
                }

                float distanceFromCenter = MathHelper.Distance(x, islandWidth / 2f) / (islandWidth / 2f);
                int height = (int)((islandHeight) - (islandHeight * distanceFromCenter));

                height += extraHeight;

                CreateLineOfBlocksVertical(i + x, center.Y, height, ModContent.TileType<Tiles.Incendiary.PyroclasticCloud>(), false, forced: false);
            }

            CreateLineOfBlocksHorizontal(center.X, center.Y + pipeOffset, islandWidth / 2 + WorldGen.genRand.Next((int)(4 * sizeMultiplier)), ModContent.TileType<Tiles.Incendiary.IncendiaryPiping>(), forced: true);
            CreateLineOfBlocksHorizontal(center.X, center.Y + pipeOffset, islandWidth / 2 + WorldGen.genRand.Next((int)(4 * sizeMultiplier)), ModContent.TileType<Tiles.Incendiary.IncendiaryPiping>(), false, true);

            for (int p = 0; p < Main.rand.Next(1, 3); p++)
            {
                int positionX = WorldGen.genRand.Next(-islandWidth / 2 + (int)(4 * sizeMultiplier), islandWidth / 2 - (int)(4 * sizeMultiplier));
                bool canPlace = false;
                while (!canPlace)
                {
                    positionX = WorldGen.genRand.Next(-islandWidth / 2 + (int)(4 * sizeMultiplier), islandWidth / 2 - (int)(4 * sizeMultiplier));
                    canPlace = true;
                    foreach (int pos in verticalPipesUp)
                    {
                        if (MathHelper.Distance(positionX, pos) <= 2)
                        {
                            canPlace = false;
                        }
                    }
                }

                verticalPipesUp.Add(positionX);
                CreateLineOfBlocksVertical(center.X + positionX, center.Y + pipeOffset, Main.rand.Next(islandHeight / 2, islandHeight), ModContent.TileType<Tiles.Incendiary.IncendiaryPiping>(), forced: true);
            }

            int extraDistance = 10;

            for (int x = -extraDistance; x < islandWidth + extraDistance; x++)
            {
                for (int y = -extraDistance; y < islandHeight + extraDistance; y++)
                {
                    Tile tile = Main.tile[i + x, j + y];
                    if (tile.type == ModContent.TileType<Tiles.Incendiary.KindlingSoil>() && TerrorbornUtils.TileShouldBeGrass(i + x, j + y))
                    {
                        tile.type = (ushort)ModContent.TileType<Tiles.Incendiary.KindlingGrass>();
                    }
                }
            }

            int extraBounds = 12;
            return new Rectangle(i - extraBounds, j - extraBounds, islandWidth + extraBounds * 2, islandHeight + extraBounds * 2);
        }

        static List<Rectangle> islands = new List<Rectangle>();
        public static void GenerateIncendiaryBiome(float size = 1f, float density = 1f, bool forced = true)
        {
            int biomeWidth = (int)(Main.maxTilesX / 4f * size);
            int biomeHeight = (int)(Main.maxTilesY / 17f * size);
            int islandCount = (int)(Main.maxTilesX / 25 * density);
            int ritualIslandCount = (int)(Main.maxTilesX / 2800 * density);
            int chestIslandCount = (int)(Main.maxTilesX / 840 * 0.65f * density);
            int skullIslandCount = (int)(Main.maxTilesX / 250 * density);
            int rogueCloudsCount = (int)(Main.maxTilesX / 360 * density);

            islands.Clear();

            int side = 1;
            if (Main.rand.NextBool())
            {
                side = -1;
            }
            incendiaryIslandsSide = side;

            int distanceFromEdge = 80;
            Point16 cornerPosition = new Point16(Main.maxTilesX - distanceFromEdge, distanceFromEdge);
            if (side == -1)
            {
                cornerPosition = new Point16(distanceFromEdge, distanceFromEdge);
            }

            for (int i = 0; i < rogueCloudsCount; i++)
            {
                Point16 position = new Point16(cornerPosition.X + WorldGen.genRand.Next(biomeWidth) * -side, cornerPosition.Y + WorldGen.genRand.Next(biomeHeight));
                WorldGen.TileRunner(position.X, position.Y, Main.rand.NextFloat(5f, 8f), 5, ModContent.TileType<Tiles.Incendiary.PyroclasticRaincloud>(), true);
            }

            Point16 biomeCenter = cornerPosition + new Point16(biomeWidth * -side / 2, biomeHeight / 2);
            islands.Add(GenerateIncendiaryIsland_Main(biomeCenter.X, biomeCenter.Y, WorldGen.genRand.NextFloat(1f, 1.2f)));

            for (int i = 0; i < chestIslandCount; i++)
            {
                Point16 position = new Point16(cornerPosition.X + WorldGen.genRand.Next(biomeWidth) * -side, cornerPosition.Y + WorldGen.genRand.Next(biomeHeight));
                islands.Add(GenerateIncendiaryIsland_Chest(position.X, position.Y, WorldGen.genRand.NextFloat(0.8f, 1.2f)));
            }

            for (int i = 0; i < skullIslandCount; i++)
            {
                Point16 position = new Point16(cornerPosition.X + WorldGen.genRand.Next(biomeWidth) * -side, cornerPosition.Y + WorldGen.genRand.Next(biomeHeight));
                islands.Add(GenerateIncendiaryIsland_Skullmound(position.X, position.Y, WorldGen.genRand.NextFloat(0.8f, 1.2f)));
            }

            for (int i = 0; i < ritualIslandCount; i++)
            {
                Point16 position = new Point16(cornerPosition.X + WorldGen.genRand.Next(biomeWidth) * -side, cornerPosition.Y + WorldGen.genRand.Next(biomeHeight));
                islands.Add(GenerateIncendiaryIsland_Ritual(position.X, position.Y, WorldGen.genRand.NextFloat(0.8f, 1.2f)));
            }

            for (int i = 0; i < islandCount; i++)
            {
                Point16 position = new Point16(cornerPosition.X + WorldGen.genRand.Next(biomeWidth) * -side, cornerPosition.Y + WorldGen.genRand.Next(biomeHeight));

                WeightedRandom<int> type = new WeightedRandom<int>();
                type.Add(0, 1f); //Normal
                type.Add(1, 0.5f); //Mechanical

                switch (type)
                {
                    case 0:
                        islands.Add(GenerateIncendiaryIsland_Normal(position.X, position.Y, WorldGen.genRand.NextFloat(1f, 2f)));
                        break;
                    case 1:
                        islands.Add(GenerateIncendiaryIsland_Mechanical(position.X, position.Y, WorldGen.genRand.NextFloat(1.5f, 2.25f)));
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            tasks.Insert(genIndex + 1, new PassLegacy("Deimostone Chests", delegate (GenerationProgress progress)
            {
                progress.Message = "Generating Deimostone Chests";
                GenerateDeimostoneChests();
            }));
            tasks.Insert(genIndex + 2, new PassLegacy("Terror Shrines", delegate (GenerationProgress progress)
            {
                progress.Message = "Building the shrines";
                GenerateShrineStructures();
            }));
            genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Granite"));
            tasks.Insert(genIndex + 1, new PassLegacy("Deimostone caves", delegate (GenerationProgress progress)
            {
                deimostoneCaves.Clear();
                progress.Message = "Darkening Deimostone";
                GenerateDeimostoneCaves();
            }));
            genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            tasks.Insert(genIndex + 1, new PassLegacy("Deimosteel Ore", delegate (GenerationProgress progress)
            {
                progress.Message = "Generating Deimosteel";
                GenerateDeimosteel();
                progress.Message = "Generating Novagold";
                GenerateNovagold();
            }));

        }

        List<Point16> deimostoneChests = new List<Point16>(500);
        List<Rectangle> deimostoneCaves = new List<Rectangle>();
        public override void PostWorldGen()
        {
            for (int i = 0; i < 1000; i++)
            {
                Chest chest = Main.chest[i];
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 11 * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            switch (Main.rand.Next(4))
                            {
                                case 0:
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.BoostRelic>());
                                    break;
                                case 1:
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.CursedShades>());
                                    break;
                                case 2:
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.Shields.BronzeBuckler>());
                                    break;
                                case 3:
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.BurstJumps.FrostFlask>());
                                    break;
                            }
                            break;
                        }
                    }
                }

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
                    if (Main.rand.Next(101) <= 40)
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                if (Main.rand.NextBool())
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Ranged.DualpipeDartgun>());
                                }
                                else
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.Shields.PalladiumShield>());
                                }
                                break;
                            }
                        }
                    }
                }

                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 13 * 36)
                {
                    if (Main.rand.Next(101) <= 65)
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.BurstJumps.SkyBurst>());
                                break;
                            }
                        }
                    }
                }

                if (chest != null && Main.tile[chest.x, chest.y].type == ModContent.TileType<Tiles.DeimostoneChestTile>())
                {
                    List<int> mainItems = new List<int>();
                    mainItems.Add(ModContent.ItemType<Items.Weapons.Magic.ShriekersLung>());
                    mainItems.Add(ModContent.ItemType<Items.Equipable.Accessories.HorrificCharm>());
                    mainItems.Add(ModContent.ItemType<Items.Weapons.Melee.NightLight>());
                    mainItems.Add(ModContent.ItemType<Items.Weapons.Ranged.MindPiercer>());

                    List<int> secondaryMainItems = new List<int>();
                    secondaryMainItems.Add(ItemID.BandofRegeneration);
                    secondaryMainItems.Add(ItemID.MagicMirror);
                    secondaryMainItems.Add(ItemID.CloudinaBottle);
                    secondaryMainItems.Add(ItemID.HermesBoots);
                    secondaryMainItems.Add(ItemID.ShoeSpikes);

                    List<int> ammosAndThrowables = new List<int>();
                    ammosAndThrowables.Add(ItemID.FlamingArrow);
                    ammosAndThrowables.Add(ItemID.ThrowingKnife);

                    List<int> bars = new List<int>();
                    bars.Add(ItemID.SilverBar);
                    bars.Add(ItemID.TungstenBar);
                    bars.Add(ItemID.GoldBar);
                    bars.Add(ItemID.PlatinumBar);

                    List<int> commonPotions = new List<int>();
                    commonPotions.Add(ItemID.SpelunkerPotion);
                    commonPotions.Add(ItemID.FeatherfallPotion);
                    commonPotions.Add(ItemID.NightOwlPotion);
                    commonPotions.Add(ItemID.WaterWalkingPotion);
                    commonPotions.Add(ItemID.ArcheryPotion);
                    commonPotions.Add(ItemID.GravitationPotion);

                    List<int> uncommonPotions = new List<int>();
                    uncommonPotions.Add(ItemID.ThornsPotion);
                    uncommonPotions.Add(ItemID.HunterPotion);
                    uncommonPotions.Add(ItemID.TrapsightPotion);
                    uncommonPotions.Add(ItemID.TeleportationPotion);

                    List<int> lightItems = new List<int>();
                    lightItems.Add(ItemID.Torch);
                    lightItems.Add(ItemID.SpelunkerGlowstick);

                    int item = 0;
                    chest.item[item].SetDefaults(WorldGen.genRand.Next(mainItems));
                    item++;
                    chest.item[item].SetDefaults(WorldGen.genRand.Next(secondaryMainItems));
                    item++;
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(ItemID.Extractinator);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.4f)
                    {
                        chest.item[item].SetDefaults(ItemID.SuspiciousLookingEye);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.33f)
                    {
                        chest.item[item].SetDefaults(ItemID.Dynamite);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.25f)
                    {
                        chest.item[item].SetDefaults(ItemID.JestersArrow);
                        chest.item[item].stack = WorldGen.genRand.Next(25, 51);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(WorldGen.genRand.Next(bars));
                        chest.item[item].stack = WorldGen.genRand.Next(5, 15);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(WorldGen.genRand.Next(ammosAndThrowables));
                        chest.item[item].stack = WorldGen.genRand.Next(25, 51);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(ItemID.HealingPotion);
                        chest.item[item].stack = WorldGen.genRand.Next(6, 11);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.666f)
                    {
                        chest.item[item].SetDefaults(WorldGen.genRand.Next(commonPotions));
                        chest.item[item].stack = WorldGen.genRand.Next(2, 5);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.333f)
                    {
                        chest.item[item].SetDefaults(WorldGen.genRand.Next(uncommonPotions));
                        chest.item[item].stack = WorldGen.genRand.Next(2, 4);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(ItemID.RecallPotion);
                        chest.item[item].stack = WorldGen.genRand.Next(3, 6);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(WorldGen.genRand.Next(lightItems));
                        chest.item[item].stack = WorldGen.genRand.Next(20, 40);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(ItemID.GoldCoin);
                        chest.item[item].stack = WorldGen.genRand.Next(2, 5);
                        item++;
                    }
                }

                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 0 * 36)
                {
                    if (Main.rand.NextFloat() <= 0.25f)
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.Nunchucks>());
                                break;
                            }
                        }
                    }
                }

                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 1 * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            switch (Main.rand.Next(4))
                            {
                                case 0:
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.BoostRelic>());
                                    break;
                                case 1:
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.CursedShades>());
                                    break;
                                case 2:
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.Shields.BronzeBuckler>());
                                    break;
                                case 3:
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.BurstJumps.CloudInAFlask>());
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
        }

        public void GenerateDeimosteel()
        {
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05 / 3); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.rockLayerHigh, (int)Main.maxTilesY - 300);
                WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(3, 5), WorldGen.genRand.Next(3, 6), ModContent.TileType<Tiles.DeimosteelOre>(), false, 0f, 0f, false, true);
            }

            foreach(Rectangle element in deimostoneCaves)
            {
                for (int i = 0; i < 45; i++)
                {
                    int x = WorldGen.genRand.Next(element.Left, element.Right);
                    int y = WorldGen.genRand.Next(element.Top, element.Bottom);
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(6, 9), WorldGen.genRand.Next(3, 6), ModContent.TileType<Tiles.DeimosteelOre>(), false, 0f, 0f, false, true);
                }
            }
        }

        public void GenerateNovagold()
        {
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05 / 3); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.rockLayerHigh, (int)Main.maxTilesY - 300);
                WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(6, 8), WorldGen.genRand.Next(1, 3), ModContent.TileType<Tiles.Novagold>(), false, 0f, 0f, false, true);
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

            Structures.StructureGenerator.GenerateSOHShrine(mod, new Point((int)ShriekOfHorrorPosition.X, (int)ShriekOfHorrorPosition.Y));

            int DungeonDirection = 1;
            if (Main.dungeonX < Main.spawnTileX)
            {
                DungeonDirection = -1;
            }
            Vector2 HorrificAdaptationPosition = new Vector2(Main.spawnTileX + (Main.maxTilesX / 4) * -DungeonDirection, Main.maxTilesY / 2);
            Structures.StructureGenerator.GenerateHAShrine(mod, new Point((int)HorrificAdaptationPosition.X, (int)HorrificAdaptationPosition.Y));

            VoidBlink = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.95f);
            Structures.StructureGenerator.GenerateVBShrine(mod, new Point((int)VoidBlink.X, (int)VoidBlink.Y));

            TerrorWarp = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.66f);
            Structures.StructureGenerator.GenerateTWShrine(mod, new Point((int)TerrorWarp.X, (int)TerrorWarp.Y));
        }

        public void GenerateDeimostoneCaves()
        {
            float amountMultiplier = 0.005f;
            GenerateDeimostoneCave(new Point16(WorldGen.genRand.Next(Main.maxTilesX / 2 - 350, Main.maxTilesX / 2 + 350), (int)(WorldGen.rockLayerHigh + 25)), WorldGen.genRand.NextFloat(1.2f, 1.6f));
            for (int i = 0; i < Main.maxTilesX * amountMultiplier; i++)
            {
                GenerateDeimostoneCave(new Point16(WorldGen.genRand.Next(500, Main.maxTilesX - 500), WorldGen.genRand.Next((int)WorldGen.rockLayerHigh, (int)Main.maxTilesY - 300)), WorldGen.genRand.NextFloat(1.2f, 1.6f));
            }
        }

        public void GenerateDeimostoneCave(Point16 position, float sizeMultiplier)
        {
            int caveHeight = (int)(125 * sizeMultiplier);
            int caveWidth = (int)(32 * sizeMultiplier);
            int sideWidth = (int)(18 * sizeMultiplier);
            int sideDistance = (int)(15 * sizeMultiplier);
            float smallPartMult = 0.3f;
            float smallPartWidth = (int)(5 * sizeMultiplier);
            int caveCenterX = position.X + caveWidth / 2;
            int maxOffset = (int)(10 * sizeMultiplier);

            int largeSpikeSize = (int)(45 * sizeMultiplier);
            int largeSpikeThickness = (int)(5 * sizeMultiplier);

            int smallSpikeSize = (int)(25 * sizeMultiplier);
            int smallSpikeThickness = (int)(2 * sizeMultiplier);


            deimostoneCaves.Add(new Rectangle(position.X - caveWidth, position.Y - caveHeight, caveWidth * 2, caveHeight * 2));

            bool dontGenerate = false;
            for (int i = -caveWidth; i < caveWidth; i++)
            {
                for (int j = -caveHeight; j < caveHeight; j++)
                {
                    Point16 tilePosition = new Point16(position.X + i, position.Y + j);
                    if (Main.tile[tilePosition.X, tilePosition.Y].type == ModContent.TileType<Tiles.Deimostone>())
                    {
                        dontGenerate = true;
                    }
                }
            }

            if (dontGenerate)
            {
                return;
            }

            int pointCount = 12;

            List<int> offset = new List<int>();
            for (int i = -pointCount; i <= pointCount; i++)
            {
                offset.Add(WorldGen.genRand.Next(-maxOffset, maxOffset));
            }

            bool spikeOnLeft = WorldGen.genRand.NextBool();

            //GENERATE LEFT SIDE
            List<Point16> points = new List<Point16>();
            for (int i = -pointCount; i <= pointCount; i++)
            {
                if (Math.Abs(i) >= pointCount * smallPartMult)
                {
                    points.Add(new Point16((int)(position.X - smallPartWidth + offset[i + pointCount]), position.Y + (int)(caveHeight * ((float)i / (float)pointCount) / 2f)));
                }
                else
                {
                    points.Add(new Point16((int)(position.X - (caveWidth * (1f - Math.Abs((float)i / (float)pointCount)) * WorldGen.genRand.NextFloat(0.8f, 1.2f))) + offset[i + pointCount], position.Y + (int)(caveHeight * ((float)i / (float)pointCount) / 2f)));
                }
            }

            for (int i = 0; i < points.Count - 1; i++)
            {
                int amount = points[i + 1].Y - points[i].Y;
                int y = points[i].Y;
                int x = points[i].X - sideDistance;
                int width = (int)(sideWidth * WorldGen.genRand.NextFloat(0.6f, 1.4f));
                for (int w = x; w < position.X; w++)
                {
                    WorldGen.KillTile(w, y);
                    WorldGen.KillWall(w, y);
                    WorldGen.PlaceWall(w, y, ModContent.WallType<Tiles.DeimostoneWallTile>());
                }
                for (int w = -width / 2; w <= width / 2; w++)
                {
                    WorldGen.PlaceTile(x + w, y, ModContent.TileType<Tiles.Deimostone>(), true, true);
                }
                for (float a = 0; a <= amount; a += 0.5f)
                {
                    y = (int)MathHelper.Lerp(points[i + 1].Y, points[i].Y, (float)a / amount);
                    x = (int)MathHelper.Lerp(points[i + 1].X, points[i].X, (float)a / amount) - sideDistance;
                    width = (int)(sideWidth * WorldGen.genRand.NextFloat(0.6f, 1.4f));
                    for (int w = x; w < position.X; w++)
                    {
                        WorldGen.KillTile(w, y);
                        WorldGen.KillWall(w, y);
                        WorldGen.PlaceWall(w, y, ModContent.WallType<Tiles.DeimostoneWallTile>(), true);
                    }
                    for (int w = -width / 2; w <= width / 2; w++)
                    {
                        WorldGen.PlaceTile(x + w, y, ModContent.TileType<Tiles.Deimostone>(), true, true);
                    }
                }
            }

            Point16 spikePos = points[pointCount];
            List<Point16> smallSpikePos = new List<Point16>();
            if (!spikeOnLeft)
            {
                smallSpikePos.Add(points[WorldGen.genRand.Next(pointCount - 1, pointCount + 3)]);
                smallSpikePos.Add(points[WorldGen.genRand.Next(pointCount + 1, pointCount + 3)]);
            }

            for (int i = 0; i < WorldGen.genRand.Next(1, 3); i++)
            {
                Point16 chestPosition = WorldGen.genRand.Next(points);
                while (deimostoneChests.Contains(chestPosition))
                {
                    chestPosition = WorldGen.genRand.Next(points);
                }
                deimostoneChests.Add(chestPosition);
            }

            //GENERATE RIGHT SIDE
            points.Clear();
            for (int i = -pointCount; i <= pointCount; i++)
            {
                if (Math.Abs(i) >= pointCount * smallPartMult)
                {
                    points.Add(new Point16((int)(position.X + smallPartWidth) + offset[i + pointCount], position.Y + (int)(caveHeight * ((float)i / (float)pointCount) / 2f)));
                }
                else
                {
                    points.Add(new Point16((int)(position.X + (caveWidth * (1f - Math.Abs((float)i / (float)pointCount)) * WorldGen.genRand.NextFloat(0.8f, 1.2f))) + offset[i + pointCount], position.Y + (int)(caveHeight * ((float)i / (float)pointCount) / 2f)));
                }
            }

            for (int i = 0; i < points.Count - 1; i++)
            {
                int amount = points[i + 1].Y - points[i].Y;
                int y = points[i].Y;
                int x = points[i].X + sideDistance;
                int width = (int)(sideWidth * WorldGen.genRand.NextFloat(0.6f, 1.4f));
                for (int w = x; w > position.X - 1; w--)
                {
                    WorldGen.KillTile(w, y);
                    WorldGen.KillWall(w, y);
                    WorldGen.PlaceWall(w, y, ModContent.WallType<Tiles.DeimostoneWallTile>());
                }
                for (int w = -width / 2; w <= width / 2; w++)
                {
                    WorldGen.PlaceTile(x + w, y, ModContent.TileType<Tiles.Deimostone>());
                }
                for (float a = 0; a <= amount; a += 0.5f)
                {
                    y = (int)MathHelper.Lerp(points[i + 1].Y, points[i].Y, (float)a / amount);
                    x = (int)MathHelper.Lerp(points[i + 1].X, points[i].X, (float)a / amount) + sideDistance;
                    width = (int)(sideWidth * WorldGen.genRand.NextFloat(0.6f, 1.4f));
                    for (int w = x; w > position.X - 1; w--)
                    {
                        WorldGen.KillTile(w, y);
                        WorldGen.KillWall(w, y);
                        WorldGen.PlaceWall(w, y, ModContent.WallType<Tiles.DeimostoneWallTile>());
                    }
                    for (int w = -width / 2; w <= width / 2; w++)
                    {
                        WorldGen.PlaceTile(x + w, y, ModContent.TileType<Tiles.Deimostone>());
                    }
                }
            }

            for (int i = 0; i < WorldGen.genRand.Next(1, 3); i++)
            {
                Point16 chestPosition = WorldGen.genRand.Next(points);
                while (deimostoneChests.Contains(chestPosition))
                {
                    chestPosition = WorldGen.genRand.Next(points);
                }
                deimostoneChests.Add(chestPosition);
            }

            if (!spikeOnLeft)
            {
                spikePos = points[pointCount];
                for (int j = -largeSpikeThickness; j <= largeSpikeThickness; j++)
                {
                    int y = spikePos.Y + j;
                    for (int i = -20; i < largeSpikeSize * (1f - Math.Abs((float)j / (float)largeSpikeThickness)); i++)
                    {
                        int x = spikePos.X - i + 10;
                        WorldGen.PlaceTile(x, y, ModContent.TileType<Tiles.Deimostone>(), true, true);
                    }
                }

                foreach (Point16 pos in smallSpikePos)
                {
                    for (int j = -smallSpikeThickness; j <= smallSpikeThickness; j++)
                    {
                        int y = pos.Y + j;
                        for (int i = -15; i < smallSpikeSize * (1f - Math.Abs((float)j / (float)smallSpikeThickness)); i++)
                        {
                            int x = pos.X + i - 10;
                            WorldGen.PlaceTile(x, y, ModContent.TileType<Tiles.Deimostone>(), true, true);
                        }
                    }
                }
            }
            else
            {
                smallSpikePos.Clear();
                smallSpikePos.Add(points[WorldGen.genRand.Next(pointCount - 1, pointCount + 3)]);
                smallSpikePos.Add(points[WorldGen.genRand.Next(pointCount + 1, pointCount + 3)]);

                for (int j = -largeSpikeThickness; j <= largeSpikeThickness; j++)
                {
                    int y = spikePos.Y + j;
                    for (int i = -20; i < largeSpikeSize * (1f - Math.Abs((float)j / (float)largeSpikeThickness)); i++)
                    {
                        int x = spikePos.X + i - 10;
                        WorldGen.PlaceTile(x, y, ModContent.TileType<Tiles.Deimostone>(), true, true);
                    }
                }
                foreach (Point16 pos in smallSpikePos)
                {
                    for (int j = -smallSpikeThickness; j <= smallSpikeThickness; j++)
                    {
                        int y = pos.Y + j;
                        for (int i = -15; i < smallSpikeSize * (1f - Math.Abs((float)j / (float)smallSpikeThickness)); i++)
                        {
                            int x = pos.X - i + 10;
                            WorldGen.PlaceTile(x, y, ModContent.TileType<Tiles.Deimostone>(), true, true);
                        }
                    }
                }
            }

            int side = 1;
            if (WorldGen.genRand.NextBool())
            {
                side = -1;
            }
        }

        public void GenerateDeimostoneChests()
        {
            while (deimostoneChests.Count > 0)
            {
                GenerateDeimostoneChest(deimostoneChests[0].X, deimostoneChests[0].Y);
                deimostoneChests.RemoveAt(0);
            }
        }

        public void GenerateDeimostoneChest(int x, int y)
        {
            WorldGen.KillTile(x, y);
            WorldGen.KillTile(x + 1, y);
            WorldGen.KillTile(x + 1, y + 1);
            WorldGen.KillTile(x, y + 1);
            WorldGen.KillTile(x, y + 2);
            WorldGen.KillTile(x + 1, y + 2);
            WorldGen.PlaceTile(x, y + 2, ModContent.TileType<Tiles.Deimostone>());
            WorldGen.PlaceTile(x + 1, y + 2, ModContent.TileType<Tiles.Deimostone>());
            WorldGen.PlaceTile(x - 1, y + 2, ModContent.TileType<Tiles.Deimostone>());
            WorldGen.PlaceTile(x + 2, y + 2, ModContent.TileType<Tiles.Deimostone>());
            WorldGen.PlaceTile(x, y + 3, ModContent.TileType<Tiles.Deimostone>());
            WorldGen.PlaceTile(x + 1, y + 3, ModContent.TileType<Tiles.Deimostone>());
            WorldGen.PlaceTile(x, y + 1, (ushort)ModContent.TileType<Tiles.DeimostoneChestTile>());
            int chest = Chest.FindChest(x, y);
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

        public void OnRainStart()
        {
            if (!terrorRain && Main.rand.NextFloat() <= 0.2f && Main.hardMode)
            {
                terrorRain = true;
                Main.NewText("Dark rain begins to fall from the sky!", Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255));
            }
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

        bool wasRaining = true;
        int thunderCounter = -69;
        public override void PostUpdate()
        {
            if (thunderCounter == -69)
            {
                thunderCounter = Main.rand.Next(60 * 4, 60 * 45);
            }
            if (terrorRain)
            {
                Main.rainTexture = ModContent.GetTexture("TerrorbornMod/TerrorRain");

                if (NPC.AnyNPCs(ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>()))
                {
                    timeSinceFrightcrawlerSpawn = 0;
                }

                if (!Main.LocalPlayer.dead && Main.LocalPlayer.ZoneRain)
                {
                    timeSinceFrightcrawlerSpawn++;
                    if (timeSinceFrightcrawlerSpawn >= 3600 * 2)
                    {
                        Main.PlaySound(SoundID.Roar, (int)Main.LocalPlayer.Center.X, (int)Main.LocalPlayer.Center.Y, 0, 1, -0.3f);
                        NPC.SpawnOnPlayer(Main.myPlayer, ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>());
                    }
                    else if (timeSinceFrightcrawlerSpawn >= 3600f * 1f && Main.rand.NextFloat() <= 0.0002f)
                    {
                        Main.PlaySound(SoundID.Roar, (int)Main.LocalPlayer.Center.X, (int)Main.LocalPlayer.Center.Y, 0, 1, -0.3f);
                        NPC.SpawnOnPlayer(Main.myPlayer, ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>());
                    }
                }

                thunderCounter--;
                if (thunderCounter <= 0)
                {
                    thunderCounter = Main.rand.Next(60 * 4, 60 * 45);
                    TerrorbornMod.TerrorThunder();
                }
            }
            else
            {
                Main.rainTexture = ModContent.GetTexture("Terraria/Rain");
            }

            if (CartographerSpawnCooldown > 0)
            {
                CartographerSpawnCooldown--;
            }

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

            if (Main.raining && !wasRaining)
            {
                wasRaining = true;
                OnRainStart();
            }
            if (!Main.raining)
            {
                if (wasRaining)
                {
                    wasRaining = false;
                }
                if (terrorRain)
                {
                    Main.NewText("The sky begins to clear...", Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255));
                    downedTerrorRain = true;
                    terrorRain = false;
                }
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
