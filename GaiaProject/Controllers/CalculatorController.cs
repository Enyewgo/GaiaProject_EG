using Microsoft.AspNetCore.Mvc; // מייבא כלים לבניית ה-API (כמו הגדרת כתובות URL ותגובות שרת)
using GaiaProject.Models; // מחבר את הקוד לתיקיית המודלים (איפה שהגדרנו איך נראה ה-SQL)
using System;
using Microsoft.EntityFrameworkCore; // מייבא פקודות בסיסיות של שפת C# (כמו תאריך ושעה או עבודה עם טקסט)

namespace GaiaProject.Controllers
{
    [ApiController] // "תווית" שאומרת למחשב: המחלקה הזו היא שרת שמחזיר נתונים (JSON) ולא דפי אינטרנט
    [Route("api/[controller]")] // קובע את הכתובת: המילה [controller] תוחלף אוטומטית בשם הקלאס - Calculator
    public class CalculatorController : ControllerBase // ControllerBase נותן לנו פקודות מוכנות כמו Ok או BadRequest
    {
        // יצירת משתנה שישמור את החיבור למסד הנתונים. readonly אומר שלא ניתן לשנות את החיבור אחרי שהוגדר.
        private readonly AppDbContext _context;

        // ה"בנאי" (Constructor) - פונקציה שרצה ברגע שה-Controller נוצר בזיכרון.
        // המערכת "מזריקה" (Inject) לכאן את החיבור ל-SQL שמוגדר ב-Program.cs.
        public CalculatorController(AppDbContext context)
        {
            _context = context; // שומרים את החיבור במשתנה הפרטי שלנו כדי להשתמש בו בכל הפונקציות למטה.
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            // 1. פנייה לטבלה ב-SQL ומיון לפי הזמן הכי חדש
            var data = await _context.CalculationLogs.OrderByDescending(x => x.ExecutionTime)
                .ToListAsync();
            // 2. החזרת הרשימה הממוינת למשתמש
            return Ok (data);
        }
            // --- פונקציית המחשבון המתמטי ---
            [HttpGet("calculate")] // מגדיר כתובת: api/Calculator/calculate שניתן להגיע אליה מהדפדפן
                                   // סעיף 2.2: הוספנו את string user כדי לקבל את השם מה-React באופן דינמי
        public IActionResult Calculate(double n1, double n2, string action, string user)
        {
            double res = 0;
            string opName = action.ToLower();

            // לוגיקה צד שרת - סעיף 1.1: ביצוע החישובים המתמטיים
            if (opName == "add") res = n1 + n2;
            else if (opName == "sub") res = n1 - n2;
            else if (opName == "mul") res = n1 * n2;
            else if (opName == "div")
            {
                if (n2 != 0) res = n1 / n2;
                else return BadRequest("שגיאה: חלוקה באפס אסורה");
            }

            // יצירת אובייקט התיעוד - סעיף אינטגרציה ל-Database
            var log = new CalculationLog
            {
                OperationType = "Math",
                ActionName = opName,
                InputData = $"n1={n1}, n2={n2}",
                ResultValue = res.ToString(),
                ExecutionTime = DateTime.Now, // זמן הביצוע (לפי דרישת ההיסטוריה)
                                              // השורה שגרמה לשגיאה: עכשיו user מגיע מחתימת הפונקציה למעלה
                PerformedBy = user ?? "Guest",
               
            };

            // שמירה ל-SQL (סעיף 2.2.1: שמירת נתונים במסד נתונים)
            _context.CalculationLogs.Add(log);
            _context.SaveChanges();

            // בונוס 1.2.1: שליפת 3 התוצאות האחרונות של אותה פעולה (למשל 'add')
            var last3Results = _context.CalculationLogs
                .Where(l => l.ActionName == opName)
                .OrderByDescending(l => l.ExecutionTime)
                .Take(3)
                .Select(l => l.ResultValue)
                .ToList();
            // בונוס 1.2.2: ספירה של כמה פעמים ביצענו את הפעולה הזו החודש
            var monthlyCount = _context.CalculationLogs
                .Count(l => l.ActionName == opName &&
                            l.ExecutionTime.Month == DateTime.Now.Month &&
                            l.ExecutionTime.Year == DateTime.Now.Year);


            // החזרת תשובה ל-React - סעיף 2.2.2: הצגת נתונים מהשרת
            // החזרנו את log.PerformedBy כדי שה-React יראה בדיוק מי נרשם ב-DB
            return Ok(new {
                Result = res,
                User = log.PerformedBy,
                LastThree = last3Results,
                MonthlyCount = monthlyCount, // הבונוס של הספירה
                Message = "Saved to SQL" }); // הבונוס של ה-3 האחרונים
        }

        // --- פונקציית עיבוד הטקסט (אלגוריתמיקה ידנית) ---
        [HttpGet("string-action")]
        public IActionResult ProcessText(string input, string action)
        {
            // בדיקת בטיחות: מוודא שהמשתמש לא שלח שדה ריק (null או empty)
            if (string.IsNullOrEmpty(input)) return BadRequest("נא להזין טקסט תקין");

            string result = ""; // משתנה שיבנה את התוצאה הסופית אות-אחרי-אות.
            string opName = action.ToLower();

            // שימוש ב-Switch: דרך נקייה יותר מ-If לבדוק אפשרויות מרובות.
            switch (opName)
            {
                case "reverse":
                    // לולאת for שרצה מהסוף להתחלה. 
                    // i מתחיל באורך המילה פחות 1 (כי סופרים מ-0). 
                    // i-- אומר שבכל סיבוב יורדים צעד אחד אחורה לכיוון האות הראשונה (אינדקס 0).
                    for (int i = input.Length - 1; i >= 0; i--)
                    {
                        result += input[i]; // לוקח את האות במיקום ה-i ומדביק אותה לסוף המילה החדשה.
                    }
                    break;

                case "uppercase":
                    // לולאת foreach: "עבור כל תו (c) שנמצא בתוך הטקסט (input)".
                    foreach (char c in input)
                    {
                        // לוגיקת ASCII: בזיכרון המחשב, אותיות הן מספרים. 
                        // האות 'a' היא 97, האות 'A' היא 65. ההפרש הוא תמיד 32.
                        if (c >= 'a' && c <= 'z')
                            result += (char)(c - 32); // מחסירים 32 כדי להפוך לקוד של אות גדולה וממירים חזרה לתו (char).
                        else
                            result += c; // אם זה לא אות קטנה (מספר או רווח), משאירים כפי שהוא.
                    }
                    break;

                case "length":
                    // אלגוריתם ידני לספירה: במקום להשתמש ב-input.Length המובנה.
                    int count = 0; // מונה שמתחיל ב-0.
                    foreach (char c in input)
                    {
                        count++; // על כל אות שהלולאה מוצאת, היא מגדילה את המונה ב-1.
                    }
                    result = count.ToString(); // הפיכת המספר הסופי לטקסט כדי שנוכל להחזיר אותו.
                    break;
            }

            // שמירת פעולת הטקסט ב-SQL (אותו תהליך כמו במחשבון)
            _context.CalculationLogs.Add(new CalculationLog
            {
                OperationType = "String",
                ActionName = opName,
                InputData = input,
                ResultValue = result,
                ExecutionTime = DateTime.Now,
                PerformedBy = "E.G."
            });
            _context.SaveChanges(); // ביצוע הכתיבה ל-Database.

            return Ok(new { Result = result, User = "E.G." });
        }
    }
}