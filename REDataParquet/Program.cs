using RealEstateDataParser.Services;

namespace REDataParquet {

    public class Program {

        private static void Main(string[] args) {
            var app = new App();
            SetMode(args);
            app.Run();
        }

        private static void SetMode(string[] args) {
            if ( args.Length == 0 ) {
                return;
            }

            var mode = args[0];
            if ( !string.IsNullOrWhiteSpace(mode) ) {
                ConfigurationService.Mode = mode;
            }
        }
    }
}