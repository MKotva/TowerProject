using Assets.Scripts.UIControllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public static class PuzzleLoader
{
    // Loads all puzzles as a flat list (same as before)
    public static List<Puzzle> LoadPuzzles(string xmlFilePath)
    {
        if (!File.Exists(xmlFilePath))
            throw new FileNotFoundException("XML file not found.", xmlFilePath);

        XDocument doc = XDocument.Load(xmlFilePath);

        var puzzles = doc.Root
            .Elements("Puzzle")
            .Select(p =>
            {
                string riddle = (string) p.Element("Question") ?? string.Empty;
                string correct = (string) p.Element("CorrectAnswer") ?? string.Empty;
                string difficultyText = (string) p.Element("Difficulty") ?? "0";

                int difficulty = 0;
                int.TryParse(difficultyText, out difficulty);

                var answers = new List<string>();

                if (!string.IsNullOrWhiteSpace(correct))
                    answers.Add(correct);

                answers.AddRange(
                    p.Elements("WrongAnswer")
                     .Select(x => ( x.Value ?? string.Empty ).Trim())
                     .Where(s => !string.IsNullOrWhiteSpace(s))
                );

                return new Puzzle
                {
                    Riddle = riddle.Trim(),
                    CorrectOne = correct.Trim(),
                    Difficulty = difficulty,
                    Answers = answers
                };
            })
            .ToList();

        return puzzles;
    }

    // New: loads and groups puzzles into List<List<Puzzle>> by difficulty
    public static List<List<Puzzle>> LoadPuzzlesGroupedByDifficulty(string xmlFilePath)
    {
        var allPuzzles = LoadPuzzles(xmlFilePath);

        // Group by Difficulty, order by Difficulty, then create the List<List<Puzzle>>
        var grouped = allPuzzles
            .GroupBy(p => p.Difficulty)
            .OrderBy(g => g.Key)
            .Select(g => g.ToList())
            .ToList();

        return grouped;
    }
}
