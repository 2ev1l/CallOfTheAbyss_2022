using UnityEngine.UI;
using Universal;

namespace GameMenu.Shop
{
    public sealed class PriceParser
    {
        private static readonly string silverColor = "EFEBE1";
        private static readonly string goldColor = "F0DF6E";
        public static void InsertSilverPrice(Text txt) => txt.text += $"<color=#{silverColor}>{TextOutline.languageData.interfaceData[22]}</color>";
        public static void InsertSilverPrice(Text txt, int count, bool compareCoinsToData)
        {
            if (compareCoinsToData && count == 0) return;
            string badColor = "F3A9BC";
            if ((GameDataInit.data.coinsSilver >= count && compareCoinsToData) || !compareCoinsToData)
            {
                txt.text += $"<color=#{silverColor}>{count}</color>";
            }
            else
            {
                txt.text += $"<color=#{badColor}>{count}</color>";
            }
            InsertSilverPrice(txt);
        }
        public static string InsertSilverPrice(string txt) => txt += $"<color=#{silverColor}>{TextOutline.languageData.interfaceData[22]}</color>";
        public static string InsertSilverPrice(string txt, int count, bool compareCoinsToData)
        {
            if (compareCoinsToData && count == 0) return txt;
            string badColor = "F3A9BC";
            if ((GameDataInit.data.coinsSilver >= count && compareCoinsToData) || !compareCoinsToData)
            {
                txt += $"<color=#{silverColor}>{count}</color>";
            }
            else
            {
                txt += $"<color=#{badColor}>{count}</color>";
            }
            return InsertSilverPrice(txt);
        }
        public static void InsertGoldPrice(Text txt) => txt.text += $"<color=#{goldColor}>{TextOutline.languageData.interfaceData[23]}</color>";
        public static void InsertGoldPrice(Text txt, int count, bool compareCoinsToData)
        {
            if (compareCoinsToData && count == 0) return;
            string badColor = "F07362";
            if ((GameDataInit.data.coinsGold >= count && compareCoinsToData) || !compareCoinsToData)
            {
                txt.text += $"<color=#{goldColor}>{count}</color>";
            }
            else
            {
                txt.text += $"<color=#{badColor}>{count}</color>";
            }
            InsertGoldPrice(txt);
        }
    }
}