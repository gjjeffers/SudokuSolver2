using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SudokuSolver2.Models;
using System.Threading.Tasks;
using System.Threading;

namespace SudokuSolver2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Puzzle emptyPuzzle = new Puzzle();
            return View(emptyPuzzle);
        }

        public PartialViewResult Solve(string startingPuzzle)
        {
            Puzzle puzzle = new Puzzle(startingPuzzle);
            puzzle.Solve();
            return PartialView(puzzle);
        }
    }
}