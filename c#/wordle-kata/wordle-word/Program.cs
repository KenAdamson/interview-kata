using System;
using System.Collections.Generic;
using System.Linq;

namespace wordle_word
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var wordDict = new Dictionary<char, float>();
            var words = System.IO.File.ReadAllLines("./sgb-words.txt");
            foreach (var word in words)
            {
                foreach (var c in word)
                {
                    if (!wordDict.ContainsKey(c))
                    {
                        wordDict[c] = 0;
                    }

                    wordDict[c] += 1;
                }
            }

            var scale = 0.5f;
            var ordered = wordDict.OrderByDescending(w => w.Value);
            var vowels = new List<char>() { 'a', 'e', 'i', 'o', 'u' };
            foreach (var item in ordered)
            {
                if (vowels.Contains(item.Key))
                {
                    var val = item.Value + item.Value * scale;
                    wordDict[item.Key] = val;
                }
                // var line = $"{item.Key}: {item.Value}";
                // Console.WriteLine(line);
            }

            var wordScores = new Dictionary<string, float>();
            foreach (var word in words)
            {
                if (!wordScores.ContainsKey(word))
                {
                    wordScores[word] = 0;
                }

                wordScores[word] = GetScoreForWord(word, wordDict);
            }

            var orderedByScores = wordScores.OrderByDescending(ws => ws.Value);
            foreach (var item in orderedByScores.Take(10))
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        static float GetScoreForWord(string word, Dictionary<char, float> wordDict)
        {
            var score = 0f;
            var currentWord = word.Distinct();
            foreach (var c in currentWord)
            {
                score += wordDict[c];
            }
            return score;
        }
    }
}
