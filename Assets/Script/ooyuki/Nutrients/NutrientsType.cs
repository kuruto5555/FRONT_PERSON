using System.Collections.Generic;



namespace FrontPerson.Constants
{
    public enum NUTRIENTS_TYPE
    {
        _A,
        _B,
        _ALL,

        COUNT
    }

    // ビタミンから栄養素の変わったので変更
    // Vitamin -> Nutrientsにする
    public static class Nutrients
	{
        public static List<string> Type = new List<string>() 
        {
            "栄養素A",
            "栄養素B",
            "栄養素ALL"
        };

        //public const string NUTRIENTS_A   = "NutrientsA";
        //public const string NUTRIENTS_A   = "NutrientsB";
        //public const string NUTRIENTS_ALL = "NutrientsALL";
    }

}
