using System;
using System.Collections.Generic;
using System.Text;

namespace LabProject.Presentation.Helpers
{
    public static class MenuManagement
    {
        public static ConsoleColor Accent = ConsoleColor.Cyan;
        public static ConsoleColor Back = ConsoleColor.Black;
        public static ConsoleColor Text = ConsoleColor.Cyan;
        public static ConsoleColor Dim = ConsoleColor.DarkCyan;
        public static ConsoleColor Error = ConsoleColor.Red;
        public static ConsoleColor WarnCol = ConsoleColor.Yellow;

        public struct Layout
        {
            public (int x1, int y1, int x2, int y2) Top;
            public (int x1, int y1, int x2, int y2) Outer;
            public (int x1, int y1, int x2, int y2) Left;
            public (int x1, int y1, int x2, int y2) Right;
            public (int x1, int y1, int x2, int y2) Bottom;
        }

        public static Layout L;
        static int _rightYBacking;

        public static void Init()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.BackgroundColor = Back;
            Console.Clear();
        }

        public static void ComputeLayout()
        {
            int W = Math.Max(120, Console.WindowWidth);
            int H = Math.Max(34, Console.WindowHeight);

            int m = Math.Max(2, W / 32);
            int topH = 3, botH = 3;

            int ox1 = m, oy1 = m, ox2 = W - m, oy2 = H - m;

            L.Top = (ox1, oy1, ox2, oy1 + topH);
            L.Bottom = (ox1, oy2 - botH, ox2, oy2);
            L.Outer = (ox1, L.Top.y2 + 1, ox2, L.Bottom.y1 - 1);

            int leftW = Math.Max(32, (int)(0.28 * (ox2 - ox1)));
            int leftX2 = L.Outer.x1 + leftW;

            L.Left = (L.Outer.x1 + 2, L.Outer.y1 + 1, leftX2, L.Outer.y2 - 1);
            L.Right = (leftX2 + 2, L.Outer.y1 + 1, L.Outer.x2 - 2, L.Outer.y2 - 1);
        }

        public static void Intro(string title = "MOVIE_CATALOG // MAIN_MENU")
        {
            Init();
            Console.ForegroundColor = Accent;

            string[] bootLines = {
                ">> LOADING CATALOG INDEX...",
                ">> CONNECTING TO MOVIE DATABASE...",
                ">> INDEXING GENRES AND ACTORS...",
                ">> SYSTEM STATUS: READY."
            };

            Random rand = new Random();
            foreach (var line in bootLines)
            {
                Console.WriteLine(line);
                Thread.Sleep(rand.Next(120, 300));
            }

            Thread.Sleep(300);
            Console.Clear();

            Console.Write("LOADING ");
            for (int i = 0; i < 20; i++)
            {
                Console.Write("█");
                Thread.Sleep(40);
            }
            Thread.Sleep(300);

            ComputeLayout();
            Frame(title);
            Footer("Ready.");
        }

        public static void Frame(string header)
        {
            Console.BackgroundColor = Back;
            Console.Clear();
            Console.ForegroundColor = Accent;
            Box(L.Top); Box(L.Outer); Box(L.Left); Box(L.Right); Box(L.Bottom);
            WriteCentered(L.Top, header, Accent);
            Footer("Ready.");
        }

        public static void Footer(string text, ConsoleColor? color = null)
        {
            Fill(L.Bottom, Back);
            Box(L.Bottom, Accent);
            var c = Console.ForegroundColor;
            Console.ForegroundColor = color ?? Dim;
            WriteLineBox(L.Bottom, text, 1);
            Console.ForegroundColor = c;
        }

        public static void LeftMenu(string title, IReadOnlyList<string> items)
        {
            Fill(L.Left, Back);
            Box(L.Left, Accent);

            var cy = L.Left.y1 + 1;
            WriteTitle(L.Left, title);

            for (int i = 0; i < items.Count && cy < L.Left.y2; i++, cy++)
            {
                SetPos(L.Left.x1 + 1, cy);
                Console.ForegroundColor = Text;
                WritePadded($"  {items[i]}  ", L.Left.x2 - L.Left.x1 - 1);
            }
            Console.ForegroundColor = Text;
        }

        public static void RightClear(string title)
        {
            Fill(L.Right, Back);
            Box(L.Right, Accent);
            WriteTitle(L.Right, title);
        }

        public static void RightWriteLine(string text)
        {
            _rightYBacking = Math.Min(_rightYBacking, L.Right.y2 - 1);
            SetPos(L.Right.x1 + 1, _rightYBacking++);
            var w = L.Right.x2 - L.Right.x1 - 1;
            WritePadded(text ?? string.Empty, w);
        }

        public static void RightWriteLines(IEnumerable<string> lines)
        {
            foreach (var s in lines) RightWriteLine(s);
        }

        public static string RightAsk(string prompt)
        {
            _rightYBacking = Math.Min(_rightYBacking, L.Right.y2 - 1);
            SetPos(L.Right.x1 + 1, _rightYBacking++);
            var p = (prompt ?? "") + " ";
            var w = L.Right.x2 - L.Right.x1 - 1;
            WritePadded(p, w);
            SetPos(L.Right.x1 + 1 + Math.Min(p.Length, w), _rightYBacking - 1);
            Console.CursorVisible = true;
            var s = Console.ReadLine() ?? "";
            Console.CursorVisible = false;
            return s;
        }

        public static string FooterAsk(string prompt)
        {
            Fill(L.Bottom, Back);
            Box(L.Bottom, Accent);

            var p = (prompt ?? "") + " ";
            var w = L.Bottom.x2 - L.Bottom.x1 - 1;

            SetPos(L.Bottom.x1 + 1, L.Bottom.y1 + 1);
            WritePadded(p, w);

            SetPos(L.Bottom.x1 + 1 + Math.Min(p.Length, w), L.Bottom.y1 + 1);
            Console.CursorVisible = true;
            var s = Console.ReadLine() ?? "";
            Console.CursorVisible = false;
            return s;
        }

        public static void RightError(string text)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = Error;
            RightWriteLine($"[ERROR] {text}");
            Console.ForegroundColor = prev;
            Footer(text, Error);
        }

        public static void RightWarn(string text)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = WarnCol;
            RightWriteLine($"[WARNING] {text}");
            Console.ForegroundColor = prev;
            Footer(text, WarnCol);
        }

        public static void RightSuccess(string text)
        {
            var prev = Console.ForegroundColor;
            Console.ForegroundColor = Accent;
            RightWriteLine($"[SUCCESS] {text}");
            Console.ForegroundColor = prev;
            Footer(text, Accent);
        }

        public static void RightBegin(string title)
        {
            RightClear(title);
            _rightYBacking = L.Right.y1 + 2;
        }

        public static void RightEnd(string footer = "Press ENTER to return to main menu...")
        {
            Footer(footer, Dim);
            Console.CursorVisible = false;
            Console.ReadLine();
        }

        static void SetPos(int x, int y)
        {
            x = Math.Max(1, Math.Min(Console.BufferWidth, x));
            y = Math.Max(1, Math.Min(Console.BufferHeight, y));
            Console.SetCursorPosition(x - 1, y - 1);
        }

        static void Box((int x1, int y1, int x2, int y2) r, ConsoleColor? color = null)
        {
            var c = Console.ForegroundColor;
            if (color != null) Console.ForegroundColor = color.Value;

            char tl = '┌', tr = '┐', bl = '└', br = '┘', h = '─', v = '│';

            SetPos(r.x1, r.y1); Console.Write(tl);
            for (int x = r.x1 + 1; x < r.x2; x++) Console.Write(h);
            Console.Write(tr);

            for (int y = r.y1 + 1; y < r.y2; y++)
            {
                SetPos(r.x1, y); Console.Write(v);
                SetPos(r.x2, y); Console.Write(v);
            }

            SetPos(r.x1, r.y2); Console.Write(bl);
            for (int x = r.x1 + 1; x < r.x2; x++) Console.Write(h);
            Console.Write(br);

            Console.ForegroundColor = c;
        }

        static void Fill((int x1, int y1, int x2, int y2) r, ConsoleColor? _ = null)
        {
            var w = r.x2 - r.x1 - 1;
            for (int y = r.y1 + 1; y < r.y2; y++)
            {
                SetPos(r.x1 + 1, y);
                Console.Write(new string(' ', Math.Max(0, w)));
            }
        }

        static void WriteCentered((int x1, int y1, int x2, int y2) r, string text, ConsoleColor? color = null)
        {
            var c = Console.ForegroundColor;
            if (color != null) Console.ForegroundColor = color.Value;
            int width = r.x2 - r.x1 - 1;
            var line = Center(text, width);
            SetPos(r.x1 + 1, r.y1 + 1);
            Console.Write(line);
            Console.ForegroundColor = c;
        }

        static void WriteTitle((int x1, int y1, int x2, int y2) r, string title)
        {
            var t = "=== " + (title ?? "").ToUpper() + " ===";
            SetPos(r.x1 + 1, r.y1);
            Console.ForegroundColor = Dim;
            WritePadded(t, r.x2 - r.x1 - 1);
            Console.ForegroundColor = Text;
        }

        static void WriteLineBox((int x1, int y1, int x2, int y2) r, string text, int lineOffset)
        {
            SetPos(r.x1 + 1, r.y1 + lineOffset);
            WritePadded(text, r.x2 - r.x1 - 1);
        }

        static void WritePadded(string s, int width)
        {
            s ??= "";
            if (s.Length >= width) Console.Write(s[..width]);
            else Console.Write(s + new string(' ', width - s.Length));
        }

        static string Center(string s, int width)
        {
            s ??= "";
            if (s.Length >= width) return s[..width];
            int left = (width - s.Length) / 2;
            return new string(' ', left) + s + new string(' ', width - left - s.Length);
        }

        public static void ErrorMsg(string text) => Footer(text, Error);
        public static void Warn(string text) => Footer(text, WarnCol);
        public static void Info(string text) => Footer(text, Text);

        public static void Run(ManageMetods manage)
        {
            var actions = new Dictionary<int, (string Label, Action Act)>
            {
                [1] = ("Create Genre", manage.CreateGenre),
                [2] = ("Create Actor", manage.CreateActor),
                [3] = ("Create Movie", manage.CreateMovie),
                [4] = ("Show All Genres", manage.ShowAllGenres),
                [5] = ("Show All Actors", manage.ShowAllActors),
                [6] = ("Show All Movies", manage.ShowAllMovies),
                [7] = ("Show Movie Details", manage.ShowMovieDetails),
                [8] = ("Assign Actor To Movie", manage.AssignActorToMovie),
                [9] = ("Show Movie Actors", manage.ShowMovieActors),
                [10] = ("Search Movie", manage.SearchMovie),
                [11] = ("Movie Statistics", manage.MovieStatistics),
                [12] = ("Delete Movie (Soft Delete)", manage.DeleteMovie),
                [13] = ("Restore Movie", manage.RestoreMovie),
            };

            Intro("MOVIE_CATALOG // MAIN_MENU");

            while (true)
            {
                ComputeLayout();
                Frame("Movie Catalog");

                var menuItems = actions
                    .OrderBy(kv => kv.Key)
                    .Select(kv => $"{kv.Key,2}. {kv.Value.Label}")
                    .ToList();
                menuItems.Add(" 14. Exit");

                LeftMenu("Menu", menuItems);

                RightBegin("Welcome");
                RightWriteLines(new[]
                {
                    "Use numbers [1-14] to navigate.",
                    "Press 14 to Exit."
                });
                RightEnd("Enter your choice [1-14] and press ENTER...");

                var raw = (FooterAsk("Enter your choice [1-14]: ") ?? "").Trim();

                if (!int.TryParse(raw, out var pick))
                {
                    ErrorMsg("Please enter a number.");
                    Thread.Sleep(900);
                    continue;
                }
                if (pick == 14)
                {
                    Info("Exiting...");
                    Thread.Sleep(500);
                    Console.Clear();
                    return;
                }
                if (!actions.TryGetValue(pick, out var item))
                {
                    Warn("Wrong selection. Try again.");
                    Thread.Sleep(900);
                    continue;
                }

                try
                {
                    item.Act();
                }
                catch (Exception ex)
                {
                    ErrorMsg(ex.Message);
                    Console.CursorVisible = true;
                    Console.ReadLine();
                    Console.CursorVisible = false;
                }
            }
        }
    }
}
