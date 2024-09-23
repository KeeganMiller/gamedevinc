using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameDevInc;

public class Theme
{
    private const string c_ThemeResourcePath = "res://Data/Theme.json";
    public string ThemeName { get; private set; }

    private static List<Theme> s_AllThemes = new List<Theme>();
    
    public Theme(string themeName)
    {
        ThemeName = themeName;
    }

    public static void LoadThemes()
    {
        if(FileAccess.FileExists(c_ThemeResourcePath))
        {
            var file = FileAccess.Open(c_ThemeResourcePath, FileAccess.ModeFlags.Read);
            if(file != null && file.IsOpen())
            {
                var data = file.GetAsText();
                var tokens = JArray.Parse(data);

                foreach(var token in tokens)
                {
                    var theme = JsonConvert.DeserializeObject<ThemeJson>(token.ToString());
                    s_AllThemes.Add(new Theme(theme.ThemeName));
                }

                file.Close();
            }
        }
    }
}

public class ThemeJson
{
    [JsonProperty]
    public string ThemeName;
}