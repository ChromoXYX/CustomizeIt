# Custom Tourism

A **Cities: Skylines II** mod that lets you control the number of tourists in your city and customize building attractiveness.

## What it does

### Tourism Control
- Set a **target tourist count** for your city from the settings panel
- The mod spawns and manages tourist households to reach your target
- **0 = disabled** (vanilla behavior)
- Works independently from the game's built-in tourism formula
- Make sure you have enough hotels to accommodate your tourists

### Attractiveness Editor
- Select any building with attractiveness (landmarks, parks, tourist attractions) and an **editor panel** appears
- **Slider** to set attractiveness from 0 to 500
- One-click **Restore Default** to revert to vanilla values
- Changes apply to **all buildings of the same type**
- Overrides are **saved automatically** and persist across game restarts
- To close the panel, click anywhere else

## Languages
- English, French

## How it works

### Tourism Control
- Go to **Options → Custom Tourism → Tourism**
- Set the **Target Tourist Count** slider (0–20000)
- The mod will gradually spawn tourists until the target is reached
- Lowering the slider will gradually remove excess tourists

### Attractiveness Editor
- Click on any building with attractiveness in-game
- The Custom Tourism panel appears on the right side of the screen
- Drag the slider or type a value, then hit **Apply**
- The override is saved automatically and reloaded next time you play

## Note
- Attractiveness changes take about **5 to 20 seconds** to show up in the tourism menu. The override is applied immediately, but the game's tourism stats take a moment to recalculate.

## Compatibility
- Safe to remove anytime. Restore all overrides to default before uninstalling for clean vanilla values.
- Does not use Harmony

## Settings file
`ModsSettings/CustomTourism/CustomTourism.coc`

## Features

| Feature | Description |
|---------|-------------|
| Target tourist count | Set a custom tourist target for your city (0–20000). |
| Attractiveness slider | Set any building's attractiveness between **0 and 500**. |
| Restore Default | Reverts the building back to its vanilla attractiveness value. |
| Persistent overrides | Your changes are saved and automatically reapplied on game load. |
| Per-prefab changes | Changing one building affects all placed buildings of the same type. |

## Credits

Thanks to **Honu / River-Mochi** from the Cities: Skylines modding discord for their help!

## License

[MIT License](LICENSE)


[2026-05-12 21:31:03,774] [INFO]  OnLoad
[2026-05-12 21:31:03,774] [INFO]  Current mod asset at C:/Users/JohnL/AppData/LocalLow/Colossal Order/Cities Skylines II/Mods/CustomizeIt/CustomizeIt.dll
[2026-05-12 21:31:03,779] [INFO]  AttractivenessOverrideSystem created.
[2026-05-12 21:31:03,780] [INFO]  BuildingAttractivenessUISystem created.
[2026-05-12 21:31:03,781] [INFO]  TouristBoostSystem created.
[2026-05-12 21:31:03,782] [INFO]  TouristTransitArrivalSystem created.
[2026-05-12 21:31:41,478] [INFO]  [CT-DBG] tick frame=10801988 target=27500 stat=7664 predicted=7664 diff=19836 agg=10 deadband=550
[2026-05-12 21:31:41,480] [INFO]  [CT-DBG] OC inventory (12 entities) ===
[2026-05-12 21:31:41,481] [INFO]  [CT-DBG]   OC[0] entity=214563:1 prefab='Ship Outside Connection - Twoway' type=Ship hasOcData=True routes=1 wpConnected=1/1 wpBoarding=1/1
[2026-05-12 21:31:41,482] [INFO]  [CT-DBG]   OC[1] entity=214564:1 prefab='Ship Outside Connection - Twoway' type=Ship hasOcData=True routes=0 wpConnected=0/0 wpBoarding=0/0
[2026-05-12 21:31:41,482] [INFO]  [CT-DBG]   OC[2] entity=216779:1 prefab='Train Outside Connection - Twoway' type=Train hasOcData=True routes=1 wpConnected=1/1 wpBoarding=1/1
[2026-05-12 21:31:41,482] [INFO]  [CT-DBG]   OC[3] entity=259473:1 prefab='Road Outside Connection - Oneway' type=Road hasOcData=True routes=0 wpConnected=0/0 wpBoarding=0/0
[2026-05-12 21:31:41,482] [INFO]  [CT-DBG]   OC[4] entity=259474:1 prefab='Road Outside Connection - Oneway' type=Road hasOcData=True routes=0 wpConnected=0/0 wpBoarding=0/0
[2026-05-12 21:31:41,482] [INFO]  [CT-DBG]   OC[5] entity=259475:1 prefab='Road Outside Connection - Oneway' type=Road hasOcData=True routes=0 wpConnected=0/0 wpBoarding=0/0
[2026-05-12 21:31:41,483] [INFO]  [CT-DBG]   OC[6] entity=259476:1 prefab='Road Outside Connection - Oneway' type=Road hasOcData=True routes=0 wpConnected=0/0 wpBoarding=0/0
[2026-05-12 21:31:41,483] [INFO]  [CT-DBG]   OC[7] entity=288161:1 prefab='Airplane Outside Connection' type=Air hasOcData=True routes=0 wpConnected=0/0 wpBoarding=0/0
[2026-05-12 21:31:41,483] [INFO]  [CT-DBG]   OC[8] entity=288162:1 prefab='Airplane Outside Connection' type=Air hasOcData=True routes=1 wpConnected=1/1 wpBoarding=1/1
[2026-05-12 21:31:41,483] [INFO]  [CT-DBG]   OC[9] entity=288163:1 prefab='Airplane Outside Connection' type=Air hasOcData=True routes=1 wpConnected=1/1 wpBoarding=1/1
[2026-05-12 21:31:41,483] [INFO]  [CT-DBG]   OC[10] entity=306192:1 prefab='Train Outside Connection - Twoway' type=Train hasOcData=True routes=1 wpConnected=1/1 wpBoarding=1/1
[2026-05-12 21:31:41,484] [INFO]  [CT-DBG]   OC[11] entity=306193:1 prefab='Train Outside Connection - Twoway' type=Train hasOcData=True routes=1 wpConnected=1/1 wpBoarding=1/1
[2026-05-12 21:31:41,484] [INFO]  [CT-DBG] OC totals Road=4 Train=3 Air=3 Ship=2 None/Cargo-only=0
[2026-05-12 21:31:41,484] [INFO]  [CT-DBG] household prefab archetypes (13 entries) ===
[2026-05-12 21:31:41,485] [INFO]  [CT-DBG]   archetype[0] prefab='DynamicHousehold' types=[PrefabRef, Household, HouseholdNeed, TaxPayer, PropertySeeker, RandomLocalizationIndex, Resources, HouseholdCitizen, Created, Updated, Simulate, UpdateFrame]
[2026-05-12 21:31:41,485] [INFO]  [CT-DBG]   archetype[1] prefab='CoupleElderHousehold' types=[PrefabRef, Household, HouseholdNeed, TaxPayer, PropertySeeker, RandomLocalizationIndex, Resources, HouseholdCitizen, Created, Updated, Simulate, UpdateFrame]
[2026-05-12 21:31:41,485] [INFO]  [CT-DBG]   archetype[2] prefab='CoupleHousehold' types=[PrefabRef, Household, HouseholdNeed, TaxPayer, PropertySeeker, RandomLocalizationIndex, Resources, HouseholdCitizen, Created, Updated, Simulate, UpdateFrame]
[2026-05-12 21:31:41,485] [INFO]  [CT-DBG]   ... 10 more not dumped
[2026-05-12 21:31:41,488] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=8 Train=22 Air=42 Ship=28 None=0 Other=0]
[2026-05-12 21:31:41,575] [INFO]  [CT-TTA] results total=4 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=4]
[2026-05-12 21:31:41,578] [INFO]  [CT-TTA] eligible=93 submitted[Train=22 Air=42 Ship=28] skippedRoad=1 skippedNoOrigin=0
[2026-05-12 21:31:41,578] [INFO]  [CT-TTA] airOrigins routeWaypoint=28 directOC=14
[2026-05-12 21:31:47,364] [INFO]  [CT-DBG] tick frame=10802052 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:47,364] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=15 Train=16 Air=41 Ship=28 None=0 Other=0]
[2026-05-12 21:31:47,368] [INFO]  [CT-DBG] census total=7751 withHotel=1773 lodgingSeeker=241 householdStillHasCB=102 movingAway=1265 (NoTarget=1016 Other=249) atOC[Road=18 Train=116 Air=110 Ship=130 None=0] dyingAtOC[Road=10 Train=21 Air=110 Ship=2 Other=0] inCity=106 noCitizen=577 noBuilding=6694
[2026-05-12 21:31:47,464] [INFO]  [CT-TTA] results total=3 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=3]
[2026-05-12 21:31:47,464] [INFO]  [CT-TTA] eligible=87 submitted[Train=16 Air=41 Ship=28] skippedRoad=2 skippedNoOrigin=0
[2026-05-12 21:31:47,464] [INFO]  [CT-TTA] airOrigins routeWaypoint=27 directOC=14
[2026-05-12 21:31:48,456] [INFO]  [CT-DBG] tick frame=10802116 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:48,457] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=9 Train=21 Air=35 Ship=35 None=0 Other=0]
[2026-05-12 21:31:48,554] [INFO]  [CT-TTA] eligible=105 submitted[Train=21 Air=35 Ship=35] skippedRoad=2 skippedNoOrigin=12
[2026-05-12 21:31:48,554] [INFO]  [CT-TTA] airOrigins routeWaypoint=24 directOC=11
[2026-05-12 21:31:49,705] [INFO]  [CT-DBG] tick frame=10802180 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:49,706] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=20 Train=16 Air=36 Ship=28 None=0 Other=0]
[2026-05-12 21:31:49,708] [INFO]  [CT-DBG] census total=7839 withHotel=1772 lodgingSeeker=248 householdStillHasCB=107 movingAway=1244 (NoTarget=1002 Other=242) atOC[Road=15 Train=106 Air=99 Ship=130 None=0] dyingAtOC[Road=10 Train=13 Air=99 Ship=1 Other=0] inCity=106 noCitizen=582 noBuilding=6801
[2026-05-12 21:31:49,822] [INFO]  [CT-TTA] results total=10 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=10]
[2026-05-12 21:31:49,823] [INFO]  [CT-TTA] eligible=82 submitted[Train=16 Air=36 Ship=28] skippedRoad=2 skippedNoOrigin=0
[2026-05-12 21:31:49,823] [INFO]  [CT-TTA] airOrigins routeWaypoint=24 directOC=12
[2026-05-12 21:31:50,772] [INFO]  [CT-DBG] tick frame=10802244 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:50,773] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=9 Train=17 Air=37 Ship=37 None=0 Other=0]
[2026-05-12 21:31:50,862] [INFO]  [CT-TTA] results total=6 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=6]
[2026-05-12 21:31:50,862] [INFO]  [CT-TTA] eligible=95 submitted[Train=17 Air=37 Ship=37] skippedRoad=4 skippedNoOrigin=0
[2026-05-12 21:31:50,862] [INFO]  [CT-TTA] airOrigins routeWaypoint=27 directOC=10
[2026-05-12 21:31:51,833] [INFO]  [CT-DBG] tick frame=10802308 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:51,833] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=8 Train=18 Air=50 Ship=24 None=0 Other=0]
[2026-05-12 21:31:51,835] [INFO]  [CT-DBG] census total=7929 withHotel=1769 lodgingSeeker=240 householdStillHasCB=114 movingAway=1223 (NoTarget=991 Other=232) atOC[Road=20 Train=79 Air=93 Ship=137 None=0] dyingAtOC[Road=11 Train=0 Air=93 Ship=0 Other=0] inCity=106 noCitizen=589 noBuilding=6905
[2026-05-12 21:31:51,935] [INFO]  [CT-TTA] results total=2 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=2]
[2026-05-12 21:31:51,935] [INFO]  [CT-TTA] eligible=93 submitted[Train=18 Air=50 Ship=24] skippedRoad=1 skippedNoOrigin=0
[2026-05-12 21:31:51,936] [INFO]  [CT-TTA] airOrigins routeWaypoint=36 directOC=14
[2026-05-12 21:31:52,901] [INFO]  [CT-DBG] tick frame=10802372 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:52,902] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=16 Train=25 Air=39 Ship=20 None=0 Other=0]
[2026-05-12 21:31:52,999] [INFO]  [CT-TTA] results total=12 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=12]
[2026-05-12 21:31:53,000] [INFO]  [CT-TTA] eligible=88 submitted[Train=25 Air=39 Ship=20] skippedRoad=4 skippedNoOrigin=0
[2026-05-12 21:31:53,000] [INFO]  [CT-TTA] airOrigins routeWaypoint=23 directOC=16
[2026-05-12 21:31:53,967] [INFO]  [CT-DBG] tick frame=10802436 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:53,968] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=11 Train=18 Air=43 Ship=28 None=0 Other=0]
[2026-05-12 21:31:53,970] [INFO]  [CT-DBG] census total=8045 withHotel=1769 lodgingSeeker=249 householdStillHasCB=119 movingAway=1242 (NoTarget=1010 Other=232) atOC[Road=22 Train=86 Air=112 Ship=116 None=0] dyingAtOC[Road=11 Train=0 Air=112 Ship=0 Other=0] inCity=106 noCitizen=594 noBuilding=7009
[2026-05-12 21:31:54,067] [INFO]  [CT-TTA] results total=11 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=11]
[2026-05-12 21:31:54,068] [INFO]  [CT-TTA] eligible=92 submitted[Train=18 Air=43 Ship=28] skippedRoad=3 skippedNoOrigin=0
[2026-05-12 21:31:54,068] [INFO]  [CT-TTA] airOrigins routeWaypoint=31 directOC=12
[2026-05-12 21:31:55,031] [INFO]  [CT-DBG] tick frame=10802500 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:55,031] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=13 Train=18 Air=42 Ship=27 None=0 Other=0]
[2026-05-12 21:31:55,128] [INFO]  [CT-TTA] results total=9 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=9]
[2026-05-12 21:31:55,128] [INFO]  [CT-TTA] eligible=91 submitted[Train=18 Air=42 Ship=27] skippedRoad=4 skippedNoOrigin=0
[2026-05-12 21:31:55,129] [INFO]  [CT-TTA] airOrigins routeWaypoint=29 directOC=13
[2026-05-12 21:31:56,111] [INFO]  [CT-DBG] tick frame=10802564 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:56,111] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=13 Train=18 Air=34 Ship=35 None=0 Other=0]
[2026-05-12 21:31:56,114] [INFO]  [CT-DBG] census total=8122 withHotel=1769 lodgingSeeker=245 householdStillHasCB=116 movingAway=1224 (NoTarget=999 Other=225) atOC[Road=21 Train=82 Air=127 Ship=115 None=0] dyingAtOC[Road=10 Train=0 Air=127 Ship=0 Other=0] inCity=106 noCitizen=594 noBuilding=7077
[2026-05-12 21:31:56,209] [INFO]  [CT-TTA] results total=7 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=7]
[2026-05-12 21:31:56,210] [INFO]  [CT-TTA] eligible=90 submitted[Train=18 Air=34 Ship=35] skippedRoad=3 skippedNoOrigin=0
[2026-05-12 21:31:56,210] [INFO]  [CT-TTA] airOrigins routeWaypoint=20 directOC=14
[2026-05-12 21:31:57,168] [INFO]  [CT-DBG] tick frame=10802628 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:57,169] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=20 Air=42 Ship=28 None=0 Other=0]
[2026-05-12 21:31:57,267] [INFO]  [CT-TTA] results total=8 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=8]
[2026-05-12 21:31:57,267] [INFO]  [CT-TTA] eligible=92 submitted[Train=20 Air=42 Ship=28] skippedRoad=2 skippedNoOrigin=0
[2026-05-12 21:31:57,267] [INFO]  [CT-TTA] airOrigins routeWaypoint=26 directOC=16
[2026-05-12 21:31:58,245] [INFO]  [CT-DBG] tick frame=10802692 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:58,245] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=18 Air=43 Ship=29 None=0 Other=0]
[2026-05-12 21:31:58,248] [INFO]  [CT-DBG] census total=8205 withHotel=1764 lodgingSeeker=245 householdStillHasCB=119 movingAway=1198 (NoTarget=982 Other=216) atOC[Road=20 Train=76 Air=109 Ship=134 None=0] dyingAtOC[Road=10 Train=0 Air=109 Ship=5 Other=0] inCity=106 noCitizen=597 noBuilding=7163
[2026-05-12 21:31:58,346] [INFO]  [CT-TTA] results total=9 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=9]
[2026-05-12 21:31:58,347] [INFO]  [CT-TTA] eligible=92 submitted[Train=18 Air=43 Ship=29] skippedRoad=2 skippedNoOrigin=0
[2026-05-12 21:31:58,347] [INFO]  [CT-TTA] airOrigins routeWaypoint=27 directOC=16
[2026-05-12 21:31:58,829] [INFO]  [CT-DBG] tick frame=10802756 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:58,829] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=24 Air=35 Ship=31 None=0 Other=0]
[2026-05-12 21:31:58,857] [INFO]  [CT-TTA] eligible=105 submitted[Train=24 Air=35 Ship=31] skippedRoad=2 skippedNoOrigin=13
[2026-05-12 21:31:58,857] [INFO]  [CT-TTA] airOrigins routeWaypoint=18 directOC=17
[2026-05-12 21:31:59,107] [INFO]  [CT-DBG] tick frame=10802820 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:59,108] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=12 Train=28 Air=30 Ship=30 None=0 Other=0]
[2026-05-12 21:31:59,111] [INFO]  [CT-DBG] census total=8288 withHotel=1761 lodgingSeeker=246 householdStillHasCB=113 movingAway=1181 (NoTarget=971 Other=210) atOC[Road=18 Train=83 Air=97 Ship=134 None=0] dyingAtOC[Road=10 Train=0 Air=97 Ship=2 Other=0] inCity=106 noCitizen=591 noBuilding=7259
[2026-05-12 21:31:59,140] [INFO]  [CT-TTA] eligible=101 submitted[Train=28 Air=30 Ship=30] skippedRoad=1 skippedNoOrigin=12
[2026-05-12 21:31:59,140] [INFO]  [CT-TTA] airOrigins routeWaypoint=16 directOC=14
[2026-05-12 21:31:59,372] [INFO]  [CT-DBG] tick frame=10802884 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:59,373] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=13 Train=17 Air=30 Ship=40 None=0 Other=0]
[2026-05-12 21:31:59,388] [INFO]  [CT-TTA] eligible=99 submitted[Train=17 Air=30 Ship=40] skippedRoad=1 skippedNoOrigin=11
[2026-05-12 21:31:59,388] [INFO]  [CT-TTA] airOrigins routeWaypoint=18 directOC=12
[2026-05-12 21:31:59,625] [INFO]  [CT-DBG] tick frame=10802948 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:59,626] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=9 Train=26 Air=44 Ship=21 None=0 Other=0]
[2026-05-12 21:31:59,629] [INFO]  [CT-DBG] census total=8387 withHotel=1758 lodgingSeeker=239 householdStillHasCB=113 movingAway=1163 (NoTarget=954 Other=209) atOC[Road=15 Train=89 Air=83 Ship=142 None=0] dyingAtOC[Road=10 Train=0 Air=83 Ship=4 Other=0] inCity=106 noCitizen=591 noBuilding=7361
[2026-05-12 21:31:59,656] [INFO]  [CT-TTA] results total=5 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=5]
[2026-05-12 21:31:59,657] [INFO]  [CT-TTA] eligible=93 submitted[Train=26 Air=44 Ship=21] skippedRoad=2 skippedNoOrigin=0
[2026-05-12 21:31:59,657] [INFO]  [CT-TTA] airOrigins routeWaypoint=23 directOC=21
[2026-05-12 21:31:59,894] [INFO]  [CT-DBG] tick frame=10803012 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:31:59,894] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=7 Train=24 Air=42 Ship=27 None=0 Other=0]
[2026-05-12 21:31:59,922] [INFO]  [CT-TTA] results total=6 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=6]
[2026-05-12 21:31:59,923] [INFO]  [CT-TTA] eligible=94 submitted[Train=24 Air=42 Ship=27] skippedRoad=1 skippedNoOrigin=0
[2026-05-12 21:31:59,923] [INFO]  [CT-TTA] airOrigins routeWaypoint=30 directOC=12
[2026-05-12 21:32:00,153] [INFO]  [CT-DBG] tick frame=10803076 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:00,155] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=3 Train=21 Air=38 Ship=38 None=0 Other=0]
[2026-05-12 21:32:00,158] [INFO]  [CT-DBG] census total=8505 withHotel=1782 lodgingSeeker=269 householdStillHasCB=115 movingAway=1178 (NoTarget=970 Other=208) atOC[Road=14 Train=93 Air=101 Ship=126 None=0] dyingAtOC[Road=10 Train=0 Air=101 Ship=2 Other=0] inCity=106 noCitizen=593 noBuilding=7472
[2026-05-12 21:32:00,182] [INFO]  [CT-TTA] results total=10 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=10]
[2026-05-12 21:32:00,183] [INFO]  [CT-TTA] eligible=98 submitted[Train=21 Air=38 Ship=38] skippedRoad=1 skippedNoOrigin=0
[2026-05-12 21:32:00,184] [INFO]  [CT-TTA] airOrigins routeWaypoint=22 directOC=16
[2026-05-12 21:32:00,420] [INFO]  [CT-DBG] tick frame=10803140 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:00,420] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=7 Train=18 Air=46 Ship=29 None=0 Other=0]
[2026-05-12 21:32:00,444] [INFO]  [CT-TTA] results total=11 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=11]
[2026-05-12 21:32:00,445] [INFO]  [CT-TTA] eligible=97 submitted[Train=18 Air=46 Ship=29] skippedRoad=4 skippedNoOrigin=0
[2026-05-12 21:32:00,445] [INFO]  [CT-TTA] airOrigins routeWaypoint=32 directOC=14
[2026-05-12 21:32:00,691] [INFO]  [CT-DBG] tick frame=10803204 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:00,692] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=9 Train=18 Air=45 Ship=28 None=0 Other=0]
[2026-05-12 21:32:00,694] [INFO]  [CT-DBG] census total=8599 withHotel=1781 lodgingSeeker=267 householdStillHasCB=99 movingAway=1177 (NoTarget=971 Other=206) atOC[Road=18 Train=84 Air=104 Ship=122 None=0] dyingAtOC[Road=10 Train=0 Air=104 Ship=0 Other=0] inCity=106 noCitizen=577 noBuilding=7588
[2026-05-12 21:32:00,720] [INFO]  [CT-TTA] eligible=103 submitted[Train=18 Air=45 Ship=28] skippedRoad=4 skippedNoOrigin=8
[2026-05-12 21:32:00,720] [INFO]  [CT-TTA] airOrigins routeWaypoint=26 directOC=19
[2026-05-12 21:32:00,953] [INFO]  [CT-DBG] tick frame=10803268 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:00,954] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=23 Air=35 Ship=32 None=0 Other=0]
[2026-05-12 21:32:00,980] [INFO]  [CT-TTA] results total=8 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=8]
[2026-05-12 21:32:00,981] [INFO]  [CT-TTA] eligible=91 submitted[Train=23 Air=35 Ship=32] skippedRoad=1 skippedNoOrigin=0
[2026-05-12 21:32:00,981] [INFO]  [CT-TTA] airOrigins routeWaypoint=19 directOC=16
[2026-05-12 21:32:01,224] [INFO]  [CT-DBG] tick frame=10803332 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:01,225] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=8 Train=21 Air=48 Ship=23 None=0 Other=0]
[2026-05-12 21:32:01,228] [INFO]  [CT-DBG] census total=8704 withHotel=1780 lodgingSeeker=262 householdStillHasCB=97 movingAway=1178 (NoTarget=972 Other=206) atOC[Road=21 Train=92 Air=105 Ship=134 None=0] dyingAtOC[Road=10 Train=0 Air=105 Ship=0 Other=0] inCity=106 noCitizen=575 noBuilding=7671
[2026-05-12 21:32:01,256] [INFO]  [CT-TTA] eligible=97 submitted[Train=21 Air=48 Ship=23] skippedRoad=2 skippedNoOrigin=3
[2026-05-12 21:32:01,256] [INFO]  [CT-TTA] airOrigins routeWaypoint=35 directOC=13
[2026-05-12 21:32:01,492] [INFO]  [CT-DBG] tick frame=10803396 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:01,493] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=16 Air=40 Ship=34 None=0 Other=0]
[2026-05-12 21:32:01,521] [INFO]  [CT-TTA] eligible=99 submitted[Train=16 Air=40 Ship=34] skippedRoad=1 skippedNoOrigin=8
[2026-05-12 21:32:01,521] [INFO]  [CT-TTA] airOrigins routeWaypoint=26 directOC=14
[2026-05-12 21:32:01,765] [INFO]  [CT-DBG] tick frame=10803460 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:01,766] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=25 Air=38 Ship=27 None=0 Other=0]
[2026-05-12 21:32:01,769] [INFO]  [CT-DBG] census total=8807 withHotel=1780 lodgingSeeker=266 householdStillHasCB=101 movingAway=1180 (NoTarget=974 Other=206) atOC[Road=19 Train=94 Air=107 Ship=133 None=0] dyingAtOC[Road=10 Train=0 Air=107 Ship=0 Other=0] inCity=106 noCitizen=580 noBuilding=7768
[2026-05-12 21:32:01,784] [INFO]  [CT-TTA] results total=7 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=7]
[2026-05-12 21:32:01,785] [INFO]  [CT-TTA] eligible=94 submitted[Train=25 Air=38 Ship=27] skippedRoad=4 skippedNoOrigin=0
[2026-05-12 21:32:01,785] [INFO]  [CT-TTA] airOrigins routeWaypoint=27 directOC=11
[2026-05-12 21:32:02,023] [INFO]  [CT-DBG] tick frame=10803524 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:02,024] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=14 Air=45 Ship=31 None=0 Other=0]
[2026-05-12 21:32:02,062] [INFO]  [CT-TTA] eligible=98 submitted[Train=14 Air=45 Ship=31] skippedRoad=0 skippedNoOrigin=8
[2026-05-12 21:32:02,062] [INFO]  [CT-TTA] airOrigins routeWaypoint=30 directOC=15
[2026-05-12 21:32:02,293] [INFO]  [CT-DBG] tick frame=10803588 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:02,294] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=13 Train=22 Air=32 Ship=33 None=0 Other=0]
[2026-05-12 21:32:02,297] [INFO]  [CT-DBG] census total=8904 withHotel=1780 lodgingSeeker=264 householdStillHasCB=102 movingAway=1175 (NoTarget=969 Other=206) atOC[Road=20 Train=90 Air=107 Ship=130 None=0] dyingAtOC[Road=10 Train=0 Air=107 Ship=1 Other=0] inCity=106 noCitizen=581 noBuilding=7870
[2026-05-12 21:32:02,322] [INFO]  [CT-TTA] results total=5 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=5]
[2026-05-12 21:32:02,323] [INFO]  [CT-TTA] eligible=90 submitted[Train=22 Air=32 Ship=33] skippedRoad=3 skippedNoOrigin=0
[2026-05-12 21:32:02,323] [INFO]  [CT-TTA] airOrigins routeWaypoint=17 directOC=15
[2026-05-12 21:32:02,560] [INFO]  [CT-DBG] tick frame=10803652 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:02,560] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=8 Train=18 Air=38 Ship=36 None=0 Other=0]
[2026-05-12 21:32:02,586] [INFO]  [CT-TTA] results total=7 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=7]
[2026-05-12 21:32:02,586] [INFO]  [CT-TTA] eligible=94 submitted[Train=18 Air=38 Ship=36] skippedRoad=2 skippedNoOrigin=0
[2026-05-12 21:32:02,586] [INFO]  [CT-TTA] airOrigins routeWaypoint=26 directOC=12
[2026-05-12 21:32:02,825] [INFO]  [CT-DBG] tick frame=10803716 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:02,826] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=17 Air=41 Ship=32 None=0 Other=0]
[2026-05-12 21:32:02,829] [INFO]  [CT-DBG] census total=9003 withHotel=1780 lodgingSeeker=272 householdStillHasCB=106 movingAway=1156 (NoTarget=953 Other=203) atOC[Road=20 Train=97 Air=90 Ship=142 None=0] dyingAtOC[Road=10 Train=0 Air=90 Ship=0 Other=0] inCity=106 noCitizen=589 noBuilding=7959
[2026-05-12 21:32:02,856] [INFO]  [CT-TTA] eligible=108 submitted[Train=17 Air=41 Ship=32] skippedRoad=5 skippedNoOrigin=13
[2026-05-12 21:32:02,856] [INFO]  [CT-TTA] airOrigins routeWaypoint=27 directOC=14
[2026-05-12 21:32:03,092] [INFO]  [CT-DBG] tick frame=10803780 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:03,092] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=12 Train=13 Air=44 Ship=31 None=0 Other=0]
[2026-05-12 21:32:03,119] [INFO]  [CT-TTA] results total=14 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=14]
[2026-05-12 21:32:03,119] [INFO]  [CT-TTA] eligible=89 submitted[Train=13 Air=44 Ship=31] skippedRoad=1 skippedNoOrigin=0
[2026-05-12 21:32:03,120] [INFO]  [CT-TTA] airOrigins routeWaypoint=31 directOC=13
[2026-05-12 21:32:03,357] [INFO]  [CT-DBG] tick frame=10803844 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:03,357] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=7 Train=29 Air=35 Ship=29 None=0 Other=0]
[2026-05-12 21:32:03,361] [INFO]  [CT-DBG] census total=9100 withHotel=1779 lodgingSeeker=270 householdStillHasCB=95 movingAway=1165 (NoTarget=965 Other=200) atOC[Road=19 Train=78 Air=99 Ship=135 None=0] dyingAtOC[Road=10 Train=0 Air=99 Ship=1 Other=0] inCity=106 noCitizen=579 noBuilding=8084
[2026-05-12 21:32:03,388] [INFO]  [CT-TTA] eligible=106 submitted[Train=29 Air=35 Ship=29] skippedRoad=2 skippedNoOrigin=11
[2026-05-12 21:32:03,388] [INFO]  [CT-TTA] airOrigins routeWaypoint=21 directOC=14
[2026-05-12 21:32:03,630] [INFO]  [CT-DBG] tick frame=10803908 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:03,631] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=14 Train=19 Air=41 Ship=26 None=0 Other=0]
[2026-05-12 21:32:03,656] [INFO]  [CT-TTA] eligible=96 submitted[Train=19 Air=41 Ship=26] skippedRoad=2 skippedNoOrigin=8
[2026-05-12 21:32:03,656] [INFO]  [CT-TTA] airOrigins routeWaypoint=26 directOC=15
[2026-05-12 21:32:03,893] [INFO]  [CT-DBG] tick frame=10803972 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:03,893] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=14 Train=16 Air=38 Ship=32 None=0 Other=0]
[2026-05-12 21:32:03,896] [INFO]  [CT-DBG] census total=9207 withHotel=1779 lodgingSeeker=262 householdStillHasCB=93 movingAway=1167 (NoTarget=967 Other=200) atOC[Road=19 Train=88 Air=101 Ship=125 None=0] dyingAtOC[Road=10 Train=1 Air=101 Ship=1 Other=0] inCity=106 noCitizen=577 noBuilding=8191
[2026-05-12 21:32:03,921] [INFO]  [CT-TTA] eligible=95 submitted[Train=16 Air=38 Ship=32] skippedRoad=6 skippedNoOrigin=3
[2026-05-12 21:32:03,922] [INFO]  [CT-TTA] airOrigins routeWaypoint=25 directOC=13
[2026-05-12 21:32:04,154] [INFO]  [CT-DBG] tick frame=10804036 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:04,154] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=7 Train=20 Air=41 Ship=32 None=0 Other=0]
[2026-05-12 21:32:04,180] [INFO]  [CT-TTA] results total=5 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=5]
[2026-05-12 21:32:04,180] [INFO]  [CT-TTA] eligible=96 submitted[Train=20 Air=41 Ship=32] skippedRoad=3 skippedNoOrigin=0
[2026-05-12 21:32:04,181] [INFO]  [CT-TTA] airOrigins routeWaypoint=32 directOC=9
[2026-05-12 21:32:04,424] [INFO]  [CT-DBG] tick frame=10804100 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:04,424] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=19 Air=44 Ship=27 None=0 Other=0]
[2026-05-12 21:32:04,428] [INFO]  [CT-DBG] census total=9315 withHotel=1778 lodgingSeeker=267 householdStillHasCB=97 movingAway=1162 (NoTarget=962 Other=200) atOC[Road=24 Train=80 Air=97 Ship=136 None=0] dyingAtOC[Road=10 Train=1 Air=97 Ship=0 Other=0] inCity=106 noCitizen=582 noBuilding=8290
[2026-05-12 21:32:04,452] [INFO]  [CT-TTA] eligible=100 submitted[Train=19 Air=44 Ship=27] skippedRoad=2 skippedNoOrigin=8
[2026-05-12 21:32:04,452] [INFO]  [CT-TTA] airOrigins routeWaypoint=31 directOC=13
[2026-05-12 21:32:04,693] [INFO]  [CT-DBG] tick frame=10804164 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:04,693] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=9 Train=19 Air=41 Ship=31 None=0 Other=0]
[2026-05-12 21:32:04,715] [INFO]  [CT-TTA] results total=6 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=6]
[2026-05-12 21:32:04,715] [INFO]  [CT-TTA] eligible=93 submitted[Train=19 Air=41 Ship=31] skippedRoad=2 skippedNoOrigin=0
[2026-05-12 21:32:04,715] [INFO]  [CT-TTA] airOrigins routeWaypoint=28 directOC=13
[2026-05-12 21:32:04,959] [INFO]  [CT-DBG] tick frame=10804228 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:04,959] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=9 Train=19 Air=43 Ship=29 None=0 Other=0]
[2026-05-12 21:32:04,962] [INFO]  [CT-DBG] census total=9421 withHotel=1778 lodgingSeeker=267 householdStillHasCB=98 movingAway=1167 (NoTarget=970 Other=197) atOC[Road=24 Train=77 Air=121 Ship=123 None=0] dyingAtOC[Road=10 Train=0 Air=121 Ship=0 Other=0] inCity=106 noCitizen=583 noBuilding=8387
[2026-05-12 21:32:04,987] [INFO]  [CT-TTA] eligible=104 submitted[Train=19 Air=43 Ship=29] skippedRoad=5 skippedNoOrigin=8
[2026-05-12 21:32:04,988] [INFO]  [CT-TTA] airOrigins routeWaypoint=25 directOC=18
[2026-05-12 21:32:05,225] [INFO]  [CT-DBG] tick frame=10804292 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:05,226] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=12 Train=23 Air=34 Ship=31 None=0 Other=0]
[2026-05-12 21:32:05,250] [INFO]  [CT-TTA] eligible=99 submitted[Train=23 Air=34 Ship=31] skippedRoad=3 skippedNoOrigin=8
[2026-05-12 21:32:05,250] [INFO]  [CT-TTA] airOrigins routeWaypoint=19 directOC=15
[2026-05-12 21:32:05,486] [INFO]  [CT-DBG] tick frame=10804356 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:05,486] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=7 Train=20 Air=41 Ship=32 None=0 Other=0]
[2026-05-12 21:32:05,490] [INFO]  [CT-DBG] census total=9508 withHotel=1777 lodgingSeeker=261 householdStillHasCB=95 movingAway=1147 (NoTarget=957 Other=190) atOC[Road=22 Train=81 Air=113 Ship=120 None=0] dyingAtOC[Road=10 Train=0 Air=113 Ship=0 Other=0] inCity=106 noCitizen=580 noBuilding=8486
[2026-05-12 21:32:05,514] [INFO]  [CT-TTA] results total=2 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=2]
[2026-05-12 21:32:05,515] [INFO]  [CT-TTA] eligible=96 submitted[Train=20 Air=41 Ship=32] skippedRoad=3 skippedNoOrigin=0
[2026-05-12 21:32:05,515] [INFO]  [CT-TTA] airOrigins routeWaypoint=31 directOC=10
[2026-05-12 21:32:05,758] [INFO]  [CT-DBG] tick frame=10804420 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:05,759] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=25 Air=33 Ship=32 None=0 Other=0]
[2026-05-12 21:32:05,783] [INFO]  [CT-TTA] eligible=104 submitted[Train=25 Air=33 Ship=32] skippedRoad=2 skippedNoOrigin=12
[2026-05-12 21:32:05,783] [INFO]  [CT-TTA] airOrigins routeWaypoint=22 directOC=11
[2026-05-12 21:32:06,024] [INFO]  [CT-DBG] tick frame=10804484 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:06,026] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=13 Train=14 Air=44 Ship=29 None=0 Other=0]
[2026-05-12 21:32:06,029] [INFO]  [CT-DBG] census total=9592 withHotel=1775 lodgingSeeker=267 householdStillHasCB=93 movingAway=1119 (NoTarget=943 Other=176) atOC[Road=22 Train=85 Air=98 Ship=127 None=0] dyingAtOC[Road=10 Train=0 Air=98 Ship=0 Other=0] inCity=106 noCitizen=578 noBuilding=8576
[2026-05-12 21:32:06,053] [INFO]  [CT-TTA] eligible=100 submitted[Train=14 Air=44 Ship=29] skippedRoad=5 skippedNoOrigin=8
[2026-05-12 21:32:06,053] [INFO]  [CT-TTA] airOrigins routeWaypoint=29 directOC=15
[2026-05-12 21:32:06,292] [INFO]  [CT-DBG] tick frame=10804548 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:06,293] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=8 Train=25 Air=37 Ship=30 None=0 Other=0]
[2026-05-12 21:32:06,315] [INFO]  [CT-TTA] eligible=100 submitted[Train=25 Air=37 Ship=30] skippedRoad=0 skippedNoOrigin=8
[2026-05-12 21:32:06,316] [INFO]  [CT-TTA] airOrigins routeWaypoint=24 directOC=13
[2026-05-12 21:32:06,558] [INFO]  [CT-DBG] tick frame=10804612 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:06,558] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=12 Train=17 Air=43 Ship=28 None=0 Other=0]
[2026-05-12 21:32:06,562] [INFO]  [CT-DBG] census total=9700 withHotel=1774 lodgingSeeker=264 householdStillHasCB=93 movingAway=1124 (NoTarget=956 Other=168) atOC[Road=20 Train=84 Air=103 Ship=132 None=0] dyingAtOC[Road=10 Train=0 Air=103 Ship=1 Other=0] inCity=106 noCitizen=579 noBuilding=8676
[2026-05-12 21:32:06,586] [INFO]  [CT-TTA] eligible=97 submitted[Train=17 Air=43 Ship=28] skippedRoad=4 skippedNoOrigin=5
[2026-05-12 21:32:06,587] [INFO]  [CT-TTA] airOrigins routeWaypoint=26 directOC=17
[2026-05-12 21:32:06,825] [INFO]  [CT-DBG] tick frame=10804676 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:06,826] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=13 Train=17 Air=33 Ship=37 None=0 Other=0]
[2026-05-12 21:32:06,849] [INFO]  [CT-TTA] eligible=96 submitted[Train=17 Air=33 Ship=37] skippedRoad=0 skippedNoOrigin=9
[2026-05-12 21:32:06,849] [INFO]  [CT-TTA] airOrigins routeWaypoint=22 directOC=11
[2026-05-12 21:32:07,086] [INFO]  [CT-DBG] tick frame=10804740 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:07,086] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=18 Air=41 Ship=31 None=0 Other=0]
[2026-05-12 21:32:07,090] [INFO]  [CT-DBG] census total=9803 withHotel=1772 lodgingSeeker=271 householdStillHasCB=100 movingAway=1117 (NoTarget=949 Other=168) atOC[Road=16 Train=77 Air=97 Ship=131 None=0] dyingAtOC[Road=10 Train=0 Air=97 Ship=0 Other=0] inCity=106 noCitizen=588 noBuilding=8788
[2026-05-12 21:32:07,113] [INFO]  [CT-TTA] results total=12 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=12]
[2026-05-12 21:32:07,114] [INFO]  [CT-TTA] eligible=92 submitted[Train=18 Air=41 Ship=31] skippedRoad=2 skippedNoOrigin=0
[2026-05-12 21:32:07,114] [INFO]  [CT-TTA] airOrigins routeWaypoint=27 directOC=14
[2026-05-12 21:32:07,355] [INFO]  [CT-DBG] tick frame=10804804 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:07,356] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=12 Train=19 Air=41 Ship=28 None=0 Other=0]
[2026-05-12 21:32:07,382] [INFO]  [CT-TTA] eligible=99 submitted[Train=19 Air=41 Ship=28] skippedRoad=4 skippedNoOrigin=7
[2026-05-12 21:32:07,383] [INFO]  [CT-TTA] airOrigins routeWaypoint=28 directOC=13
[2026-05-12 21:32:07,624] [INFO]  [CT-DBG] tick frame=10804868 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:07,624] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=8 Train=23 Air=41 Ship=28 None=0 Other=0]
[2026-05-12 21:32:07,628] [INFO]  [CT-DBG] census total=9900 withHotel=1772 lodgingSeeker=270 householdStillHasCB=97 movingAway=1115 (NoTarget=947 Other=168) atOC[Road=17 Train=70 Air=95 Ship=118 None=0] dyingAtOC[Road=10 Train=0 Air=95 Ship=0 Other=0] inCity=106 noCitizen=585 noBuilding=8909
[2026-05-12 21:32:07,652] [INFO]  [CT-TTA] eligible=106 submitted[Train=23 Air=41 Ship=28] skippedRoad=3 skippedNoOrigin=11
[2026-05-12 21:32:07,653] [INFO]  [CT-TTA] airOrigins routeWaypoint=29 directOC=12
[2026-05-12 21:32:07,894] [INFO]  [CT-DBG] tick frame=10804932 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:07,894] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=6 Train=14 Air=52 Ship=28 None=0 Other=0]
[2026-05-12 21:32:07,919] [INFO]  [CT-TTA] results total=13 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=13]
[2026-05-12 21:32:07,919] [INFO]  [CT-TTA] eligible=96 submitted[Train=14 Air=52 Ship=28] skippedRoad=2 skippedNoOrigin=0
[2026-05-12 21:32:07,920] [INFO]  [CT-TTA] airOrigins routeWaypoint=41 directOC=11
[2026-05-12 21:32:08,154] [INFO]  [CT-DBG] tick frame=10804996 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:08,155] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=12 Train=19 Air=52 Ship=17 None=0 Other=0]
[2026-05-12 21:32:08,159] [INFO]  [CT-DBG] census total=10000 withHotel=1772 lodgingSeeker=264 householdStillHasCB=82 movingAway=1132 (NoTarget=964 Other=168) atOC[Road=18 Train=75 Air=112 Ship=116 None=0] dyingAtOC[Road=10 Train=0 Air=112 Ship=0 Other=0] inCity=106 noCitizen=571 noBuilding=9002
[2026-05-12 21:32:08,182] [INFO]  [CT-TTA] results total=5 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=5]
[2026-05-12 21:32:08,182] [INFO]  [CT-TTA] eligible=91 submitted[Train=19 Air=52 Ship=17] skippedRoad=3 skippedNoOrigin=0
[2026-05-12 21:32:08,183] [INFO]  [CT-TTA] airOrigins routeWaypoint=29 directOC=23
[2026-05-12 21:32:08,424] [INFO]  [CT-DBG] tick frame=10805060 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:08,424] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=9 Train=20 Air=42 Ship=29 None=0 Other=0]
[2026-05-12 21:32:08,449] [INFO]  [CT-TTA] eligible=94 submitted[Train=20 Air=42 Ship=29] skippedRoad=0 skippedNoOrigin=3
[2026-05-12 21:32:08,450] [INFO]  [CT-TTA] airOrigins routeWaypoint=27 directOC=15
[2026-05-12 21:32:08,691] [INFO]  [CT-DBG] tick frame=10805124 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:08,692] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=4 Train=22 Air=42 Ship=32 None=0 Other=0]
[2026-05-12 21:32:08,695] [INFO]  [CT-DBG] census total=10105 withHotel=1772 lodgingSeeker=264 householdStillHasCB=92 movingAway=1139 (NoTarget=971 Other=168) atOC[Road=15 Train=73 Air=119 Ship=105 None=0] dyingAtOC[Road=10 Train=0 Air=119 Ship=1 Other=0] inCity=106 noCitizen=581 noBuilding=9106
[2026-05-12 21:32:08,719] [INFO]  [CT-TTA] eligible=101 submitted[Train=22 Air=42 Ship=32] skippedRoad=0 skippedNoOrigin=5
[2026-05-12 21:32:08,719] [INFO]  [CT-TTA] airOrigins routeWaypoint=26 directOC=16
[2026-05-12 21:32:08,954] [INFO]  [CT-DBG] tick frame=10805188 target=33500 stat=7664 predicted=7764 diff=25736 agg=10 deadband=670
[2026-05-12 21:32:08,954] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=7 Train=21 Air=36 Ship=36 None=0 Other=0]
[2026-05-12 21:32:08,977] [INFO]  [CT-TTA] results total=8 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=8]
[2026-05-12 21:32:08,979] [INFO]  [CT-TTA] eligible=96 submitted[Train=21 Air=36 Ship=36] skippedRoad=3 skippedNoOrigin=0
[2026-05-12 21:32:08,980] [INFO]  [CT-TTA] airOrigins routeWaypoint=28 directOC=8
[2026-05-12 21:32:09,224] [INFO]  [CT-DBG] tick frame=10805252 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:09,224] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=6 Train=22 Air=47 Ship=25 None=0 Other=0]
[2026-05-12 21:32:09,228] [INFO]  [CT-DBG] census total=10188 withHotel=1771 lodgingSeeker=268 householdStillHasCB=87 movingAway=1113 (NoTarget=945 Other=168) atOC[Road=15 Train=73 Air=94 Ship=116 None=0] dyingAtOC[Road=10 Train=0 Air=94 Ship=0 Other=0] inCity=106 noCitizen=576 noBuilding=9208
[2026-05-12 21:32:09,254] [INFO]  [CT-TTA] results total=9 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=9]
[2026-05-12 21:32:09,254] [INFO]  [CT-TTA] eligible=95 submitted[Train=22 Air=47 Ship=25] skippedRoad=1 skippedNoOrigin=0
[2026-05-12 21:32:09,254] [INFO]  [CT-TTA] airOrigins routeWaypoint=32 directOC=15
[2026-05-12 21:32:09,492] [INFO]  [CT-DBG] tick frame=10805316 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:09,492] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=7 Train=17 Air=41 Ship=35 None=0 Other=0]
[2026-05-12 21:32:09,519] [INFO]  [CT-TTA] eligible=101 submitted[Train=17 Air=41 Ship=35] skippedRoad=2 skippedNoOrigin=6
[2026-05-12 21:32:09,519] [INFO]  [CT-TTA] airOrigins routeWaypoint=25 directOC=16
[2026-05-12 21:32:09,762] [INFO]  [CT-DBG] tick frame=10805380 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:09,763] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=20 Air=42 Ship=28 None=0 Other=0]
[2026-05-12 21:32:09,766] [INFO]  [CT-DBG] census total=10292 withHotel=1771 lodgingSeeker=260 householdStillHasCB=82 movingAway=1120 (NoTarget=952 Other=168) atOC[Road=15 Train=68 Air=103 Ship=118 None=0] dyingAtOC[Road=10 Train=0 Air=103 Ship=0 Other=0] inCity=106 noCitizen=572 noBuilding=9310
[2026-05-12 21:32:09,780] [INFO]  [CT-TTA] results total=1 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=1]
[2026-05-12 21:32:09,781] [INFO]  [CT-TTA] eligible=91 submitted[Train=20 Air=42 Ship=28] skippedRoad=1 skippedNoOrigin=0
[2026-05-12 21:32:09,782] [INFO]  [CT-TTA] airOrigins routeWaypoint=27 directOC=15
[2026-05-12 21:32:10,022] [INFO]  [CT-DBG] tick frame=10805444 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:10,022] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=16 Air=43 Ship=31 None=0 Other=0]
[2026-05-12 21:32:10,046] [INFO]  [CT-TTA] results total=11 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=11]
[2026-05-12 21:32:10,048] [INFO]  [CT-TTA] eligible=92 submitted[Train=16 Air=43 Ship=31] skippedRoad=2 skippedNoOrigin=0
[2026-05-12 21:32:10,048] [INFO]  [CT-TTA] airOrigins routeWaypoint=33 directOC=10
[2026-05-12 21:32:10,290] [INFO]  [CT-DBG] tick frame=10805508 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:10,291] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=17 Air=37 Ship=36 None=0 Other=0]
[2026-05-12 21:32:10,294] [INFO]  [CT-DBG] census total=10406 withHotel=1770 lodgingSeeker=267 householdStillHasCB=87 movingAway=1131 (NoTarget=965 Other=166) atOC[Road=16 Train=73 Air=115 Ship=112 None=0] dyingAtOC[Road=10 Train=0 Air=115 Ship=0 Other=0] inCity=106 noCitizen=577 noBuilding=9407
[2026-05-12 21:32:10,319] [INFO]  [CT-TTA] eligible=101 submitted[Train=17 Air=37 Ship=36] skippedRoad=3 skippedNoOrigin=8
[2026-05-12 21:32:10,319] [INFO]  [CT-TTA] airOrigins routeWaypoint=22 directOC=15
[2026-05-12 21:32:10,559] [INFO]  [CT-DBG] tick frame=10805572 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:10,560] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=16 Air=41 Ship=33 None=0 Other=0]
[2026-05-12 21:32:10,584] [INFO]  [CT-TTA] eligible=98 submitted[Train=16 Air=41 Ship=33] skippedRoad=3 skippedNoOrigin=5
[2026-05-12 21:32:10,584] [INFO]  [CT-TTA] airOrigins routeWaypoint=31 directOC=10
[2026-05-12 21:32:10,821] [INFO]  [CT-DBG] tick frame=10805636 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:10,822] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=15 Train=23 Air=37 Ship=25 None=0 Other=0]
[2026-05-12 21:32:10,826] [INFO]  [CT-DBG] census total=10507 withHotel=1770 lodgingSeeker=263 householdStillHasCB=88 movingAway=1123 (NoTarget=957 Other=166) atOC[Road=18 Train=68 Air=107 Ship=123 None=0] dyingAtOC[Road=10 Train=0 Air=107 Ship=0 Other=0] inCity=106 noCitizen=578 noBuilding=9507
[2026-05-12 21:32:10,850] [INFO]  [CT-TTA] results total=4 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=4]
[2026-05-12 21:32:10,851] [INFO]  [CT-TTA] eligible=87 submitted[Train=23 Air=37 Ship=25] skippedRoad=2 skippedNoOrigin=0
[2026-05-12 21:32:10,851] [INFO]  [CT-TTA] airOrigins routeWaypoint=22 directOC=15
[2026-05-12 21:32:11,092] [INFO]  [CT-DBG] tick frame=10805700 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:11,093] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=11 Train=20 Air=43 Ship=26 None=0 Other=0]
[2026-05-12 21:32:11,117] [INFO]  [CT-TTA] eligible=107 submitted[Train=20 Air=43 Ship=26] skippedRoad=6 skippedNoOrigin=12
[2026-05-12 21:32:11,118] [INFO]  [CT-TTA] airOrigins routeWaypoint=23 directOC=20
[2026-05-12 21:32:11,354] [INFO]  [CT-DBG] tick frame=10805764 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:11,354] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=9 Train=21 Air=48 Ship=22 None=0 Other=0]
[2026-05-12 21:32:11,359] [INFO]  [CT-DBG] census total=10599 withHotel=1770 lodgingSeeker=269 householdStillHasCB=90 movingAway=1111 (NoTarget=946 Other=165) atOC[Road=22 Train=71 Air=96 Ship=118 None=0] dyingAtOC[Road=10 Train=0 Air=96 Ship=0 Other=0] inCity=106 noCitizen=580 noBuilding=9606
[2026-05-12 21:32:11,382] [INFO]  [CT-TTA] results total=10 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=10]
[2026-05-12 21:32:11,383] [INFO]  [CT-TTA] eligible=91 submitted[Train=21 Air=48 Ship=22] skippedRoad=0 skippedNoOrigin=0
[2026-05-12 21:32:11,383] [INFO]  [CT-TTA] airOrigins routeWaypoint=30 directOC=18
[2026-05-12 21:32:11,622] [INFO]  [CT-DBG] tick frame=10805828 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:11,623] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=11 Train=30 Air=29 Ship=30 None=0 Other=0]
[2026-05-12 21:32:11,646] [INFO]  [CT-TTA] results total=6 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=6]
[2026-05-12 21:32:11,647] [INFO]  [CT-TTA] eligible=89 submitted[Train=30 Air=29 Ship=30] skippedRoad=0 skippedNoOrigin=0
[2026-05-12 21:32:11,648] [INFO]  [CT-TTA] airOrigins routeWaypoint=19 directOC=10
[2026-05-12 21:32:11,887] [INFO]  [CT-DBG] tick frame=10805892 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:11,887] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=13 Air=47 Ship=30 None=0 Other=0]
[2026-05-12 21:32:11,891] [INFO]  [CT-DBG] census total=10710 withHotel=1770 lodgingSeeker=263 householdStillHasCB=94 movingAway=1115 (NoTarget=950 Other=165) atOC[Road=14 Train=84 Air=100 Ship=96 None=0] dyingAtOC[Road=10 Train=0 Air=100 Ship=0 Other=0] inCity=106 noCitizen=584 noBuilding=9726
[2026-05-12 21:32:11,916] [INFO]  [CT-TTA] results total=4 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=4]
[2026-05-12 21:32:11,917] [INFO]  [CT-TTA] eligible=93 submitted[Train=13 Air=47 Ship=30] skippedRoad=3 skippedNoOrigin=0
[2026-05-12 21:32:11,917] [INFO]  [CT-TTA] airOrigins routeWaypoint=35 directOC=12
[2026-05-12 21:32:12,160] [INFO]  [CT-DBG] tick frame=10805956 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:12,160] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=8 Train=17 Air=39 Ship=36 None=0 Other=0]
[2026-05-12 21:32:12,184] [INFO]  [CT-TTA] eligible=107 submitted[Train=17 Air=39 Ship=36] skippedRoad=1 skippedNoOrigin=14
[2026-05-12 21:32:12,185] [INFO]  [CT-TTA] airOrigins routeWaypoint=29 directOC=10
[2026-05-12 21:32:12,421] [INFO]  [CT-DBG] tick frame=10806020 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:12,421] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=9 Train=19 Air=47 Ship=25 None=0 Other=0]
[2026-05-12 21:32:12,424] [INFO]  [CT-DBG] census total=10803 withHotel=1770 lodgingSeeker=272 householdStillHasCB=90 movingAway=1112 (NoTarget=947 Other=165) atOC[Road=14 Train=73 Air=97 Ship=115 None=0] dyingAtOC[Road=10 Train=0 Air=97 Ship=0 Other=0] inCity=106 noCitizen=580 noBuilding=9818
[2026-05-12 21:32:12,447] [INFO]  [CT-TTA] results total=13 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=13]
[2026-05-12 21:32:12,448] [INFO]  [CT-TTA] eligible=91 submitted[Train=19 Air=47 Ship=25] skippedRoad=0 skippedNoOrigin=0
[2026-05-12 21:32:12,448] [INFO]  [CT-TTA] airOrigins routeWaypoint=34 directOC=13
[2026-05-12 21:32:12,690] [INFO]  [CT-DBG] tick frame=10806084 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:12,691] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=11 Train=20 Air=38 Ship=31 None=0 Other=0]
[2026-05-12 21:32:12,719] [INFO]  [CT-TTA] eligible=94 submitted[Train=20 Air=38 Ship=31] skippedRoad=2 skippedNoOrigin=3
[2026-05-12 21:32:12,720] [INFO]  [CT-TTA] airOrigins routeWaypoint=30 directOC=8
[2026-05-12 21:32:12,958] [INFO]  [CT-DBG] tick frame=10806148 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:12,959] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=14 Train=18 Air=36 Ship=32 None=0 Other=0]
[2026-05-12 21:32:12,963] [INFO]  [CT-DBG] census total=10905 withHotel=1770 lodgingSeeker=267 householdStillHasCB=92 movingAway=1115 (NoTarget=950 Other=165) atOC[Road=15 Train=67 Air=100 Ship=116 None=0] dyingAtOC[Road=10 Train=0 Air=100 Ship=0 Other=0] inCity=106 noCitizen=582 noBuilding=9919
[2026-05-12 21:32:12,986] [INFO]  [CT-TTA] eligible=98 submitted[Train=18 Air=36 Ship=32] skippedRoad=4 skippedNoOrigin=8
[2026-05-12 21:32:12,986] [INFO]  [CT-TTA] airOrigins routeWaypoint=24 directOC=12
[2026-05-12 21:32:13,224] [INFO]  [CT-DBG] tick frame=10806212 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:13,224] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=14 Air=48 Ship=28 None=0 Other=0]
[2026-05-12 21:32:13,248] [INFO]  [CT-TTA] eligible=102 submitted[Train=14 Air=48 Ship=28] skippedRoad=1 skippedNoOrigin=11
[2026-05-12 21:32:13,248] [INFO]  [CT-TTA] airOrigins routeWaypoint=38 directOC=10
[2026-05-12 21:32:13,494] [INFO]  [CT-DBG] tick frame=10806276 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:13,496] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=29 Air=33 Ship=28 None=0 Other=0]
[2026-05-12 21:32:13,499] [INFO]  [CT-DBG] census total=11006 withHotel=1770 lodgingSeeker=269 householdStillHasCB=92 movingAway=1119 (NoTarget=954 Other=165) atOC[Road=15 Train=65 Air=114 Ship=101 None=0] dyingAtOC[Road=10 Train=0 Air=114 Ship=0 Other=0] inCity=106 noCitizen=582 noBuilding=10023
[2026-05-12 21:32:13,512] [INFO]  [CT-TTA] results total=10 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=10]
[2026-05-12 21:32:13,514] [INFO]  [CT-TTA] eligible=92 submitted[Train=29 Air=33 Ship=28] skippedRoad=2 skippedNoOrigin=0
[2026-05-12 21:32:13,514] [INFO]  [CT-TTA] airOrigins routeWaypoint=19 directOC=14
[2026-05-12 21:32:13,755] [INFO]  [CT-DBG] tick frame=10806340 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:13,756] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=9 Train=20 Air=43 Ship=28 None=0 Other=0]
[2026-05-12 21:32:13,782] [INFO]  [CT-TTA] results total=5 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=5]
[2026-05-12 21:32:13,784] [INFO]  [CT-TTA] eligible=94 submitted[Train=20 Air=43 Ship=28] skippedRoad=3 skippedNoOrigin=0
[2026-05-12 21:32:13,784] [INFO]  [CT-TTA] airOrigins routeWaypoint=29 directOC=14
[2026-05-12 21:32:14,024] [INFO]  [CT-DBG] tick frame=10806404 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:14,025] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=8 Train=18 Air=38 Ship=36 None=0 Other=0]
[2026-05-12 21:32:14,029] [INFO]  [CT-DBG] census total=11109 withHotel=1768 lodgingSeeker=260 householdStillHasCB=91 movingAway=1113 (NoTarget=950 Other=163) atOC[Road=20 Train=75 Air=110 Ship=111 None=0] dyingAtOC[Road=10 Train=0 Air=110 Ship=0 Other=0] inCity=106 noCitizen=581 noBuilding=10106
[2026-05-12 21:32:14,054] [INFO]  [CT-TTA] eligible=96 submitted[Train=18 Air=38 Ship=36] skippedRoad=3 skippedNoOrigin=1
[2026-05-12 21:32:14,054] [INFO]  [CT-TTA] airOrigins routeWaypoint=26 directOC=12
[2026-05-12 21:32:14,294] [INFO]  [CT-DBG] tick frame=10806468 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:14,294] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=14 Train=25 Air=37 Ship=24 None=0 Other=0]
[2026-05-12 21:32:14,318] [INFO]  [CT-TTA] eligible=94 submitted[Train=25 Air=37 Ship=24] skippedRoad=2 skippedNoOrigin=6
[2026-05-12 21:32:14,319] [INFO]  [CT-TTA] airOrigins routeWaypoint=25 directOC=12
[2026-05-12 21:32:14,559] [INFO]  [CT-DBG] tick frame=10806532 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:14,559] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=16 Train=13 Air=43 Ship=28 None=0 Other=0]
[2026-05-12 21:32:14,563] [INFO]  [CT-DBG] census total=11222 withHotel=1766 lodgingSeeker=269 householdStillHasCB=101 movingAway=1108 (NoTarget=948 Other=160) atOC[Road=18 Train=80 Air=105 Ship=104 None=0] dyingAtOC[Road=10 Train=0 Air=105 Ship=0 Other=0] inCity=106 noCitizen=592 noBuilding=10217
[2026-05-12 21:32:14,590] [INFO]  [CT-TTA] eligible=95 submitted[Train=13 Air=43 Ship=28] skippedRoad=1 skippedNoOrigin=10
[2026-05-12 21:32:14,590] [INFO]  [CT-TTA] airOrigins routeWaypoint=28 directOC=15
[2026-05-12 21:32:14,824] [INFO]  [CT-DBG] tick frame=10806596 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:14,824] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=9 Train=26 Air=33 Ship=32 None=0 Other=0]
[2026-05-12 21:32:14,849] [INFO]  [CT-TTA] eligible=99 submitted[Train=26 Air=33 Ship=32] skippedRoad=0 skippedNoOrigin=8
[2026-05-12 21:32:14,851] [INFO]  [CT-TTA] airOrigins routeWaypoint=23 directOC=10
[2026-05-12 21:32:15,087] [INFO]  [CT-DBG] tick frame=10806660 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:15,087] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=16 Train=21 Air=31 Ship=32 None=0 Other=0]
[2026-05-12 21:32:15,091] [INFO]  [CT-DBG] census total=11326 withHotel=1766 lodgingSeeker=263 householdStillHasCB=107 movingAway=1106 (NoTarget=946 Other=160) atOC[Road=17 Train=68 Air=103 Ship=107 None=0] dyingAtOC[Road=10 Train=0 Air=103 Ship=0 Other=0] inCity=106 noCitizen=599 noBuilding=10326
[2026-05-12 21:32:15,114] [INFO]  [CT-TTA] results total=4 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=4]
[2026-05-12 21:32:15,115] [INFO]  [CT-TTA] eligible=87 submitted[Train=21 Air=31 Ship=32] skippedRoad=3 skippedNoOrigin=0
[2026-05-12 21:32:15,115] [INFO]  [CT-TTA] airOrigins routeWaypoint=18 directOC=13
[2026-05-12 21:32:15,354] [INFO]  [CT-DBG] tick frame=10806724 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:15,355] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=7 Train=19 Air=39 Ship=35 None=0 Other=0]
[2026-05-12 21:32:15,379] [INFO]  [CT-TTA] results total=8 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=8]
[2026-05-12 21:32:15,381] [INFO]  [CT-TTA] eligible=95 submitted[Train=19 Air=39 Ship=35] skippedRoad=2 skippedNoOrigin=0
[2026-05-12 21:32:15,381] [INFO]  [CT-TTA] airOrigins routeWaypoint=30 directOC=9
[2026-05-12 21:32:15,643] [INFO]  [CT-DBG] tick frame=10806788 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:15,643] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=8 Train=18 Air=45 Ship=29 None=0 Other=0]
[2026-05-12 21:32:15,647] [INFO]  [CT-DBG] census total=11428 withHotel=1766 lodgingSeeker=274 householdStillHasCB=113 movingAway=1090 (NoTarget=930 Other=160) atOC[Road=17 Train=78 Air=87 Ship=118 None=0] dyingAtOC[Road=10 Train=0 Air=87 Ship=0 Other=0] inCity=105 noCitizen=607 noBuilding=10416
[2026-05-12 21:32:15,662] [INFO]  [CT-TTA] eligible=108 submitted[Train=18 Air=45 Ship=29] skippedRoad=1 skippedNoOrigin=15
[2026-05-12 21:32:15,662] [INFO]  [CT-TTA] airOrigins routeWaypoint=32 directOC=13
[2026-05-12 21:32:15,892] [INFO]  [CT-DBG] tick frame=10806852 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:15,892] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=10 Train=21 Air=39 Ship=30 None=0 Other=0]
[2026-05-12 21:32:15,916] [INFO]  [CT-TTA] eligible=97 submitted[Train=21 Air=39 Ship=30] skippedRoad=1 skippedNoOrigin=6
[2026-05-12 21:32:15,916] [INFO]  [CT-TTA] airOrigins routeWaypoint=28 directOC=11
[2026-05-12 21:32:16,171] [INFO]  [CT-DBG] tick frame=10806916 target=33500 stat=19960 predicted=20060 diff=13440 agg=10 deadband=670
[2026-05-12 21:32:16,172] [INFO]  [CT-DBG] spawn req=100 created=100 attempts=100 params=R0.10/T0.20/A0.40/S0.30 picks[Road=8 Train=19 Air=41 Ship=32 None=0 Other=0]
[2026-05-12 21:32:16,175] [INFO]  [CT-DBG] census total=11535 withHotel=1766 lodgingSeeker=262 householdStillHasCB=108 movingAway=1102 (NoTarget=942 Other=160) atOC[Road=17 Train=73 Air=99 Ship=113 None=0] dyingAtOC[Road=10 Train=0 Air=99 Ship=0 Other=0] inCity=105 noCitizen=602 noBuilding=10526
[2026-05-12 21:32:16,197] [INFO]  [CT-TTA] results total=3 ok[Train=0 Air=0 Ship=0 Other=0] fail[Train=0 Air=0 Ship=0 Other=3]
[2026-05-12 21:32:16,198] [INFO]  [CT-TTA] eligible=93 submitted[Train=19 Air=41 Ship=32] skippedRoad=1 skippedNoOrigin=0
[2026-05-12 21:32:16,199] [INFO]  [CT-TTA] airOrigins routeWaypoint=30 directOC=11

