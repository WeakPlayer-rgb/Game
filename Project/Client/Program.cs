﻿using System;

class BRS
{
    public static void Main()
    {
        var баллЗаДЗ = 2051;
        var баллЗаСеминары = 10;
        var баллЗаАктивности = 11;
        var баллБРСЗаРаботуВСеместре = ВерниБаллБРСЗаРаботуВСеместре(баллЗаДЗ, баллЗаСеминары, баллЗаАктивности);
        var баллБРС = ВерниБаллБРС(баллБРСЗаРаботуВСеместре, 0);
        Console.WriteLine(баллБРСЗаРаботуВСеместре);
        Console.WriteLine(баллБРС);
    }

    private static int ВерниБаллБРС(int баллБРСЗаРаботуВСеместре, int баллЗаЭкзамен = 0)
    {
        var максимумЗаЭкзамен = 100;
        return (int) Math.Round(0.6 * баллБРСЗаРаботуВСеместре + 0.4 * баллЗаЭкзамен);
    }

    private static int ВерниБаллБРСЗаРаботуВСеместре(int баллЗаДЗ, int баллЗаСеминары, int баллЗаАктивности)
    {
        var максимумЗаСеминары = 13;
        var максимумЗаДопАктивности = 12;
        var максимумЗаДЗ = 2400;
        var баллЗаРаботуВСеместре =
            20
            + 0.5 * Math.Min(баллЗаДЗ, 0.8 * максимумЗаДЗ) / (максимумЗаДЗ * 0.8) * 100
            + 0.15 * Math.Min(баллЗаСеминары, 0.8 * баллЗаСеминары) / (максимумЗаСеминары * 0.8) * 100
            + 0.1 * Math.Min(баллЗаАктивности, 0.8 * баллЗаАктивности) / (максимумЗаДопАктивности * 0.8) * 100;
        return (int) Math.Round(баллЗаРаботуВСеместре);
    }
}