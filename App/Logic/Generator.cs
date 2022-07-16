using App.Models;

namespace App.Logic
{
    public class Generator
    {


        public static long GenerateGLAccountCode(string mainCat)
        {
            long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var start = 0;

            switch (mainCat)
            {
                case "Asset":
                    start = 1;
                    break;

                case "Liability":
                    start = 2;
                    break;

                case "Capital":
                    start = 3;
                    break;

                case "Income":
                    start = 4;
                    break;

                case "Expenses":
                    start = 5;
                    break;

                default:
                    break;
            }
            return start + milliseconds;

            //switch (mainCat)
            //{
            //    case "Asset":
            //        start = 100001;
            //        break;

            //    case "Liability":
            //        start = 200002;
            //        break;

            //    case "Capital":
            //        start = 300003;
            //        break;

            //    case "Income":
            //        start = 400004;
            //        break;

            //    case "Expenses":
            //        start = 500005;
            //        break;

            //    default:
            //        return 000000;
            //        break;
            //}
            //return start += 1;
        }
    }
}
