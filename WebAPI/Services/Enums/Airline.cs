using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebAPI
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Airline
    {
        Undefined = 0,
        Delta,
        AmericanAirlines,
        United,
        Southwest,
        AirFrance,
        Lufthansa,
        BritishAirways,
        Emirates,
        CathayPacific,
        Qantas,
        AirCanada,
        SingaporeAirlines,
        TurkishAirlines,
        ANA,
        JapanAirlines,
        KoreanAir,
        EtihadAirways,
        VirginAtlantic,
        QatarAirways,
        JetBlue
    }
}
