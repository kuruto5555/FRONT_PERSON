using System.Collections.Generic;



namespace FrontPerson.Constants
{
    public enum NUTRIENTS_COLOR
    {
        _A,
        _B,
        _ALL,

        COUNT
    }

    // ビタミンから栄養素の変わったので変更
    // Vitamin -> Nutrientsにする
    public static class NutrientsColor
    {
        public static List<string> Type = new List<string>()
        {
            "赤",
            "青",
            ""
        };

        //public const string NUTRIENTS_A   = "NutrientsA";
        //public const string NUTRIENTS_A   = "NutrientsB";
        //public const string NUTRIENTS_ALL = "NutrientsALL";
    }

}