using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SudokuSolver2.Models;

namespace SudokuSolver2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Solve(string startingPuzzle)
        {
            Puzzle puzzle = new Puzzle(startingPuzzle);
            puzzle.Start();
            return PartialView(puzzle);
        }
    }
}