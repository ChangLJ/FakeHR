namespace HumanResource.Api.Data;

/// <summary>產生示範用假資料（非真實個資）。固定種子以確保每次全新資料庫內容一致。</summary>
public static class DemoDataGenerator
{
    private static readonly Random Rng = new(20260520);

    public static readonly string[] Surnames = ["王", "李", "陳", "林", "黃", "張", "劉", "吳"];
    public static readonly string[] GivenNames = ["大明", "淑芬", "志明", "雅婷", "俊宏", "佳慧", "冠宇", "怡君"];
    public static readonly string[] EnglishFirst = ["Alex", "Jamie", "Chris", "Taylor", "Jordan", "Morgan", "Casey", "Riley"];
    public static readonly string[] Cities = ["台北市", "新北市", "桃園市", "台中市", "台南市", "高雄市"];
    public static readonly string[] Districts = ["中正區", "大安區", "西屯區", "北區", "東區", "前鎮區"];
    public static readonly string[] Schools = ["國立台灣大學", "國立成功大學", "逢甲大學", "實踐大學", "輔仁大學"];
    public static readonly string[] Majors = ["資訊工程", "資訊管理", "企業管理", "電機工程", "工業工程"];
    public static readonly string[] Companies = ["示例科技股份有限公司", "測試資訊有限公司", "樣本軟體股份有限公司", "模擬系統有限公司"];
    public static readonly string[] JobTitles = ["軟體工程師", "系統分析師", "專案經理", "後端工程師", "全端工程師"];
    public static readonly string[] Positions = ["軟體工程師", "資深工程師", "系統分析師", "專案管理師"];

    public const string DemoIdNumber = "D123456789";
    public const string DemoPassword = "demo1234";

    public static string Pick(params string[] items) => items[Rng.Next(items.Length)];

    public static string RandomChineseName() => Pick(Surnames) + Pick(GivenNames);

    public static string RandomEnglishName() => $"{Pick(EnglishFirst)} {Pick(Surnames)}";

    public static string RandomMobile() => $"09{Rng.Next(10000000, 99999999)}";

    public static string RandomEmail(string prefix) =>
        $"{prefix}{Rng.Next(1000, 9999)}@demo.example.com";

    public static string RandomZip() => Rng.Next(100, 999).ToString();

    public static string RandomStreet() => $"示範路{Rng.Next(1, 200)}號";

    public static int RandomYear(int min, int max) => Rng.Next(min, max + 1);

    public static int RandomMonth() => Rng.Next(1, 13);

    public static int RandomDay() => Rng.Next(1, 29);
}
