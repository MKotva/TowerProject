using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.UIControllers
{
    public class Puzzle
    {
        public string Riddle {  get; set; }
        public List<string> Answers { get; set; }
        public string CorrectOne { get; set; }
        public int Difficulty { get; set; }
    }
}
