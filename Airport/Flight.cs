using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class Flight
    {
        public City DepartureCity { get; private set; }
        public City ArrivalCity { get; private set; }
        public DateTime DepartureDatetime { get; private set; }
        public DateTime ArrivalDatetime { get; private set; }
        public Airline Airline { get; private set; }
        public double Price { get; private set; }

    }

    enum City
    {
        NewYork,
        LosAngeles,
        Chicago,
        Houston,
        Phoenix,
        Philadelphia,
        SanAntonio,
        SanDiego,
        Dallas,
        SanJose,
        Austin,
        Jacksonville,
        SanFrancisco,
        Indianapolis,
        Columbus,
        FortWorth,
        Charlotte,
        Seattle,
        Denver,
        Washington
    }
    enum Airline
    {
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