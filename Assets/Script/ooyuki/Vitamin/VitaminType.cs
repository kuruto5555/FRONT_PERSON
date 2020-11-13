using System.Collections.Generic;



namespace FrontPerson.Constants
{
    public enum NUTRIENTS_TYPE
    {
        NUTRIENTS_A,
        NUTRIENTS_B,
        NUTRIENTS_ALL,

        COUNT
    }

    // ビタミンから栄養素の変わったので変更
    // Vitamin -> Nutrientsにする
    public static class Nutrients
	{
        public static List<string> Type = new List<string>() 
        {
            "NutrientsA",
            "NutrientsB",
            "NutrientsALL"
        };

        //public const string NUTRIENTS_A   = "NutrientsA";
        //public const string NUTRIENTS_A   = "NutrientsB";
        //public const string NUTRIENTS_ALL = "NutrientsALL";
    }

}
