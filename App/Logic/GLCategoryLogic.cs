using App.Models;

namespace App.Logic
{
    public class GLCategoryLogic
    {
        public long GenerateGLAccouuntCode(MainGLCategory mainGLCategory, int id)
        {
            long GLCategoryCode = 0;
            long Id = id;

            switch (mainGLCategory)
            {
                case MainGLCategory.Asset:
                    GLCategoryCode = 1000 + Id;
                    break;
                case MainGLCategory.Liability:
                    GLCategoryCode = 2000 + Id;
                    break;

                case MainGLCategory.Income:
                    GLCategoryCode = 3000 + Id;
                    break;

                case MainGLCategory.Capital:
                    GLCategoryCode = 4000 + Id;
                    break;

                case MainGLCategory.Expenses:
                    GLCategoryCode = 5000 + Id;
                    break;
            }
            return GLCategoryCode;
        }
    }
}
