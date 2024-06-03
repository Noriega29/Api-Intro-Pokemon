using PokedexClient.Models;

namespace PokedexClient.Helpers
{
    public class AlertMessages
    {
        public PartialViewAlert Alert { get; }

        public AlertMessages(string color, string message)
        {
            Alert = new PartialViewAlert
            {
                Color = color,
                Message = message
            };
        }

        public static PartialViewAlert Error409x2(int num)
        {
            var alert = new AlertMessages(BootstrapColors.Amarillo, $"Ya hay un Pokémon en la Pokédex registrado con el número: {num}");
            return alert.Alert;
        }

        public static PartialViewAlert Error409x3(string pokemon)
        {
            var alert = new AlertMessages(BootstrapColors.Amarillo, $"{pokemon} ya está registrado en la Pokédex.");
            return alert.Alert;
        }
    }
}
