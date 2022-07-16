using App.Data;
using App.Models;

namespace App.Logic
{

    public class GlAccountLogic
    {
        private AppDbContext db;
        public bool AnyGlIn(MainGLCategory mainCategory)
        {
            return db.GLAccount.Any(gl => gl.GLCategory.MainCategory == mainCategory);
        }

        public GLAccount GetLastGlIn(MainGLCategory mainCategory)
        {
            return db.GLAccount.Where(gl => gl.GLCategory.MainCategory == mainCategory).OrderByDescending(a => a.GlCategoryID).First();
        }


        public long GeneratedGLAccountNumber(MainGLCategory glMainCategory)
        {
            long code = 0;

            if (AnyGlIn(glMainCategory))
            {
                var lastAcctNo = GetLastGlIn(glMainCategory);
                code = lastAcctNo.GLAccountCode + 1;
            }

            else
            {
                switch (glMainCategory)
                {
                    case MainGLCategory.Asset:
                        code = 1000072022;
                        break;

                    case MainGLCategory.Liability:
                        code = 2000072022;
                        break;

                    case MainGLCategory.Capital:
                        code = 3000072022;
                        break;

                    case MainGLCategory.Income:
                        code = 4000072022;
                        break;

                    case MainGLCategory.Expenses:
                        code = 5000072022;
                        break;
                }
            }
            return code;
        }
    }
}

