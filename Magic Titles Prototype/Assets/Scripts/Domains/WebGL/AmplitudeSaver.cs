using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Apps.Runtime.Domains.WebGL
{
    public static class AmplitudeSaver
    {
        static readonly string s_path = "Assets/Resources";
        static readonly string s_file = "amplitudess";

        public static void Save(int id, Dictionary<float, float> amplitudes)
        {
            try
            {
                if (!Directory.Exists(s_path))
                {
                    Directory.CreateDirectory(s_path);
                }

            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            File.WriteAllText(s_path + "/" + s_file + id + ".txt", GetText(amplitudes));
        }

        public static Dictionary<float, float> Load(int id)
        {
            var txtData = Resources.Load<TextAsset>(s_file + id);
            Assert.IsNotNull(txtData, "Can't load resources");

            return GetDict(txtData.text);
        }

        private static string GetText(Dictionary<float, float> amplitudes)
        {
            if (amplitudes == null) return string.Empty;

            var builder = new StringBuilder();
            foreach (var pair in amplitudes)
            {
                builder.Append(pair.Key).Append(":").Append(pair.Value).Append(',');
            }
            string result = builder.ToString();
            result = result.TrimEnd(',');
            return result;
        }

        public static Dictionary<float, float> GetDict(string text)
        {
            var result = new Dictionary<float, float>();
            var tokens = text.Split(new char[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i += 2)
            {
                var frame = float.Parse(tokens[i]);
                var amplitudes = float.Parse(tokens[i + 1]);
                result[frame] = amplitudes;
            }
            return result;
        }
    }
}