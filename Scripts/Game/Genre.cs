using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameDevInc;

public class Genre
{
    private const string GenreResourcePath = "res://Data/Genre.json";

    public string GenreName { get; protected set; }

    private static List<Genre> LoadedGenres = new List<Genre>();

    public Genre(string genreName)
    {
        GenreName = genreName;
    }

    public static void LoadGenres()
    {
        if(FileAccess.FileExists(GenreResourcePath))
        {
            var file = FileAccess.Open(GenreResourcePath, FileAccess.ModeFlags.Read);
            if(file != null && file.IsOpen())
            {
                var dataText = file.GetAsText();
                var tokens = JArray.Parse(dataText);

                foreach(var token in tokens)
                {
                    var genreData = JsonConvert.DeserializeObject<GenreJson>(token.ToString());
                    LoadedGenres.Add(new Genre(genreData.Name));
                }
            }
        }
    }
}

public class GenreJson
{
    [JsonProperty]
    public string Name;
}