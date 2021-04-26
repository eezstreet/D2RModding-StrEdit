using System.Collections.Generic;
using System.Globalization;

namespace D2RModding_StrEdit
{
    public class StringEntry
    {
        public StringEntry(string stringName, int idNum)
        {
            dict = new Dictionary<StringLanguages, string>();
            for (var i = StringLanguages.LANG_enUS; i < StringLanguages.LANG_MAX; i++)
            {
                dict[i] = "";
            }
            Key = stringName;
            id = idNum;
        }
        public StringEntry()
        {
            dict = new Dictionary<StringLanguages, string>();
            for(var i = StringLanguages.LANG_enUS; i < StringLanguages.LANG_MAX; i++)
            {
                dict[i] = "";
            }
        }

        public override string ToString()
        {
            return Key;
        }

        public enum StringLanguages
        {
            LANG_enUS,
            LANG_zhTW,
            LANG_deDE,
            LANG_esES,
            LANG_frFR,
            LANG_itIT,
            LANG_koKR,
            LANG_plPL,
            LANG_esMX,
            LANG_jaJP,
            LANG_ptBR,
            LANG_ruRU,
            LANG_zhCN,
            LANG_MAX,
        };

        public static string LanguageEnumToString(StringLanguages e)
        {
            switch (e)
            {
                case StringLanguages.LANG_deDE:
                    return "deDE";
                case StringLanguages.LANG_enUS:
                    return "enUS";
                case StringLanguages.LANG_esES:
                    return "esES";
                case StringLanguages.LANG_esMX:
                    return "esMX";
                case StringLanguages.LANG_frFR:
                    return "frFR";
                case StringLanguages.LANG_itIT:
                    return "itIT";
                case StringLanguages.LANG_jaJP:
                    return "jaJP";
                case StringLanguages.LANG_koKR:
                    return "koKR";
                case StringLanguages.LANG_plPL:
                    return "plPL";
                case StringLanguages.LANG_ptBR:
                    return "ptBR";
                case StringLanguages.LANG_ruRU:
                    return "ruRU";
                case StringLanguages.LANG_zhCN:
                    return "zhCN";
                case StringLanguages.LANG_zhTW:
                    return "zhTW";
                default:
                    return "unknown";
            }
        }

        public static StringLanguages LanguageStringToEnum(string s)
        {
            if (s.Equals("deDE")) { return StringLanguages.LANG_deDE; }
            else if (s.Equals("enUS")) { return StringLanguages.LANG_enUS; }
            else if (s.Equals("esES")) { return StringLanguages.LANG_esES; }
            else if (s.Equals("esMX")) { return StringLanguages.LANG_esMX; }
            else if (s.Equals("frFR")) { return StringLanguages.LANG_frFR; }
            else if (s.Equals("itIT")) { return StringLanguages.LANG_itIT; }
            else if (s.Equals("jaJP")) { return StringLanguages.LANG_jaJP; }
            else if (s.Equals("koKR")) { return StringLanguages.LANG_koKR; }
            else if (s.Equals("plPL")) { return StringLanguages.LANG_plPL; }
            else if (s.Equals("ptBR")) { return StringLanguages.LANG_ptBR; }
            else if (s.Equals("ruRU")) { return StringLanguages.LANG_ruRU; }
            else if (s.Equals("zhCN")) { return StringLanguages.LANG_zhCN; }
            else if (s.Equals("zhTW")) { return StringLanguages.LANG_zhTW; }
            return StringLanguages.LANG_MAX;
        }

        public static CultureInfo CultureInfoForLanguage(StringLanguages language)
        {
            switch(language)
            {
                case StringLanguages.LANG_deDE:
                    return new CultureInfo("de-DE", false);
                default:
                case StringLanguages.LANG_enUS:
                    return new CultureInfo("en-US", false);
                case StringLanguages.LANG_esES:
                    return new CultureInfo("es-ES", false);
                case StringLanguages.LANG_esMX:
                    return new CultureInfo("es-MX", false);
                case StringLanguages.LANG_frFR:
                    return new CultureInfo("fr-FR", false);
                case StringLanguages.LANG_itIT:
                    return new CultureInfo("it-IT", false);
                case StringLanguages.LANG_jaJP:
                    return new CultureInfo("ja-JP", false);
                case StringLanguages.LANG_koKR:
                    return new CultureInfo("ko-KR", false);
                case StringLanguages.LANG_plPL:
                    return new CultureInfo("pl-PL", false);
                case StringLanguages.LANG_ptBR:
                    return new CultureInfo("pt-BR", false);
                case StringLanguages.LANG_ruRU:
                    return new CultureInfo("ru-RU", false);
                case StringLanguages.LANG_zhCN:
                    return new CultureInfo("zh-CN", false);
                case StringLanguages.LANG_zhTW:
                    return new CultureInfo("zh-TW", false);
            }
        }

        public int id { get; set; }
        public string Key { get; set; }

        private Dictionary<StringLanguages, string> dict;
        public string getStringForLanguage(StringLanguages language)
        {
            return dict[language];
        }
        public void setStringForLanguage(StringLanguages language, string newString)
        {
            dict[language] = newString;
        }
        public string deDE
        {
            get
            {
                return dict[StringLanguages.LANG_deDE];
            }
            set
            {
                dict[StringLanguages.LANG_deDE] = value;
            }
        }
        public string enUS
        {
            get
            {
                return dict[StringLanguages.LANG_enUS];
            }
            set
            {
                dict[StringLanguages.LANG_enUS] = value;
            }
        }
        public string esES
        {
            get
            {
                return dict[StringLanguages.LANG_esES];
            }
            set
            {
                dict[StringLanguages.LANG_esES] = value;
            }
        }
        public string esMX
        {
            get
            {
                return dict[StringLanguages.LANG_esMX];
            }
            set
            {
                dict[StringLanguages.LANG_esMX] = value;
            }
        }
        public string frFR
        {
            get
            {
                return dict[StringLanguages.LANG_frFR];
            }
            set
            {
                dict[StringLanguages.LANG_frFR] = value;
            }
        }
        public string itIT
        {
            get
            {
                return dict[StringLanguages.LANG_itIT];
            }
            set
            {
                dict[StringLanguages.LANG_itIT] = value;
            }
        }
        public string jaJP
        {
            get
            {
                return dict[StringLanguages.LANG_jaJP];
            }
            set
            {
                dict[StringLanguages.LANG_jaJP] = value;
            }
        }
        public string koKR
        {
            get
            {
                return dict[StringLanguages.LANG_koKR];
            }
            set
            {
                dict[StringLanguages.LANG_koKR] = value;
            }
        }
        public string plPL
        {
            get
            {
                return dict[StringLanguages.LANG_plPL];
            }
            set
            {
                dict[StringLanguages.LANG_plPL] = value;
            }
        }
        public string ptBR
        {
            get
            {
                return dict[StringLanguages.LANG_ptBR];
            }
            set
            {
                dict[StringLanguages.LANG_ptBR] = value;
            }
        }
        public string ruRU
        {
            get
            {
                return dict[StringLanguages.LANG_ruRU];
            }
            set
            {
                dict[StringLanguages.LANG_ruRU] = value;
            }
        }
        public string zhCN
        {
            get
            {
                return dict[StringLanguages.LANG_zhCN];
            }
            set
            {
                dict[StringLanguages.LANG_zhCN] = value;
            }
        }
        public string zhTW
        {
            get
            {
                return dict[StringLanguages.LANG_zhTW];
            }
            set
            {
                dict[StringLanguages.LANG_zhTW] = value;
            }
        }
    }

    public class StringEntryEqualityComparer : IEqualityComparer<StringEntry>
    {
        public bool Equals(StringEntry T1, StringEntry T2)
        {
            return T1.id == T2.id;
        }

        public int GetHashCode(StringEntry T1)
        {
            return T1.id;
        }
    }
}
