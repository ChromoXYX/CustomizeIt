using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using System.Collections.Generic;

namespace CustomizeIt
{
    [FileLocation("ModsSettings/CustomTourism/CustomTourism")]
    [SettingsUIGroupOrder(TourismGroup, ArrivalGroup, ResetGroup)]
    [SettingsUIShowGroupName(TourismGroup, ArrivalGroup, ResetGroup)]
    public class Setting : ModSetting
    {
        private const int MinTarget = 0;
        private const int MaxTarget = 60000;
        private const int DefaultTarget = 0;
        private const int DefaultAggressiveness = 5;
        private const int DefaultRoadWeight = 25;
        private const int DefaultTrainWeight = 25;
        private const int DefaultAirWeight = 25;
        private const int DefaultShipWeight = 25;

        public const string TourismTab = "Tourism";

        public const string TourismGroup = "Tourism Settings";
        public const string ArrivalGroup = "Arrival Distribution";
        public const string ResetGroup = "Reset";

        public Setting(IMod mod) : base(mod)
        {
        }

        public string[] OverridePrefabNames { get; set; } = new string[0];
        public int[] OverrideValues { get; set; } = new int[0];

        [SettingsUISlider(min = 0, max = 60000, step = 500, scalarMultiplier = 1, unit = Unit.kInteger)]
        [SettingsUISection(TourismTab, TourismGroup)]
        public int TargetTouristCount { get; set; }

        [SettingsUISlider(min = 1, max = 10, step = 1, scalarMultiplier = 1, unit = Unit.kInteger)]
        [SettingsUISection(TourismTab, TourismGroup)]
        public int Aggressiveness { get; set; }

        [SettingsUISlider(min = 0, max = 100, step = 1, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(TourismTab, ArrivalGroup)]
        public int RoadWeight { get; set; }

        [SettingsUISlider(min = 0, max = 100, step = 1, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(TourismTab, ArrivalGroup)]
        public int TrainWeight { get; set; }

        [SettingsUISlider(min = 0, max = 100, step = 1, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(TourismTab, ArrivalGroup)]
        public int AirWeight { get; set; }

        [SettingsUISlider(min = 0, max = 100, step = 1, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(TourismTab, ArrivalGroup)]
        public int ShipWeight { get; set; }

        [SettingsUIButton]
        [SettingsUISection(TourismTab, ResetGroup)]
        public bool ResetTourism
        {
            set
            {
                if (!value) return;
                TargetTouristCount = DefaultTarget;
                Aggressiveness = DefaultAggressiveness;
                RoadWeight = DefaultRoadWeight;
                TrainWeight = DefaultTrainWeight;
                AirWeight = DefaultAirWeight;
                ShipWeight = DefaultShipWeight;
                ApplyAndSave();
            }
        }

        public override void SetDefaults()
        {
            OverridePrefabNames = new string[0];
            OverrideValues = new int[0];
            TargetTouristCount = DefaultTarget;
            Aggressiveness = DefaultAggressiveness;
            RoadWeight = DefaultRoadWeight;
            TrainWeight = DefaultTrainWeight;
            AirWeight = DefaultAirWeight;
            ShipWeight = DefaultShipWeight;
        }

        public override void Apply()
        {
            if (TargetTouristCount < MinTarget || TargetTouristCount > MaxTarget)
                TargetTouristCount = DefaultTarget;
            if (Aggressiveness < 1 || Aggressiveness > 10)
                Aggressiveness = DefaultAggressiveness;
            if (RoadWeight < 0 || RoadWeight > 100) RoadWeight = DefaultRoadWeight;
            if (TrainWeight < 0 || TrainWeight > 100) TrainWeight = DefaultTrainWeight;
            if (AirWeight < 0 || AirWeight > 100) AirWeight = DefaultAirWeight;
            if (ShipWeight < 0 || ShipWeight > 100) ShipWeight = DefaultShipWeight;
            base.Apply();
        }
    }

    public class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Custom Tourism" },

                { m_Setting.GetOptionTabLocaleID(Setting.TourismTab), "Tourism" },

                { m_Setting.GetOptionGroupLocaleID(Setting.TourismGroup), "Tourism Settings" },
                { m_Setting.GetOptionGroupLocaleID(Setting.ArrivalGroup), "Arrival Distribution" },
                { m_Setting.GetOptionGroupLocaleID(Setting.ResetGroup), "Reset" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TargetTouristCount)), "Target Tourist Count" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.TargetTouristCount)),
                    "Set the target number of tourists for your city.\n" +
                    "**0 = disabled** (uses the game's vanilla formula).\n" +
                    "Higher values allow more tourists to spawn. The vanilla cap is around 2100. Range: 0-60,000.\n\n" +
                    "**Note:** Targets above 20,000 may impact game performance depending on your computer.\n" +
                    "Tourists also need good access to your city - make sure you have road, train, air, or ship outside connections, otherwise tourists won't be able to reach your city."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Aggressiveness)), "Spawn Speed" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.Aggressiveness)),
                    "How fast the mod adds or removes tourists each tick to reach your target.\n" +
                    "Lower values are gentler; higher values close big gaps faster but can cause brief hitches."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWeight)), "Road Arrivals" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWeight)),
                    "Relative share of tourists arriving by road. Values are relative — only the ratio between modes matters."
                },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainWeight)), "Train Arrivals" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainWeight)),
                    "Relative share of tourists arriving by train. Requires a passenger train line that touches a Train outside connection."
                },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirWeight)), "Air Arrivals" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.AirWeight)),
                    "Relative share of tourists arriving by air. Requires a passenger air route between your airport and an Air outside connection."
                },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipWeight)), "Ship Arrivals" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipWeight)),
                    "Relative share of tourists arriving by ship. Requires a passenger ferry line that touches a Ship outside connection."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetTourism)), "Reset to Default" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetTourism)),
                    "Set the target tourist count back to 0 (disabled). Re-enables the game's vanilla tourist spawning."
                },
            };
        }

        public void Unload()
        {
        }
    }

    public class LocaleFR : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleFR(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Custom Tourism" },

                { m_Setting.GetOptionTabLocaleID(Setting.TourismTab), "Tourisme" },

                { m_Setting.GetOptionGroupLocaleID(Setting.TourismGroup), "Parametres de tourisme" },
                { m_Setting.GetOptionGroupLocaleID(Setting.ArrivalGroup), "Distribution des arrivees" },
                { m_Setting.GetOptionGroupLocaleID(Setting.ResetGroup), "Reinitialiser" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TargetTouristCount)), "Nombre cible de touristes" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.TargetTouristCount)),
                    "Definit le nombre cible de touristes pour votre ville.\n" +
                    "**0 = desactive** (utilise la formule vanilla du jeu).\n" +
                    "Des valeurs plus elevees permettent l'arrivee de plus de touristes. Le plafond vanilla est d'environ 2100. Plage : 0-60000.\n\n" +
                    "**Note :** Au-dela de 20000, les performances peuvent etre affectees selon votre ordinateur.\n" +
                    "Les touristes ont aussi besoin d'un bon acces a votre ville - assurez-vous d'avoir des connexions exterieures par route, train, air ou bateau, sinon les touristes ne pourront pas atteindre votre ville."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.Aggressiveness)), "Vitesse d'arrivee" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.Aggressiveness)),
                    "Vitesse a laquelle le mod ajoute ou retire des touristes a chaque tick pour atteindre la cible.\n" +
                    "Des valeurs basses sont plus douces; des valeurs plus elevees comblent les grands ecarts plus vite mais peuvent causer de brefs ralentissements."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RoadWeight)), "Arrivees par route" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.RoadWeight)),
                    "Part relative des touristes arrivant par la route. Les valeurs sont relatives - seul le rapport entre les modes compte."
                },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TrainWeight)), "Arrivees par train" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.TrainWeight)),
                    "Part relative des touristes arrivant par train. Necessite une ligne de train passagers reliee a une connexion exterieure ferroviaire."
                },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.AirWeight)), "Arrivees par avion" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.AirWeight)),
                    "Part relative des touristes arrivant par avion. Necessite une ligne aerienne passagers entre votre aeroport et une connexion exterieure aerienne."
                },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ShipWeight)), "Arrivees par bateau" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.ShipWeight)),
                    "Part relative des touristes arrivant par bateau. Necessite une ligne de ferry passagers reliee a une connexion exterieure maritime."
                },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetTourism)), "Reinitialiser" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetTourism)),
                    "Remet le nombre cible de touristes a 0 (desactive). Reactive le systeme de tourisme vanilla du jeu."
                },
            };
        }

        public void Unload()
        {
        }
    }
}
