using System.Collections.Generic;



namespace FrontPerson.Constants
{
    public enum VITAMIN_TYPE
    {
        VITAMIN_A,
        VITAMIN_B,
        VITAMIN_C,
        VITAMIN_D,
        VITAMIN_ALL,

        COUNT
    }

    // ビタミンから栄養素の変わったので変更
    // Vitamin -> Nutrientsにする
    public static class Vitamin
	{
        public static List<string> Type = new List<string>() 
        {
            "VitaminA",
            "VitaminB",
            "VitaminC",
            "VitaminD",
            "VitaminALL"
        };

        //public const string VITAMIN_A = "VitaminA";
        //public const string VITAMIN_B = "VitaminB";
        //public const string VITAMIN_C = "VitaminC";
        //public const string VITAMIN_D = "VitaminD";
        //public const string VITAMIN_ALL = "VitaminALL";
    }

}
