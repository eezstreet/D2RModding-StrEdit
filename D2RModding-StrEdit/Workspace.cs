using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Linq;
using System.Globalization;
using System;

namespace D2RModding_StrEdit
{
    // Workspace is legacy and resurrected strings but also the last ref id
    class Workspace
    {
        private int lastRefId;
        private string directory;
        private Dictionary<string, StringEntry[]> legacyBanks;
        private Dictionary<string, StringEntry[]> resurrectedBanks;

        private StringEntry[] EntriesFromFile(string fileName, StringEntry[] fallback)
        {
            string fileText = File.ReadAllText(fileName);
            StringEntry[] newEntries = JsonSerializer.Deserialize<StringEntry[]>(fileText);
            if(fallback != null)
            {
                // go through each entry and look for any missing languages.
                for(var i = 0; i < fallback.Length; i++)
                {
                    for(var j = StringEntry.StringLanguages.LANG_enUS; j < StringEntry.StringLanguages.LANG_MAX; j++)
                    {
                        if(i >= newEntries.Length)
                        {
                            newEntries = newEntries.Concat(fallback.Skip(newEntries.Length)).ToArray();
                            break; // just append new entries here.
                        }
                        string langString = newEntries[i].getStringForLanguage(j);
                        if(langString == null || langString == "")
                        {
                            string fallbackStr = fallback[i].getStringForLanguage(j);
                            if(fallback[i].id == newEntries[i].id && fallbackStr != null && fallbackStr != "")
                            { // this language for this bank has a bad entry on this string but the fallback does not.
                                // just load data from the fallback.
                                newEntries[i].setStringForLanguage(j, fallbackStr);
                            }
                        }
                    }
                }
            }
            return newEntries;
        }

        private Dictionary<string, StringEntry[]> MakeBanks(string dir, Dictionary<string, StringEntry[]> fallback)
        {
            string[] files = Directory.GetFiles(dir);
            Dictionary<string, StringEntry[]> dict = new Dictionary<string, StringEntry[]>();
            foreach(string file in files)
            {
                if(fallback != null)
                {
                    StringEntry[] value;
                    fallback.TryGetValue(Path.GetFileNameWithoutExtension(file), out value);
                    dict[Path.GetFileNameWithoutExtension(dir + file)] = EntriesFromFile(file, value);
                }
                else
                {
                    dict[Path.GetFileNameWithoutExtension(dir + file)] = EntriesFromFile(file, null);
                }
                
            }
            return dict;
        }

        public Workspace(string dir)
        {
            directory = dir;

            // load up!
            resurrectedBanks = MakeBanks(dir + "\\strings", null);
            legacyBanks = MakeBanks(dir + "\\strings-legacy", resurrectedBanks);

            // parse the next_string_id.txt crap
            string next_valid_id = File.ReadAllText(dir + "\\next_string_id.txt");
            Regex regex = new Regex("([0-9]+)");
            Match match = regex.Match(next_valid_id);

            lastRefId = Int32.Parse(match.Value);
        }

        public void GetStringsInBank(string bankName, out StringEntry[] legacy, out StringEntry[] resurrected)
        {
            if(!legacyBanks.ContainsKey(bankName))
            {
                legacy = resurrectedBanks[bankName];
            }
            else
            {
                legacy = legacyBanks[bankName];
            }
            resurrected = resurrectedBanks[bankName];
        }

        public void GetStringByKey(string bankName, string keyName, StringEntry.StringLanguages language, out string legacy, out string resurrected)
        {
            StringEntry[] legacyBank, resurrectedBank;
            GetStringsInBank(bankName, out legacyBank, out resurrectedBank);
            legacy = (from se in legacyBank where se.Key.Equals(keyName) select se.getStringForLanguage(language)).First();
            resurrected = (from se in resurrectedBank where se.Key.Equals(keyName) select se.getStringForLanguage(language)).First();
        }
        public void GetStringById(string bankName, int id, StringEntry.StringLanguages language, out string legacy, out string resurrected)
        {
            StringEntry[] legacyBank, resurrectedBank;
            GetStringsInBank(bankName, out legacyBank, out resurrectedBank);
            var legacyItems = from se in legacyBank where se.id == id select se.getStringForLanguage(language);
            var resurrectedItems = from se in resurrectedBank where se.id == id select se.getStringForLanguage(language);
            resurrected = resurrectedItems.First();
            if (!legacyItems.Any() || legacyItems == null)
            {
                legacy = resurrected;
            }
            else
            {
                legacy = legacyItems.First();
            }
            
        }
        public ref StringEntry GetLegacyEntryById(string bankName, int id)
        {
            if(!legacyBanks.ContainsKey(bankName))
            {
                return ref GetResurrectedEntryById(bankName, id);
            }

            for (var i = 0; i < legacyBanks[bankName].Length; i++)
            {
                if(legacyBanks[bankName][i].id == id)
                {
                    return ref legacyBanks[bankName][i];
                }
            }
            throw new InvalidOperationException("Not found");
        }
        public ref StringEntry GetResurrectedEntryById(string bankName, int id)
        {
            for (var i = 0; i < resurrectedBanks[bankName].Length; i++)
            {
                if (resurrectedBanks[bankName][i].id == id)
                {
                    return ref resurrectedBanks[bankName][i];
                }
            }
            throw new System.InvalidOperationException("Not found");
        }
        public void CopyResurrectedToLegacy(string bankName, int id)
        {
            for(var i = 0; i < resurrectedBanks[bankName].Length; i++)
            {
                if(resurrectedBanks[bankName][i].id == id)
                {
                    legacyBanks[bankName] = legacyBanks[bankName].Append(resurrectedBanks[bankName][i]).ToArray();
                    return;
                }
            }
        }
        public void GetEntriesById(string bankName, int id, out StringEntry legacy, out StringEntry resurrected)
        {
            legacy = GetLegacyEntryById(bankName, id);
            resurrected = GetResurrectedEntryById(bankName, id);
        }
        public string[] GetAllBanks()
        {
            string[] legacyBankNames = new string[legacyBanks.Keys.Count];
            string[] resurrectedBankNames = new string[resurrectedBanks.Keys.Count];

            legacyBanks.Keys.CopyTo(legacyBankNames, 0);
            resurrectedBanks.Keys.CopyTo(resurrectedBankNames, 0);

            // union the two arrays
            return legacyBankNames.Union(resurrectedBankNames).ToArray();
        }

        public string GetDirectory()
        {
            return directory;
        }

        public int GetLastId()
        {
            return lastRefId;
        }

        public void AddString(string stringName, string bank)
        {
            StringEntry entry = new StringEntry(stringName, lastRefId++);
            legacyBanks[bank] = legacyBanks[bank].Append(entry).ToArray();
            resurrectedBanks[bank] = resurrectedBanks[bank].Append(entry).ToArray();
        }

        public void RemoveString(int id)
        {
            // we have to go into each and every bank to find it
            string[] allBanks = GetAllBanks();
            foreach(string bank in allBanks)
            {
                if(legacyBanks.ContainsKey(bank))
                {
                    int beforeLength = legacyBanks[bank].Length;
                    StringEntry[] after = legacyBanks[bank].Where(x => x.id != id).ToArray();
                    if(after.Length != beforeLength)
                    {
                        legacyBanks[bank] = after;
                        if(resurrectedBanks.ContainsKey(bank))
                        {
                            StringEntry[] afterRes = resurrectedBanks[bank].Where(x => x.id != id).ToArray();
                            resurrectedBanks[bank] = afterRes;
                        }
                        return;
                    }
                }
                else if(resurrectedBanks.ContainsKey(bank))
                {
                    int beforeLength = resurrectedBanks[bank].Length;
                    StringEntry[] after = resurrectedBanks[bank].Where(x => x.id != id).ToArray();
                    if(after.Length != beforeLength)
                    {
                        resurrectedBanks[bank] = after;
                        return;
                    }
                }
            }
            // hm...string didn't exist..?
        }
        public string[] FindKeysWithKeyMatching(string text, string bank)
        {
            Regex regex = new Regex(text, RegexOptions.IgnoreCase);
            return (from se in resurrectedBanks[bank] where regex.IsMatch(se.Key) select se.Key).ToArray();
        }
        public string[] FindKeysWithTextMatching(string text, string bank, StringEntry.StringLanguages language)
        {
            CultureInfo culture = StringEntry.CultureInfoForLanguage(language);
            Regex regex = new Regex(text, RegexOptions.IgnoreCase);
            var legacyKeys = from se in legacyBanks[bank] where regex.IsMatch(se.getStringForLanguage(language)) select se.Key;
            var resurrectedKeys = from se in resurrectedBanks[bank] where regex.IsMatch(se.getStringForLanguage(language)) select se.Key;
            return legacyKeys.Union(resurrectedKeys).ToArray();
        }
        public void RenameKeyTo(string bank, int id, string newKey)
        {
            GetLegacyEntryById(bank, id).Key = newKey;
            GetResurrectedEntryById(bank, id).Key = newKey;
        }
        public void ChangeLegacyStringTo(string bank, int id, string newString, StringEntry.StringLanguages language)
        {
            try
            {
                GetLegacyEntryById(bank, id).setStringForLanguage(language, newString);
            }
            catch (InvalidOperationException)
            {
                // can happen if legacy didn't have it to begin with
                CopyResurrectedToLegacy(bank, id);
                ChangeLegacyStringTo(bank, id, newString, language);
            }
            
        }
        public void ChangeResurrectedStringTo(string bank, int id, string newString, StringEntry.StringLanguages language)
        {
            GetResurrectedEntryById(bank, id).setStringForLanguage(language, newString);
        }
        public void SaveWorkspace()
        {
            // save ref id
            string next_valid_id = File.ReadAllText(directory + "\\next_string_id.txt");
            Regex regex = new Regex("([0-9]+)");
            string asTxt = regex.Replace(next_valid_id, lastRefId.ToString(), 1);
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;

            File.WriteAllText(directory + "\\next_string_id.txt", asTxt);
            // now write all the banks
            var legacyBankKeys = legacyBanks.Keys;
            var resurrectedBankKeys = resurrectedBanks.Keys;

            foreach(var key in legacyBankKeys)
            {
                // json serialize me, captain!
                string serialized = JsonSerializer.Serialize(legacyBanks[key], options);
                File.WriteAllText(directory + "\\strings-legacy\\" + key + ".json", serialized);
            }

            foreach(var key in resurrectedBankKeys)
            {
                string serialized = JsonSerializer.Serialize(resurrectedBanks[key], options);
                File.WriteAllText(directory + "\\strings\\" + key + ".json", serialized);
            }
        }

        public void ExportAsExcel(string filePath)
        {
            var Filestream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            var stream = new StreamWriter(Filestream);

            stream.WriteLine("Bank\tKey\tID\tenUS (Legacy)\tenUS (Resurrected)\tzhTW (Legacy)\tzhTW (Resurrected)\tdeDE (Legacy)\tdeDE (Resurrected)\tesES (Legacy)\tesES (Resurrected)\tfrFR (Legacy)\tfrFR (Resurrected)\titIT (Legacy)\titIT (Resurrected)\tkokR (Legacy)\tkoKR (Resurrected)\tplPL (Legacy)\tplPL (Resurrected)\tesMX (Legacy)\tesMX (Resurrected)\tjaJP (Legacy)\tjaJP (Resurrected)\tptBR (Legacy)\tptBR (Resurrected)\truRU (Legacy)\truRU (Resurrected)\tzhCN (Legacy)\tzhCN (Resurrected)\tEOL");
            
            // iterate through each bank...
            var keys = resurrectedBanks.Keys;
            foreach(var key in keys)
            {
                if(legacyBanks.ContainsKey(key))
                {
                    foreach(var value in legacyBanks[key])
                    {
                        var valRes = resurrectedBanks[key][value.id];
                        //                Bank Key ID + 26 means 28 is max key
                        stream.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\t{19}\t{20}\t{21}\t{22}\t{23}\t{24}\t{25}\t{26}\t{27}\t{28}\t0",
                            key, value.Key, value.id,
                            value.enUS, valRes.enUS,
                            value.zhTW, valRes.zhTW,
                            value.deDE, valRes.deDE,
                            value.esES, valRes.esES,
                            value.frFR, valRes.frFR,
                            value.itIT, valRes.itIT,
                            value.koKR, valRes.koKR,
                            value.plPL, valRes.plPL,
                            value.esMX, valRes.esMX,
                            value.jaJP, valRes.jaJP,
                            value.ptBR, valRes.ptBR,
                            value.ruRU, valRes.ruRU,
                            value.zhCN, valRes.zhCN);
                    }
                }
                else
                {

                }
            }
        }
        public static void CompileExcel(string filePath)
        {

        }
    }
}
