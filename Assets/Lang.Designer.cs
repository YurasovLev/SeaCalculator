namespace SeaCalculator.Assets {
    using System.Resources;
    using System.Globalization;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Lang {
        
        private static ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        internal Lang() {
        }

        internal static ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    ResourceManager temp = new ResourceManager("SeaCalculator.Assets.Lang", typeof(Lang).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        internal static CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string ReceiversTitle {
            get {
                return ResourceManager.GetString("ReceiversTitle", resourceCulture);
            }
        }

        public static string Name {
            get {
                return ResourceManager.GetString("Name", resourceCulture);
            }
        }

        public static string ReceiverColumnCountTitle {
            get {
                return ResourceManager.GetString("ReceiverColumnCountTitle", resourceCulture);
            }
        }

        public static string ReceiverColumnRatedSteadyPowerTitle {
            get {
                return ResourceManager.GetString("ReceiverColumnRatedSteadyPowerTitle", resourceCulture);
            }
        }

        public static string CosPhi {
            get {
                return ResourceManager.GetString("CosPhi", resourceCulture);
            }
        }

        public static string ReceiverColumnEfficiencyTitle {
            get {
                return ResourceManager.GetString("ReceiverColumnEfficiencyTitle", resourceCulture);
            }
        }

        public static string ReceiverColumnRatedPowerConsumptionTitle {
            get {
                return ResourceManager.GetString("ReceiverColumnRatedPowerConsumptionTitle", resourceCulture);
            }
        }

        public static string ReceiverModesTitle {
            get {
                return ResourceManager.GetString("ReceiverModesTitle", resourceCulture);
            }
        }

        public static string ReceiverModesWorkModeTitle {
            get {
                return ResourceManager.GetString("ReceiverModesWorkModeTitle", resourceCulture);
            }
        }

        public static string ReceiverModesWorkModeContinuesTitle {
            get {
                return ResourceManager.GetString("ReceiverModesWorkModeContinuesTitle", resourceCulture);
            }
        }
        public static string ReceiverModesWorkModePeriodicTitle {
            get {
                return ResourceManager.GetString("ReceiverModesWorkModePeriodicTitle", resourceCulture);
            }
        }
        public static string ReceiverModesWorkModeEpisodicTitle {
            get {
                return ResourceManager.GetString("ReceiverModesWorkModeEpisodicTitle", resourceCulture);
            }
        }

        public static string ReceiverModesLoadFactorTitle {
            get {
                return ResourceManager.GetString("ReceiverModesLoadFactorTitle", resourceCulture);
            }
        }

        public static string ReceiverModesCountTitle {
            get {
                return ResourceManager.GetString("ReceiverModesCountTitle", resourceCulture);
            }
        }

        public static string ReceiverModesActivePowerTitle {
            get {
                return ResourceManager.GetString("ReceiverModesActivePowerTitle", resourceCulture);
            }
        }

        public static string ReceiverModesReactivePowerTitle {
            get {
                return ResourceManager.GetString("ReceiverModesReactivePowerTitle", resourceCulture);
            }
        }

    }
}