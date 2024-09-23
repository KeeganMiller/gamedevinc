using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Godot;

namespace GameDevInc;

public class Game
{
    // == Game Details == //
    public string GameName { get; private set; }
    public Genre GenreOne { get; private set; }
    public Genre? GenreTwo { get; private set; }
    public Theme GameTheme { get; private set; }

    // == Module Properties == //
    private List<BaseModule> m_RequiredModules = new List<BaseModule>();                        // List of all the required modules


    // == Release Properties == //
    public float GameRating { get; private set; }
    public Date GameReleaseDate { get; private set; }


    public Game(string name, Theme theme, Genre genreOne, Genre genreTwo = null)
    {
        GameName = name;
        GameTheme = theme;
        GenreOne = genreOne;
        GenreTwo = genreTwo;
    }

    public void ReleaseGame()
    {
        
    }

    /// <summary>
    /// Checks if we have all modules in the inventory
    /// </summary>
    /// <returns>If all modules created</returns>
    public bool HasAllModules()
    {
        bool hasModule = true;
        foreach(var module in ModuleDatabase.ModuleInventory)
        {
            foreach(var reqMod in m_RequiredModules)
            {
                if(reqMod.ModuleName == module.ModuleName)
                {
                    hasModule = false;
                    break;
                }
            }

            if (!hasModule)
                break;
        }

        return hasModule;
    }
}